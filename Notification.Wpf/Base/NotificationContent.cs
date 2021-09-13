using System;
using Notification.Wpf.Classes;
using Notification.Wpf.Constants;

namespace Notification.Wpf
{
    /// <summary> Message </summary>
    public class NotificationContent : BaseNotificationContent, INotification
    {
        ///<inheritdoc/>
        public NotificationImage Image { get; set; }

        ///<inheritdoc/>
        public NotificationType Type { get; set; }

        #region Left button

        private object _LeftButtonContent = NotificationConstants.DefaultLeftButtonContent;

        ///<inheritdoc/>
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

        ///<inheritdoc/>
        public Action LeftButtonAction { get; set; }

        #endregion

        #region RightButton

        private object _RightButtonContent = NotificationConstants.DefaultRightButtonContent;

        ///<inheritdoc/>
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

        ///<inheritdoc/>
        public Action RightButtonAction { get; set; }

        #endregion

        ///<inheritdoc/>
        public bool CloseOnClick { get; set; } = true;

    }
}
