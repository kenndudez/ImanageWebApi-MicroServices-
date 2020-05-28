using System;
using Imanage.Shared.Dtos;

namespace Auth.Core.Dtos
{
    public class PefUserDto: BaseDto
    {
        public Guid User_Id { get; set; }
        public Guid? Unit_Id { get; set; }
        public string UnitTitle { get; set; }
        public Guid? Office_Id { get; set; }
        public string OfficeTitle { get; set; }
        public Guid? UserTypeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        //public int? UserType {get;set;}
        public string RoleName { get; set; }
        public int Jurisdication { get; set; }
    }
}
