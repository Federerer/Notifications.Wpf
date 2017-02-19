using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Notifications.Wpf
{
    public class NotificationTemplateSelector : DataTemplateSelector
    {
        private DataTemplate _defaultStringTemplate;
        private DataTemplate _defaultNotificationTemplate;

        public List<DataTemplate> Templates { private get; set; }

        public NotificationTemplateSelector()
        {
            Templates = new List<DataTemplate>();
        }       

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (_defaultStringTemplate == null)
            {
                _defaultStringTemplate =
                    (container as FrameworkElement)?.TryFindResource("DefaultStringTemplate") as DataTemplate;
                if (_defaultStringTemplate != null && (Type)_defaultStringTemplate.DataType == typeof(string))
                {
                    Templates.Insert(0, _defaultStringTemplate);
                }
            }

            if (_defaultNotificationTemplate == null)
            {
                _defaultNotificationTemplate =
                    (container as FrameworkElement)?.TryFindResource("DefaultNotificationTemplate") as DataTemplate;
                if (_defaultNotificationTemplate != null && (Type)_defaultNotificationTemplate.DataType == typeof(NotificationContent))
                {
                    Templates.Insert(0, _defaultNotificationTemplate);
                }
            }

            return Templates.FirstOrDefault(t => (Type) t.DataType == item?.GetType());
        }
    }
}