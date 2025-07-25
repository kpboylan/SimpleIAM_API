using SimpleIAM_API.Service;
using static SimpleIAM_API.Factory.Enum.FactoryEnum;

namespace SimpleIAM_API.Factory
{
    public class NotificationFactory
    {
        public static INotification CreateNotification(NotificationType type)
        {
            return type switch
            {
                NotificationType.Email => new EmailNotification(),
                NotificationType.Sms => new SmsNotification(),
                _ => throw new NotSupportedException("Notification type not supported.")
            };
        }
    }
}
