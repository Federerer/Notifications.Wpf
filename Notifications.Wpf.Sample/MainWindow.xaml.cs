using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FontAwesome5;
using Notification.Wpf.Classes;
using WPF.ColorPicker;
using Timer = System.Timers.Timer;

namespace Notification.Wpf.Sample
{
    public partial class MainWindow
    {
        #region ShowLeftButton : bool - Show left button

        /// <summary>Show left button</summary>
        public static readonly DependencyProperty ShowLeftButtonProperty =
            DependencyProperty.Register(
                nameof(ShowLeftButton),
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(default(bool)));

        /// <summary>Show left button</summary>
        public bool ShowLeftButton { get => (bool) GetValue(ShowLeftButtonProperty); set => SetValue(ShowLeftButtonProperty, value); }

        #endregion

        #region ShowRightButton : bool - Show right button

        /// <summary>Show right button</summary>
        public static readonly DependencyProperty ShowRightButtonProperty =
            DependencyProperty.Register(
                nameof(ShowRightButton),
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(true));

        /// <summary>Show right button</summary>
        public bool ShowRightButton { get => (bool) GetValue(ShowRightButtonProperty); set => SetValue(ShowRightButtonProperty, value); }

        #endregion

        #region LeftButtonText : string - Left button text

        /// <summary>Left button text</summary>
        public static readonly DependencyProperty LeftButtonTextProperty =
            DependencyProperty.Register(
                nameof(LeftButtonText),
                typeof(string),
                typeof(MainWindow),
                new PropertyMetadata("Ok"));

        /// <summary>Left button text</summary>
        public string LeftButtonText { get => (string) GetValue(LeftButtonTextProperty); set => SetValue(LeftButtonTextProperty, value); }

        #endregion

        #region RightButtonText : string - Right button text

        /// <summary>Right button text</summary>
        public static readonly DependencyProperty RightButtonTextProperty =
            DependencyProperty.Register(
                nameof(RightButtonText),
                typeof(string),
                typeof(MainWindow),
                new PropertyMetadata("Cancel"));

        /// <summary>Right button text</summary>
        public string RightButtonText { get => (string) GetValue(RightButtonTextProperty); set => SetValue(RightButtonTextProperty, value); }

        #endregion

        #region ContentText : string - Content string

        /// <summary>Content string</summary>
        public static readonly DependencyProperty ContentTextProperty =
            DependencyProperty.Register(
                nameof(ContentText),
                typeof(string),
                typeof(MainWindow),
                new PropertyMetadata(
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis euismod accumsan orci vel varius. Nulla consectetur egestas est,"
                    + " in porttitor elit placerat non. Cras dapibus cursus magna. Nunc ac malesuada lacus. Etiam non luctus magna, nec vulputate diam."
                    + " Sed porta mi at tristique bibendum. Nunc luctus libero ut mauris cursus, eget dignissim est luctus.Sed ac nibh dignissim, elementum mi ut,"
                    + " tempor quam.Donec quis ornare sapien. Maecenas arcu elit, blandit quis odio eu, elementum bibendum leo."
                    + " Etiam iaculis consectetur metus. Donec in bibendum massa. Nam nec facilisis eros, sit amet blandit magna.Duis vitae"
                    + " justo nec nisi maximus efficitur vitae non mauris."));

        /// <summary>Content string</summary>
        public string ContentText { get => (string) GetValue(ContentTextProperty); set => SetValue(ContentTextProperty, value); }

        #endregion

        #region SelectedTrimType : NotificationTextTrimType - способ обрезки текста

        /// <summary>способ обрезки текста</summary>
        public static readonly DependencyProperty SelectedTrimTypeProperty =
            DependencyProperty.Register(
                nameof(SelectedTrimType),
                typeof(NotificationTextTrimType),
                typeof(MainWindow),
                new PropertyMetadata(NotificationTextTrimType.AttachIfMoreRows));

        /// <summary>способ обрезки текста</summary>
        public NotificationTextTrimType SelectedTrimType { get => (NotificationTextTrimType)GetValue(SelectedTrimTypeProperty); set => SetValue(SelectedTrimTypeProperty, value); }

