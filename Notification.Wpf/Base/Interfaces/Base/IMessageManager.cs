using System;

namespace Notification.Wpf.Base.Interfaces.Base
{
    /// <summary> Message manager </summary>
    public interface IMessageManager
    {
        /// <summary> Show any content </summary>
        /// <param name="content">window content</param>
        /// <param name="areaName">window are where show notification</param>
        /// <param name="expirationTime">time after which the window will disappear</param>
        /// <param name="onClick">what should happen when clicking on the window</param>
        /// <param name="onClose">what should happen when window closing</param>
        /// <param name="CloseOnClick">close window after clicking</param>
        /// <param name="ShowXbtn">Show X (close) btn</param>
        void Show(object content, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null, Action onClose = null, bool CloseOnClick = true, bool ShowXbtn = true);

        /// <summary> Show message </summary>
        /// <param name="title">window title</param>
        /// <param name="message">Message in window</param>
        /// <param name="type">Window type</param>
        /// <param name="areaName">window are where show notification</param>
        /// <param name="expirationTime">time after which the window will disappear</param>
        /// <param name="onClick">what should happen when clicking on the window</param>
        /// <param name="onClose">what should happen when window closing</param>
        /// <param name="LeftButton">what should happen when clicking on the left button (if null - button not visible)</param>
        /// <param name="LeftButtonText">what should be written on the left button</param>
        /// <param name="RightButton">what should happen when clicking on the right button (if null - button not visible)</param>
        /// <param name="RightButtonText">what should be written on the right button</param>
        /// <param name="trim">trim text if it is longer than the number of visible lines</param>
        /// <param name="RowsCountWhenTrim">Base number of rows when trims</param>
        /// <param name="CloseOnClick">close window after clicking</param>
        /// <param name="TitleSettings">Настройки отображения Title</param>
        /// <param name="MessageSettings">Настройки отображения сообщения</param>
        /// <param name="ShowXbtn">Show X (close) btn</param>
        void Show(string title, string message, NotificationType type, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null,
            Action onClose = null, Action LeftButton = null, string LeftButtonText = null, Action RightButton = null, string RightButtonText = null,
            NotificationTextTrimType trim = NotificationTextTrimType.NoTrim,
            uint RowsCountWhenTrim = 2,
            bool CloseOnClick = true,
            TextContentSettings TitleSettings = null, TextContentSettings MessageSettings = null, bool ShowXbtn = true);

    }
}
