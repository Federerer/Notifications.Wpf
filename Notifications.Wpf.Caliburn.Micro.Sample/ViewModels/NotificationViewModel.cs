using System.Windows.Media;
using Caliburn.Micro;

namespace Notifications.Wpf.Caliburn.Micro.Sample.ViewModels
{
    public class NotificationViewModel : PropertyChangedBase
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public Brush BackgroundBrush { get; set; } = Brushes.DimGray;
    }
}
