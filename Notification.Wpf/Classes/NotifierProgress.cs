using System;
using System.Threading;
using System.Windows;
using Notifications.Wpf.Classes;

namespace Notification.Wpf.Classes
{
    public sealed class NotifierProgress<T> : Progress<T>, IDisposable
    {
        #region IsFinished : bool - progress was finished

        /// <summary>progress was finished</summary>
        public bool IsFinished => _IsFinished;

        /// <summary>progress was finished</summary>
        private bool _IsFinished;

        #endregion

        public readonly OperationTimer WaitingTimer = new();
        private Controls.Notification Area;
        private readonly CancellationTokenSource _CancelSource = new();
        public CancellationTokenSource CancelSource => _CancelSource;
        public CancellationToken Cancel => _CancelSource.Token;


        public NotifierProgress(Action<T> handler, CancellationTokenSource source) : base(handler) { _CancelSource = source; }
        public NotifierProgress(Action<T> handler) : base(handler) { }
        public void Report(T value) => base.OnReport(value);

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
    public static class ProgressExtensions
    {
        static int TimeOut = 50;
        public static IProgress<T> GetProgress<T>(this NotifierProgress<(double?, string, string, bool?)> progress, bool? showCancel)
        {
            if (typeof(T) == typeof(double))
                return (IProgress<T>)new SlowedProgress<double>(
                    d => progress.Report((d, null, null, showCancel)),
                    TimeOut);
            if (typeof(T) == typeof((double, string)))
                return (IProgress<T>)new SlowedProgress<(double, string)>(
                    d => progress.Report((d.Item1, d.Item2, null, showCancel)),
                    TimeOut);
            if (typeof(T) == typeof((double, string, string)))
                return (IProgress<T>)new SlowedProgress<(double, string, string)>(
                    d => progress.Report((d.Item1, d.Item2, d.Item3, showCancel)),
                    TimeOut);
            if (typeof(T) == typeof(int))
                return (IProgress<T>)new SlowedProgress<int>(
                    d => progress.Report((d, null, null, showCancel)),
                    TimeOut);
            if (typeof(T) == typeof((int, string)))
                return (IProgress<T>)new SlowedProgress<(int, string)>(
                    d => progress.Report((d.Item1, d.Item2, null, showCancel)),
                    TimeOut);
            if (typeof(T) == typeof((int, string, string)))
                return (IProgress<T>)new SlowedProgress<(int, string, string)>(
                    d => progress.Report((d.Item1, d.Item2, d.Item3, showCancel)),
                    TimeOut);
            throw new NotSupportedException("type of progress not supported");
        }

    }
}
