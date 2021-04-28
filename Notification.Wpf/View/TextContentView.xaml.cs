using System.Windows;
using System.Windows.Controls;

namespace Notifications.Wpf.View
{
    /// <summary>
    /// Логика взаимодействия для TextContentView.xaml
    /// </summary>
    public partial class TextContentView : UserControl
    {
        public TextContentView()
        {
            InitializeComponent();
            if (this.Parent is Window)
            {
                TopPanelButtons.Visibility = Visibility.Collapsed;
            }
        }

        private void MinimizeButton_OnClick(object Sender, RoutedEventArgs E)
        {
            if (this.Parent is Window win)
            {
                win.WindowState = WindowState.Minimized;
            }
        }

        private void MaximizeButton_OnClick(object Sender, RoutedEventArgs E)
        {
            if (!(this.Parent is Window win)) return;
            win.WindowState = win.WindowState switch
            {
                WindowState.Maximized => WindowState.Normal,
                WindowState.Normal => WindowState.Maximized,
                _ => win.WindowState
            };
        }

        private void CloseButton_OnClick(object Sender, RoutedEventArgs E)
        {
            if (this.Parent is Window win)
            {
                win.Close();
            }
        }
    }
}
