using System.ComponentModel;

namespace Imanage.Shared.Enums
{
    public enum IdentityType
    {
        None = 0,
        [Description("Driver Licence")]
        Driver_Licence,
        [Description("National ID")]
        National_Id,
        [Description("International Passport")]
        International_Passport,
        [Description("Company ID Card")]
        Company_ID_Card
    }
}
