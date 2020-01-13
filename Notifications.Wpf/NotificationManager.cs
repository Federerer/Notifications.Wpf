using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Notifications.Wpf.Classes;
using Notifications.Wpf.Controls;
using Utilities.WPF.Notifications;

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

        public void Show(object content, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null,
            Action onClose = null)
        {
            if (areaName == null) areaName = "";
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
        public void Show(string title, string message, string areaName = "", TimeSpan? expirationTime = null, RoutedEventHandler LeftButton = null, string LeftButtonText = null,
            RoutedEventHandler RightButton = null, string RightButtonText = null)
        {
            var content = new NotificationViewModel();
            if (message != null)
                content.Message = message;
            if (title != null) content.Title = title;
            if (LeftButton != null)
            {
                content.LeftButtonContent = LeftButtonText ?? "Ok";

                content.LeftButtonVisibility = true;
            } 
            if (RightButton != null)
            {
                content.RightButtonContent = RightButtonText ?? "Cancel";
                content.RightButtonVisibility = true;
            }

            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(
                    new Action(() => Show(title, message, areaName, expirationTime, LeftButton, LeftButtonText, RightButton, RightButtonText)));
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
                area.Show(content, (TimeSpan) expirationTime, LeftButton, RightButton);
            }
        }
        


        /// <summary>
        /// Show ProgressBar
        /// </summary>
        /// <param name="Title">Title of window</param>
        /// <param name="ShowProgress">Show or not progress status</param>
        /// <param name="progress">Progress</param>
        /// <param name="Cancel">CancellationTokenSource, if it null - ShowCancelButton will state false</param>
        /// <param name="ShowCancelButton">Show Cancel button or not</param>
        /// <param name="areaName">window are where show notification</param>
        public void ShowProgressBar(out ProgressFinaly<(int,string,string,bool?)> progress, out CancellationToken Cancel, string Title = null,
            bool ShowCancelButton = true,  bool ShowProgress = true, string areaName = "")
        {
            var CancelSource = new CancellationTokenSource();
            Cancel = CancelSource.Token;
            var model = new NotificationProgressViewModel(out var progressModel, CancelSource, ShowCancelButton, ShowProgress);
            progress = progressModel;
            if (Title != null) model.Title = Title;
            Cancel.ThrowIfCancellationRequested();

            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(
                    new Action(() => ShowProgressBar(out var progress1, out var Cancel1, Title, ShowCancelButton, ShowProgress, areaName)));
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