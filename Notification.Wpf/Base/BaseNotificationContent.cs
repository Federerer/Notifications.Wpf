using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Media;
using Notification.Wpf.Base;
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

        /// <inheritdoc />
        public TextContentSettings TitleTextSettings { get; set; } = new()
        {
            FontStyle = FontStyles.Normal,
            FontFamily = new FontFamily(NotificationConstants.FontName),
            FontSize = NotificationConstants.TitleSize,
            FontWeight = FontWeights.Bold,
            TextAlignment = NotificationConstants.TitleTextAlignment,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalTextAlignment = VerticalAlignment.Stretch
        };
        /// <inheritdoc />
        public TextContentSettings MessageTextSettings { get; set; } = new()
        {
            FontStyle = FontStyles.Normal,
            FontFamily = new FontFamily(NotificationConstants.FontName),
            FontSize = NotificationConstants.MessageSize,
            FontWeight = FontWeights.Normal,
            TextAlignment = NotificationConstants.MessageTextAlignment,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalTextAlignment = VerticalAlignment.Stretch
        };
    }
}