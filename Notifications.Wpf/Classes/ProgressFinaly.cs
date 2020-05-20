using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Notification.Wpf.Classes
{
    public class ProgressFinaly<T> : Progress<T>,IDisposable
    {
        #region IsFinished : bool - progress was finished

        /// <summary>progress was finished</summary>
        public bool IsFinished => _IsFinished;

        /// <summary>progress was finished</summary>
        private bool _IsFinished { get; set; }

        #endregion

        private Controls.Notification Area;
        public ProgressFinaly(Action<T> handler) : base(handler) { }

        public void Report(T value) { base.OnReport(value); }

        public void Dispose()
        {
            _IsFinished = true;
            Application.Current.Dispatcher.Invoke(() => Area.Close());
        }

        public void SetArea(Controls.Notification area)
        {
            Area = area;
        }

    }
}
