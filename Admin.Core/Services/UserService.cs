using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Core.Dtos;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.DataProtection;
using Imanage.Shared.Models;
using Imanage.Shared.PubSub;
using Imanage.Shared.ViewModels;
using Imanage.Shared.Helpers;
using Imanage.Shared.Enums;
using Imanage.Shared.ViewModels.MarketerSharedModels;

namespace Auth.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ImanageUser> _userManager;
        private readonly RoleManager<ImanageRole> _roleManager;
        private readonly SignInManager<ImanageUser> _signInManager;
       
        private readonly IProducerClient<BusMessage> _producerClient;
        private readonly ILogger<UserService> _logger;
        private readonly IDataProtector _protector;
        private readonly List<ValidationResult> results = new List<ValidationResult>();
        public UserService(UserManager<ImanageUser> userManager, RoleManager<ImanageRole> roleManager,
            
            SignInManager<ImanageUser> signInManager,
            ILogger<UserService> logger,
            IProducerClient<BusMessage> producerClient,
            IDataProtectionProvider provider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
         
            _logger = logger;
            _producerClient = producerClient;
            _protector = provider.CreateProtector("Auth");
        }

        public IEnumerable<SetupUserViewModel> GetAllUsersByType(UserTypes? userTypes)
        {
            var user = _userManager.Users.Where(u => !userTypes.HasValue || u.UserType == userTypes.Value)
                .Select(u => new SetupUserViewModel()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    MiddleName = u.MiddleName,
                    Email = u.Email
                });

            return user;
        }

        public IEnumerable<PermissionViewModel> GetPermissions(int? userRoles)
        {
            var rsp = new ApiResponse<List<PermissionViewModel>>();
            var _perms = ((Permission[])Enum.GetValues(typeof(Permission)));

            var perms = _perms.Where(p => !userRoles.HasValue || p == (Permission)userRoles)
                .Select(p => new PermissionViewModel
                {
                    Id = Convert.ToInt16(p),
                    Name = p.ToString(),
                    Category = PermissionHelper.GetPermissionCategory(p),
                    Description = PermissionHelper.GetPermissionDescription(p)
                });

            return perms;
        }

        public async Task<(List<ValidationResult>, IEnumerable<PermissionViewModel>)> GetUserPermissions(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                results.Add(new ValidationResult("User not found"));
                return (results, Enumerable.Empty<PermissionViewModel>());
            }

            var _perms = await _userManager.GetClaimsAsync(user);

            var perms = _perms.Select(p => new PermissionViewModel
            {
                Id = Convert.ToInt32(p.Value),
                Name = p.Type,
                Category = PermissionHelper.GetPermissionCategory((Permission)Convert.ToInt32(p.Value)),
                Description = PermissionHelper.GetPermissionDescription((Permission)Convert.ToInt32(p.Value))
            });

            return (results, perms);
        }
        public async Task<List<ValidationResult>> RemoveAllUserPermissions(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                results.Add(new ValidationResult("User not found"));
                return results;
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var removeClaims = await _userManager.RemoveClaimsAsync(user, claims);

            if (!removeClaims.Succeeded)
            {
                results.Add(new ValidationResult("Unable to remove all permissions at the moment"));
                return results;
            }

            return results;
        }

        public async Task<List<ValidationResult>> RemoveUserPermission(string userId, string permission)
        {
            var perm = EnumHelper.Parse<Permission>(permission); // convert string to enum

            if (perm == 0)
            {
                results.Add(new ValidationResult($"Permission {perm} does not exist"));
                return results;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                results.Add(new ValidationResult("User not found"));
                return results;
            }

            var claims = await _userManager.GetClaimsAsync(user); // get user claims
            var claimToRmv = claims.FirstOrDefault(a => a.Type == permission.ToString()); // find claim to remove

            // remove claim
            var removeClaim = await _userManager.RemoveClaimAsync(user, claimToRmv);
            if (!removeClaim.Succeeded)
            {
                results.Add(new ValidationResult("Unable to remove all permissions at the moment"));
                return results;
            }

            return results;
        }

        public async Task<List<ValidationResult>> RemovePermissionFromUser(UserPermissionActionViewModel model)
        {
            if (model.UserId == Guid.Empty)
            {
                results.Add(new ValidationResult("Invalid User Id"));
                return results;
            }

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());

            if (user == null)
            {
                results.Add(new ValidationResult("User not found"));
                return results;
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var claimsToRmv = new List<Claim>();

            foreach (var i in model.Permissions)
            {
                var perm = EnumHelper.Parse<Permission>(i);

                if (perm == 0)
                {
                    results.Add(new ValidationResult($"Permission {i} does not exist"));
                    return results;
                }

                claimsToRmv.Add(new Claim(perm.ToString(), Convert.ToInt32(perm).ToString()));
            }

            var removesClaim = await _userManager.RemoveClaimsAsync(user, claimsToRmv);

            if (!removesClaim.Succeeded)
            {
                results.Add(new ValidationResult(removesClaim.Errors.FirstOrDefault().Description));
                return results;
            }

            return results;
        }

        public async Task<List<ValidationResult>> BatchUpdatePersmission(IEnumerable<UserPermissionActionViewModel> permissionList)
        {
            foreach (var i in permissionList)
            {
                if (i.UserId == Guid.Empty) continue;
                var p = await UpdateUserPermissions(i);
            }

            return results;
        }

        public async Task<List<ValidationResult>> UpdateUserPermissions(UserPermissionActionViewModel model)
        {
            if (model.UserId == Guid.Empty)
            {
                results.Add(new ValidationResult("Invalid User Id"));
                return results;
            }

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());

            if (user == null)
            {
                results.Add(new ValidationResult("User not found"));
                return results;
            }

            var claims = await _userManager.GetClaimsAsync(user); // get user claims
            var claimsToAdd = new List<Claim>();
            foreach (var i in model.Permissions)
            {
                var perm = EnumHelper.Parse<Permission>(i);

                if (perm == 0)
                {
                    results.Add(new ValidationResult($"Permission {i} does not exist"));
                    return results;
                }

                var claim = new Claim(perm.ToString(), Convert.ToInt32(perm).ToString());

                if (claims.Any(t => t.Type == claim.Type))
                    continue;

                claimsToAdd.Add(claim);
            }

            //var rmvClaims = await _userManager.RemoveClaimsAsync(user, claims);

            //if (!rmvClaims.Succeeded)
            //{
            //    results.Add(new ValidationResult(rmvClaims.Errors.FirstOrDefault().Description));
            //    return results;
            //}

            var addClaim = await _userManager.AddClaimsAsync(user, claimsToAdd);

            if (!addClaim.Succeeded)
            {
                results.Add(new ValidationResult(addClaim.Errors.FirstOrDefault().Description));
                return results;
            }

            return results;
        }

        public async Task<List<ValidationResult>> UpdateSetupUser(SetupUserViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(userViewModel.User_Id);

            if (user != null && user.UserTypeId.HasValue && user.UserTypeId.Value.ToString() == userViewModel.Id)
            {
                user.LastName = userViewModel.LastName;
                user.FirstName = userViewModel.FirstName;
                user.MiddleName = userViewModel.MiddleName;
                user.Gender = userViewModel.Gender;

                await _userManager.UpdateAsync(user);
            }
            else
            {
                results.Add(new ValidationResult("Couldn't find a user for this request."));
                return results;
            }

        

            return results;
        }

        public async Task<List<ValidationResult>> SetupUser(SetupUserViewModel userViewModel)
        {
            bool isValid = Validator.TryValidateObject(userViewModel, new ValidationContext(userViewModel, null, null), results, false);

            if (!isValid)
                return results;

            Enum.TryParse(typeof(UserTypes), userViewModel.UserType.ToString(), out object userType);

            var user = new ImanageUser()
            {
                UserName = userViewModel.Email,
                LastName = userViewModel.LastName,
                FirstName = userViewModel.FirstName,
                MiddleName = userViewModel.MiddleName,
                Gender = userViewModel.Gender,
                Email = userViewModel.Email,
                UserType = (UserTypes)userType
            };

            Enum.TryParse(typeof(UserRoles), userViewModel.Role.ToString(), out object role);
            var status = await CreateIdentityUser(user);

            userViewModel.Role = (int)role;

            if (status.Any())
                return results;

            userViewModel.User_Id = user.Id.ToString();
            var createResult = await CreateUserType(userViewModel, user, (UserTypes)userType);

            if (!createResult.Any())
            {
                user.UserTypeId = Guid.Parse(userViewModel.UserType_Id);
                await _userManager.UpdateAsync(user);

                var roleName = role.ToString();

                if (!await _roleManager.RoleExistsAsync(roleName))
                    await _roleManager.CreateAsync(new ImanageRole() { Name = roleName });

                await _userManager.AddToRoleAsync(user, roleName);
                await AssignDefaultPermission(user, roleName);

                userViewModel.RoleName = roleName;
            }
            else
            {
                await _userManager.DeleteAsync(user);
            }

            return createResult;
        }

        private async Task<List<ValidationResult>> CreateUserType(SetupUserViewModel userViewModel,
           ImanageUser user,
           UserTypes userType) 
        {
          
            if (userType == UserTypes.LandLord)
            {
                if (userViewModel.Role != (int)UserRoles.LandLord)
                {
                    results.Add(new ValidationResult("Partner can only be assigned basic user role."));
                    await ReverseUserCreated(user);
                    return results;
                }

               
            }

            return results;
        }
        private async Task ReverseUserCreated(ImanageUser user)
        {
            await _userManager.DeleteAsync(user);
        }

        private async Task<List<ValidationResult>> CreateIdentityUser(ImanageUser user)
        {
            string password = Utils.GenerateRandom(6).ToUpper();
            var userResult = await _userManager.CreateAsync(user, password);

            if (!userResult.Succeeded)
            {
                results.Add(new ValidationResult(userResult.Errors.FirstOrDefault().Description));
                return results;
            }

            return results;
        }

        // Todo: switch to mine if Leye doesn't respond
        private async Task AssignDefaultPermission(ImanageUser user, string role)
        {
            //var customerPermission = ((Permission[])Enum.GetValues(typeof(Permission)))
            //        .Where(p => p.GetPermissionCategory() == role)
            //        .Select(item => item);

            var permissionFromRole = PermisionProvider
                                    .GetSystemDefaultRoles()
                                    .Where(x => x.Key == role).SelectMany(x => x.Value);

            await AssignPermissionsToUser(permissionFromRole.ToList(), user.Id);
        }

        public async Task AssignPermissionsToUser(List<Permission> permissions, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return;

            var userClaims = await _userManager.GetClaimsAsync(user);

            if (userClaims.Any())
                await _userManager.RemoveClaimsAsync(user, userClaims);

            var userNewClaims = permissions.Select(p => new Claim(p.ToString(), ((int)p).ToString()));
            _userManager.AddClaimsAsync(user, userNewClaims).Wait();
        }


        public async Task<List<ValidationResult>> RequestPasswordReset(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                results.Add(new ValidationResult($"User with {email} does not exist"));
                return results;
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebUtility.UrlEncode(code);
            var tokenQueryModel = new PasswordResetQueryModel { Email = user.Email, Token = code };
            var tokenQueryModelString = JsonConvert.SerializeObject(tokenQueryModel);
            code = _protector.Protect(tokenQueryModelString);
            await SendMessage("email", BusMessageTypes.USER_MSG, new UserEmailTokenViewModel
            {
                Code = code,
                User = user,
                EmailType = EmailTypeEnum.UserResetPassword
            });
            return results;
        }

        public async Task<List<ValidationResult>> PasswordReset(PasswordResetModel model)
        {
            try
            {
                var passwordResetModelString = "";
                try
                {
                    passwordResetModelString =  _protector.Unprotect(model.Token);

                }catch(Exception e)
                {
                    results.Add(new ValidationResult($"Invalid Token"));
                    return results;
                }
                var passwordResetModel = JsonConvert.DeserializeObject<PasswordResetQueryModel>(passwordResetModelString);
                passwordResetModel.Token = WebUtility.UrlDecode(passwordResetModel.Token);
                var user = await _userManager.FindByEmailAsync(passwordResetModel.Email);
                if (user == null)
                {
                    results.Add(new ValidationResult($"User with {passwordResetModel?.Email} does not exist"));
                    return results;
                }
                if (!user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                }
                if (!user.Activated)
                {
                    user.Activated = true;
                }
                /*if (!user.EmailConfirmed)
                {
                    results.Add(new ValidationResult($"User with {passwordResetModel?.Email} is not yet confirmed"));
                    return results;
                }*/
                /*if (!user.Activated)
                {
                    results.Add(new ValidationResult($"User with {passwordResetModel?.Email} is not yet activated"));
                    return results;
                }*/

                //Update Password
                var response = await _userManager.ResetPasswordAsync(user, passwordResetModel.Token, model.NewPassword);
                if (!response.Succeeded)
                {
                    results.Add(new ValidationResult($"Failed to reset password"));
                    return results;
                }

                await SendMessage("email", BusMessageTypes.USER_MSG, new UserEmailTokenViewModel
                {
                    User = user,
                    EmailType = EmailTypeEnum.PasswordResetSuccess
                });
                return results;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public async Task<List<ValidationResult>> TriggerPhoneNumberConfirmation(PhoneVerificationQueryModel model)
        {
            _logger.Log(LogLevel.Information, "Verifying phone number", model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                results.Add(new ValidationResult($"User with {model.Email} does not exist"));
                return results;
            }

            //This is so that user can change phone number even when it is already confirmed
            /*if(user.PhoneNumberConfirmed && (string.IsNullOrEmpty(model.Phone) || user.PhoneNumber.Contains(model.Phone)))
            {
                results.Add(new ValidationResult($"Phone Number {user.PhoneNumber} already confirmed"));
                return results;
            }*/
            
            if (user.PhoneNumberConfirmed)
            {
                results.Add(new ValidationResult($"Phone Number {user.PhoneNumber} already confirmed"));
                return results;
            }

            if (user.PhoneNumber != model.Phone)
            {
                results.Add(new ValidationResult($"Wrong phone number"));
                return results;
            }

            //await _userManager.SetPhoneNumberAsync(user, model.Phone);
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            await SendMessage("sms", BusMessageTypes.USER_MSG, new UserEmailTokenViewModel
            {
                Code = code,
                User = user,
                EmailType = EmailTypeEnum.UserPhoneActivation
            });
            return results;
        }

        public async Task<List<ValidationResult>> Send2FAToken(ImanageUser user)
        {
            if (user == null)
            {
                results.Add(new ValidationResult($"User not found"));
                return results;
            }

            var isTwoFactoreEnabled = user.TwoFactorEnabled;
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone2FA");
            await SendMessage("sms", BusMessageTypes.USER_MSG, new UserEmailTokenViewModel
            {
                Code = code,
                User = user,
                EmailType = EmailTypeEnum.UserPhoneActivation
            });
            return results;
        }

        public async Task<List<ValidationResult>> VerifyTwoFactorToken(VerifyTwoFactorModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                results.Add(new ValidationResult($"User with {model.Email} does not exist"));
                return results;
            }

            var result = await _userManager.VerifyTwoFactorTokenAsync(user,
                _userManager.Options.Tokens.ChangePhoneNumberTokenProvider, model.Token);
            
            if (!result)
            {
                results.Add(new ValidationResult($"Failed to verify user"));
                return results;
            }

            return results;
        }

        public async Task<List<ValidationResult>> ConfirmEmail(ConfirmationEmailQueryModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                results.Add(new ValidationResult($"User with {model.Email} does not exist"));
                return results;
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);

            if (!result.Succeeded)
            {
                results.Add(new ValidationResult(string.Join(";", result.Errors.Select(x => x.Code))));
                return results;
            }
            //Activate User
            user.Activated = true;
            await _userManager.UpdateAsync(user);

            //Send PasswordReset Email
            await RequestPasswordReset(user.Email);
            return results;
        }

        public async Task<List<ValidationResult>> ConfirmPhoneNumber(ConfirmationEmailQueryModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                results.Add(new ValidationResult($"User with {model.Email} does not exist"));
                return results;
            }

            var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, model.Token, user.PhoneNumber);
            if(result)
            {
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);
            }

            if (!result)
            {
                results.Add(new ValidationResult($"Failed to verify user phoneNumber {user.PhoneNumber ?? ""}"));
                return results;
            }
            return results;
        }

        private async Task<List<ValidationResult>> SendMessage(string messageType, BusMessageTypes msgType, object data)
        {
            try
            {
                await _producerClient.Produce(messageType, new BusMessage
                {
                    BusMessageType = (int)msgType,
                    Data = JsonConvert.SerializeObject(data)
                });
                return results;
            }
            catch (Exception e)
            {
                //TODO Log Error
                _logger.Log(LogLevel.Error, e.Message, e);
                return results;
            }
        }
    }
}
