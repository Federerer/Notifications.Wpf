using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Notification.Wpf.View;
using Notification.Wpf.Classes;
using Notifications.Wpf.ViewModels;
using Utilities.WPF.Notifications;

namespace Notification.Wpf.Controls
{
    public class NotificationArea : Control
    {

        public NotificationPosition Position
        {
            get => (NotificationPosition)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        // Using a DependencyProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(NotificationPosition), typeof(NotificationArea), new PropertyMetadata(NotificationPosition.BottomRight));


        public int MaxItems
        {
            get => (int)GetValue(MaxItemsProperty);
            set => SetValue(MaxItemsProperty, value);
        }
        
        public static readonly DependencyProperty MaxItemsProperty =
            DependencyProperty.Register("MaxItems", typeof(int), typeof(NotificationArea), new PropertyMetadata(int.MaxValue));

        private IList _items;

        public NotificationArea()
        {
            NotificationManager.AddArea(this);
        }

        static NotificationArea()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationArea),
                new FrameworkPropertyMetadata(typeof(NotificationArea)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var itemsControl = GetTemplateChild("PART_Items") as Panel;
            _items = itemsControl?.Children;
        }

#if NET40
        public void Show(object content, TimeSpan expirationTime, Action onClick, Action onClose)
#else
        public async void Show(object content, TimeSpan expirationTime, Action onClick, Action onClose)
#endif
        {
            if(content is NotificationContent model)
                if(model.Type==NotificationType.Notification)
                {
                    var temp = new NotificationViewModel {Title = model.Title, Message = model.Message};

                    content = new NotificationInfoView {DataContext = temp };
                }
            var notification = new Notification
            {
                Content = content
            };
            
            notification.MouseLeftButtonDown += (sender, args) =>
            {
                if (onClick != null)
                {
                    onClick.Invoke();
                    (sender as Notification)?.Close();
                }
            };
            notification.NotificationClosed += (sender, args) => onClose?.Invoke();
            notification.NotificationClosed += OnNotificationClosed;

            if (!IsLoaded)
            {
                return;
            }

            var w = Window.GetWindow(this);
            var x = PresentationSource.FromVisual(w);
            if (x == null)
            {
                return;
            }

            lock (_items)
            {
                _items.Add(notification);

                if (_items.OfType<Notification>().Count(i => !i.IsClosing) > MaxItems)
                {
                    _items.OfType<Notification>().First(i => !i.IsClosing).Close();
                }
            }

#if NET40 
            DelayExecute(expirationTime, () =>
            {
#else
            if (expirationTime == TimeSpan.MaxValue)
            {
                return;
            }
            await Task.Delay(expirationTime);
#endif
                notification.Close();
#if NET40
            });
#endif
        }
#if NET40
        public void ShowAction(object content, TimeSpan expirationTime, RoutedEventHandler LeftButton, RoutedEventHandler RightButton)
#else
        public async void Show(object model, TimeSpan expirationTime, RoutedEventHandler LeftButton = null, RoutedEventHandler RightButton = null)
#endif
        {

            var content = new NotificationInfoView {DataContext = model};
            if(RightButton!=null) content.Ok.Click += RightButton;
            if(LeftButton!=null) content.Cancel.Click += LeftButton;

            var notification = new Notification
            {
                Content = content
            };
            
            notification.NotificationClosed += OnNotificationClosed;

            if (!IsLoaded)
            {
                return;
            }

            var w = Window.GetWindow(this);
            var x = PresentationSource.FromVisual(w);
            if (x == null)
            {
                return;
            }

            lock (_items)
            {
                _items.Add(notification);

                if (_items.OfType<Notification>().Count(i => !i.IsClosing) > MaxItems)
                {
                    _items.OfType<Notification>().First(i => !i.IsClosing).Close();
                }
            }

#if NET40 
            DelayExecute(expirationTime, () =>
            {
#else
            if (expirationTime == TimeSpan.MaxValue)
            {
                return;
            }
            await Task.Delay(expirationTime);
#endif
                notification.Close();
#if NET40
            });
#endif
        }

        public async void Show(NotificationProgressViewModel model)
        {
            var content = new NotificationProgress { DataContext = model };
            content.Cancel.Click += model.CancelProgress;
            var notification = new Notification
            {
                Content = content
            };
            notification.NotificationClosed += model.CancelProgress;
            notification.NotificationClosed += OnNotificationClosed;

            if (!IsLoaded)
            {
                return;
            }

            var w = Window.GetWindow(this);

            var x = PresentationSource.FromVisual(w);
            if (x == null)
            {
                return;
            }
            model.progress.SetArea(notification);
            lock (_items)
            {
                _items.Add(notification);

                if (_items.OfType<Notification>().Count(i => !i.IsClosing) > MaxItems)
                {
                    _items.OfType<Notification>().First(i => !i.IsClosing).Close();
                }
            }

            try
            {
                while (model.progress.IsFinished != true)
                {
                    model.Cancel.Token.ThrowIfCancellationRequested();
                    await Task.Delay(TimeSpan.FromSeconds(1), model.Cancel.Token);
                }
            }
            catch (OperationCanceledException)
            {
                notification.Close();
            }
            notification.Close();
        }

        private void OnNotificationClosed(object sender, RoutedEventArgs routedEventArgs)
        {
            var notification = sender as Notification;
            _items.Remove(notification);
        }

#if NET40
        private static void DelayExecute(TimeSpan delay, Action actionToExecute)
        {
            if (actionToExecute != null)
            {
                var timer = new DispatcherTimer
                {
                    Interval = delay
                };
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    actionToExecute();
                };
                timer.Start();
            }
        }
#endif
    }

    public enum NotificationPosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}