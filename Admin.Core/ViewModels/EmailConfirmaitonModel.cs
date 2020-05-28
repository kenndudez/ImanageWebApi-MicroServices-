using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels
{
    public class ConfirmationEmailQueryModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class PhoneVerificationQueryModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class PasswordResetModel
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class PasswordResetQueryModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class VerifyTwoFactorModel
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMachine { get; set; }
    }
}
