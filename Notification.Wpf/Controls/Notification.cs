using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Notification.Wpf.Utils;
using Notification.Wpf.View;
using Notifications.Wpf.View;

namespace Notification.Wpf.Controls
{
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    public partial class Notification : ContentControl
    {
        private TimeSpan _closingAnimationTime = TimeSpan.Zero;

        public bool IsClosing { get; set; }

        public static readonly RoutedEvent NotificationCloseInvokedEvent = EventManager.RegisterRoutedEvent(
            "NotificationCloseInvoked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Notification));

        public static readonly RoutedEvent NotificationClosedEvent = EventManager.RegisterRoutedEvent(
            "NotificationClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Notification));

        static Notification()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Notification),
                new FrameworkPropertyMetadata(typeof(Notification)));
        }

        public event RoutedEventHandler NotificationCloseInvoked
        {
            add => AddHandler(NotificationCloseInvokedEvent, value);
            remove => RemoveHandler(NotificationCloseInvokedEvent, value);
        }

        public event RoutedEventHandler NotificationClosed
        {
            add => AddHandler(NotificationClosedEvent, value);
            remove => RemoveHandler(NotificationClosedEvent, value);
        }

        public static bool GetCloseOnClick(DependencyObject obj)
        {
            return (bool)obj.GetValue(CloseOnClickProperty);
        }

        public static void SetCloseOnClick(DependencyObject obj, bool value)
        {
            obj.SetValue(CloseOnClickProperty, value);
        }

        public static readonly DependencyProperty CloseOnClickProperty =
            DependencyProperty.RegisterAttached("CloseOnClick", typeof(bool), typeof(Notification), new FrameworkPropertyMetadata(false, CloseOnClickChanged));

        private static void CloseOnClickChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is not Button button)
            {
                return;
            }

            var value = (bool)dependencyPropertyChangedEventArgs.NewValue;

            if (value)
            {
                button.Click += (sender, args) =>
                {
                    var notification = VisualTreeHelperExtensions.GetParent<Notification>(button);
                    notification?.Close();
                };
            }
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_CloseButton") is Button closeButton)
            {
                closeButton.Click += OnCloseButtonOnClick;
            }
            if (GetTemplateChild("PART_AttachButton") is Button AttachButton)
                AttachButton.Click += OnAttachButtonOnClick;

            var storyboards = Template.Triggers.OfType<EventTrigger>().FirstOrDefault(t => t.RoutedEvent == NotificationCloseInvokedEvent)?.Actions.OfType<BeginStoryboard>().Select(a => a.Storyboard);
            _closingAnimationTime = new TimeSpan(storyboards?.Max(s => Math.Min((s.Duration.HasTimeSpan ? s.Duration.TimeSpan + (s.BeginTime ?? TimeSpan.Zero) : TimeSpan.MaxValue).Ticks, s.Children.Select(ch => ch.Duration.TimeSpan + (s.BeginTime ?? TimeSpan.Zero)).Max().Ticks)) ?? 0);

        }

        private void OnCloseButtonOnClick(object sender, RoutedEventArgs args)
        {
            if (sender is not Button button) return;

            button.Click -= OnCloseButtonOnClick;

            Close();
        }

        //TODO: .NET40
        public async void Close()
        {
            if (IsClosing)
            {
                return;
            }

            IsClosing = true;

            RaiseEvent(new RoutedEventArgs(NotificationCloseInvokedEvent));
            await Task.Delay(_closingAnimationTime);
            RaiseEvent(new RoutedEventArgs(NotificationClosedEvent));

            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.Title.Equals("ToastWindow"));
            if(currentWindow == null)return;
            var notificationCount = VisualTreeHelperExtensions.GetActiveNotificationCount(currentWindow);

            if (notificationCount == 0)
                currentWindow.Close();

        }

        #region XbtnVisibility : Visibility - X Button visibility

        /// <summary>X Button visibility</summary>
        public static readonly DependencyProperty XbtnVisibilityProperty =
            DependencyProperty.Register(
                nameof(XbtnVisibility),
                typeof(Visibility),
                typeof(Notification),
                new PropertyMetadata(Visibility.Visible));

        /// <summary>X Button visibility</summary>
        public Visibility XbtnVisibility { get => (Visibility)GetValue(XbtnVisibilityProperty); set => SetValue(XbtnVisibilityProperty, value); }

        #endregion

    }
   
    [TemplatePart(Name = "PART_AttachButton", Type = typeof(Button))]
    public partial class Notification : ContentControl
    {

        public static readonly RoutedEvent NotificationAttachInvokedEvent = EventManager.RegisterRoutedEvent(
            "NotificationAttachInvoked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Notification));

        public static readonly RoutedEvent NotificationAttachEvent = EventManager.RegisterRoutedEvent(
            "NotificationAttach", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Notification));

        public event RoutedEventHandler NotificationAttachInvoked
        {
            add => AddHandler(NotificationAttachInvokedEvent, value);
            remove => RemoveHandler(NotificationAttachInvokedEvent, value);
        }

        public event RoutedEventHandler NotificationAttach
        {
            add => AddHandler(NotificationAttachEvent, value);
            remove => RemoveHandler(NotificationAttachEvent, value);
        }

        public static bool GetAttachOnClick(DependencyObject obj)
        {
            return (bool)obj.GetValue(AttachOnClickProperty);
        }

        public static void SetAttachOnClick(DependencyObject obj, bool value)
        {
            obj.SetValue(AttachOnClickProperty, value);
        }

        public static readonly DependencyProperty AttachOnClickProperty =
            DependencyProperty.RegisterAttached("AttachOnClick", typeof(NotificationContent), typeof(Notification),
                new FrameworkPropertyMetadata(new NotificationContent{Message = string.Empty, Title = string.Empty,
                    TrimType = NotificationTextTrimType.NoTrim, Type = NotificationType.Notification}, AttachOnClickChanged));

        private static void AttachOnClickChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is not Button button)
            {
                return;
            }

            var value = (NotificationContent)dependencyPropertyChangedEventArgs.NewValue;

            if (value is not null)
            {
                button.Click += (sender, args) =>
                {
                    var window = new Window();
                    var win_content = new TextContentView { DataContext = value };
                    window.Content = win_content;
                    window.Title = "Message";
                    window.Height = 500;
                    window.Width = 650;
                    window.WindowStyle = WindowStyle.None;
                    window.MouseDown += (Sender, Args) =>
                    {
                        if (Args.ChangedButton != MouseButton.Left) return;
                        window.DragMove();
                    };
                    window.MouseDoubleClick += (Sender, Args) =>
                    {
                        if (window.WindowState == WindowState.Maximized)
                        {
                            window.WindowState = WindowState.Normal;
                        }
                        else if (window.WindowState == WindowState.Normal)
                        {
                            window.WindowState = WindowState.Maximized;
                        }
                    };
                    window.Show();
                };
            }
        }


        private void OnAttachButtonOnClick(object sender, RoutedEventArgs args)
        {
            if (sender is not Button button) return;

            button.Click -= OnAttachButtonOnClick;
            Attach();
        }

        public void Attach()
        {
            if (IsClosing)
            {
                return;
            }

            RaiseEvent(new RoutedEventArgs(NotificationCloseInvokedEvent));

        }


    }
}
