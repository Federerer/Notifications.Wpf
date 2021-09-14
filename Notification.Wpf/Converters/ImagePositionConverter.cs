using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Notification.Wpf.Converters
{
    [ValueConversion(typeof(ImagePosition), typeof(bool)), MarkupExtensionReturnType(typeof(ImagePositionConverter))]
    internal class ImagePositionConverter : ValueConverter
    {
        public ImagePosition Position { get; set; }
        public override object Convert(object v, Type t, object p, CultureInfo c) => v is ImagePosition pos && (int)pos == (int)Position;

        public override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();

    }
    [ValueConversion(typeof(ImagePosition), typeof(int)), MarkupExtensionReturnType(typeof(ImagePositionGridRowConverter))]
    internal class ImagePositionGridRowConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not ImagePosition position)
                return null;
            return position switch
            {
                ImagePosition.Top => 0,
                ImagePosition.Bottom => 2,
                _ => 0
            };
        }

        public override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();

    }
    [ValueConversion(typeof(ImagePosition), typeof(Thickness)), MarkupExtensionReturnType(typeof(ImagePositionMarginConverter))]
    internal class ImagePositionMarginConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not ImagePosition position)
                return null;
            return position switch
            {
                ImagePosition.Top=> new Thickness(0,0,0,5),
                ImagePosition.Bottom => new Thickness(0, 5, 0, 0),
                _ => 0
            };
        }

        public override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();

    }
}