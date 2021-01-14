using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace Notification.Wpf.Controls
{
    public class ProgresBarAnimateBehavior : Behavior<ProgressBar>
    {
        bool _IsAnimating;

        protected override void OnAttached()
        {
            base.OnAttached();
            ProgressBar progressBar = this.AssociatedObject;
            progressBar.ValueChanged += ProgressBar_ValueChanged;
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.OldValue > e.NewValue)
                return;
            if (_IsAnimating)
                return;

            _IsAnimating = true;

            DoubleAnimation doubleAnimation = new DoubleAnimation
                (e.OldValue, e.NewValue, new Duration(TimeSpan.FromSeconds(0.1)), FillBehavior.Stop);
            doubleAnimation.Completed += Db_Completed;

            ((ProgressBar)sender).BeginAnimation(RangeBase.ValueProperty, doubleAnimation);

            e.Handled = true;
        }

        private void Db_Completed(object sender, EventArgs e)
        {
            _IsAnimating = false;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            ProgressBar progressBar = this.AssociatedObject;
            progressBar.ValueChanged -= ProgressBar_ValueChanged;
        }
    }
}