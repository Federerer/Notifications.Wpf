using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using Notification.Wpf.Converters;
using Notifications.Wpf.Command;

namespace Notification.Wpf
{
    [ValueConversion(typeof(Action), typeof(ICommand)), MarkupExtensionReturnType(typeof(BoolInverse))]
    internal class ActionToCommandConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c) => v is not Action act ? null : new LamdaCommand(o => act.Invoke());
    }
}