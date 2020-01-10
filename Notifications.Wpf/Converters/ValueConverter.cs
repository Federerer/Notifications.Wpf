using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using MathCore.Annotations;

namespace Notifications.Wpf.Converters
{
    [MarkupExtensionReturnType(typeof(ValueConverter))]
    public abstract class ValueConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider sp) => this;

        public abstract object Convert(object v, [NotNull] Type t, object p, [NotNull] CultureInfo c);

        public virtual object ConvertBack(object v, [NotNull] Type t, object p, [NotNull] CultureInfo c) => throw new NotSupportedException();
    }
    [MarkupExtensionReturnType(typeof(BoolToVisibility)), ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibility : ValueConverter
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
