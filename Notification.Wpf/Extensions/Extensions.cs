using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.Wpf.Extensions
{
    internal static class Extensions
    {
        private class ProgressSelector<T, TSource> : IProgress<TSource>
        {
            private readonly IProgress<T> _Progress;
            private readonly Func<TSource, T> _Selector;

            public ProgressSelector(IProgress<T> Progress, Func<TSource, T> Selector)
            {
                _Progress = Progress;
                _Selector = Selector;
            }

            public void Report(TSource value) => _Progress.Report(_Selector(value));
        }

        public static IProgress<TSource> Select<TSource, T>(this IProgress<T> Progress, Func<TSource, T> Selector) =>
            Progress is null ? null : new ProgressSelector<T,TSource>(Progress, Selector);

        public static async Task<TResult> WhenAny<T, TResult>(
            this IEnumerable<T> items,
            Func<T, CancellationToken, Task<TResult>> Selector)
        {
            var cancellation = new CancellationTokenSource();

            var tasks = items.Select(item =>
            {
                var task = Selector(item, cancellation.Token);
                task.ContinueWith(_ => cancellation.Cancel(), TaskContinuationOptions.OnlyOnRanToCompletion);
                return task;
            }).TakeWhile(_ => !cancellation.IsCancellationRequested);

            var result = await Task.WhenAny(tasks).ConfigureAwait(false);
            return await result;
        }
    }
}
