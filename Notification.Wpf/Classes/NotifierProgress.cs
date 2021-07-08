using System;
using System.Threading;
using System.Windows;
using Notifications.Wpf.Classes;

namespace Notification.Wpf.Classes
{
    public class NotifierProgress<T> : Progress<T>, IDisposable
    {
        #region IsFinished : bool - progress was finished

        /// <summary>progress was finished</summary>
        public bool IsFinished => _IsFinished;

        /// <summary>progress was finished</summary>
        private bool _IsFinished;

        #endregion

        public readonly OperationTimer WaitingTimer = new();
        private Controls.Notification Area;
        private  readonly CancellationTokenSource _CancelSource = new ();
        public CancellationTokenSource CancelSource => _CancelSource;
        public CancellationToken Cancel => _CancelSource.Token;


        public NotifierProgress(Action<T> handler, CancellationTokenSource source) : base(handler) { _CancelSource = source; }
        public NotifierProgress(Action<T> handler) : base(handler) { }

        public void Report(T value)
        {
            base.OnReport(value);
        }

        public void Dispose()
        {
            _IsFinished = true;
            try
            {
                Application.Current.Dispatcher.Invoke(() => Area?.Close());
                WaitingTimer.Dispose();
            }
            catch
            {
                // ignored
            }
        }

        public void SetArea(Controls.Notification area)
        {
            Area = area;
        }

    }
}
