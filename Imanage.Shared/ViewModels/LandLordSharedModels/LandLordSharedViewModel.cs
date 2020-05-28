using System;
using System.Collections.Generic;
using System.Text;

namespace Imanage.Shared.ViewModels.MarketerSharedModels
{
    public class LandLordSharedViewModel : BaseViewModel<Guid>
    {
        public string Name { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public string PhoneNumber2 { get; set; }
        public string Email { get; set; }

        public string Website { get; set; }
        public string CAC { get; set; }

        public string NIN { get; set; }

        public string FormerName { get; set; }

        public string FormerDepot { get; set; }
        public string StateId { get; set; }
        public string City { get; set; }

        public string Status { get; set; }

        public int SignatoryInstruction { get; set; }
    }
}
