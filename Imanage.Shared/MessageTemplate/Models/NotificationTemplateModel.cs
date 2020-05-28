using System;
using System.Collections.Generic;
using System.Text;

namespace Imanage.Shared.MessageTemplate
{
    public class NotificationTemplate
    {
        public string ActionUser { get; set; }
        public string Message { get; set; }

        public DateTime DateCreated { get; set; }

        public string URLParams { get; set; }

    }
}
