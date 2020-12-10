using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Notification.Wpf.Converters;

namespace Notification.Wpf
{
    [ValueConversion(typeof(bool), typeof(bool)), MarkupExtensionReturnType(typeof(BoolInverse))]
    public class BoolInverse : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c) => v is null ? false : (object)!(bool)v;
    }
    [ValueConversion(typeof(NotificationTextTrimType), typeof(bool)), MarkupExtensionReturnType(typeof(NotificationTextAttachConverter))]
    public class NotificationTextAttachConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (!(v is NotificationTextTrimType type)) return false;
            return type == NotificationTextTrimType.Attach;
        }
    }
    [ValueConversion(typeof(NotificationTextTrimType), typeof(TextTrimming)), MarkupExtensionReturnType(typeof(NotificationTextTextTrimmingConverter))]
    public class NotificationTextTextTrimmingConverter : ValueConverter
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
}