using System.Windows.Media;
using Notification.Wpf.Classes;

namespace Notification.Wpf.Base.Interfaces.Base
{
    /// <summary> Progress message manager </summary>
    public interface IProgressManager
    {
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
        /// <param name="TitleSettings">Настройки отображения Title</param>
        /// <param name="MessageSettings">Настройки отображения сообщения</param>
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
            object icon = default, 
            TextContentSettings TitleSettings = null, TextContentSettings MessageSettings = null);

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
