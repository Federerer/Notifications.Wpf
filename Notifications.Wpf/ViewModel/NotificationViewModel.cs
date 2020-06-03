using System.Threading.Tasks;
using Caliburn.Micro;
using Notification.Wpf;

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

        public virtual void Ok() { }

        public virtual void Cancel() { }
    }
}