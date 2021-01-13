using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
                new PropertyMetadata("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis euismod accumsan orci vel varius. Nulla consectetur egestas est,"
                                     + " in porttitor elit placerat non. Cras dapibus cursus magna. Nunc ac malesuada lacus. Etiam non luctus magna, nec vulputate diam."
                                     + " Sed porta mi at tristique bibendum. Nunc luctus libero ut mauris cursus, eget dignissim est luctus.Sed ac nibh dignissim, elementum mi ut,"
                                     + " tempor quam.Donec quis ornare sapien. Maecenas arcu elit, blandit quis odio eu, elementum bibendum leo."
                                     + " Etiam iaculis consectetur metus. Donec in bibendum massa. Nam nec facilisis eros, sit amet blandit magna.Duis vitae"
                                     + " justo nec nisi maximus efficitur vitae non mauris."));

        /// <summary>Content string</summary>
        public string ContentText { get => (string) GetValue(ContentTextProperty); set => SetValue(ContentTextProperty, value); }

        #endregion

        #region TrimType : NotificationTextTrimType - способ обрезки текста

        /// <summary>способ обрезки текста</summary>
        public static readonly DependencyProperty TrimTypeProperty =
            DependencyProperty.Register(
                nameof(TrimType),
                typeof(NotificationTextTrimType),
                typeof(MainWindow),
                new PropertyMetadata(default(NotificationTextTrimType)));

        /// <summary>способ обрезки текста</summary>
        public NotificationTextTrimType TrimType { get => (NotificationTextTrimType) GetValue(TrimTypeProperty); set => SetValue(TrimTypeProperty, value); }

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

        
        private readonly NotificationManager _notificationManager = new NotificationManager();

        Action ButtonClick(string button) => ()=>_notificationManager.Show($"{button} button click");

        public MainWindow()
        {
            InitializeComponent();
            Timer = new Timer { Interval = 1000 };
            Timer.Elapsed += (s, a) => _notificationManager.Show("Pink string from another thread!");
        }

        private readonly Timer Timer;

        private void Button_Timer(object sender, RoutedEventArgs e)
        {
            if(!Timer.Enabled) Timer.Start();
            else Timer.Stop();
        }

        private async void UpperPanel(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i <= 5; i++)
            {
                var content = new NotificationContent
                {
                    Title = "Sample notification",
                    Message = ContentText,
                    Type = (NotificationType)i,
                    LeftButtonAction = ShowLeftButton ? ButtonClick("Left") : null,
                    RightButtonAction = ShowRightButton ? ButtonClick("Left") : null,
                    LeftButtonContent = LeftButtonText,
                    RightButtonContent = RightButtonText,
                    RowsCount = RowCount,
                    TrimType = TrimType,
                    CloseOnClick = false
                };
                _notificationManager.Show(content,expirationTime:TimeSpan.FromSeconds(5));
                await Task.Delay(TimeSpan.FromSeconds(1));

            }

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
                    Type = (NotificationType)rnd.Next(0,5),
                    LeftButtonAction = ShowLeftButton ? ButtonClick("Left") : null,
                    RightButtonAction = ShowRightButton ? ButtonClick("Left") : null,
                    LeftButtonContent = LeftButtonText,
                    RightButtonContent = RightButtonText,
                    RowsCount = RowCount,
                    TrimType = TrimType

                };
                _notificationManager.Show(content, "WindowArea", expirationTime: TimeSpan.FromSeconds(5), onClick: () => _notificationManager.Show(clickContent));

        }

        private void Message_Click(object sender, RoutedEventArgs e)
        {
            var clickContent = new NotificationContent
            {
                Title = "Clicked!",
                Message = "Window notification was clicked!",
                Type = NotificationType.Success
            };

            const string title = "Sample notification";
            const string Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
            const NotificationType type = NotificationType.Notification;

            _notificationManager.Show(title, Message, type, "WindowArea", onClick: () => _notificationManager.Show(clickContent));
            _notificationManager.Show(title, Message, type, string.Empty, onClick: () => _notificationManager.Show(clickContent));

        }
        private async void Progress_Click(object sender, RoutedEventArgs e)
        {
            var title = "Прогресс бар";

            _notificationManager.ShowProgressBar(out var progress, out var Cancel, title, true, true, "",true, 2u);
            using (progress)
                try
                {
                    //await CalcAsync(progress, Cancel).ConfigureAwait(false);

                    await Task.Run(async () =>
                    {
                        for (var i = 0; i <= 100; i++)
                        {
                            Cancel.ThrowIfCancellationRequested();
                            progress.Report((i, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\n"
                                                + "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", null, null));
                            await Task.Delay(TimeSpan.FromSeconds(0.03), Cancel);
                        }
                    }, Cancel).ConfigureAwait(false);

                    for (var i = 0; i <= 100; i++)
                    {
                        Cancel.ThrowIfCancellationRequested();
                        progress.Report((i,$"Progress {i}", "Whith progress", true));
                        await Task.Delay(TimeSpan.FromSeconds(0.02), Cancel).ConfigureAwait(false);
                    }

                    for (var i = 0; i <= 100; i++)
                    {
                        Cancel.ThrowIfCancellationRequested();
                        progress.Report((null,$"{i}", "Whithout progress", null));
                        await Task.Delay(TimeSpan.FromSeconds(0.05), Cancel).ConfigureAwait(false);
                    }

                    for (var i = 0; i <= 100; i++)
                    {
                        Cancel.ThrowIfCancellationRequested();
                        progress.Report((i, null, "Agane whith progress", null));
                        await Task.Delay(TimeSpan.FromSeconds(0.01), Cancel).ConfigureAwait(false);
                    }


                }
                catch (OperationCanceledException)
                {
                    _notificationManager.Show("Операция отменена", string.Empty, TimeSpan.FromSeconds(3));
                }
        }

        public Task CalcAsync(IProgress<(int, string,string,bool?)> progress, CancellationToken cancel) =>
            Task.Run(async () =>
            {
                for (var i = 0; i <= 100; i++)
                {
                    cancel.ThrowIfCancellationRequested();
                    progress.Report((i, $"Процесс {i}",null, null));
                    await Task.Delay(TimeSpan.FromSeconds(0.01), cancel);
                }
            }, cancel);

        private void Show_Any_content(object sender, RoutedEventArgs e)
        {
            var grid = new Grid();
            var text_block = new TextBlock { Text = "Some Text", Margin = new Thickness(0, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Center };


            var panelBTN = new StackPanel { Height = 100, Margin = new Thickness(0, 40, 0, 0) };
            var btn1 = new Button { Width = 200, Height = 40, Content = "Cancel" };
            var text = new TextBlock {Foreground = Brushes.White, Text = "Hello, world", Margin = new Thickness(0, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Center};
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

            _notificationManager.Show(content,null,TimeSpan.MaxValue);

        }

        private async void ShowAttachMessage(object Sender, RoutedEventArgs E)
        {
            for (var i = 0; i <= 5; i++)
            {
                var content = new NotificationContent
                {
                    Title = "Sample notification",
                    Message = ContentText,
                    Type = (NotificationType)i,
                    TrimType = TrimType,
                    RowsCount = RowCount
                };
                await Task.Delay(TimeSpan.FromSeconds(1));
                _notificationManager.Show(content);

            }


        }
    }
}
