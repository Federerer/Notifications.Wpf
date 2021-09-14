using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Notification.Wpf.Converters;

namespace Notification.Wpf
{
    [ValueConversion(typeof(double), typeof(bool)), MarkupExtensionReturnType(typeof(GreateThenConverter))]
    internal class GreateThenConverter : ValueConverter
    {
        public double Value { get; set; }

        public GreateThenConverter() { }

        public GreateThenConverter(double value) => Value = value;

        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            var value = DoubleValueConverter.ToDouble(v);
            var result = double.IsNaN(value) ? (object)false : value > Value;
            return result;
        }
    }
}