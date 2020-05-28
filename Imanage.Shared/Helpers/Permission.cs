using System.Collections.Generic;
using System.ComponentModel;

namespace Imanage.Shared.Helpers
{
    public enum Permission
    {
        /// <summary>
        /// Basic User Delete
        /// </summary>
        [Category(RoleHelpers.LandLord), Description(@"Dashboard")]
        BSU_1 = 001,
        [Category(RoleHelpers.LandLord), Description(@"dhfhdfhdfd dfdf")]
        BSU_2 = 002,


        //DFMIS SUPER ADMIN ROLE
        [Category(RoleHelpers.SUPER_ADMIN), Description(@"Creates and manages other super admin user")]
        SPA_1 = 401,
        [Category(RoleHelpers.SUPER_ADMIN), Description(@"Assign users to role")]
        SPA_2 = 402,
        [Category(RoleHelpers.SUPER_ADMIN), Description(@"Edit users to role")]
        SPA_3 = 403,
        [Category(RoleHelpers.SUPER_ADMIN), Description(@"Manage users")]
        SPA_4 = 404,
        [Category(RoleHelpers.SUPER_ADMIN), Description(@"Create users")]
        SPA_5 = 405,
        [Category(RoleHelpers.SUPER_ADMIN), Description(@"Edit users")]
        SPA_6 = 406,
        [Category(RoleHelpers.SUPER_ADMIN), Description(@"Can assign or remove permission")]
        SPA_7 = 407,
        //DFMIS SYS_ADMIN ROLE
        [Category(RoleHelpers.SYS_ADMIN), Description(@"Manage All IMANAGE Users")]
        SYS_1 = 501,

        //Newly added groups
        //[Description("UI COMPONENT MENU")]
        SEARCH = 1001,

        TABLE_VIEW = 1002,
        TABLE_ACTION = 1003,
        TABLE_BULK_ACTION = 1004,

        TABLE_2_VIEW = 1005,
        TABLE_2_ACTION = 1006,
        TABLE_2_BULK_ACTION = 1007,

        DOCUMENT_VIEWER = 1008,
        DOCUMENT_VIEW_ACTIONS = 1009,

        DASHBOARD_VIEWER = 1010,
        DASHBOARD_VIEW_DETAILS = 1011,

        NAV_INFO = 1012,

        //[Description("MISCELLANEOUS")]
        SYSTEM_VALIDATION = 1051,
        REQUEST_ID = 1052,
        GENERAL_INFO = 1053,
        VIEW_APPROVALS = 1054,

        //[Description("ROLE_SPECIFICS")] 
        //For Front End Page Access only
        SYS_ADMIN_PERM = 2001,
        SUPER_ADMIN_PERM = 2002,
        LANDLORD_PERM = 2003,
        ESTATE_MANAGER_PERM = 2004,
        APPROVE_MARKETER_FILE = 2005,
        ASSIGN_USER_TO_ROLE = 2006,
    }

