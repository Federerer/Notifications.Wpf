using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Notification.Wpf
{
    public class NotificationTemplateSelector : DataTemplateSelector
    {
        private DataTemplate _defaultStringTemplate;
        private DataTemplate _defaultNotificationTemplate;

        private void GetTemplatesFromResources(FrameworkElement container)
        {
            _defaultStringTemplate =
                    container?.FindResource("DefaultStringTemplate") as DataTemplate;
            _defaultNotificationTemplate =
                    container?.FindResource("DefaultNotificationTemplate") as DataTemplate;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (_defaultStringTemplate == null && _defaultNotificationTemplate == null)
            {
                GetTemplatesFromResources((FrameworkElement)container);                            
            }           

            if (item is string)
            {
                return _defaultStringTemplate;
            }
            if (item is NotificationContent)
            {
                return _defaultNotificationTemplate;
            }

            return base.SelectTemplate(item, container);

        }
    }
}