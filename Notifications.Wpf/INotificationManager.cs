using System;
using System.Threading;
using System.Windows;
using Notification.Wpf.Classes;

namespace Notification.Wpf
{
    public interface INotificationManager
    {
        void Show(object content, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null, Action onClose = null);

        void Show(string title, string message, NotificationType type, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null,
            Action onClose = null, TextTrimming trim = TextTrimming.None);

        void Show(string title, string message, string areaName = "", TimeSpan? expirationTime = null, RoutedEventHandler LeftButton = null,
            string LeftButtonText = null,
            RoutedEventHandler RightButton = null, string RightButtonText = null);

        void ShowProgressBar(
            out ProgressFinaly<(int? value, string message, string title, bool? showCancel)> progress, out CancellationToken Cancel, string Title = null,
            bool ShowCancelButton = true, bool ShowProgress = true, string areaName = "");
    }
}