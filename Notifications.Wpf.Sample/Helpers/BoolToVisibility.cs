using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Notification.Wpf.Converters;

namespace Notification.Wpf.Sample.Helpers
{
    [MarkupExtensionReturnType(typeof(BoolToVisibility)), ValueConversion(typeof(bool), typeof(Visibility))]
    internal class BoolToVisibility : ValueConverter
    {
        public bool Collapse { get; set; }
        public bool Inverse { get; set; }

        public override object Convert(object v, Type t, object parameter, CultureInfo c)
        {
            switch (v)
            {
                case bool bool_value when bool_value: return Inverse ? Collapse ? Visibility.Collapsed : Visibility.Hidden : Visibility.Visible;
                case bool bool_value when !bool_value: return Inverse ? Visibility.Visible : Collapse ? Visibility.Collapsed : Visibility.Hidden;
                case Visibility visible when visible == Visibility.Visible: return !Inverse;
                case Visibility visible when visible == Visibility.Hidden: return Inverse;
                case Visibility visible when visible == Visibility.Collapsed: return Inverse;
            }

            return null;
        }

        public override object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            switch (v)
            {
                case bool bool_value when bool_value: return Inverse ? Collapse ? Visibility.Collapsed : Visibility.Hidden : Visibility.Visible;
                case bool bool_value when !bool_value: return Inverse ? Visibility.Visible : Collapse ? Visibility.Collapsed : Visibility.Hidden;
                case Visibility visible when visible == Visibility.Visible: return !Inverse;
                case Visibility visible when visible == Visibility.Hidden: return Inverse;
                case Visibility visible when visible == Visibility.Collapsed: return Inverse;
            }

            return null;
        }
    }
}