using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Auth.Core.Dtos;
using Auth.Core.Models;
using Imanage.Shared.Enums;
using Imanage.Shared.ViewModels;

namespace Auth.Core.ViewModels
{
    public class SetupUserViewModel : BaseViewModel
    {
        public string PartnerAddress { get; set; }
        public string PartnerContactEmail { get; set; }
        public string Partner_Id { get; set; }
        public string PartnerName { get; set; }
        public string User_Id { get; set; }
        public string UserType_Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string DepartmentTitle { get; set; }
        public string Department_Id { get; set; }
        public string Unit_Id { get; set; }
        public string UnitTitle { get; set; }
        public int Gender { get; set; }
        public string GenderTitle { get; set; }
        [EmailAddress(ErrorMessage = "Email address isn't valid")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "User type is required")]
        public int UserType { get; set; }
        public int Role { get; set; }
        public string RoleName { get; set; }

        public string JurisdicationTitle { get; set; }
        public int Jurisdication { get; set; }
        public string Office_Id { get; set; }
        public string OfficeTitle { get; set; }

        public static explicit operator SetupUserViewModel(UserDto source)
        {
            var destination = new SetupUserViewModel();
            destination.Id = source.Id.ToString();
            destination.LastName = source.LastName;
            destination.FirstName = source.FirstName;
            destination.MiddleName = source.MiddleName;
            destination.Email = source.Email;
            destination.RoleName = source.RoleName;
            destination.TotalCount = source.TotalCount;
            return destination;
        }


        public override IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (this.UserType_Id == Guid.Empty.ToString() && !Guid.TryParse(this.UserType_Id, out Guid _))
            {
                yield return new ValidationResult("User type id isn't valid.");
            }

            if (!string.IsNullOrEmpty(this.Unit_Id) && !Guid.TryParse(this.Unit_Id, out Guid _))
            {
                yield return new ValidationResult("Unit id isn't valid.");
            }

            if (!string.IsNullOrEmpty(this.Office_Id) && !Guid.TryParse(this.Office_Id, out Guid _))
            {
                yield return new ValidationResult("Office id isn't valid.");
            }

            if (this.Gender != 1 && this.Gender != 2)
            {
                yield return new ValidationResult("Gender isn't valid user either 0 (Male) or 1 (Female)");
            }

            base.Validate(context);
        }
    }
}
