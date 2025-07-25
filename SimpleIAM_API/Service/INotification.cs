namespace SimpleIAM_API.Service
{
    public interface INotification
    {
        void Send(string to, string message);
    }
}
