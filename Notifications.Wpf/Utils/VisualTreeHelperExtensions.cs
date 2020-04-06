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

        private static List<Visual> _activeControls = new List<Visual>();

        public static T GetParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);

            if (parent == null) return null;

            var tParent = parent as T;
            if (tParent != null)
            {
                return tParent;
            }

            return GetParent<T>(parent);
        }

        public static int GetActiveNotificationCount(Visual element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(String.Format("Element {0} is null !", element.ToString()));
            }

            _activeControls.Clear();

            GetControlsList(element, 0);

            var count = _activeControls.Count(x => x.GetType().Name.Equals("Notification"));

            return count;
        }

        private static void GetControlsList(Visual control, int level)
        {
            const int indent = 4;
            int ChildNumber = VisualTreeHelper.GetChildrenCount(control);

            for (int i = 0; i <= ChildNumber - 1; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(control, i);

                _activeControls.Add(v);

                if (VisualTreeHelper.GetChildrenCount(v) > 0)
                {
                    GetControlsList(v, level + 1);
                }
            }
        }

    }
}
