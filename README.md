# Notification.Wpf
WPF toast notifications. (Messages and progress bars)

## [Last Updates](https://github.com/Platonenkov/Notification.Wpf/blob/dev/Updates.md)
### Installation:
Install-Package Notification.WPF -Version 5.4.0

![Demo](https://github.com/Platonenkov/Notifications.Wpf/blob/dev/Files/notification.gif)
![Demo](https://github.com/Platonenkov/Notifications.Wpf/blob/dev/Files/progress.gif)
![Demo](https://github.com/Platonenkov/Notifications.Wpf/blob/dev/Files/info_button.gif)
![Demo](https://github.com/Platonenkov/Notifications.Wpf/blob/dev/Files/content.gif)
![Demo](https://github.com/Platonenkov/Notifications.Wpf/blob/dev/Files/all_styles.gif)
![Demo](https://github.com/Platonenkov/Notifications.Wpf/blob/dev/Files/Colored.gif)
![Demo](https://github.com/Platonenkov/Notifications.Wpf/blob/dev/Files/Image.gif)
### Known issue

If you have problem with close notification window after closing you app, use this row: 
```C#
Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

```
### Information
This project was forked from  https://github.com/Federerer/Notifications.Wpf

Project was re-released due to lack of owner interest in updating it.

### Usage:

### Notifi type:
    public enum NotificationType   
    {
        None,
        Information,
        Success,
        Warning,
        Error,
        Notification
    }
### Text Trim type
    public enum NotificationTextTrimType
    {
        NoTrim,
        Trim,
        Attach,
        AttachIfMoreRows
    }

    
#### Notification over the taskbar:
```C#
var notificationManager = new NotificationManager();
notificationManager.Show(title, Message, type, onClick: () => SomeAction();

```

#### Notification inside application window:
- Adding namespace:
```XAML
xmlns:notifications="clr-namespace:Notification.Wpf.Controls;assembly=Notification.Wpf"
```
- Adding new NotificationArea:
```XAML
<notifications:NotificationArea x:Name="WindowArea" Position="TopLeft" MaxItems="3"/>
```
- Displaying notification:
```C#
notificationManager.Show(title, Message, type, "WindowArea", onClick: () => SomeAction();
```
-  Attach a long text to view window, without changing a size of notification message
-  
if you need to show all long text - use TrimTipe - NoTrim;

if you need to show small message window - use TrimTipe - Trim;

if you need All text, but you dont need big window - use TrimTipe - Attach,
it will trim text and show small button that open other window to operate with full text;

if you need All text, but you dont need big window and you want defined window size - use TrimTipe - AttachIfMoreRows,
it will trim text and show small button if count of rows in message will be more that that you set.

### Full content sample
```C#
var content = new NotificationContent
    {
        Title = "Sample notification",
        Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
        Type = NotificationType.Error,
        TrimType = NotificationTextTrimType.Attach, // will show attach button on message
        RowsCount = 3, //Will show 3 rows and trim after
        LeftButtonAction = () => SomeAction(), //Action on left button click, button will not show if it null 
        RightButtonAction = () => SomeAction(), //Action on right button click,  button will not show if it null
        LeftButtonContent, // Left button content (string or what u want
        RightButtonContent, // Right button content (string or what u want
        CloseOnClick = true, // Set true if u want close message when left mouse button click on message (base = true)
        
        Background = new SolidColorBrush(Colors.White),
        Foreground = new SolidColorBrush(Colors.DarkRed),
        
		Icon = new SvgAwesome()
            {
                Icon = EFontAwesomeIcon.Regular_Star,
                Height = 25,
                Foreground = new SolidColorBrush(Colors.Yellow)
            },
		
		Image = new NotificationImage()
			{
				Source = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Test image.png")));,
				Position = ImagePosition.Top
			}

    };
    _notificationManager.Show(content);
```

#### Notification ProgressBar:

using next type IProgress<(int? progress, string message, string title, bool? showCancel)>
    
```C#
public NotifierProgress<(double? value, string message, string title, bool? showCancel)> ShowProgressBar(
            string Title = null,
            bool ShowCancelButton = true,
            bool ShowProgress = true,
            string areaName = "",
            bool TrimText = false,
            uint DefaultRowsCount = 1U,
            string BaseWaitingMessage = "Calculation time",
            bool IsCollapse = false,
            bool TitleWhenCollapsed = true,
            Brush background = null,
            Brush foreground = null,
            Brush progressColor = null,
            object icon = default)
      
      
using var progress = notificationManager.ShowProgressBar(title, true, false, true, 2U, "Wait");
    try
    {
        await SomeMetod(progress, progress.Cancel).ConfigureAwait(false);

         await Task.Run(async () =>
        {
            for (var i = 0; i <= 100; i++)
            {
                progress.Cancel.ThrowIfCancellationRequested();
                progress.Report((i, $"Lorem ipsum dolor sit amet, consectetur adipiscing elit.\n"
                                    + $"Lorem ipsum dolor sit amet, consectetur adipiscing elit.", null, null));
                await Task.Delay(TimeSpan.FromSeconds(0.03), progress.Cancel);
            }
        }, progress.Cancel).ConfigureAwait(false);

        for (var i = 0; i <= 100; i++)
        {
            progress.Cancel.ThrowIfCancellationRequested();
            progress.Report((i,$"Progress {i}", "Whith progress", true));
            await Task.Delay(TimeSpan.FromSeconds(0.02), progress.Cancel).ConfigureAwait(false);
        }

        for (var i = 0; i <= 100; i++)
        {
            progress.Cancel.ThrowIfCancellationRequested();
            progress.Report((null,$"{i}", "Whithout progress", null));
            await Task.Delay(TimeSpan.FromSeconds(0.05), progress.Cancel).ConfigureAwait(false);
        }

        for (var i = 0; i <= 100; i++)
        {
            progress.Cancel.ThrowIfCancellationRequested();
            progress.Report((i, null, "Agane whith progress", null));
            await Task.Delay(TimeSpan.FromSeconds(0.01), progress.Cancel).ConfigureAwait(false);
        }
    }
    catch (OperationCanceledException)
    {
        _notificationManager.Show("operation was cancelled", "", TimeSpan.FromSeconds(3));
    }
                
     public Task SomeMetod(IProgress<(int?, string,string,bool?)> progress, CancellationToken cancel) =>
            Task.Run(async () =>
            {
                for (var i = 0; i <= 100; i++)
                {
                    cancel.ThrowIfCancellationRequested();
                    progress.Report((i, $"Процесс {i}",null, null));
                    await Task.Delay(TimeSpan.FromSeconds(0.05), cancel);
                }
            }, cancel);            
```

##### Progress will calculate approximate waiting time and show it in left bottom corner, if u not need it - set progress.WaitingTimer.BaseWaitingMessage = null, or set it in intializer

#### Simple text with OnClick & OnClose actions:
```C#
notificationManager.Show("String notification", onClick: () => Console.WriteLine("Click"),
               onClose: () => Console.WriteLine("Closed!"));
```
#### Notifi with button:
```C#
notificationManager.Show("2 button","This is 2 button on form","",TimeSpan.MaxValue,
     () => _notificationManager.Show("Left button click","",TimeSpan.FromSeconds(3)),"Left Button",
     () => _notificationManager.Show("Right button click", "", TimeSpan.FromSeconds(3)), "Right Button"); 

notificationManager.Show("2 button","This is 2 button on form","",TimeSpan.MaxValue,
     () => _notificationManager.Show("Left button click","",TimeSpan.FromSeconds(3)),null,
     () => _notificationManager.Show("Right button click", "", TimeSpan.FromSeconds(3)), null);

notificationManager.Show("1 right button","This is 2 button on form","",TimeSpan.MaxValue,
     () => _notificationManager.Show("Right button click", "", TimeSpan.FromSeconds(3)));
```

![Demo](http://i.imgur.com/G1ZU2ID.gif)

#### Show any content
```C#
var grid = new Grid();
var text_block = new TextBlock { Text = "Some Text", Margin = new Thickness(0, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Center };


var panelBTN = new StackPanel { Height = 100, Margin = new Thickness(0, 40, 0, 0) };
var btn1 = new Button { Width = 200, Height = 40, Content = "Cancel" };
var text = new TextBlock {Foreground = Brushes.White, Text = "Hello, world", Margin = new Thickness(0, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Center};
panelBTN.VerticalAlignment = VerticalAlignment.Bottom;
panelBTN.Children.Add(btn1);

var row1 = new RowDefinition();
var row2 = new RowDefinition();
var row3 = new RowDefinition();

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

notificationManager.Show(content,null,TimeSpan.MaxValue);
```
![Demo](https://github.com/Platonenkov/Notifications.Wpf/blob/master/Files/content.gif)

