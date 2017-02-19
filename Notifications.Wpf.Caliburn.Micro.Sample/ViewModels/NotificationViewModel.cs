using System.Windows.Media;
using Caliburn.Micro;

namespace Notifications.Wpf.Caliburn.Micro.Sample.ViewModels
{
    public class NotificationViewModel : PropertyChangedBase
    {
        private readonly INotificationManager _manager;

        public string Title { get; set; }
        public string Message { get; set; }

        public NotificationViewModel(INotificationManager manager)
        {
            _manager = manager;
        }

        public void Ok()
        {
            _manager.Show(new NotificationContent { Title ="Success!", Message = "Ok button clicked.", Type = NotificationType.Success});
        }

        public void Cancel()
        {
            _manager.Show(new NotificationContent { Title = "Error!",  Message = "Cancel button clicked!", Type = NotificationType.Error});
        }
    }
}
