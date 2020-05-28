
using Imanage.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imanage.Shared.Notification
{
    public static class HubGroupHelper
    {
        public static readonly List<HubGroup> allowedGroups = new List<HubGroup>() {
                new HubGroup{Name = "Invistigation" , AllowedPermissions = new List<Permission>{
                    Permission.APPROVE_MARKETER_FILE,
                    Permission.ASSIGN_USER_TO_ROLE
                }
                }
        };

        public static bool ValidateGroupExists(string group)
        {
            return allowedGroups.Select(x=> x.Name).Contains(group);
        }

        public static IEnumerable<string> GetUserGroups(List<Permission> permissions) {
            return allowedGroups.Where(x => x.AllowedPermissions.Any(y => permissions.Contains(y))).Select(x => x.Name).Distinct();
        }
    }
}
