using System.Threading.Tasks;
using Caliburn.Micro;
using Notifications.Wpf;

// ReSharper disable once CheckNamespace
namespace Utilities.WPF.Notifications
{
    public class NotificationViewModel : PropertyChangedBase
    {
        public string Title { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Содержимое левой кнопки
        /// </summary>
        public object LeftButtonContent { get; set; }

        public bool LeftButtonVisibility { get; set; }
        public bool RightButtonVisibility { get; set; }

        /// <summary>
        /// Содержимое правой кнопки
        /// </summary>
        public object RightButtonContent { get; set; }

        public NotificationViewModel()
        {}

        public virtual async void Ok(){}

        public virtual async void Cancel(){}
    }
}