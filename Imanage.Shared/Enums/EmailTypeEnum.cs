using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Imanage.Shared.Enums
{
    public enum EmailTypeEnum
    {
        None = 0,
        UserEmailActivation,
        UserResetPassword,
        UserPhoneActivation,
        PasswordResetSuccess
    }
}
