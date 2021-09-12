using System.Windows.Media;
using Notification.Wpf.Constants;

namespace Notification.Wpf
{
    /// <inheritdoc />
    public class BaseNotificationContent : ICustomizedNotification
    {
        /// <inheritdoc />
        public string Title { get; set; }
        /// <inheritdoc />
        public string Message { get; set; }
        /// <inheritdoc />
        public object Icon { get; set; }

        /// <inheritdoc />
        public Brush Background { get; set; }

        /// <inheritdoc />
        public Brush Foreground { get; set; }
        /// <inheritdoc />
        public NotificationTextTrimType TrimType { get; set; } = NotificationConstants.DefaulTextTrimType;
        /// <inheritdoc />
        public uint RowsCount { get; set; } = NotificationConstants.DefaultRowCounts;
    }
}