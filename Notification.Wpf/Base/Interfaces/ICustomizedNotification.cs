using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Media;
using Notification.Wpf.Base;

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

        #region Text settings
        /// <summary> Title text style settings </summary>
        public TextContentSettings TitleTextSettings { get; set; }
        /// <summary> Message text style settings </summary>
        public TextContentSettings MessageTextSettings { get; set; }
        #endregion

    }
}