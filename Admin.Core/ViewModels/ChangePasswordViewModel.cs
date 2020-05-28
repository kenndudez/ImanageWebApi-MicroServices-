using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.ViewModels
{
    public class ChangePasswordViewModel
    {

        [Required(ErrorMessage = "Provide your current password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please set a new password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm new password")]
        [Compare(nameof(NewPassword), ErrorMessage = "Both passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
