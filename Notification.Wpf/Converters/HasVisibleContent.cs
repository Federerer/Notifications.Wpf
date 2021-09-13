using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Notification.Wpf.Converters;

namespace Notification.Wpf
{
    [ValueConversion(typeof(DependencyObject), typeof(bool)), MarkupExtensionReturnType(typeof(HasVisibleContent))]
    internal class HasVisibleContent : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not DependencyObject control)
                return false;
            var childNumber = VisualTreeHelper.GetChildrenCount(control);
            if (childNumber == 0)
                return false;
            for (var i = 0; i < childNumber; i++)
            {
                var child = VisualTreeHelper.GetChild(control, i);
                if (child is Control { Visibility: Visibility.Visible, ActualWidth:>0 })
                    return true;
            }
            return false;
        }

        public override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();
    }
}