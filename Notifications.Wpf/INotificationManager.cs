using System;

namespace Notification.Wpf
{
    public interface INotificationManager
    {
        void Show(object content, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null, Action onClose = null);
    }
}