        #endregion

        #region SelectedNotificationType : NotificationType - Тип сообщения

        /// <summary>Тип сообщения</summary>
        public static readonly DependencyProperty SelectedNotificationTypeProperty =
            DependencyProperty.Register(
                nameof(SelectedNotificationType),
                typeof(NotificationType),
                typeof(MainWindow),
                new PropertyMetadata(NotificationType.None));

        /// <summary>Тип сообщения</summary>
        public NotificationType SelectedNotificationType { get => (NotificationType)GetValue(SelectedNotificationTypeProperty); set => SetValue(SelectedNotificationTypeProperty, value); }

        #endregion

        #region RowCount : int - количество строк в сообщении

        /// <summary>количество строк в сообщении</summary>
        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.Register(
                nameof(RowCount),
                typeof(uint),
                typeof(MainWindow),
                new PropertyMetadata(2U));

        /// <summary>количество строк в сообщении</summary>
        public uint RowCount { get => (uint) GetValue(RowCountProperty); set => SetValue(RowCountProperty, value); }

        #endregion
        #region NotifiTypes : NotificationType - Notyfi Type

        /// <summary>Message Type</summary>
        public static readonly DependencyProperty NotifiTypesProperty =
            DependencyProperty.Register(
                nameof(NotifiTypes),
                typeof(IEnumerable<NotificationType>),
                typeof(MainWindow),
                new PropertyMetadata(default(IEnumerable<NotificationType>)));

        /// <summary>Message Type</summary>
        public IEnumerable<NotificationType> NotifiTypes { get => (IEnumerable<NotificationType>)GetValue(NotifiTypesProperty); set => SetValue(NotifiTypesProperty, value); }
        private static IEnumerable<NotificationType> GetTypes() => Enum.GetValues<NotificationType>();
        #endregion

        #region CloseOnClick : bool - Закрыть окно при клике

        /// <summary>Закрыть окно при клике</summary>
        public static readonly DependencyProperty CloseOnClickProperty =
            DependencyProperty.Register(
                nameof(CloseOnClick),
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(true));

        /// <summary>Закрыть окно при клике</summary>
        public bool CloseOnClick { get => (bool)GetValue(CloseOnClickProperty); set => SetValue(CloseOnClickProperty, value); }

        #endregion

        private readonly NotificationManager _notificationManager = new();

        Action ButtonClick(string button) => () => _notificationManager.Show($"{button} button click");

        public MainWindow()
        {
            InitializeComponent();
            BcgButton.Background = new SolidColorBrush(Colors.White);
            FrgButton.Background = new SolidColorBrush(Colors.DarkRed);
            IconFrgButton.Background = new SolidColorBrush(Colors.DarkBlue);
            Icons = GetIcons();
            NotifiTypes = GetTypes();

            Timer = new Timer {Interval = 1000};
            Timer.Elapsed += (_, _) => _notificationManager.Show("Pink string from another thread!");
        }

        private readonly Timer Timer;

        private void Button_Timer(object sender, RoutedEventArgs e)
        {
            if (!Timer.Enabled) Timer.Start();
            else Timer.Stop();
        }

