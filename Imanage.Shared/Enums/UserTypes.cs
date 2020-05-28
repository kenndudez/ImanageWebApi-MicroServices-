using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.Enums
{
    public enum UserTypes
    {
       
        [Description("Estate Manager")]
        Estate_Manager,
        [Description("LandLord")]
        LandLord,
        [Description("Admin")]
        Admin
    }
}