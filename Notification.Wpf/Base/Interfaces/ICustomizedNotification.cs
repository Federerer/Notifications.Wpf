using System.Windows.Media;

namespace Notification.Wpf
{
    /// <summary>
    /// Customized notification
    /// </summary>
    public interface ICustomizedNotification : INotificationBase
    {
        /// <summary> icon in left bar side </summary>
        public object Icon { get; set; }
        /// <summary> Notification background </summary>
        public Brush Background { get; set; }
        /// <summary> Text foreground </summary>
        public Brush Foreground { get; set; }
        /// <summary> Trimming long text if need </summary>
        public NotificationTextTrimType TrimType { get; set; }
        /// <summary> Set rows of message that will show if set Trim </summary>
        public uint RowsCount { get; set; }

    }
}