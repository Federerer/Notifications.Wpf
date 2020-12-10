using Notifications.Wpf.ViewModels.Base;

namespace Utilities.WPF.Notifications
{
    public class NotificationAttachTextViewModel : ViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public bool AttachButtonVisibility => Message.Length > 108;
    }
}