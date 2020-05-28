using Dafmis.Shared.ViewModels;
using Dafmis.Shared.ViewModels.MarketerSharedModels;
using Imanage.Shared.ViewModels.MarketerSharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services.Interfaces
{
    public interface ILandLordUserService
    {
        Task<List<ValidationResult>> CreateMarketerUser(LandLordSetUpUser model);
        Task<List<ValidationResult>> ValidatePEFUserRegistrationModel(LandLordSetUpUser userViewModel);
    }
}
