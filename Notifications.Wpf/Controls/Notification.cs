using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Notifications.Wpf.Controls
{
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    public class Notification : ContentControl
    {
        public bool IsClosing { get; set; }

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Notification));

        public static readonly RoutedEvent NotificationClosedEvent = EventManager.RegisterRoutedEvent(
            "NotificationClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Notification));

        static Notification()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Notification),
                new FrameworkPropertyMetadata(typeof(Notification)));
        }

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        public event RoutedEventHandler NotificationClosed
        {
            add { AddHandler(NotificationClosedEvent, value); }
            remove { RemoveHandler(NotificationClosedEvent, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var closeButton = GetTemplateChild("PART_CloseButton") as Button;
            if (closeButton != null)
                closeButton.Click += OnCloseButtonOnClick;

            var loadingAnimation = Template.Resources["LoadingAnimation"] as Storyboard;

            

            if (loadingAnimation == null)
            {
                return;
            }

            if (Equals(LayoutTransform, Transform.Identity))
            {
                LayoutTransform = new ScaleTransform(1, 1);
            }

            loadingAnimation.Begin(this, Template);
        }

        private void OnCloseButtonOnClick(object sender, RoutedEventArgs args)
        {
            var button = sender as Button;
            if (button == null) return;
            
            button.Click -= OnCloseButtonOnClick;
            Close();
        }

        public void Close()
        {
            if (IsClosing)
            {
                return;
            }

            IsClosing = true;

            var closingAnimation = (Template.Resources["ClosingAnimation"] as Storyboard)?.Clone();

            if (closingAnimation == null)
            {
                RaiseEvent(new RoutedEventArgs(NotificationClosedEvent));
                return;
            }

            closingAnimation.Completed += (sender, args) =>
            {
                RaiseEvent(new RoutedEventArgs(NotificationClosedEvent));
            };

            closingAnimation.Begin(this, Template, true);
        }
    }
        
}
