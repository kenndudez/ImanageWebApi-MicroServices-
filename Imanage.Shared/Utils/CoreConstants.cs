using System;
using System.Collections.Generic;
using System.Text;

namespace Imanage.Shared.Utils
{
    public abstract class CoreConstants
    {
        public const string DateFormat = "dd MMMM, yyyy";
        public const string TimeFormat = "hh:mm tt";
        public const string SystemDateFormat = "dd/MM/yyyy";
        public const string HttpClientNoToken = "client_no_token";
        public const string HttpClientWithToken = "client_with_token";
        public const int MaxBatchsize = 40;
        public const int DefaultPageIndex = 1;
        public const int DefaultPageSize = 10;

        public class Url
        {
            public const string PasswordResetEmail = "messaging/emailtemplates/password-resetcode-email.html";
            public const string AccountActivationEmail = "messaging/emailtemplates/account-email.html";
            public const string BookingSuccessEmail = "messaging/emailtemplates/confirm-email.html";
            public const string BookingAndReturnSuccessEmail = "messaging/emailtemplates/confirm-return-email.html";
            public const string ActivationCodeEmail = "messaging/emailtemplates/activation-code-email.html";
            public const string BookingUnSuccessEmail = "messaging/emailtemplates/failed-email.html";
            public const string RescheduleSuccessEmail = "messaging/emailtemplates/reschedule-success.html";
            public const string AdminHireBookingEmail = "messaging/emailtemplates/hirebooking-admin.html";
            public const string CustomerHireBookingEmail = "messaging/emailtemplates/hirebooking.html";
        }
        public class WorkflowConstants
        {
            public const string CSU = "CSU";
            public const string GMCSU = "GMCSU";
            public const string SI = "SI";
            public const string AUDIT = "AUDIT";
            public const string ES = "ES";
            public const string ZH = "ZH";
            public const string HEADSI = "HEADSI";
        }
    }
}
