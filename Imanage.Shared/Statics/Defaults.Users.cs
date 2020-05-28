using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.Statics
{
    public partial class Defaults
    {
        public const string SysUserEmail = "system@imanager.com";
        public static readonly Guid SysUserId = Guid.Parse("1989883f-4f99-43bf-a754-239bbbfec00b");
        public const string SysUserMobile = "08062066851";

        public const string SuperAdminEmail = "superadmin@imanage.com";
        public static readonly Guid SuperAdminId = Guid.Parse("3fb897c8-c25d-4328-9813-cb1544369fba");
        public const string SuperAdminMobile = "08062066851";

        public static Guid AdminId = Guid.Parse("973AF7A9-7F18-4E8B-ACD3-BC906580561A");
        public const string AdminEmail = "admin@imanage.com";
        public const string AdminMobile = "08062066851";

        public static Guid AdminUserId = Guid.Parse("5bcb4f11-3177-4905-bff8-fa645d0e055e");
        public const string AdminUserEmail = "adminuser@imanage.com";
        public const string AdminUserMobile = "08062066851";

        public static readonly Guid BasicUserId = Guid.Parse("129712e3-9214-4dd3-9c03-cfc4eb9ba979");
        public const string BasicUserMobile = "08062066851";
        public const string BasicUserEmail = "basic@imanage.com";

        public static readonly Guid EstateManagerUserId = Guid.Parse("3d26739c-e070-4e99-8844-6a2e4c2bc216");
        public const string EstateManagerUserMobile = "08062066851";
        public const string EstateManagerUserEmail = "em@imanage.com";
        public const string EstateManagerUserFirstName = "Adekunle";
        public const string EstateManagerUserLastName = "Gold";

        public static readonly Guid LandLordUserId = Guid.Parse("6ff03683-f832-4288-97b3-a79be8c6549c");
        public const string LandLordUserMobile = "08062066851";
        public const string LandLordUserEmail = "LandLord@imanage.com";
        public const string LandLordUserFirstName = "Oga";
        public const string LandLordUserLastName = "Gboro";

        public static readonly Guid GeneralUserId = Guid.Parse("30349956-8a58-40e2-a06e-72073e1b0174");
        public const string GeneralUserMobile = "08062066851";
        public const string GeneralUserEmail = "si@dafmis.com";
        public const string GeneralUserFirstName = "Inspector";
        public const string GeneralUserLastName = "Gadget";

    }
}
