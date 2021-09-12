using System;
using Notification.Wpf.Constants;

namespace Notification.Wpf
{
    /// <summary> Message </summary>
    public class NotificationContent : BaseNotificationContent, INotification
    {
        /// <summary> Notification type (change color) </summary>
        public NotificationType Type { get; set; }

        #region Left button

        private object _LeftButtonContent = NotificationConstants.DefaultLeftButtonContent;

        /// <summary>
        /// left button content
        /// </summary>
        public object LeftButtonContent
        {
            get => _LeftButtonContent;
            set
            {
                switch (value)
                {
                    case string button_name when string.IsNullOrWhiteSpace(button_name):
                    case null:
                        _LeftButtonContent = NotificationConstants.DefaultLeftButtonContent;
                        break;
                    default:
                        _LeftButtonContent = value;
                        break;
                }
            }
        }

        /// <summary>
        /// Left button action
        /// </summary>
        public Action LeftButtonAction { get; set; }

        #endregion

        #region RightButton

        private object _RightButtonContent = NotificationConstants.DefaultRightButtonContent;

        /// <summary>
        /// Right button content
        /// </summary>
        public object RightButtonContent
        {
            get => _RightButtonContent;
            set
            {
                switch (value)
                {
                    case string button_name when string.IsNullOrWhiteSpace(button_name):
                    case null:
                        _RightButtonContent = NotificationConstants.DefaultRightButtonContent;
                        break;
                    default:
                        _RightButtonContent = value;
                        break;
                }
            }
        }

        /// <summary>
        /// Right button action
        /// </summary>
        public Action RightButtonAction { get; set; }

        #endregion

        /// <summary> close message when OnClick to message window </summary>
        public bool CloseOnClick { get; set; } = true;

    }
}
