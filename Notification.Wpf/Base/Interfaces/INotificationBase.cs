namespace Notification.Wpf
{
    /// <summary>
    /// Base notification
    /// </summary>
    public interface INotificationBase
    {
        /// <summary> Top Title text </summary>
        public string Title { get; set; }
        /// <summary> Body message text </summary>
        public string Message { get; set; }

    }
}