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
                new PropertyMetadata(default(uint)));

        /// <summary>количество строк в сообщении</summary>
        public uint RowCount { get => (uint) GetValue(RowCountProperty); set => SetValue(RowCountProperty, value); }

        #endregion
        private readonly NotificationManager _notificationManager = new NotificationManager();
        //private readonly Random _random = new Random();

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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i <= 5; i++)
            {
                var content = new NotificationContent
                {
                    Title = "Sample notification",
                    Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    Type = (NotificationType)i,
                };
                await Task.Delay(TimeSpan.FromSeconds(1));
                _notificationManager.Show(content);

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var content = new NotificationContent {Title = "Notification in window", Message = "Click me!"};
            var clickContent = new NotificationContent
            {
                Title = "Clicked!",
                Message = "Window notification was clicked!",
                Type = NotificationType.Success,
                
            };
            _notificationManager.Show(content, "WindowArea", onClick: () => _notificationManager.Show(clickContent));
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

            _notificationManager.ShowProgressBar(out var progress, out var Cancel, title, true, true);
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

        private void Message_button(object sender, RoutedEventArgs e)
        {
            _notificationManager.Show("2 button","This is 2 button on form", string.Empty,TimeSpan.MaxValue,
                (o, args) => _notificationManager.Show("Left button click", string.Empty,TimeSpan.FromSeconds(3)),"Left Button",
                (o, args) => _notificationManager.Show("Right button click", string.Empty, TimeSpan.FromSeconds(3)), "Right Button"); 
            
            _notificationManager.Show("2 button", "This is 2 button on form with standard name", string.Empty,TimeSpan.MaxValue,
                (o, args) => _notificationManager.Show("Left button click", string.Empty,TimeSpan.FromSeconds(3)),null,
                (o, args) => _notificationManager.Show("Right button click", string.Empty, TimeSpan.FromSeconds(3)), null);

            _notificationManager.Show("1 right button","This is 1 button on form with standard name","",TimeSpan.MaxValue,
                (o, args) => _notificationManager.Show("Right button click", string.Empty, TimeSpan.FromSeconds(3)));

        }

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
            var long_text =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis euismod accumsan orci vel varius. Nulla consectetur egestas est,"
                + " in porttitor elit placerat non. Cras dapibus cursus magna. Nunc ac malesuada lacus. Etiam non luctus magna, nec vulputate diam."
                + " Sed porta mi at tristique bibendum. Nunc luctus libero ut mauris cursus, eget dignissim est luctus.Sed ac nibh dignissim, elementum mi ut,"
                + " tempor quam.Donec quis ornare sapien. Maecenas arcu elit, blandit quis odio eu, elementum bibendum leo."
                + " Etiam iaculis consectetur metus. Donec in bibendum massa. Nam nec facilisis eros, sit amet blandit magna.Duis vitae justo nec nisi maximus efficitur vitae non mauris.";


            for (var i = 0; i <= 5; i++)
            {
                var content = new NotificationContent
                {
                    Title = "Sample notification",
                    Message = long_text,
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
