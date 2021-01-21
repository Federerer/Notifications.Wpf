using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Notification.Wpf.Converters;

namespace Notification.Wpf
{
    [ValueConversion(typeof(NotificationContent), typeof(Visibility)), MarkupExtensionReturnType(typeof(NotificationAttachVisibleConverter))]
    internal class NotificationAttachVisibleConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (!(v is NotificationContent content)) return Visibility.Collapsed;
            return content.Message.Length < 43 * content.RowsCount ? Visibility.Collapsed : Visibility.Visible;

        }
    }
}