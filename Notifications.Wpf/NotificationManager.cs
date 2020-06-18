using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Notifications.Wpf.Controls;

namespace Notifications.Wpf
{
    public class NotificationManager : INotificationManager
    {
        private readonly Dispatcher _dispatcher;
        private static readonly List<NotificationArea> Areas = new List<NotificationArea>();
        private static NotificationsOverlayWindow _window;

        public NotificationManager(Dispatcher dispatcher = null)
        {
            if (dispatcher == null)
            {
                dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;
            }

            _dispatcher = dispatcher;
        }

        public void Show(object content, string areaIdentifier = "", TimeSpan? expirationTime = null, Action onClick = null,
            Action onClose = null)
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(
                    new Action(() => Show(content, areaIdentifier, expirationTime, onClick, onClose)));
                return;
            }

            if (expirationTime == null) expirationTime = TimeSpan.FromSeconds(5);

            if (areaIdentifier == string.Empty && _window == null)
            {
                var workArea = SystemParameters.WorkArea;

                _window = new NotificationsOverlayWindow
                {
                    Left = workArea.Left,
                    Top = workArea.Top,
                    Width = workArea.Width,
                    Height = workArea.Height
                };

                _window.Show();
            }

            foreach (var area in Areas.Where(a => a.AreaName == areaIdentifier))
            {
                area.Show(content, (TimeSpan)expirationTime, onClick, onClose);
            }
        }

        internal static void AddArea(NotificationArea area)
        {
            Areas.Add(area);
        }

        internal static void RemoveArea(NotificationArea area)
        {
            Areas.Remove(area);
        }
    }
}