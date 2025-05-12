namespace Project.Management.Domain.Services.Notificator
{
    public interface INotificator
    {
        IEnumerable<Notification> GetErrorNotifications();
        void HandleError(Notification notification);
    }
}
