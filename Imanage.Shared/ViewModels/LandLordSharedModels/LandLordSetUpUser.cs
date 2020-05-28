using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Imanage.Shared.ViewModels.MarketerSharedModels
{
    public class LandLordSetUpUser
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string MarketerId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Email address is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "User type is required")]
        public int UserType { get; set; }
        public bool IsValid { get; set; }
        [Required(ErrorMessage = "CAC number is required")]
        public string CAC { get; set; }
        public string NIN { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Valid phone mumber is required")]
        public string PhoneNumber { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Website { get; set; }
    }
}
