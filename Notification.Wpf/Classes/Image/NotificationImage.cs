using System.Windows.Media;

namespace Notification.Wpf.Classes
{
    /// <summary> Image for notification message </summary>
    public class NotificationImage
    {
        /// <summary> Image source </summary>
        public ImageSource Source { get; set; }
        /// <summary> Image position in view </summary>
        public ImagePosition Position { get; set; }
    }
}
