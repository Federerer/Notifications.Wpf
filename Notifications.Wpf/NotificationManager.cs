using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Notifications.Wpf.Controls;
using Utilities.WPF.Notifications;

namespace Notifications.Wpf
{
    public class NotificationManager : INotificationManager
    {
        private readonly Dispatcher _dispatcher;
        private static readonly List<NotificationArea> Areas = new List<NotificationArea>();
        private static NotificationsOverlayWindow _window;

        public IProgress<(int, string, string, bool?)> GetProgressWind(string name)
        {
            foreach (var area in Areas)
                if (area.ProgressWind.ContainsKey(name)) return area.ProgressWind.FirstOrDefault(p => p.Key == name).Value;

            return null;
        }

        public NotificationManager(Dispatcher dispatcher = null)
        {
            if (dispatcher == null)
            {
                dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;
            }

            _dispatcher = dispatcher;
        }

        public void Show(object content, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null,
            Action onClose = null)
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(
                    new Action(() => Show(content, areaName, expirationTime, onClick, onClose)));
                return;
            }

            if (expirationTime == null) expirationTime = TimeSpan.FromSeconds(5);

            if (areaName == string.Empty && _window == null)
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

            foreach (var area in Areas.Where(a => a.Name == areaName))
            {
                area.Show(content, (TimeSpan) expirationTime, onClick, onClose);
            }
        }

        public void Show(string title, string message, NotificationType type, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null,
            Action onClose = null)
        {
            var content = new NotificationContent {Type = type};
            if (message != null)
                content.Message = message;
            if (title != null) content.Title = title;


            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(
                    new Action(() => Show(title, message, type, areaName, expirationTime, onClick, onClose)));
                return;
            }

            if (expirationTime == null) expirationTime = TimeSpan.FromSeconds(5);

            if (areaName == string.Empty && _window == null)
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

            foreach (var area in Areas.Where(a => a.Name == areaName))
            {
                area.Show(content, (TimeSpan) expirationTime, onClick, onClose);
            }
        }


        /// <summary>
        /// Show ProgressBar
        /// </summary>
        /// <param name="ProgressName">Need to identification</param>
        /// <param name="Title">Title of window</param>
        /// <param name="ShowProgress">Show or not progress status</param>
        /// <param name="ShowCancelButton">Show Cancel button or not</param>
        /// <param name="areaName">window are where show notification</param>
        public void ShowProgressBar(string ProgressName, string Title = null,
            bool ShowCancelButton = true,  bool ShowProgress = false, string areaName = "")
        {
            var cancelTokenSource = new CancellationTokenSource();
            var model = new NotificationProgressViewModel(ProgressName, cancelTokenSource, ShowCancelButton);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (ShowProgress) model.IsIndeterminate = false;
            else model.IsIndeterminate = true;
            if (Title != null) model.Title = Title;
            cancelTokenSource.Token.ThrowIfCancellationRequested();
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(
                    new Action(() => ShowProgressBar(ProgressName, Title, ShowCancelButton, ShowProgress, areaName)));
                return;
            }

            if (areaName == string.Empty && _window == null)
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

            foreach (var area in Areas.Where(a => a.Name == areaName))
            {
                area.Show(model);
            }
        }

        internal static void AddArea(NotificationArea area)
        {
            Areas.Add(area);
        }

    }
}