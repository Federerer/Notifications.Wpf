using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Notifications.Wpf.Controls;

namespace Notifications.Wpf
{
    public class NotificationManager : INotificationManager
    {
        private static readonly List<NotificationArea> Areas = new List<NotificationArea>();
        private static NotificationsOverlayWindow _window;
      
        public void Show(object content, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null, Action onClose = null)
        {
            if (expirationTime == null) expirationTime = TimeSpan.FromSeconds(5);

            if (areaName == string.Empty && _window == null)
            {
                var workArea = SystemParameters.WorkArea;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    _window = new NotificationsOverlayWindow
                    {
                        Left = workArea.Left,
                        Top = workArea.Top,
                        Width = workArea.Width,
                        Height = workArea.Height
                    };

                    _window.Show();
                });
            }

            foreach (var area in Areas.Where(a => a.Name == areaName))
            {
                area.Show(content, (TimeSpan)expirationTime, onClick, onClose);
            }
        }

        internal static void AddArea(NotificationArea area)
        {
            Areas.Add(area);
        }
       
    }
}