using System;
using System.Threading;
using System.Windows;
using Notification.Wpf.Extensions;
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
        #region Sub operations

        /// <summary>
        /// выбор операции над числом
        /// </summary>
        /// <param name="value">значение</param>
        /// <param name="MultyOrDeVide100">null - вернет значение без изменений, true - домножит на 100, false - разделит на 100</param>
        /// <returns></returns>
        private static double ChooseValueDouble(double value, bool? MultyOrDeVide100 = default)
            => MultyOrDeVide100 is null ? value : MultyOrDeVide100 is true ? value * 100 : value / 100;
        /// <summary>
        /// выбор операции над числом
        /// </summary>
        /// <param name="value">значение</param>
        /// <param name="MultyOrDeVide100">null - вернет значение без изменений, true - домножит на 100, false - разделит на 100</param>
        /// <returns></returns>
        private static int ChooseValueInt(int value, bool? MultyOrDeVide100 = default)
            => MultyOrDeVide100 is null ? value : MultyOrDeVide100 is true ? value * 100 : value / 100;

        #endregion

        /// <summary>
        /// получить прогресс типа T - (double, (double,string), (double,string,string), int, (int,string), (int,string,string)
        /// </summary>
        /// <typeparam name="T">тип прогресса </typeparam>
        /// <param name="progress">NotifierProgress</param>
        /// <param name="showCancel">showCancel</param>
        /// <param name="MultyOrDeVide100">null - вернет значение без изменений, true - домножит на 100, false - разделит на 100</param>
        /// <returns></returns>
        public static IProgress<T> GetProgress<T>(this IProgress<(double?, string, string, bool?)> progress, bool? showCancel, bool? MultyOrDeVide100 = default)
        {
            if (typeof(T) == typeof(double))
                return (IProgress<T>)progress.Select<double, (double?, string, string, bool?)>(
                    p => (ChooseValueDouble(p,MultyOrDeVide100), null, null, showCancel));

            if (typeof(T) == typeof((double, string)))
                return (IProgress<T>) progress.Select<(double, string), (double?, string, string, bool?)>(
                    p => (ChooseValueDouble(p.Item1, MultyOrDeVide100), p.Item2, null, showCancel));
            if (typeof(T) == typeof((double, string, string)))
                return (IProgress<T>) progress.Select<(double, string, string), (double?, string, string, bool?)>(
                    p => (ChooseValueDouble(p.Item1, MultyOrDeVide100), p.Item2, p.Item3, showCancel));
            if (typeof(T) == typeof(int))
                return (IProgress<T>) progress.Select<int, (double?, string, string, bool?)>(
                    p => (ChooseValueInt(p, MultyOrDeVide100), null, null, showCancel));
            if (typeof(T) == typeof((int, string)))
                return (IProgress<T>) progress.Select<(int, string), (double?, string, string, bool?)>(
                    p => (ChooseValueInt(p.Item1, MultyOrDeVide100), p.Item2, null, showCancel));
            if (typeof(T) == typeof((int, string, string)))
                return (IProgress<T>) progress.Select<(int, string, string), (double?, string, string, bool?)>(
                    p => (ChooseValueInt(p.Item1, MultyOrDeVide100), p.Item2, p.Item3, showCancel));

            throw new NotSupportedException("type of progress not supported");
        }

        /// <summary>
        /// получить прогресс типа T c задержкой на пропуск частых сообщений
        /// тип прогрессов - (double, (double,string), (double,string,string), int, (int,string), (int,string,string)
        /// </summary>
        /// <typeparam name="T">тип прогресса </typeparam>
        /// <param name="progress">NotifierProgress</param>
        /// <param name="showCancel">showCancel</param>
        /// <param name="MultyOrDeVide100">null - вернет значение без изменений, true - домножит на 100, false - разделит на 100</param>
        /// <param name="UpdateTimeOut">время задержки в миллисекундах</param>
        /// <returns></returns>
        public static IProgress<T> GetSlowedProgress<T>(
            this IProgress<(double?, string, string, bool?)> progress,
            bool? showCancel,
            bool? MultyOrDeVide100 = default,
            int UpdateTimeOut = 50)
        {
            if (typeof(T) == typeof(double))
                return (IProgress<T>)new SlowedProgress<double>(d => progress.Select<double, (double?, string, string, bool?)>(
                    p => (ChooseValueDouble(p, MultyOrDeVide100), null, null, showCancel)).Report(d), UpdateTimeOut);

            if (typeof(T) == typeof((double, string)))
                return (IProgress<T>)new SlowedProgress<(double, string)>(d => progress.Select<(double, string), (double?, string, string, bool?)>(
                    p => (ChooseValueDouble(p.Item1, MultyOrDeVide100), p.Item2, null, showCancel)).Report((d.Item1, d.Item2)), UpdateTimeOut);
            if (typeof(T) == typeof((double, string, string)))
                return (IProgress<T>)new SlowedProgress<(double, string, string)>(d => progress.Select<(double, string, string), (double?, string, string, bool?)>(
                    p => (ChooseValueDouble(p.Item1, MultyOrDeVide100), p.Item2, p.Item3, showCancel)).Report((d.Item1, d.Item2, d.Item3)), UpdateTimeOut);
            if (typeof(T) == typeof(int))
                return (IProgress<T>)new SlowedProgress<int>(d => progress.Select<int, (double?, string, string, bool?)>(
                    p => (ChooseValueInt(p, MultyOrDeVide100), null, null, showCancel)).Report(d),UpdateTimeOut);
            if (typeof(T) == typeof((int, string)))
                return (IProgress<T>)new SlowedProgress<(int, string)>(d => progress.Select<(int, string), (double?, string, string, bool?)>(
                    p => (ChooseValueInt(p.Item1, MultyOrDeVide100), p.Item2, null, showCancel)).Report((d.Item1, d.Item2)), UpdateTimeOut);
            if (typeof(T) == typeof((int, string, string)))
                return (IProgress<T>)new SlowedProgress<(int, string, string)>(d => progress.Select<(int, string, string), (double?, string, string, bool?)>(
                    p => (ChooseValueInt(p.Item1, MultyOrDeVide100), p.Item2, p.Item3, showCancel)).Report((d.Item1, d.Item2, d.Item3)),UpdateTimeOut);

            throw new NotSupportedException("type of progress not supported");
        }
    }
}
