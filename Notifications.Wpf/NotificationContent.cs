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
        
        public bool LeftButtonVisibility => LeftButtonAction is not null;

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
        public bool RightButtonVisibility => RightButtonAction is not null;

        #region LeftButtonCommand : ICommand - Левая кнопка нажата

        /// <summary>Левая кнопка нажата</summary>
        private ICommand _LeftButtonCommand;

        /// <summary>Левая кнопка нажата</summary>
        public ICommand LeftButtonCommand
        {
            get
            {
                if (LeftButtonVisibility)
                    _LeftButtonCommand = new LamdaCommand(O => LeftButtonAction?.Invoke());
                return _LeftButtonCommand;
            }
        }

        #endregion

        #region RightButtonCommand : ICommand - Правая кнопка нажата

        /// <summary>Правая кнопка нажата</summary>
        private ICommand _RightButtonCommand;

        /// <summary>Правая кнопка нажата</summary>
        public ICommand RightButtonCommand
        {
            get
            {
                if (RightButtonVisibility)
                    _RightButtonCommand = new LamdaCommand(O => RightButtonAction?.Invoke());
                return _RightButtonCommand;
            }
        }


        #endregion
        public NotificationType Type { get; set; }
        public NotificationTextTrimType TrimType { get; set; } = NotificationTextTrimType.NoTrim;
        public uint RowsCount { get; set; } = 2;
    }

    /// <summary>
    /// Способ отображения текста в сообщении
    /// </summary>
    public enum NotificationTextTrimType
    {
        NoTrim,
        Trim,
        Attach,
        AttachIfMoreRows
    }
    /// <summary>
    /// Тип сообщения
    /// </summary>
    public enum NotificationType   
    {
        Information,
        Success,
        Warning,
        Error,
        Notification,
        AttachText
    }
}
