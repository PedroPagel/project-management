namespace Project.Management.Domain.Services.Notificator
{
    public class Notification(string message, int statusCodes)
    {
        public DateTime TimeStamp { get; } = DateTime.Now;
        public string Message { get; } = message;
        public int StatusCode { get; } = statusCodes;
    }
}
