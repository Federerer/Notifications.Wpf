using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using FontAwesome5;
using Notification.Wpf.Converters;

namespace Notifications.Wpf.Sample.Helpers
{
    [ValueConversion(typeof(object), typeof(object)), MarkupExtensionReturnType(typeof(IconConverter))]
    internal class IconConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c) => v is SvgAwesome icon? (EFontAwesomeIcon)(int)icon.Icon:null;

        public override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();

    }
}