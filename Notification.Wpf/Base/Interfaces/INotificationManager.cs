using System;
using System.Windows.Media;
using Notification.Wpf.Classes;

namespace Notification.Wpf
{
    /// <summary>
    /// Notification manager for popup messages
    /// </summary>
    public interface INotificationManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">window content</param>
        /// <param name="areaName">window are where show notification</param>
        /// <param name="expirationTime">time after which the window will disappear</param>
        /// <param name="onClick">what should happen when clicking on the window</param>
        /// <param name="onClose">what should happen when window closing</param>
        /// <param name="CloseOnClick">close window after clicking</param>
        void Show(object content, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null, Action onClose = null, bool CloseOnClick = true);
        /// <summary>
        /// Show message
        /// </summary>
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
        void Show(string title, string message, NotificationType type, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null,
            Action onClose = null, Action LeftButton = null, string LeftButtonText = null, Action RightButton = null, string RightButtonText = null,
            NotificationTextTrimType trim = NotificationTextTrimType.NoTrim, uint RowsCountWhenTrim = 2, bool CloseOnClick = true);

        /// <summary> Show ProgressBar </summary>
        /// <param name="Title">Title of window</param>
        /// <param name="ShowProgress">Show or not progress status</param>
        /// <param name="ShowCancelButton">Show Cancel button or not</param>
        /// <param name="areaName">window are where show notification</param>
        /// <param name="TrimText">trim text if it is longer than the number of visible lines</param>
        /// <param name="DefaultRowsCount">Base number of rows when trims</param>
        /// <param name="BaseWaitingMessage">Timeout message, set null if you do not want to see time in bar progress</param>
        /// <param name="IsCollapse">Start progress bar in collapsed form</param>
        /// <param name="TitleWhenCollapsed">When bar collapsed - will show title or message</param>
        /// <param name="background">popup window background</param>
        /// <param name="foreground">popup window foreground</param>
        /// <param name="progressColor">progress foreground</param>
        /// <param name="icon">progress icon</param>
        NotifierProgress<(double? value, string message, string title, bool? showCancel)> ShowProgressBar(
            string Title = null,
            bool ShowCancelButton = true,
            bool ShowProgress = true,
            string areaName = "",
            bool TrimText = false,
            uint DefaultRowsCount = 1U,
            string BaseWaitingMessage = "Calculation time",
            bool IsCollapse = false,
            bool TitleWhenCollapsed = true,
            Brush background = null,
            Brush foreground = null,
            Brush progressColor = null,
            object icon = default);

        /// <summary> Show ProgressBar </summary>
        /// <param name="content">Base content settings</param>
        /// <param name="ShowProgress">Show or not progress status</param>
        /// <param name="ShowCancelButton">Show Cancel button or not</param>
        /// <param name="areaName">window are where show notification</param>
        /// <param name="BaseWaitingMessage">Timeout message, set null if you do not want to see time in bar progress</param>
        /// <param name="IsCollapse">Start progress bar in collapsed form</param>
        /// <param name="TitleWhenCollapsed">When bar collapsed - will show title or message</param>
        /// <param name="progressColor">progress line color</param>
        NotifierProgress<(double? value, string message, string title, bool? showCancel)> ShowProgressBar(
            ICustomizedNotification content,
            bool ShowCancelButton = true,
            bool ShowProgress = true,
            string areaName = "",
            string BaseWaitingMessage = "Calculation time",
            bool IsCollapse = false,
            bool TitleWhenCollapsed = true,
            Brush progressColor = null);

    }
}