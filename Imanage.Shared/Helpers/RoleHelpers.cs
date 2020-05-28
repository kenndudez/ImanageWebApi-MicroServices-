using System;

namespace Imanage.Shared.Helpers
{
    public static class RoleHelpers
    {
        public static Guid SYS_ADMIN_ID() => Guid.Parse("a1b6b6b0-0825-4975-a93d-df3dc86f8cc7");
        public const string SYS_ADMIN = nameof(SYS_ADMIN);

        public static Guid SUPER_ADMIN_ID() => Guid.Parse("0718ddef-4067-4f29-aaa1-98c1548c1807");
        public const string SUPER_ADMIN = nameof(SUPER_ADMIN);

        public static Guid LandLord_ID() => Guid.Parse("8b93d395-a71a-4620-9352-5b9e6b3b6045");
        public const string LandLord = nameof(LandLord);

        public static Guid Admin_User_ID() => Guid.Parse("02bde570-4aa8-4c60-a462-07154aa69520");
        public const string Admin_User = nameof(Admin_User);

        public static Guid Estate_Manager_ID() => Guid.Parse("c586e0ba-64a9-427b-8888-6f16755e5d9d");
        public const string Estate_Manager = nameof(Estate_Manager);

        public static Guid BASIC_ID() => Guid.Parse("880fd08d-044c-4068-b4e4-84c970344b3a");
        public const string BASIC = nameof(BASIC);

        public static Guid GENERAL_ID() => Guid.Parse("16d9503e-5322-4126-90ef-34eecf3d5a46");
        public const string GENERAL = nameof(GENERAL);


    }
}
