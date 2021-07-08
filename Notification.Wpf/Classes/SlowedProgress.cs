using System;
using System.Diagnostics;

namespace Notification.Wpf.Classes
{
    /// <summary>
    /// прогресс с задержкой при обновлении чаще выставленного интервала
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SlowedProgress<T> : Progress<T>, IDisposable
    {
        /// <summary> Время задержки обновления данных в прогрессе </summary>
        private readonly int _UpdateTimeOut;
        public SlowedProgress(Action<T> handler, int UpdateTimeOut) : base(handler)
        {
            _UpdateTimeOut = UpdateTimeOut;
            _Watch.Start();
        }

        private Stopwatch _Watch = new();
        protected override void OnReport(T value)
        {
            if (_Watch.ElapsedMilliseconds <= _UpdateTimeOut)
                return;
            _Watch.Restart();
            base.OnReport(value);
        }

        public void Dispose()
        {
            _Watch = null;
        }

    }
}