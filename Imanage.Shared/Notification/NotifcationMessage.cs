using System;
using System.Collections.Generic;
using System.Text;

namespace Imanage.Shared.Notification
{
    public enum NotificationTarget
    {
        All,
        Group,
        Individual
    }
    
    public class NotificationMessage<T> where T : class
    {
        public NotificationMessage(NotificationTarget targetType)
        {
            TargetType = targetType;
        }
        public NotificationMessage(NotificationTarget targetType, string target)
        {
            TargetType = targetType;
            Target = target;
        }

        public NotificationTarget TargetType { get; set; }

        public string Target { get; set; }
        
        public string Message { get; set; }

        public string Type { get; set; }
        
        public string ActionBy { get; set; }

        public string Recipient { get; set; }

        public T Data { get; set; }
    }
}
