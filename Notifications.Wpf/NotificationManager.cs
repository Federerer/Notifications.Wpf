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
        /// Show Progress Bar
        /// </summary>
        /// <typeparam name="T">Type of progress</typeparam>
        /// <param name="cancelTokenSource">cancelTokenSource</param>
        /// <param name="cancel">CancellationToken</param>
        /// <param name="progress">IProgress</param>
        /// <param name="Title">Title of window</param>
        /// <param name="ShowProgress">Show or not progress status</param>
        /// <param name="ShowCancelButton">Show Cancel button or not</param>
        /// <param name="areaName">window are where show notification</param>
        public void Show<T>(CancellationTokenSource cancelTokenSource, CancellationToken cancel, IProgress<T> progress, string Title = null,
            bool ShowCancelButton = true,  bool ShowProgress = false, string areaName = "")
        {
            if(cancelTokenSource is null)throw new ArgumentNullException(nameof(cancelTokenSource));
            if(cancel == default)throw new ArgumentNullException(nameof(cancel));
            if(progress is null)throw new ArgumentNullException(nameof(progress));

            var model = /*ShowCancelButton ? */new NotificationProgressViewModel<T>(cancelTokenSource, progress) /*: new NotificationProgressViewModel<T>(progress)*/;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (ShowProgress) model.IsIndeterminate = false;
            else model.IsIndeterminate = true;
            if (Title != null) model.Title = Title;
            cancel.ThrowIfCancellationRequested();
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(
                    new Action(() => Show(cancelTokenSource, cancel, progress, Title, ShowCancelButton, ShowProgress, areaName)));
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
                area.Show(model, cancelTokenSource, cancel, progress);
            }
        }

        internal static void AddArea(NotificationArea area)
        {
            Areas.Add(area);
        }

    }
}