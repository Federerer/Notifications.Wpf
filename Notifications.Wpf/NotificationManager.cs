using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Notification.Wpf.Classes;
using Notification.Wpf.Controls;
using Notifications.Wpf.ViewModels;
using Utilities.WPF.Notifications;

namespace Notification.Wpf
{
    public class NotificationManager : INotificationManager
    {
        private readonly Dispatcher _dispatcher;
        private static readonly List<NotificationArea> Areas = new();
        private static NotificationsOverlayWindow _window;

        public NotificationManager(Dispatcher dispatcher = null)
        {
            dispatcher ??= Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

            _dispatcher = dispatcher;
        }

        public void Show(object content, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null,
            Action onClose = null)
        {
            areaName ??= "";
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(
                    new Action(() => Show(content, areaName, expirationTime, onClick, onClose)));
                return;
            }
            ShowContent(content, expirationTime, areaName, onClick, onClose);
        }

        public void Show(string title, string message, NotificationType type, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null,
            Action onClose = null, NotificationTextTrimType trim = NotificationTextTrimType.NoTrim, uint RowsCountWhenTrim = 2)
        {
            var content = new NotificationContent { Type = type, TrimType = trim, RowsCount = RowsCountWhenTrim };
            if (message != null)
                content.Message = message;
            if (title != null) content.Title = title;


            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(
                    new Action(() => Show(title, message, type, areaName, expirationTime, onClick, onClose, trim, RowsCountWhenTrim)));
                return;
            }
            ShowContent(content, expirationTime, areaName, onClick, onClose);
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

            ShowContent(content, expirationTime, areaName, null, null, LeftButton, RightButton);
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
        public void ShowProgressBar(out ProgressFinaly<(int? value, string message, string title, bool? showCancel)> progress, out CancellationToken Cancel, string Title = null,
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
                ProgressFinaly<(int?, string, string, bool?)> bar = null;
                var CancelFirst= new CancellationToken();

                _dispatcher.Invoke(
                    () =>
                    {
                        ShowProgressBar(out var progress1, out var Cancel1, Title, ShowCancelButton, ShowProgress, areaName);
                        bar = progress1;
                        CancelFirst = Cancel1;
                    });
                progress = bar;
                Cancel = CancelFirst;
                return;
            }

            ShowContent(model);
        }
        /// <summary>
        /// Запуск отображения в зависимости от типа контента
        /// </summary>
        /// <param name="content">контент</param>
        /// <param name="expirationTime">время отображения</param>
        /// <param name="areaName">зона отображения</param>
        /// <param name="onClick">действие при клике</param>
        /// <param name="onClose">действие при закрытии</param>
        /// <param name="LeftButton">левая кнопка</param>
        /// <param name="RightButton">правая кнопка</param>
        static void ShowContent( object content, TimeSpan? expirationTime = null, string areaName = "",
            Action onClick = null, Action onClose = null,
            RoutedEventHandler LeftButton = null, RoutedEventHandler RightButton = null)
        {
            expirationTime ??= TimeSpan.FromSeconds(5);

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

            if (Areas != null && _window != null && !_window.IsVisible)
                _window.Show();


            if (Areas == null) return;
            foreach (var area in Areas.Where(a => a.Name == areaName))
            {
                switch (content)
                {
                    case NotificationViewModel : area.Show(content, (TimeSpan)expirationTime, LeftButton, RightButton);
                        break;
                    case NotificationProgressViewModel : area.Show(content);
                        break;
                    default: area.Show(content, (TimeSpan)expirationTime, onClick, onClose);
                        break;
                }
            }
        }

        internal static void AddArea(NotificationArea area) => Areas.Add(area);
    }
}