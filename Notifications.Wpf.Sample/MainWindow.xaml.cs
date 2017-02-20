using System;
using System.Timers;
using System.Windows;

namespace Notifications.Wpf.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NotificationManager _notificationManager = new NotificationManager();
        private readonly Random _random = new Random();

        public MainWindow()
        {
            InitializeComponent();     

            var timer = new Timer {Interval = 3000};
            timer.Elapsed += (sender, args) => _notificationManager.Show("Pink string from another thread!");
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var content = new NotificationContent
            {
                Title = "Sample notification",
                Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Type = (NotificationType) _random.Next(0, 4)
            };
            _notificationManager.Show(content);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var content = new NotificationContent {Title = "Notification in window", Message = "Click me!"};
            var clickContent = new NotificationContent
            {
                Title = "Clicked!",
                Message = "Window notification was clicked!",
                Type = NotificationType.Success
            };
            _notificationManager.Show(content, "WindowArea", onClick: () => _notificationManager.Show(clickContent));
        }
    }
}
