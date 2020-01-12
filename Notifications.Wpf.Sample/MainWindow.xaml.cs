using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Notifications.Wpf.Classes;
using Notifications.Wpf.View;
using Utilities.WPF.Notifications;
using Timer = System.Timers.Timer;

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

        private async void Message_Click(object sender, RoutedEventArgs e)
        {
            var clickContent = new NotificationContent
            {
                Title = "Clicked!",
                Message = "Window notification was clicked!",
                Type = NotificationType.Success
            };

            var title = "Sample notification";
            var Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
            var type = (NotificationType) _random.Next(0, 5);

            _notificationManager.Show(title, Message, type, "WindowArea", onClick: () => _notificationManager.Show(clickContent));
            _notificationManager.Show(title, Message, type, "", onClick: () => _notificationManager.Show(clickContent));

        }
        private async void Progress_Click(object sender, RoutedEventArgs e)
        {
            var title = "Прогресс бар";

            _notificationManager.ShowProgressBar(out var progress, out var Cancel, title, true, true);
            _notificationManager.ShowProgressBar(out var progress2, out var Cancel2, title, true, false);
            using (progress)
            using (progress2)
                try
                {
                    await CalcAsync(progress, Cancel).ConfigureAwait(false);
                    await CalcAsync(progress2, Cancel2).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    _notificationManager.Show("Операция отменена", "", TimeSpan.FromSeconds(3));
                    var p = new Progress<int>();
                }
        }

        public Task CalcAsync(IProgress<(int, string,string,bool?)> progress, CancellationToken cancel) =>
            Task.Run(async () =>
            {
                for (var i = 0; i <= 100; i++)
                {
                    cancel.ThrowIfCancellationRequested();
                    progress.Report((i, $"Процесс {i}",null, null));
                    await Task.Delay(TimeSpan.FromSeconds(0.1), cancel);
                }
            }, cancel);

    }
}
