using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FontAwesome5;
using Microsoft.Win32;
using Notification.Wpf.Constants;
using Notification.Wpf.Sample.Elements;
using WPF.ColorPicker;
using Timer = System.Timers.Timer;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using Notification.Wpf.Base;
using Notification.Wpf.Classes;
using Notification.Wpf.Controls;

namespace Notification.Wpf.Sample
{
    public partial class MainWindow
    {
        #region Overlay window

        #region CollapseProgressIfMoreRows : bool - progress collapse auto

        /// <summary>progress collapse auto</summary>
        public static readonly DependencyProperty CollapseProgressIfMoreRowsProperty =
            DependencyProperty.Register(
                nameof(CollapseProgressIfMoreRows),
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(NotificationConstants.CollapseProgressIfMoreRows, (O, Args) =>
                {
                    NotificationConstants.CollapseProgressIfMoreRows = (bool)Args.NewValue;
                }));

        /// <summary>progress collapse auto</summary>
        public bool CollapseProgressIfMoreRows
        {
            get => (bool)GetValue(CollapseProgressIfMoreRowsProperty);
            set => SetValue(CollapseProgressIfMoreRowsProperty, value);
        }

        #endregion

        #region MaxItems : uint - Items max count

        /// <summary>Items max count</summary>
        public static readonly DependencyProperty MaxItemsProperty =
            DependencyProperty.Register(
                nameof(MaxItems),
                typeof(uint),
                typeof(MainWindow),
                new PropertyMetadata(NotificationConstants.NotificationsOverlayWindowMaxCount, (O, Args) =>
                {
                    NotificationConstants.NotificationsOverlayWindowMaxCount = (uint)Args.NewValue;
                }));

        /// <summary>Items max count</summary>
        public uint MaxItems
        {
            get => (uint)GetValue(MaxItemsProperty);
            set => SetValue(MaxItemsProperty, value);
        }

        #endregion

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

        #region Buttons

        #region ShowLeftButton : bool - Show left button

        /// <summary>Show left button</summary>
        public static readonly DependencyProperty ShowLeftButtonProperty =
            DependencyProperty.Register(
                nameof(ShowLeftButton),
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(default(bool)));

        /// <summary>Show left button</summary>
        public bool ShowLeftButton { get => (bool)GetValue(ShowLeftButtonProperty); set => SetValue(ShowLeftButtonProperty, value); }

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
        public bool ShowRightButton { get => (bool)GetValue(ShowRightButtonProperty); set => SetValue(ShowRightButtonProperty, value); }

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
        public string LeftButtonText { get => (string)GetValue(LeftButtonTextProperty); set => SetValue(LeftButtonTextProperty, value); }

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
        public string RightButtonText { get => (string)GetValue(RightButtonTextProperty); set => SetValue(RightButtonTextProperty, value); }

        #endregion


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
        public string ContentText { get => (string)GetValue(ContentTextProperty); set => SetValue(ContentTextProperty, value); }

        #endregion

        #region SelectedTrimType : NotificationTextTrimType - способ обрезки текста

        /// <summary>способ обрезки текста</summary>
        public static readonly DependencyProperty SelectedTrimTypeProperty =
            DependencyProperty.Register(
                nameof(SelectedTrimType),
                typeof(NotificationTextTrimType),
                typeof(MainWindow),
                new PropertyMetadata(NotificationTextTrimType.Trim));

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
                new PropertyMetadata(NotificationConstants.DefaultRowCounts));

        /// <summary>количество строк в сообщении</summary>
        public uint RowCount
        {
            get => (uint)GetValue(RowCountProperty);
            set => SetValue(RowCountProperty, value);
        }

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

        #region ShowInWindow : bool - Show notification In Window

        /// <summary>ShowInWindow</summary>
        public static readonly DependencyProperty ShowInWindowProperty =
            DependencyProperty.Register(
                nameof(ShowInWindow),
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(default(bool)));

