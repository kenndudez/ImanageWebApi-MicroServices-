using Auth.Core.Models;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Dafmis.Shared.ViewModels.MarketerSharedModels;
using Imanage.Shared.EF;
using Imanage.Shared.EF.Services;
using Imanage.Shared.Enums;
using Imanage.Shared.Helpers;
using Imanage.Shared.Models;
using Imanage.Shared.PubSub;
using Imanage.Shared.ViewModels.MarketerSharedModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Auth.Core.Services
{
    public class LandLordUserService : Service<Account>, ILandLordUserService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ImanageUser> _userManager;
        private readonly IProducerClient<BusMessage> _producerClient;
        private readonly ILogger<LandLordUserService> _logger;

        public LandLordUserService(IUserService userService, 
            UserManager<ImanageUser> userManager,
            ILogger<LandLordUserService> logger,
            IProducerClient<BusMessage> producerClient,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
            _producerClient = producerClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ValidationResult>> CreateMarketerUser(LandLordSetUpUser model)
        {
            _logger.Log(LogLevel.Debug, "Creating new Marketer User", model);
            Enum.TryParse(typeof(UserTypes), model.UserType.ToString(), out object userType);

            try
            {
                var user = new ImanageUser()
                {
                    UserName = model.Email,
                    LastName = model.Name,
                    FirstName = model.Name,
                    MiddleName = model.Name,
                    Gender = 1,
                    Email = model.Email,
                    UserType = (UserTypes)userType,
                    PhoneNumber = model.PhoneNumber,
                    //TwoFactorEnabled = true
                };
                var status = await CreateIdentityUser(user);
                if (status.Any())
                {
                    return status;
                }
                //Create new Account      
                var account = new Account
                {
                    Name = model.Name,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumber2 = model.PhoneNumber2,
                    Website = model.Website,
                    Email = model.Email,
                    CAC = model.CAC,
                    NIN = model.NIN,
                    AccountType = Enums.AccountType.LandLord,
                    Status = Enums.AccountStatus.PendingVerification,
                    User_Id = user.Id
                };
                var status2 = await AddAsync(account);
                model.Id = user.Id.ToString();
                model.AccountId = account.Id.ToString();
                //Add Claims
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypesHelper.AccountId, account.Id.ToString()));
                //Broadcast new user (accountId)
                /*await _producerClient.Produce("new_marketer", new BusMessage
                {
                    BusMessageType = (int)BusMessageTypes.NEW_MARKETER,
                    Data = JsonConvert.SerializeObject(model)
                });*/
                //Send Email Confirmation
                /*var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebUtility.UrlEncode(code);
                await SendMessage("email", BusMessageTypes.USER_MSG, new UserEmailTokenViewModel
                {
                    Code = code,
                    User = user,
                    EmailType = EmailTypeEnum.UserEmailActivation
                });*/

                //Activate User and Confirm Email
                //user.Activated = true;
                //user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);


                //send Password Reset Email
                await _userService.RequestPasswordReset(user.Email);
                return results;
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, "Failed to create new Marketer User", model);
                results.Add(new ValidationResult(ex.Message));
                model.IsValid = false;
                return results;
            }

        }

        public async Task<List<ValidationResult>> ValidatePEFUserRegistrationModel(LandLordSetUpUser model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    results.Add(new ValidationResult($"User with Email {model.Email} already exists"));
                    model.IsValid = false;
                    return results;
                }
                model.IsValid = true;
                return results;
            }
            catch (Exception e)
            {
                //TODO Log Error
                results.Add(new ValidationResult(e.Message));
                model.IsValid = false;
                return results;
            }
        }

        private async Task<List<ValidationResult>> CreateIdentityUser(ImanageUser user)
        {
            try
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
            catch(Exception e)
            {
                 //TODO Log Error
                results.Add(new ValidationResult(e.Message));
                return results;
            }
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
            catch(Exception e)
            {
                //TODO Log Error
                return results;
            }
        }
    }
}
