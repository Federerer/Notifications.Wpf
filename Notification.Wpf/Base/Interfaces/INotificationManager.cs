using System;
using System.Windows.Media;
using Notification.Wpf.Base.Interfaces.Base;
using Notification.Wpf.Classes;

namespace Notification.Wpf
{
    /// <summary> Notification manager for popup messages </summary>
    public interface INotificationManager : IMessageManager, IProgressManager
    {
    }
}