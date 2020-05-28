
using Imanage.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imanage.Shared.Models
{
    [Table(nameof(ImanageUser))]
    public class ImanageUser :  IdentityUser<Guid>
    {
        public ImanageUser()
        {
            Id = Guid.NewGuid();
            CreatedOnUtc = DateTime.UtcNow;
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Unit { get; set; }
        public int Gender { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {

                return $"{LastName} {FirstName}";
            }
        }

        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool Activated { get; set; }
        public bool IsDeleted { get; set; }
        public UserTypes UserType { get; set; }
        public Guid? UserTypeId { get; set; }
    }

    public class ImanageUserClaim : IdentityUserClaim<Guid>
    {
    }

    public class ImanageUserLogin : IdentityUserLogin<Guid>
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }

    public class ImanageRole : IdentityRole<Guid>
    {
        public ImanageRole()
        {
            Id = Guid.NewGuid();
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }
    }

    public class ImanageUserRole : IdentityUserRole<Guid>
    {
    }

    public class ImanageRoleClaim : IdentityRoleClaim<Guid>
    {
    }

    public class ImanageUserToken : IdentityUserToken<Guid>
    {
    }
}
