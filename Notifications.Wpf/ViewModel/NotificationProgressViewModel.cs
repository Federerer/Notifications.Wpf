using System;
using System.Threading;
using System.Windows.Markup;
using Caliburn.Micro;

// ReSharper disable once CheckNamespace
namespace Utilities.WPF.Notifications
{
    [MarkupExtensionReturnType(typeof(NotificationProgressViewModel<>))]
    public class NotificationProgressViewModel<T> : PropertyChangedBase
    {
        private readonly CancellationTokenSource _cancel;

        #region Титул окна

        private string _Title;
        public string Title { get => _Title; set => Set(ref _Title, value); }

        #endregion


        #region Message : string - Текст сообщения

        /// <summary>Текст сообщения</summary>
        private string _Message;

        /// <summary>Текст сообщения</summary>
        public string Message { get => _Message; set => Set(ref _Message, value); }

        #endregion

        #region Progress : double - Прогресс задачи

        /// <summary>Прогресс задачи</summary>
        private double _process;

        /// <summary>Прогресс задачи</summary>
        public double process { get => _process; set => Set(ref _process, value); }

        #endregion

        #region IsIndeterminate : bool - Состояние прогресс бара - бегунок или прогресс

        /// <summary>Состояние прогресс бара - бегунок или прогресс</summary>
        private bool _IsIndeterminate;

        /// <summary>Состояние прогресс бара - бегунок или прогресс</summary>
        public bool IsIndeterminate { get => _IsIndeterminate; set => Set(ref _IsIndeterminate, value); }

        #endregion

        /// <summary>
        /// Содержимое левой кнопки
        /// </summary>
        public object RightButtonContent { get; set; } = "Cancel";

        #region ProgressBar : IProgress<(int, string)> - Прогресс

        /// <summary>Прогресс</summary>
        private IProgress<T> _progress;

        /// <summary>Прогресс</summary>
        private IProgress<T> progress
        {
            get => _progress;
            set => Set(ref _progress, value);
        }

        #endregion


        //public NotificationProgressViewModel(T Progress)
        //{
        //    switch (Progress)
        //    {
        //        case IProgress<(double, string, string)> _:
        //            progress = new Progress<T>(OnProgressDblStrStr);
        //            break;
        //        case IProgress<(double, string)> _:
        //            progress = new Progress<T>(OnProgressDblStr);
        //            break;
        //        case IProgress<double> _:
        //            progress = new Progress<T>(OnProgressDbl);
        //            break;
        //    }
        //}

        //private void OnProgressDblStrStr(T ProgressInfo)
        //{
        //    (process, Message, Title) = ProgressInfo as IProgress<(double, string, string)>;
        //}

        //void OnProgressDblStrStr((double percent, string msg, string title) ProgressInfo) =>
        //    (process, Message, Title) = ProgressInfo;

        //void OnProgressDblStr((double percent, string msg) ProgressInfo)
        //{
        //    (process, Message) = ProgressInfo;
        //}

        //void OnProgressDbl(double ProgressInfo) =>
        //    process = ProgressInfo;

        public NotificationProgressViewModel(CancellationTokenSource cancel, IProgress<T> progress)
        {
            _cancel = cancel;
            this.progress= progress;
        }

        #region Close : bool - статус окна

        /// <summary>статус окна</summary>
        private bool _State;

        /// <summary>статус окна</summary>
        private bool State { get => _State; set => Set(ref _State, value); }

        #endregion
        public void Close() => State = true;

        public async void Cancel() => _cancel.Cancel();
    }
}
