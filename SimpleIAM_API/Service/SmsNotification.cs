namespace SimpleIAM_API.Service
{
    public class SmsNotification : INotification
    {
        public void Send(string to, string message)
        {
            Console.WriteLine($"SMS sent to {to}: {message}");
        }
    }
}
