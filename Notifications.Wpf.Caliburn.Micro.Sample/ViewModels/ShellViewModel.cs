using System;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;

namespace Notifications.Wpf.Caliburn.Micro.Sample.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        private readonly INotificationManager _manager;

        public ShellViewModel(INotificationManager manager)
        {
            _manager = manager;
        }

        public void Show()
        {
            var content = new NotificationViewModel
            {
                Message = $"{DateTime.Now} some message",
                BackgroundBrush = Brushes.LimeGreen
            };

            _manager.Show(content, expirationTime: TimeSpan.FromSeconds(6), onClick: () => Console.WriteLine("Clicked!!!"));          
        }

        public void ShowInWindow()
        {
            _manager.Show(new NotificationContent { Title ="Message", Message = "Message in window"}, areaName: "WindowArea");
        }
    }
}