using System;
using System.Windows;
using System.Windows.Input;
using Notifications.Wpf.Command;

namespace Notification.Wpf
{
    public class NotificationContent
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public NotificationType Type { get; set; }
        public NotificationTextTrimType TrimType { get; set; } = NotificationTextTrimType.NoTrim;
        public uint RowsCount { get; set; } = 2;

        #region Left button

        private object _LeftButtonContent = "Ok";

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
                        _LeftButtonContent = "Ok";
                        break;
                    default: _LeftButtonContent = value;
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

        private object _RightButtonContent = "Cancel";

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
                        _RightButtonContent = "Cancel";
                        break;
                    default: _RightButtonContent = value;
                        break;
                }
            }
        }

        /// <summary>
        /// Right button action
        /// </summary>
        public Action RightButtonAction { get; set; }

        #endregion

    }
}