        /// <summary>ShowInWindow</summary>
        public bool ShowInWindow { get => (bool)GetValue(ShowInWindowProperty); set => SetValue(ShowInWindowProperty, value); }

        private string GetArea() => ShowInWindow ? "WindowArea" : "";
        #endregion

        #region UseTitleSettings : bool - Использовать настройки для Title

        /// <summary>Использовать настройки для Title</summary>
        public static readonly DependencyProperty UseTitleSettingsProperty =
            DependencyProperty.Register(
                nameof(UseTitleSettings),
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(default(bool)));

        /// <summary>Использовать настройки для Title</summary>
        public bool UseTitleSettings { get => (bool)GetValue(UseTitleSettingsProperty); set => SetValue(UseTitleSettingsProperty, value); }

        #endregion

        #region UseMessageSettings : bool - Использовать настройки для Message

        /// <summary>Использовать настройки для Message</summary>
        public static readonly DependencyProperty UseMessageSettingsProperty =
            DependencyProperty.Register(
                nameof(UseMessageSettings),
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(default(bool)));

        /// <summary>Использовать настройки для Message</summary>
        public bool UseMessageSettings { get => (bool)GetValue(UseMessageSettingsProperty); set => SetValue(UseMessageSettingsProperty, value); }

        #endregion

        #region ProgressCollapsed : bool - ProgressCollapsed

        /// <summary>ProgressCollapsed</summary>
        public static readonly DependencyProperty ProgressCollapsedProperty =
            DependencyProperty.Register(
                nameof(ProgressCollapsed),
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(default(bool)));

        /// <summary>ProgressCollapsed</summary>
        public bool ProgressCollapsed
        {
            get => (bool)GetValue(ProgressCollapsedProperty);
            set => SetValue(ProgressCollapsedProperty, value);
        }

        #endregion

        #region ProgressTitleOrMessage : bool - ProgressTitleOrMessage

        /// <summary>ProgressTitleOrMessage</summary>
        public static readonly DependencyProperty ProgressTitleOrMessageProperty =
            DependencyProperty.Register(
                nameof(ProgressTitleOrMessage),
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(default(bool)));

        /// <summary>ProgressTitleOrMessage</summary>
        public bool ProgressTitleOrMessage { get => (bool)GetValue(ProgressTitleOrMessageProperty); set => SetValue(ProgressTitleOrMessageProperty, value); }

        #endregion

        #region Colors

        #region ContentBackground : SolidColorBrush - фон сообщения

