using MediatR;

namespace Imanage.Shared.EventModels
{
    public class SetupUserEvent: INotification
    {
        public SetupUserEvent(string message)
        {
            Message = message;
        }
        public string Message { get; }
    }

    public class EmailEvent : INotification
    {
        public EmailEvent(string message)
        {
            Message = message;
        }
        public string Message { get; }
    }
    public class SetupMarketerEvent : INotification
    {
        public SetupMarketerEvent(string message)
        {
            Message = message;
        }
        public string Message { get; }
    }
}
