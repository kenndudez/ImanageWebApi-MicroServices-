using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Imanage.Shared.ViewModels
{
    public class UserViewModel : BaseViewModel<Guid>
    {
        [Required(ErrorMessage = "Unique Company name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vali phone mumber is required")]
        public string PhoneNumber { get; set; }
        public string PhoneNumber2 { get; set; }
        [Required(ErrorMessage = "Valid email address is required")]
        public string Email { get; set; }
    }
}
