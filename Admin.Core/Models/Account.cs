using Auth.Core.Enums;
using Imanage.Shared;
using Imanage.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Core.Models
{
    public class Account : AuditedEntity
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string PhoneNumber2 { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string CAC { get; set; }

        public string NIN { get; set; }

        public AccountType AccountType { get; set; }

        public AccountStatus Status { get; set; }

        public Guid User_Id { get; set; }
        [ForeignKey(nameof(User_Id))]
        public ImanageUser User { get; set; }
    }
}
