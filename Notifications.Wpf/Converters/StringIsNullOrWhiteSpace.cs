using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Notification.Wpf.Converters;

namespace Notification.Wpf
{
    [ValueConversion(typeof(string), typeof(bool)), MarkupExtensionReturnType(typeof(StringIsNullOrWhiteSpace))]
    internal class StringIsNullOrWhiteSpace : ValueConverter
    {
        /// <inheritdoc />
        public override object Convert(object v, Type t, object p, CultureInfo c) => string.IsNullOrWhiteSpace(v is string str ? str : v?.ToString());
    }
}