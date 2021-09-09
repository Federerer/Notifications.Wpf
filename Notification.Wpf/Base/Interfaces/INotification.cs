using System;
using System.Windows.Media;

namespace Notification.Wpf
{
    public interface INotification: INotificationBase
    {
        /// <summary> Notification type (change color) </summary>
        public NotificationType Type { get; set; }

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

        #region Left button

        /// <summary>
        /// left button content
        /// </summary>
        public object LeftButtonContent { get; set; }

        /// <summary>
        /// Left button action
        /// </summary>
        public Action LeftButtonAction { get; set; }

        #endregion

        #region RightButton

        /// <summary>
        /// Right button content
        /// </summary>
        public object RightButtonContent { get; set; }

        /// <summary>
        /// Right button action
        /// </summary>
        public Action RightButtonAction { get; set; }

        #endregion

        /// <summary> close message when OnClick to message window </summary>
        public bool CloseOnClick { get; set; }

    }
}