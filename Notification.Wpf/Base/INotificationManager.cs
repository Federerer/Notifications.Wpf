using System;
using System.Threading;
using Notification.Wpf.Classes;

namespace Notification.Wpf
{
    public interface INotificationManager
    {
        void Show(object content, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null, Action onClose = null, bool CloseOnClick = true);

        void Show(string title, string message, NotificationType type, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null,
            Action onClose = null, Action LeftButton = null, string LeftButtonText = null, Action RightButton = null, string RightButtonText = null,
            NotificationTextTrimType trim = NotificationTextTrimType.NoTrim, uint RowsCountWhenTrim = 2, bool CloseOnClick = true);

        ProgressFinaly<(int? value, string message, string title, bool? showCancel)> ShowProgressBar(out CancellationToken Cancel, string Title = null,
            bool ShowCancelButton = true, bool ShowProgress = true, string areaName = "", bool TrimText = false, uint DefaultRowsCount = 1U, string BaseWaitingMessage = "Calculation time");
    }
}