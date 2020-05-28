using Imanage.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imanage.Shared.Notification
{
    // what if other paramters define a group like users that use mobile devices for verification
    public  class HubGroup
    {
        public string Name { get; set; }

        public List<Permission> AllowedPermissions { get; set; }
       
    }
}
