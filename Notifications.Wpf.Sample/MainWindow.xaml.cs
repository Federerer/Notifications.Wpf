using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Notifications.Wpf.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotificationManager _notificationManager;

        public MainWindow()
        {
            InitializeComponent();

            _notificationManager = new NotificationManager();

            var timer = new Timer {Interval = 3000};
            timer.Elapsed += (sender, args) => _notificationManager.Show("From another thread!");
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _notificationManager.Show(new  NotificationContent { Title = "Sample notification", Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", Type = NotificationType.Information});
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _notificationManager.Show("Sample notification", areaName: "WindowArea", onClick: () => _notificationManager.Show(new NotificationContent { Title = "Click", Message = "Notification was clicked!" , Type = NotificationType.Success}));
        }
    }
}
