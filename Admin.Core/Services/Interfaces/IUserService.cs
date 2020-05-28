using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Auth.Core.ViewModels;
using Dafmis.Shared.ViewModels.MarketerSharedModels;

using Dafmis.Shared.ViewModels;
using Imanage.Shared.ViewModels;
using Imanage.Shared.Enums;
using Imanage.Shared.Helpers;
using Imanage.Shared.Models;

namespace Auth.Core.Services.Interfaces
{
    public interface IUserService
    {
        
        IEnumerable<SetupUserViewModel> GetAllUsersByType(UserTypes? userTypes);
        IEnumerable<PermissionViewModel> GetPermissions(int? userRoles);
        Task<(List<ValidationResult>, IEnumerable<PermissionViewModel>)> GetUserPermissions(string userId);
        Task<List<ValidationResult>> RemoveAllUserPermissions(string userId);
        Task<List<ValidationResult>> RemoveUserPermission(string userId, string permission);
        Task<List<ValidationResult>> RemovePermissionFromUser(UserPermissionActionViewModel model);
        Task<List<ValidationResult>> BatchUpdatePersmission(IEnumerable<UserPermissionActionViewModel> permissionList);
        Task<List<ValidationResult>> UpdateUserPermissions(UserPermissionActionViewModel model);
       
        Task<List<ValidationResult>> SetupUser(SetupUserViewModel userViewModel);
        Task<List<ValidationResult>> UpdateSetupUser(SetupUserViewModel model);
        Task AssignPermissionsToUser(List<Permission> permissions, Guid userId);

        Task<List<ValidationResult>> RequestPasswordReset(string email);
        Task<List<ValidationResult>> PasswordReset(PasswordResetModel model);
        Task<List<ValidationResult>> ConfirmEmail(ConfirmationEmailQueryModel model);
        Task<List<ValidationResult>> TriggerPhoneNumberConfirmation(PhoneVerificationQueryModel model);
        Task<List<ValidationResult>> ConfirmPhoneNumber(ConfirmationEmailQueryModel model);
        Task<List<ValidationResult>> Send2FAToken(ImanageUser user);
        Task<List<ValidationResult>> VerifyTwoFactorToken(VerifyTwoFactorModel model);
    }
}
