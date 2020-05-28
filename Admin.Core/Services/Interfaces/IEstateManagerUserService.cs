using Imanage.Shared.ViewModels.TruckOwnerSharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services.Interfaces
{
    public interface IEstateManagerUserService
    {
        Task<List<ValidationResult>> CreateTruckOwnerUser(EstateManagerSetUpUser model);
        Task<List<ValidationResult>> ValidatePEFUserRegistrationModel(EstateManagerSetUpUser userViewModel);
    }
}
