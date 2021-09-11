using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Notification.Wpf.Classes;
using Notification.Wpf.Controls;
using Notifications.Wpf.ViewModels;

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

        /// <inheritdoc />
        public void Show(object content, string areaName = "", TimeSpan? expirationTime = null, Action onClick = null,
            Action onClose = null, bool CloseOnClick = true)
        {
            areaName ??= "";
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(
                    new Action(() => Show(content, areaName, expirationTime, onClick, onClose, CloseOnClick)));
                return;
            }
            ShowContent(content, expirationTime, areaName, onClick, onClose, CloseOnClick);
        }
        /// <inheritdoc />
        public void Show(string title, string message, NotificationType type, string areaName = "", TimeSpan? expirationTime = null,
            Action onClick = null, Action onClose = null, Action LeftButton = null, string LeftButtonText = null, Action RightButton = null, string RightButtonText = null,
            NotificationTextTrimType trim = NotificationTextTrimType.NoTrim, uint RowsCountWhenTrim = 2, bool CloseOnClick = true)
        {
            var content = new NotificationContent
            {
                Type = type,
                TrimType = trim,
                RowsCount = RowsCountWhenTrim,
                LeftButtonAction = LeftButton,
                LeftButtonContent = LeftButtonText,
                RightButtonAction = RightButton,
                RightButtonContent = RightButtonText,
                Message = message,
                Title = title,
                CloseOnClick = CloseOnClick
            };
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(
                    new Action(() => Show(title, message, type, areaName, expirationTime, onClick, onClose, LeftButton, LeftButtonText, RightButton, RightButtonText, trim, RowsCountWhenTrim, CloseOnClick)));
                return;
            }
            ShowContent(content, expirationTime, areaName, onClick, onClose, CloseOnClick);
        }

        /// <inheritdoc />
        public NotifierProgress<(double? value, string message, string title, bool? showCancel)> ShowProgressBar(string Title = null,
            bool ShowCancelButton = true,
            bool ShowProgress = true,
            string areaName = "",
            bool TrimText = false,
            uint DefaultRowsCount = 1U,
            string BaseWaitingMessage = "Calculation time",
            bool IsCollapse = false,
            bool TitleWhenCollapsed = true)
        {
            var model = new NotificationProgressViewModel(
                ShowCancelButton,
                ShowProgress,
                TrimText,
                DefaultRowsCount,
                BaseWaitingMessage,
                IsCollapse,
                TitleWhenCollapsed);

            if (Title != null) model.Title = Title;

            if (!_dispatcher.CheckAccess())
            {
                return _dispatcher.Invoke(
                    () => ShowProgressBar(
                        Title,
                        ShowCancelButton,
                        ShowProgress,
                        areaName,
                        TrimText,
                        DefaultRowsCount,
                        BaseWaitingMessage,
                        IsCollapse,
                        TitleWhenCollapsed));
            }

            ShowContent(model, areaName: areaName);
            return model.NotifierProgress;
        }

        /// <summary>
        /// Запуск отображения в зависимости от типа контента
        /// </summary>
        /// <param name="content">контент</param>
        /// <param name="expirationTime">время отображения</param>
        /// <param name="areaName">зона отображения</param>
        /// <param name="onClick">действие при клике</param>
        /// <param name="onClose">действие при закрытии</param>
        /// <param name="CloseOnClick">Закрыть сообщение при клике по телу</param>
        static void ShowContent(object content, TimeSpan? expirationTime = null, string areaName = "",
            Action onClick = null, Action onClose = null, bool CloseOnClick = true)
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
                _window.Closed += (_,_) =>
                {
                    _window = null;
                };
            }

            if (Areas != null && _window is { IsVisible: false })
                _window.Show();

            if (Areas == null) return;
            foreach (var area in Areas.Where(a => a.Name == areaName))
            {
                switch (content)
                {
                    case NotificationProgressViewModel:
                        area.Show(content);
                        break;
                    default:
                        area.Show(content, (TimeSpan)expirationTime, onClick, onClose, CloseOnClick);
                        break;
                }
            }
        }

        internal static void AddArea(NotificationArea area) => Areas.Add(area);
    }
}