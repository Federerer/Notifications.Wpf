using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Notification.Wpf.Converters;

namespace Notification.Wpf
{
    [ValueConversion(typeof(int), typeof(int)), MarkupExtensionReturnType(typeof(MessageRowsCountConverter))]
    internal class MessageRowsCountConverter : ValueConverter
    {
        public double BaseSize { get; set; }

        public MessageRowsCountConverter(double size = 20)
        {
            BaseSize = size;
        }

        public MessageRowsCountConverter()
        {
            BaseSize = 20;
        }
        public static double ToDouble(object v) => v is null ? double.NaN : v is double d ? d : System.Convert.ToDouble(v);

        public override object Convert(object v, Type t, object p, CultureInfo c)
        {

            var multiplier = ToDouble(v);
            if (multiplier == 0) multiplier = 1;
            return BaseSize * multiplier;
        }
    }
}