        private async void UpperPanel(object sender, RoutedEventArgs e)
        {
            foreach (var type in NotifiTypes)
            {
                    var content = new NotificationContent
                    {
                        Title = "Sample notification",
                        Message = ContentText,
                        Type = type,
                        LeftButtonAction = ShowLeftButton ? ButtonClick("Left") : null,
                        RightButtonAction = ShowRightButton ? ButtonClick("Right") : null,
                        LeftButtonContent = LeftButtonText,
                        RightButtonContent = RightButtonText,
                        RowsCount = RowCount,
                        TrimType = SelectedTrimType,
                        CloseOnClick = CloseOnClick
                    };
                    _notificationManager.Show(content, expirationTime: TimeSpan.FromSeconds(5));
                    await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
        private void TestMessage(object sender, RoutedEventArgs e)
        {
            var type = SelectedNotificationType;
            var isNone = type == NotificationType.None;
            var content = new NotificationContent
            {
                Title = "Sample notification",
                Message = ContentText,
                Background = isNone? BcgButton.Background:null,
                Foreground = isNone? FrgButton.Background:null,
                Type = type,
                LeftButtonAction = ShowLeftButton ? ButtonClick("Left") : null,
                RightButtonAction = ShowRightButton ? ButtonClick("Right") : null,
                LeftButtonContent = LeftButtonText,
                RightButtonContent = RightButtonText,
                RowsCount = RowCount,
                TrimType = SelectedTrimType,
                CloseOnClick = CloseOnClick,
                Icon = isNone? new SvgAwesome()
                {
                    Icon = (EFontAwesomeIcon)(int)(SelectedIcon ?? new SvgAwesome()).Icon,
                    Height = 25,
                    Foreground = IconFrgButton.Background
                }:
                    null
            };
            _notificationManager.Show(content, expirationTime: TimeSpan.FromSeconds(5));
        }

        private void InWindow(object sender, RoutedEventArgs e)
        {
            var clickContent = new NotificationContent
            {
                Title = "Clicked!",
                Message = "Window notification was clicked!",
                Type = NotificationType.Success,

            };
            var rnd = new Random();
            var content = new NotificationContent
            {
                Title = "Sample notification",
                Message = ContentText,
                Type = (NotificationType) rnd.Next(0, 6),
                LeftButtonAction = ShowLeftButton ? ButtonClick("Left") : null,
                RightButtonAction = ShowRightButton ? ButtonClick("Right") : null,
                LeftButtonContent = LeftButtonText,
                RightButtonContent = RightButtonText,
                RowsCount = RowCount,
                TrimType = SelectedTrimType

            };
            _notificationManager.Show(content, "WindowArea", expirationTime: TimeSpan.FromSeconds(5), onClick: () => _notificationManager.Show(clickContent));

        }

        private async void Progress_Click(object sender, RoutedEventArgs e)
        {
            var title = "Прогресс бар";

            using var progress = _notificationManager.ShowProgressBar(title, true, true, "", true, 2u);
            try
            {
                //await CalcAsync(progress, Cancel).ConfigureAwait(false);

                await Task.Run(
                    async () =>
                    {
                        for (var i = 0; i <= 100; i++)
                        {
                            progress.Cancel.ThrowIfCancellationRequested();
                            progress.Report(
                                (i, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\n"
                                    + "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", null, null));
                            if (i > 30 && i < 70)
                                progress.WaitingTimer.BaseWaitingMessage = null;
                            else
                            {
                                progress.WaitingTimer.BaseWaitingMessage = "Calculation time";

                            }

                            await Task.Delay(TimeSpan.FromSeconds(0.03), progress.Cancel);
                        }
                    }, progress.Cancel).ConfigureAwait(false);

                for (var i = 0; i <= 100; i++)
                {
                    progress.Cancel.ThrowIfCancellationRequested();
                    progress.Report((i, $"Progress {i}", "Whith progress", true));
                    await Task.Delay(TimeSpan.FromSeconds(0.02), progress.Cancel).ConfigureAwait(false);
                }

                for (var i = 0; i <= 100; i++)
                {
                    progress.Cancel.ThrowIfCancellationRequested();
                    progress.Report((null, $"{i}", "Whithout progress", null));
                    await Task.Delay(TimeSpan.FromSeconds(0.03), progress.Cancel).ConfigureAwait(false);
                }

                for (var i = 0; i <= 100; i++)
                {
                    progress.Cancel.ThrowIfCancellationRequested();
                    progress.Report((i, null, "Agane whith progress", null));
                    await Task.Delay(TimeSpan.FromSeconds(0.02), progress.Cancel).ConfigureAwait(false);
                }


            }
            catch (OperationCanceledException)
            {
                _notificationManager.Show("Операция отменена", string.Empty, TimeSpan.FromSeconds(3));
            }
        }


        private void Show_Any_content(object sender, RoutedEventArgs e)
        {
            var grid = new Grid();
            var text_block = new TextBlock {Text = "Some Text", Margin = new Thickness(0, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Center};


            var panelBTN = new StackPanel {Height = 100, Margin = new Thickness(0, 40, 0, 0)};
            var btn1 = new Button {Width = 200, Height = 40, Content = "Cancel"};
            var text = new TextBlock
                {Foreground = Brushes.White, Text = "Hello, world", Margin = new Thickness(0, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Center};
            panelBTN.VerticalAlignment = VerticalAlignment.Bottom;
            panelBTN.Children.Add(btn1);

            //var row1 = new RowDefinition();
            //var row2 = new RowDefinition();
            //var row3 = new RowDefinition();

            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());


            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Children.Add(text_block);
            grid.Children.Add(text);
            grid.Children.Add(panelBTN);

            Grid.SetRow(panelBTN, 1);
            Grid.SetRow(text_block, 0);
            Grid.SetRow(text, 2);

            object content = grid;

            _notificationManager.Show(content, null, TimeSpan.MaxValue);

        }

        public Task CalcAsync(IProgress<(double?, string, string, bool?)> progress, CancellationToken cancel) =>
            Task.Run(
                async () =>
                {
                    for (var i = 0; i <= 100; i++)
                    {
                        cancel.ThrowIfCancellationRequested();
                        progress.Report((i, $"Процесс {i}", null, null));
                        await Task.Delay(TimeSpan.FromSeconds(0.03), cancel);
                    }
                }, cancel);
        public Task CalcAsync(IProgress<(double, string, string)> progress, CancellationToken cancel) =>
            Task.Run(
                async () =>
                {
                    for (var i = 0; i <= 100; i++)
                    {
                        cancel.ThrowIfCancellationRequested();
                        progress.Report((i, $"Процесс {i}", "Title"));
                        await Task.Delay(TimeSpan.FromSeconds(0.03), cancel);
                    }
                }, cancel);
        public Task CalcAsync(IProgress<(double, string)> progress, CancellationToken cancel) =>
            Task.Run(
                async () =>
                {
                    for (var i = 0; i <= 100; i++)
                    {
                        cancel.ThrowIfCancellationRequested();
                        progress.Report((i, $"Процесс {i}"));
                        await Task.Delay(TimeSpan.FromSeconds(0.03), cancel);
                    }
                }, cancel);

        public Task CalcAsync(IProgress<double> progress, CancellationToken cancel) =>
            Task.Run(
                async () =>
                {
                    for (var i = 0; i <= 100; i++)
                    {
                        cancel.ThrowIfCancellationRequested();
                        progress.Report(i);
                        await Task.Delay(TimeSpan.FromSeconds(0.03), cancel);
                    }
                }, cancel);

        #region Color section


        private void ColorSelect_Click(object Sender, RoutedEventArgs E)
        {
            if (!ColorPickerWindow.ShowDialog(out var color))
                return;
            if (Sender is not Button btn)
                return;
            btn.Background = new SolidColorBrush(color);
        }

        #region Icons : IEnumerabSvgAwesome> - Icons

        /// <summary>Icons</summary>
        public static readonly DependencyProperty IconsProperty =
            DependencyProperty.Register(
                nameof(Icons),
                typeof(IEnumerable<SvgAwesome>),
                typeof(MainWindow),
                new PropertyMetadata(default(IEnumerable<SvgAwesome>)));

        /// <summary>Icons</summary>
        public IEnumerable<SvgAwesome> Icons { get => (IEnumerable<SvgAwesome>)GetValue(IconsProperty); set => SetValue(IconsProperty, value); }

        private static IEnumerable<SvgAwesome> GetIcons() => Enum.GetValues<EFontAwesomeIcon>().Select(s => new SvgAwesome() { Icon = s, Height = 20});

        #endregion

        #region SelectedIcon : SvgAwesome - выбранная иконка

        /// <summary>выбранная иконка</summary>
        public static readonly DependencyProperty SelectedIconProperty =
            DependencyProperty.Register(
                nameof(SelectedIcon),
                typeof(SvgAwesome),
                typeof(MainWindow),
                new PropertyMetadata(default(SvgAwesome)));

        /// <summary>выбранная иконка</summary>
        public SvgAwesome SelectedIcon { get => (SvgAwesome)GetValue(SelectedIconProperty); set => SetValue(SelectedIconProperty, value); }

        #endregion
        #endregion

    }
}
