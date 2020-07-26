namespace Notification.Wpf
{
    public class NotificationContent
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public NotificationType Type { get; set; }
    }

    public enum NotificationType   
    {
        Information,
        Success,
        Warning,
        Error,
        Notification
    }
}
