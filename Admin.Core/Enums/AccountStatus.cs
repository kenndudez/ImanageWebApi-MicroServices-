using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Enums
{
   public enum AccountStatus
    {
        PendingVerification = 1,
        Verified,
        Blocked
    }
}
