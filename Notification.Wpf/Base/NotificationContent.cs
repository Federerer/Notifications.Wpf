using System;

namespace Notification.Wpf
{
    public class NotificationContent
    {
        /// <summary> Top Title text </summary>
        public string Title { get; set; }
        /// <summary> Body message text </summary>
        public string Message { get; set; }
        /// <summary> Notification type (change color) </summary>
        public NotificationType Type { get; set; }
        /// <summary> Trimming long text if need </summary>
        public NotificationTextTrimType TrimType { get; set; } = NotificationTextTrimType.NoTrim;
        /// <summary> Set rows of message that will show if set Trim </summary>
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

        /// <summary> close message when OnClick to message window </summary>
        public bool CloseOnClick { get; set; } = true;

    }
}
