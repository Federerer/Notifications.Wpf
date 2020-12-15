using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Notification.Wpf.Converters;

namespace Notification.Wpf
{
    [ValueConversion(typeof(bool), typeof(bool)), MarkupExtensionReturnType(typeof(BoolInverse))]
    class BoolInverse : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c) => v is null ? false : (object)!(bool)v;
    } 
    [ValueConversion(typeof(int), typeof(int)), MarkupExtensionReturnType(typeof(MessageRowsCountConverter))]
    class MessageRowsCountConverter : ValueConverter
    {
        public double BaseSize { get; set; }

        public MessageRowsCountConverter(double size = 60)
        {
            BaseSize = size;
        }

        public MessageRowsCountConverter()
        {
            BaseSize = 60;
        }
        public static double ToDouble(object v) => v is null ? double.NaN : v is double d ? d : System.Convert.ToDouble(v);

        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            var multiplier = ToDouble(v);
            if (multiplier == 0) multiplier = 1;
            return BaseSize + 18 * multiplier;
        }
    }
    [ValueConversion(typeof(NotificationTextTrimType), typeof(bool)), MarkupExtensionReturnType(typeof(NotificationTextAttachConverter))]
    class NotificationTextAttachConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (!(v is NotificationTextTrimType type)) return false;
            return type == NotificationTextTrimType.Attach;
        }
    }
    [ValueConversion(typeof(NotificationTextTrimType), typeof(TextTrimming)), MarkupExtensionReturnType(typeof(NotificationTextTrimmingConverter))]
    class NotificationTextTrimmingConverter : ValueConverter
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