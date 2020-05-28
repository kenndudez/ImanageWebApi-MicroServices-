using Auth.Core.Models;
using Auth.Core.Services.Interfaces;
using Imanage.Shared.EF;
using Imanage.Shared.EF.Services;
using Imanage.Shared.Enums;
using Imanage.Shared.Helpers;
using Imanage.Shared.Models;
using Imanage.Shared.PubSub;
using Imanage.Shared.ViewModels.TruckOwnerSharedModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services
{
    public class EstateManagerUserService : Service<Account>, IEstateManagerUserService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ImanageUser> _userManager;
        private readonly IProducerClient<BusMessage> _producerClient;
        private readonly ILogger<EstateManagerUserService> _logger;

        public EstateManagerUserService(IUserService userService,
            UserManager<ImanageUser> userManager,
            ILogger<EstateManagerUserService> logger,
            IProducerClient<BusMessage> producerClient,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
            _producerClient = producerClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ValidationResult>> CreateTruckOwnerUser(EstateManagerSetUpUser model)
        {
            _logger.Log(LogLevel.Debug, "Creating new Truck Owner User", model);
            Enum.TryParse(typeof(UserTypes), model.UserType.ToString(), out object userType);
            try
            {
                var user = new ImanageUser()
                {
                    UserName = model.Email,
                    FirstName = model.Name,
                    MiddleName = model.Name,
                    LastName = model.Name,
                    Gender = 1,
                    Email = model.Email,
                    UserType = (UserTypes)userType,
                    PhoneNumber = model.PhoneNumber
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
                    AccountType = Enums.AccountType.EstateManagementCompany,
                    Status = Enums.AccountStatus.PendingVerification,
                    User_Id = user.Id
                };
                var status2 = await AddAsync(account);
                model.AccountId = account.Id.ToString();
                //Add Claims
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypesHelper.AccountId, account.Id.ToString()));

                //await _producerClient.Produce("new_truck_owner", new BusMessage
                //{
                //    BusMessageType = (int)BusMessageTypes.NEW_TRUCK_OWNER,
                //    Data = JsonConvert.SerializeObject(model)
                //});

                await _userManager.UpdateAsync(user);
                //send Password Reset Email
                await _userService.RequestPasswordReset(user.Email);
                return results;
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, "Failed to create new Truck Owner User", model);
                results.Add(new ValidationResult(ex.Message));
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
            catch (Exception e)
            {
                //TODO Log Error
                results.Add(new ValidationResult(e.Message));
                return results;
            }
        }

        public async Task<List<ValidationResult>> ValidatePEFUserRegistrationModel(EstateManagerSetUpUser model)
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
    }
}
