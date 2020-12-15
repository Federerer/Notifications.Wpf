using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Notification.Wpf.Converters;

namespace Notification.Wpf
{
    [ValueConversion(typeof(NotificationTextTrimType), typeof(TextTrimming)), MarkupExtensionReturnType(typeof(NotificationTextTrimmingConverter))]
    internal class NotificationTextTrimmingConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (!(v is NotificationTextTrimType type)) return TextTrimming.None;
            return type switch
            {
                NotificationTextTrimType.NoTrim => TextTrimming.None,
                NotificationTextTrimType.Attach => TextTrimming.CharacterEllipsis,
                NotificationTextTrimType.Trim => TextTrimming.CharacterEllipsis,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
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