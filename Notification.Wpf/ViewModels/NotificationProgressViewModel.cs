using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Notification.Wpf;
using Notification.Wpf.Classes;
using Notifications.Wpf.Command;
using Notifications.Wpf.ViewModels.Base;

namespace Notifications.Wpf.ViewModels
{
    public class NotificationProgressViewModel : ViewModel
    {
        public CancellationTokenSource Cancel => NotifierProgress.CancelSource;

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
        private bool _ShowProgress;

        /// <summary>Состояние прогресс бара - бегунок или прогресс</summary>
        public bool ShowProgress { get => _ShowProgress; set => Set(ref _ShowProgress, value); }

        #endregion

        #region ProgressBar : IProgress<(double, string, string,bool)> - Прогресс

        /// <summary>Прогресс</summary>
        private NotifierProgress<(double? percent, string message, string title, bool? showCancel)> _NotifierProgress;

        /// <summary>Прогресс</summary>
        public NotifierProgress<(double? percent, string message, string title, bool? showCancel)> NotifierProgress
        {
            get => _NotifierProgress;
            set => Set(ref _NotifierProgress, value);
        }

        #endregion

        #region ShowCancelButton : bool - видимость кнопки отмены

        /// <summary>видимость кнопки отмены</summary>
        private bool _ShowCancelButton;

        /// <summary>видимость кнопки отмены</summary>
        public bool ShowCancelButton { get => _ShowCancelButton; set => Set(ref _ShowCancelButton, value); }

        #endregion

        #region Collapse : bool - Вид прогресса - свернуть до полосы

        /// <summary>Вид прогресса - свернуть до полосы</summary>
        private bool _Collapse;

        /// <summary>Вид прогресса - свернуть до полосы</summary>
        public bool Collapse
        {
            get => _Collapse;
            set
            {
                Set(ref _Collapse, value);
                GeneralPadding = value ? new Thickness(1) : new Thickness(12);
                BarMargin = value ? new Thickness(1) : new Thickness(5);
                BarHeight = value ? 32 : 20;
            }
        }

        #region GeneralPadding : int - Отступ элементов от внешней рамки

        /// <summary>Отступ элементов от внешней рамки</summary>
        private Thickness _GeneralPadding = new Thickness(12);

        /// <summary>Отступ элементов от внешней рамки</summary>
        public Thickness GeneralPadding { get => _GeneralPadding; set => Set(ref _GeneralPadding, value); }

        #endregion

        #region BarMargin : Thickness - отступ прогресс бара от рамки строки

        /// <summary>Отступ прогресс бара от рамки строки</summary>
        private Thickness _BarMargin = new Thickness(5);

        /// <summary>Отступ прогресс бара от рамки строки</summary>
        public Thickness BarMargin { get => _BarMargin; set => Set(ref _BarMargin, value); }

        #endregion

        #region BarHeight : double - ввысота прогресс бара

        /// <summary>высота прогресс бара</summary>
        private double _BarHeight = 20;

        /// <summary>высота прогресс бара</summary>
        public double BarHeight { get => _BarHeight; set => Set(ref _BarHeight, value); }

        #endregion

        #endregion

        #region CollapseWindowCommand : ICommand - Команда свертывания прогресс бара в строку

        /// <summary>Команда свертывания прогресс бара в строку</summary>
        private ICommand _CollapseWindowCommand;

        /// <summary>Команда свертывания прогресс бара в строку</summary>
        public ICommand CollapseWindowCommand { get => _CollapseWindowCommand; set => Set(ref _CollapseWindowCommand, value); }
        private void CollapseWindow(object Obj)
        {
            Collapse = !Collapse;
        }

        #endregion

        #region TrimType : NotificationTextTrimType - Обрезать сообщения за выходом размера

        /// <summary>Обрезать сообщения за выходом размера</summary>
        private NotificationTextTrimType _TrimType = NotificationTextTrimType.NoTrim;

        /// <summary>Обрезать сообщения за выходом размера</summary>
        public NotificationTextTrimType TrimType { get => _TrimType; set => Set(ref _TrimType, value); }

        #endregion

        #region RowsCount : uint - Число строк текста

        /// <summary>Число строк текста</summary>
        private uint _RowsCount = 2U;

        /// <summary>Число строк текста</summary>
        public uint RowsCount { get => _RowsCount; set => Set(ref _RowsCount, value); }

        #endregion

        #region WaitingTime : string - Время ожидания окончания операции

        /// <summary>Время ожидания окончания операции</summary>
        private string _WaitingTime;

        /// <summary>Время ожидания окончания операции</summary>
        public string WaitingTime { get => _WaitingTime; set => Set(ref _WaitingTime, value); }

        #endregion
        /// <summary>
        /// Содержимое левой кнопки
        /// </summary>
        public object RightButtonContent { get; set; } = "Cancel";

        public NotificationProgressViewModel(bool showCancelButton, bool showProgress, bool trimText, uint DefaultRowsCount, string BaseWaitingMessage)
        {
            ShowProgress = showProgress;
            NotifierProgress = new NotifierProgress<(double? percent, string message, string title, bool? showCancel)>(OnProgress);
            ShowCancelButton = showCancelButton;
            CollapseWindowCommand = new LamdaCommand(CollapseWindow);
            if (trimText)
                TrimType = NotificationTextTrimType.Trim;
            RowsCount = DefaultRowsCount;
            if (BaseWaitingMessage != null) NotifierProgress.WaitingTimer.BaseWaitingMessage = BaseWaitingMessage;
            _Timer.Start();
        }

        private Stopwatch _Timer = new();
        void OnProgress((double? percent, string message, string title, bool? showCancel) ProgressInfo)
        {
            if (_Timer.ElapsedMilliseconds < 100 && ProgressInfo.percent is not null && ProgressInfo.percent != 100 && ProgressInfo.percent != 0) 
                return;
            _Timer.Restart();
            if (ProgressInfo.percent is null)
            {
                if (ShowProgress)
                {
                    ShowProgress = false;
                    NotifierProgress.WaitingTimer.Restart();
                    WaitingTime = string.Empty;
                }
            }
            else
            {
                if (!ShowProgress)
                {
                    ShowProgress = true;
                    NotifierProgress.WaitingTimer.Restart();
                }
                process = (double)ProgressInfo.percent;
                if (NotifierProgress.WaitingTimer.BaseWaitingMessage is null)
                    WaitingTime = null;
                else if (process > 10)
                {
                    WaitingTime = NotifierProgress.WaitingTimer.GetStringTime((double)ProgressInfo.percent, 100);

                }
                else
                    WaitingTime = NotifierProgress.WaitingTimer.BaseWaitingMessage;
                //WaitingTime = NotifierProgress.WaitingTimer.BaseWaitingMessage is null 
                //        ? null
                //        : process > 10
                //            ? NotifierProgress.WaitingTimer.GetStringTime((double)ProgressInfo.percent, 100) 
                //            : NotifierProgress.WaitingTimer.BaseWaitingMessage;
            }
            if (ProgressInfo.message != null) Message = ProgressInfo.message;
            if (ProgressInfo.title != null) Title = ProgressInfo.title;
            if (ProgressInfo.showCancel != null)
                ShowCancelButton = (bool)ProgressInfo.showCancel;
        }


        public void CancelProgress(object Sender, RoutedEventArgs E) => Cancel.Cancel();

    }
}
