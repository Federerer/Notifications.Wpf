using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Notification.Wpf.View;
using Notifications.Wpf.ViewModels;

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
        public async void Show(object content, TimeSpan expirationTime, Action onClick, Action onClose, bool CloseOnClick)
#endif
        {
            var notification = new Notification
            {
                Content = content
            };
            
            notification.MouseLeftButtonDown += (sender, _) =>
            {
                if(content is NotificationContent message)
                    CloseOnClick = message.CloseOnClick;

                if (CloseOnClick)
                    (sender as Notification)?.Close();
                
                if (onClick == null) return;
                onClick.Invoke();
                (sender as Notification)?.Close();
            };
            notification.NotificationClosed += (_, _) => onClose?.Invoke();

            notification.NotificationClosed += OnNotificationClosed;


            await OnShowContent(notification, expirationTime);

        }

        /// <summary>
        /// Отображает окно прогресса
        /// </summary>
        /// <param name="model">модель прогрессбара</param>
        public async void Show(object model)
        {
            var progress = (NotificationProgressViewModel) model;
            var content = new NotificationProgress { DataContext = progress };
            content.Cancel.Click += progress.CancelProgress;
            var notification = new Notification
            {
                Content = content
            };
            notification.NotificationClosed += progress.CancelProgress;
            notification.NotificationClosed += OnNotificationClosed;
            progress.NotifierProgress.SetArea(notification);

            await OnShowContent(notification);

            try
            {
                while (progress.NotifierProgress.IsFinished != true)
                {
                    progress.Cancel.Token.ThrowIfCancellationRequested();
                    await Task.Delay(TimeSpan.FromSeconds(1), progress.Cancel.Token);
                }
            }
            catch (OperationCanceledException)
            { }
            notification.Close();
        }

        /// <summary>
        /// Добавляет уведомление в список отображения
        /// </summary>
        /// <param name="notification">уведомление</param>
        /// <param name="expirationTime">время отображения</param>
        /// <returns></returns>
        private async Task OnShowContent(Notification notification, TimeSpan? expirationTime = null)
        {

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

            if (expirationTime is null)
                return;
#if NET40 
            DelayExecute(expirationTime, () =>
            {
#else

            if (expirationTime == TimeSpan.MaxValue)
            {
                return;
            }
            await Task.Delay((TimeSpan)expirationTime);

#endif
            notification.Close();
#if NET40
            });
#endif
        }
        private void OnNotificationClosed(object sender, RoutedEventArgs routedEventArgs)
        {
            _items.Remove(sender);
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
}