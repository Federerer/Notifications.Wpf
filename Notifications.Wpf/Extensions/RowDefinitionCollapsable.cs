using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Notifications.Wpf.Extensions
{
    public class RowDefinitionCollapsable : RowDefinition
    {
        static RowDefinitionCollapsable()
        {
            HeightProperty.OverrideMetadata(
                typeof(RowDefinitionCollapsable),
                new FrameworkPropertyMetadata(
                    new GridLength(1, GridUnitType.Star),
                    null,
                    (d, v) => ((RowDefinitionCollapsable)d).Visible ? v : new GridLength(0)));


            MinHeightProperty.OverrideMetadata(
                typeof(RowDefinitionCollapsable),
                new FrameworkPropertyMetadata(0d, null, (d, v) => ((RowDefinitionCollapsable)d).Visible ? v : 0d));
        }

        #region Visible : bool - Видимость

        /// <summary>Видимость</summary>
        public static readonly DependencyProperty VisibleProperty =
            DependencyProperty.Register(
                nameof(Visible),
                typeof(bool),
                typeof(RowDefinitionCollapsable),
                new PropertyMetadata(true, OnVisibleChanged));

        private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(HeightProperty);
            d.CoerceValue(MinHeightProperty);
        }

        /// <summary>Видимость</summary>
        public bool Visible
        {
            get => (bool)GetValue(VisibleProperty);
            set => SetValue(VisibleProperty, value);
        }

        #endregion
    }
}
