using System;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Caliburn.Micro;

// ReSharper disable once CheckNamespace
namespace Utilities.WPF.Notifications
{
    [MarkupExtensionReturnType(typeof(NotificationProgressViewModel))]
    public class NotificationProgressViewModel : PropertyChangedBase
    {
        public readonly CancellationTokenSource Cancel;

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

        #region process : double - Прогресс задачи

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

        #region Close : bool - статус окна

        /// <summary>статус окна</summary>
        private bool _State;

        /// <summary>статус окна</summary>
        private bool State { get => _State; set => Set(ref _State, value); }

        #endregion
        public void Close() => State = true;

        #region ProgressBar : IProgress<(double, string, string,bool)> - Прогресс

        /// <summary>Прогресс</summary>
        private IProgress<(int, string, string, bool?)> _progress;

        /// <summary>Прогресс</summary>
        public IProgress<(int, string, string, bool?)> progress
        {
            get => _progress;
            set => Set(ref _progress, value);
        }

        #endregion

        #region ShowCancelButton : bool - видимость кнопки отмены

        /// <summary>видимость кнопки отмены</summary>
        private bool _ShowCancelButton;

        /// <summary>видимость кнопки отмены</summary>
        public bool ShowCancelButton { get => _ShowCancelButton; set => Set(ref _ShowCancelButton, value); }

        #endregion
        
        /// <summary>
        /// Содержимое левой кнопки
        /// </summary>
        public object RightButtonContent { get; set; } = "Cancel";

        #region ProgressName : string - Имя прогресса для идентификации

        /// <summary>Имя прогресса для идентификации</summary>
        public readonly string ProgressName;

        #endregion
        public NotificationProgressViewModel(string progressName,CancellationTokenSource cancel, bool showCancelButton)
        {
            ProgressName = progressName;
            Cancel = cancel;
            progress= new Progress<(int, string, string, bool?)>(OnProgress);
            ShowCancelButton = showCancelButton;
        }

        void OnProgress((int percent, string msg, string title, bool? showCancel) ProgressInfo)
        {
            process = (double)ProgressInfo.percent;
            Message = ProgressInfo.msg;
            if (ProgressInfo.title != null) Title = ProgressInfo.title;
            if(ProgressInfo.showCancel != null)
                ShowCancelButton = (bool) ProgressInfo.showCancel;
        }

        public void CancelProgress(object Sender, RoutedEventArgs E) => Cancel.Cancel();
    }
}
