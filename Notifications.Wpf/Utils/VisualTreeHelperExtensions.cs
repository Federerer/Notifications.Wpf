using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Notification.Wpf.Utils
{
    internal class VisualTreeHelperExtensions
    {
        public static T GetParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            
            if (parent == null) return null;

            var tParent  = parent as T;
            if (tParent != null)
            {
                return tParent;
            }

            return GetParent<T>(parent);
        }
    }
}