    public static class PermisionProvider
    {
        public static Dictionary<string, IEnumerable<Permission>> GetSystemDefaultRoles()
        {
            return new Dictionary<string, IEnumerable<Permission>>
            {
                    {  RoleHelpers.SYS_ADMIN, new Permission []{
                       
                    
                        //Newly Added
                        Permission.SEARCH,
                        Permission.TABLE_VIEW,
                        Permission.TABLE_ACTION,
                        Permission.TABLE_BULK_ACTION,

                        Permission.TABLE_2_VIEW,
                        Permission.TABLE_2_ACTION,
                        Permission.TABLE_2_BULK_ACTION,

                        Permission.DOCUMENT_VIEWER,
                        Permission.DOCUMENT_VIEW_ACTIONS,

                        Permission.DASHBOARD_VIEWER,
                        Permission.DASHBOARD_VIEW_DETAILS,

                        Permission.NAV_INFO,

                        Permission.SYSTEM_VALIDATION,
                        Permission.REQUEST_ID,
                        Permission.GENERAL_INFO,
                        Permission.VIEW_APPROVALS,
                        Permission.SYS_ADMIN_PERM,

                        }
                    },
                    {    RoleHelpers.SUPER_ADMIN, new Permission []{
                        Permission.LANDLORD_PERM,
                        Permission.GENERAL_INFO,
                        Permission.ESTATE_MANAGER_PERM,
                        Permission.SYSTEM_VALIDATION,
                        Permission.DASHBOARD_VIEWER,
                       // Permission.MARKETER_REQUEST_ES

                        //Newly Added
                        Permission.SEARCH,
                        Permission.TABLE_VIEW,
                        Permission.TABLE_ACTION,
                        Permission.TABLE_BULK_ACTION,

                        Permission.TABLE_2_VIEW,
                        Permission.TABLE_2_ACTION,
                        Permission.TABLE_2_BULK_ACTION,

                        Permission.DOCUMENT_VIEWER,
                        Permission.DOCUMENT_VIEW_ACTIONS,

                        Permission.DASHBOARD_VIEWER,
                        Permission.DASHBOARD_VIEW_DETAILS,

                        Permission.NAV_INFO,

                        Permission.SYSTEM_VALIDATION,
                        Permission.REQUEST_ID,
                        Permission.GENERAL_INFO,
                        Permission.VIEW_APPROVALS,
                        Permission.SUPER_ADMIN_PERM,
                         }
                    },
                    {    RoleHelpers.LandLord, new Permission []{
                            Permission.LANDLORD_PERM,

                         }
                    },
                    {    RoleHelpers.Admin_User, new Permission []{
                        Permission.LANDLORD_PERM,
                        Permission.GENERAL_INFO,
                        Permission.ESTATE_MANAGER_PERM,
                        Permission.SYSTEM_VALIDATION,
                        Permission.DASHBOARD_VIEWER,

                            //Newly Added
                            Permission.SEARCH,
                            Permission.TABLE_VIEW,
                            Permission.TABLE_ACTION,
                            Permission.TABLE_BULK_ACTION,

                            Permission.TABLE_2_VIEW,
                            Permission.TABLE_2_ACTION,
                            Permission.TABLE_2_BULK_ACTION,

                            Permission.DOCUMENT_VIEWER,
                            Permission.DOCUMENT_VIEW_ACTIONS,

                            Permission.DASHBOARD_VIEWER,
                            Permission.DASHBOARD_VIEW_DETAILS,

                            Permission.NAV_INFO,

                            Permission.SYSTEM_VALIDATION,
                            Permission.REQUEST_ID,
                            Permission.GENERAL_INFO,
                            Permission.VIEW_APPROVALS,
                            
                         }
                    },
                   
                    {    RoleHelpers.Estate_Manager, new Permission []{
                           // Permission.CSU_1,
                           // Permission.CSU_2,
                          //  Permission.MARKETER_REQUEST,
                             Permission.ESTATE_MANAGER_PERM,
                            Permission.ESTATE_MANAGER_PERM,
                           // Permission.MARKETER_REQUEST_LIST,
                           // Permission.OUTLET_REQUEST,
                           /// Permission.OUTLET_REQUEST_LIST,
                           // Permission.VIEW_MARKETER_REQUEST_DETAILS,
                         

                            //Newly Added
                            Permission.SEARCH,
                            Permission.TABLE_VIEW,
                            Permission.TABLE_ACTION,
                            Permission.TABLE_BULK_ACTION,

                            Permission.TABLE_2_VIEW,
                            Permission.TABLE_2_ACTION,
                            Permission.TABLE_2_BULK_ACTION,

                            Permission.DOCUMENT_VIEWER,
                            Permission.DOCUMENT_VIEW_ACTIONS,

                            Permission.DASHBOARD_VIEWER,
                            Permission.DASHBOARD_VIEW_DETAILS,

                            Permission.NAV_INFO,

                            Permission.SYSTEM_VALIDATION,
                            Permission.REQUEST_ID,
                            Permission.GENERAL_INFO,
                            Permission.VIEW_APPROVALS,
                           
                         }
                    },
                   
            };
        }
    }
}