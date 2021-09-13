using System;
using System.Globalization;
using Notification.Wpf.Converters;

namespace Notification.Wpf
{
    internal abstract class DoubleValueConverter : ValueConverter
    {
        public static double ToDouble(object v) => v is null ? double.NaN : v is double d ? d : System.Convert.ToDouble(v);
        public static double ToDouble(object v, IFormatProvider format) =>
            v is null ? double.NaN : v is double d ? d : System.Convert.ToDouble(v, format);

        public bool PassNaN { get; set; }

        /// <inheritdoc />
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            var value = ToDouble(v, c);
            if (double.IsNaN(value) && !PassNaN) return value;
            return To(value, p);
        }

        /// <inheritdoc />
        public override object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            var value = ToDouble(v, c);
            if (double.IsNaN(value) && !PassNaN) return value;
            return From(value, p);
        }

        protected abstract double To(double v, object p);
        protected virtual double From(double v, object p) => throw new NotSupportedException();
    }
}