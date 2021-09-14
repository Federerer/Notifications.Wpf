using System;
using Notification.Wpf.Classes;

namespace Notification.Wpf
{
    /// <summary>
    /// Notification template
    /// </summary>
    public interface INotification: ICustomizedNotification
    {
        /// <summary> Image </summary>
        public NotificationImage Image { get; set; }
        /// <summary> Notification type (change color) </summary>
        NotificationType Type { get; set; }

        /// <summary> close message when OnClick to message window </summary>
        public bool CloseOnClick { get; set; }

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

    }
}