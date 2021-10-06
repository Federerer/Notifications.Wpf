using System.Windows;
using Notification.Wpf.Constants;
using Notification.Wpf.Controls;

namespace Notification.Wpf
{
    /// <summary>
    /// Interaction logic for ToastWindow
    /// </summary>
    public partial class NotificationsOverlayWindow : Window
    {
        #region MaxWindowItems : int - Maximum Window Items

        /// <summary>Maximum Window Items</summary>
        public static readonly DependencyProperty MaxWindowItemsProperty =
            DependencyProperty.Register(
                nameof(MaxWindowItems),
                typeof(uint),
                typeof(NotificationsOverlayWindow),
                new PropertyMetadata(NotificationConstants.NotificationsOverlayWindowMaxCount));

        /// <summary>Maximum Window Items</summary>
        public uint MaxWindowItems { get => (uint)GetValue(MaxWindowItemsProperty); set => SetValue(MaxWindowItemsProperty, value); }

        #endregion

        #region MessagePosition : NotificationPosition - Позиция сообщений в окне

        /// <summary>Позиция сообщений в окне</summary>
        public static readonly DependencyProperty MessagePositionProperty =
            DependencyProperty.Register(
                nameof(MessagePosition),
                typeof(NotificationPosition),
                typeof(NotificationsOverlayWindow),
                new PropertyMetadata(NotificationConstants.MessagePosition));

        /// <summary>Позиция сообщений в окне</summary>
        public NotificationPosition MessagePosition { get => (NotificationPosition)GetValue(MessagePositionProperty); set => SetValue(MessagePositionProperty, value); }

        #endregion

        #region CollapseProgressAutoIfMoreMessages : bool - Need collapse notification if count more that maximum

        /// <summary>Need collapse notification if count more that maximum</summary>
        public static readonly DependencyProperty CollapseProgressAutoIfMoreMessagesProperty =
            DependencyProperty.Register(
                nameof(CollapseProgressAutoIfMoreMessages),
                typeof(bool),
                typeof(NotificationsOverlayWindow),
                new PropertyMetadata(NotificationConstants.CollapseProgressIfMoreRows));

        /// <summary>Need collapse notification if count more that maximum</summary>
        public bool CollapseProgressAutoIfMoreMessages { get => (bool)GetValue(CollapseProgressAutoIfMoreMessagesProperty); set => SetValue(CollapseProgressAutoIfMoreMessagesProperty, value); }

        #endregion
        public NotificationsOverlayWindow()
        {
            InitializeComponent();
        }
        
    }
}