        /// <summary>фон сообщения</summary>
        public static readonly DependencyProperty ContentBackgroundProperty =
            DependencyProperty.Register(
                nameof(ContentBackground),
                typeof(SolidColorBrush),
                typeof(MainWindow),
                new PropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#FF444444")));

        /// <summary>фон сообщения</summary>
        public SolidColorBrush ContentBackground { get => (SolidColorBrush)GetValue(ContentBackgroundProperty); set => SetValue(ContentBackgroundProperty, value); }

        #endregion

        #region ContentForeground : SolidColorBrush - цвет текста сообщения

        /// <summary>цвет текста сообщения</summary>
        public static readonly DependencyProperty ContentForegroundProperty =
            DependencyProperty.Register(
                nameof(ContentForeground),
                typeof(SolidColorBrush),
                typeof(MainWindow),
                new PropertyMetadata(new SolidColorBrush(Colors.WhiteSmoke)));

        /// <summary>цвет текста сообщения</summary>
        public SolidColorBrush ContentForeground { get => (SolidColorBrush)GetValue(ContentForegroundProperty); set => SetValue(ContentForegroundProperty, value); }

        #endregion

        #region IconForeground : SolidColorBrush - цвет иконки

        /// <summary>цвет иконки</summary>
        public static readonly DependencyProperty IconForegroundProperty =
            DependencyProperty.Register(
                nameof(IconForeground),
                typeof(SolidColorBrush),
                typeof(MainWindow),
                new PropertyMetadata(new SolidColorBrush(Colors.WhiteSmoke)));

        /// <summary>цвет иконки</summary>
        public SolidColorBrush IconForeground { get => (SolidColorBrush)GetValue(IconForegroundProperty); set => SetValue(IconForegroundProperty, value); }

        #endregion

        #region ProgressColor : SolidColorBrush - Цвет прогресс бара

        /// <summary>Цвет прогресс бара</summary>
        public static readonly DependencyProperty ProgressColorProperty =
            DependencyProperty.Register(
                nameof(ProgressColor),
                typeof(SolidColorBrush),
                typeof(MainWindow),
                new PropertyMetadata((Brush)new BrushConverter().ConvertFrom("#FF01D328")));

        /// <summary>Цвет прогресс бара</summary>
        public SolidColorBrush ProgressColor { get => (SolidColorBrush)GetValue(ProgressColorProperty); set => SetValue(ProgressColorProperty, value); }

        #endregion

        #endregion

        #region ImageAsIcon : bool - Use image as Icon

        /// <summary>Use image as Icon</summary>
        public static readonly DependencyProperty ImageAsIconProperty =
            DependencyProperty.Register(
                nameof(ImageAsIcon),
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(default(bool)));

        /// <summary>Use image as Icon</summary>
        public bool ImageAsIcon { get => (bool)GetValue(ImageAsIconProperty); set => SetValue(ImageAsIconProperty, value); }

        #endregion

        #region ExpirationTime : int - Время показа сообщения

        /// <summary>Время показа сообщения</summary>
        public static readonly DependencyProperty ExpirationTimeProperty =
            DependencyProperty.Register(
                nameof(ExpirationTime),
                typeof(int),
                typeof(MainWindow),
                new PropertyMetadata(5));

        /// <summary>Время показа сообщения</summary>
        public int ExpirationTime { get => (int)GetValue(ExpirationTimeProperty); set => SetValue(ExpirationTimeProperty, value); }

        #endregion

        #region UseExpirationTime : bool - Скрывать сообщение по таймеру

        /// <summary>Скрывать сообщение по таймеру</summary>
        public static readonly DependencyProperty UseExpirationTimeProperty =
            DependencyProperty.Register(
                nameof(UseExpirationTime),
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(true));

        /// <summary>Скрывать сообщение по таймеру</summary>
        public bool UseExpirationTime { get => (bool)GetValue(UseExpirationTimeProperty); set => SetValue(UseExpirationTimeProperty, value); }

        #endregion

        private readonly NotificationManager _notificationManager = new();

        Action ButtonClick(string button) => () => _notificationManager.Show($"{button} button click");

        public MainWindow()
        {
            //NotificationConstants.FontName = "Segoe";
            //NotificationConstants.BaseTextSize = 10;
            //NotificationConstants.TitleSize = 20;
            //NotificationConstants.MessageSize = 16;
            //NotificationConstants.TitleTextAlignment = TextAlignment.Center;
            //NotificationConstants.MessageTextAlignment = TextAlignment.Center;
            InitializeComponent();
            Icons = GetIcons();
            NotifiTypes = GetTypes();
            Image = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Test image.png")));
            Timer = new Timer { Interval = 1000 };
            Timer.Elapsed += (_, _) => Dispatcher.Invoke(() => _notificationManager.Show("Pink string from another thread!", areaName: GetArea()));
        }

        private readonly Timer Timer;

        private void Button_Timer(object sender, RoutedEventArgs e)
        {
            if (!Timer.Enabled) Timer.Start();
            else Timer.Stop();
        }

        private void TestMessage(object sender, RoutedEventArgs e)
        {
            var type = SelectedNotificationType;
            var isNone = type == NotificationType.None;
            var clickContent = new NotificationContent
            {
                Title = "Clicked!",
                Message = "Window notification was clicked!",
                Type = NotificationType.Success,
            };
            CustomizeMessage(clickContent);

            var content = new NotificationContent
            {
                Title = "Sample notification",
                Message = ContentText,
                Background = isNone ? ContentBackground : null,
                Foreground = isNone ? ContentForeground : null,
                Type = type,
                LeftButtonAction = ShowLeftButton ? ButtonClick("Left") : null,
                RightButtonAction = ShowRightButton ? ButtonClick("Right") : null,
                LeftButtonContent = LeftButtonText,
                RightButtonContent = RightButtonText,
                RowsCount = RowCount,
                TrimType = SelectedTrimType,
                CloseOnClick = CloseOnClick,
                Icon = ImageAsIcon ? Image : isNone ? new SvgAwesome()
                {
                    Icon = (EFontAwesomeIcon)(int)(SelectedIcon ?? new SvgAwesome()).Icon,
                    Height = 25,
                    Foreground = IconForeground
                } :
                    null,
                Image = new NotificationImage() { Source = Image, Position = SelectedImgPosition },
                //TitleTextSettings = new TextContentSettings()
                //{
                //    FontStyle = TitleSettings.FontStyle,
                //    FontFamily = TitleSettings.FontFamily,
                //    FontSize = TitleSettings.FontSize,
                //    FontWeight = TitleSettings.FontWeight,
                //    TextAlignment = TitleSettings.TextAlign,
                //    HorizontalAlignment = TitleSettings.HorizontalAlignment,
                //    VerticalTextAlignment = TitleSettings.VerticalAlignment
                //},
                //MessageTextSettings = new TextContentSettings()
                //{
                //    FontStyle = MessageSettings.FontStyle,
                //    FontFamily = MessageSettings.FontFamily,
                //    FontSize = MessageSettings.FontSize,
                //    FontWeight = MessageSettings.FontWeight,
                //    TextAlignment = MessageSettings.TextAlign,
                //    HorizontalAlignment = MessageSettings.HorizontalAlignment,
                //    VerticalTextAlignment = MessageSettings.VerticalAlignment
                //},
            };
            CustomizeMessage(content);

            _notificationManager.Show(content,
                areaName: GetArea(),
                expirationTime: UseExpirationTime ? TimeSpan.FromSeconds(ExpirationTime) : TimeSpan.MaxValue,
                onClick: CloseOnClick ? () => _notificationManager.Show(clickContent) : null);
        }

        private void CustomizeMessage(ICustomizedNotification content)
        {
            if (UseTitleSettings)
            {
                content.TitleTextSettings.FontStyle = TitleSettings.FontStyle;
                content.TitleTextSettings.FontFamily = TitleSettings.FontFamily;
                content.TitleTextSettings.FontSize = TitleSettings.FontSize;
                content.TitleTextSettings.FontWeight = TitleSettings.FontWeight;
                content.TitleTextSettings.TextAlignment = TitleSettings.TextAlign;
                content.TitleTextSettings.HorizontalAlignment = TitleSettings.HorizontalAlignment;
                content.TitleTextSettings.VerticalTextAlignment = TitleSettings.VerticalAlignment;
            }

            if (UseMessageSettings)
            {
                content.MessageTextSettings.FontStyle = MessageSettings.FontStyle;
                content.MessageTextSettings.FontFamily = MessageSettings.FontFamily;
                content.MessageTextSettings.FontSize = MessageSettings.FontSize;
                content.MessageTextSettings.FontWeight = MessageSettings.FontWeight;
                content.MessageTextSettings.TextAlignment = MessageSettings.TextAlign;
                content.MessageTextSettings.HorizontalAlignment = MessageSettings.HorizontalAlignment;
                content.MessageTextSettings.VerticalTextAlignment = MessageSettings.VerticalAlignment;
            }

        }
        private async void Progress_Click(object sender, RoutedEventArgs e)
        {
            var iconN = SelectedIcon is null ? 0 : (int)SelectedIcon.Icon;
            var title = "Прогресс бар";

            //#region First sample

            //using var progress = _notificationManager.ShowProgressBar(
            //    title,
            //    true,
            //    true,
            //    GetArea(),
            //    SelectedTrimType == NotificationTextTrimType.Trim,
            //    2u,
            //    IsCollapse: ProgressCollapsed,
            //    TitleWhenCollapsed: ProgressTitleOrMessage,
            //    progressColor: ProgressColor,
            //    background: ContentBackground,
            //    foreground: ContentForeground, icon: iconN == 0 ? null : new SvgAwesome()
            //    {
            //        Icon = (EFontAwesomeIcon)iconN,
            //        Height = 25,
            //        Foreground = IconForeground
            //    });

            //#endregion
            #region Second sample

            var content = new BaseNotificationContent()
            {
                Title = title,
                Message = "Test message",
                Background = ContentBackground,
                Foreground = ContentForeground,
                TrimType = SelectedTrimType,
                Icon = ImageAsIcon ? Image : iconN == 0 ? null : new SvgAwesome()
                {
                    Icon = (EFontAwesomeIcon)iconN,
                    Height = 25,
                    Foreground = IconForeground
                },
                RowsCount = RowCount,
                //TitleTextSettings = new TextContentSettings()
                //    {
                //        FontStyle = TitleSettings.FontStyle,
                //        FontFamily = TitleSettings.FontFamily,
                //        FontSize = TitleSettings.FontSize,
                //        FontWeight = TitleSettings.FontWeight,
                //        TextAlignment = TitleSettings.TextAlign,
                //        HorizontalAlignment = TitleSettings.HorizontalAlignment,
                //        VerticalTextAlignment = TitleSettings.VerticalAlignment
                //    },
                //MessageTextSettings = new TextContentSettings()
                //    {
                //        FontStyle = MessageSettings.FontStyle,
                //        FontFamily = MessageSettings.FontFamily,
                //        FontSize = MessageSettings.FontSize,
                //        FontWeight = MessageSettings.FontWeight,
                //        TextAlignment = MessageSettings.TextAlign,
                //        HorizontalAlignment = MessageSettings.HorizontalAlignment,
                //        VerticalTextAlignment = MessageSettings.VerticalAlignment
                //    },
            };
            CustomizeMessage(content);
            using var progress = _notificationManager.ShowProgressBar(
                content,
                true,
                true,
                GetArea(),
                IsCollapse: ProgressCollapsed,
                TitleWhenCollapsed: ProgressTitleOrMessage, progressColor: ProgressColor);

            #endregion
            try
            {
                var message = ContentText;

                await Task.Run(
                    async () =>
                    {
                        for (var i = 0; i <= 100; i++)
                        {
                            progress.Cancel.ThrowIfCancellationRequested();
                            progress.Report((i, message, null, null));
                            progress.WaitingTimer.BaseWaitingMessage = i is > 30 and < 70 ? null : "Calculation time";

                            await Task.Delay(TimeSpan.FromSeconds(0.03), progress.Cancel);
                        }
                    }, progress.Cancel).ConfigureAwait(false);

                for (var i = 0; i <= 100; i++)
                {
                    progress.Cancel.ThrowIfCancellationRequested();
                    progress.Report((i, $"Progress {i}", "With progress", true));
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
            var text_block = new TextBlock { Text = "Some Text", Margin = new Thickness(0, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Center };


            var panelBTN = new StackPanel { Height = 100, Margin = new Thickness(0, 40, 0, 0) };
            var btn1 = new Button { Width = 200, Height = 40, Content = "Cancel" };
            var text = new TextBlock
            { Foreground = Brushes.White, Text = "Hello, world", Margin = new Thickness(0, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Center };
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

        #region Progress async calc

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

        #endregion

        #region Color section


        private void BackgroundColorSelect_Click(object Sender, RoutedEventArgs E)
        {
            if (!ColorPickerWindow.ShowDialog(out var color))
                return;
            if (Sender is not Button btn)
                return;
            ContentBackground = new SolidColorBrush(color);

        }
        private void ForegroundColorSelect_Click(object Sender, RoutedEventArgs E)
        {
            if (!ColorPickerWindow.ShowDialog(out var color))
                return;
            if (Sender is not Button btn)
                return;
            ContentForeground = new SolidColorBrush(color);

        }
        private void IconColorSelect_Click(object Sender, RoutedEventArgs E)
        {
            if (!ColorPickerWindow.ShowDialog(out var color))
                return;
            if (Sender is not Button btn)
                return;
            IconForeground = new SolidColorBrush(color);
        }
        private void ProgressColorSelect_Click(object Sender, RoutedEventArgs E)
        {
            if (!ColorPickerWindow.ShowDialog(out var color))
                return;
            if (Sender is not Button btn)
                return;
            ProgressColor = new SolidColorBrush(color);
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

        private static IEnumerable<SvgAwesome> GetIcons() => Enum.GetValues<EFontAwesomeIcon>().Select(s => new SvgAwesome() { Icon = s, Height = 20 });

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

        private void NumericUpDownControl_OnValueChanged(object Sender, RoutedEventArgs E)
        {
            if (Sender is not NumericUpDownControl num)
                return;
            var value = num.Value;
            NotificationConstants.NotificationsOverlayWindowMaxCount = (uint)value;
        }

        private void OpenImage_Click(object Sender, RoutedEventArgs E)
        {
            var openFileDialog = new OpenFileDialog();

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;

            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                openFileDialog.Filter = string.Format("{0}{1}{2} ({3})|{3}", openFileDialog.Filter, sep, codecName, c.FilenameExtension);
                sep = "|";
            }

            openFileDialog.Filter = string.Format("{0}{1}{2} ({3})|{3}", openFileDialog.Filter, sep, "All Files", "*.*");

            openFileDialog.DefaultExt = ".png"; // Default file extension 
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    Image = new BitmapImage(new Uri(openFileDialog.FileName));
                }
            }
            catch (Exception e)
            {
                _notificationManager.Show("Error", e.Message, type: NotificationType.Error);
            }
        }

        #region Image : ImageSource - Image

        /// <summary>Image</summary>
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
                nameof(Image),
                typeof(ImageSource),
                typeof(MainWindow),
                new PropertyMetadata(default));

        /// <summary>Image</summary>
        public ImageSource Image { get => (ImageSource)GetValue(ImageProperty); set => SetValue(ImageProperty, value); }

        #endregion

        #region SelectedImgPosition : ImagePosition - Image position

        /// <summary>Image position</summary>
        public static readonly DependencyProperty SelectedImgPositionProperty =
            DependencyProperty.Register(
                nameof(SelectedImgPosition),
                typeof(ImagePosition),
                typeof(MainWindow),
                new PropertyMetadata(default));

        /// <summary>Image position</summary>
        public ImagePosition SelectedImgPosition { get => (ImagePosition)GetValue(SelectedImgPositionProperty); set => SetValue(SelectedImgPositionProperty, value); }

        #endregion

        #region MessagePosition : NotificationPosition - Message position in window

        /// <summary>Message position in window</summary>
        public static readonly DependencyProperty MessagePositionProperty =
            DependencyProperty.Register(
                nameof(MessagePosition),
                typeof(NotificationPosition),
                typeof(MainWindow),
                new PropertyMetadata(NotificationConstants.MessagePosition, (O, Args) =>
                {
                    NotificationConstants.MessagePosition = (NotificationPosition)Args.NewValue;
                }));

        /// <summary>Message position in window</summary>
        public NotificationPosition MessagePosition
        {
            get => (NotificationPosition)GetValue(MessagePositionProperty);
            set
            {
                SetValue(MessagePositionProperty, value);
                //NotificationConstants.MessagePosition = value;
            }
        }

        #endregion
    }
}
