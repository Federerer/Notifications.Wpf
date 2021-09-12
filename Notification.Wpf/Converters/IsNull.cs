using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Notification.Wpf.Constants;
using Notification.Wpf.Converters;

namespace Notification.Wpf
{
    [ValueConversion(typeof(object), typeof(bool)), MarkupExtensionReturnType(typeof(IsNull))]
    internal class IsNull : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c) => v is null;

        public override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();

    }
    [ValueConversion(typeof(object), typeof(object)), MarkupExtensionReturnType(typeof(IsNull))]
    internal class IconConverter : ValueConverter
    {
        public int Type { get; set; }

        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not NotificationConstants constant)
                return null;
            return Type switch
            {
                0 => NotificationConstants.InformationIcon,
                1 => NotificationConstants.SuccessIcon,
                2 => NotificationConstants.WarningIcon,
                3 => NotificationConstants.ErrorIcon,
                4 => NotificationConstants.NotificationIcon,
                _ => null
            };
        }

        public override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();

    }
}