# Notifications.Wpf
WPF toast notifications.

![Demo](http://i.imgur.com/UvYIVFV.gif)
### Installation:
```
Install-Package Notifications.Wpf
```
### Usage:

### Notifi type:
 public enum NotificationType   
    {
        Information,
        Success,
        Warning,
        Error,
        Notification
    }
    
#### Notification over the taskbar:
```C#
var notificationManager = new NotificationManager();
notificationManager.Show(title, Message, type, onClick: () => SomeAction();

```

#### Notification inside application window:
- Adding namespace:
```XAML
xmlns:notifications="clr-namespace:Notifications.Wpf.Controls;assembly=Notifications.Wpf"
```
- Adding new NotificationArea:
```XAML
<notifications:NotificationArea x:Name="WindowArea" Position="TopLeft" MaxItems="3"/>
```
- Displaying notification:
```C#
notificationManager.Show(title, Message, type, "WindowArea", onClick: () => SomeAction();

```
#### Notification ProgressBar:

using next type IProgress<(int progress, string message, string title, bool? showCancel)>
```C#
notificationManager.ShowProgressBar(out var progress2, out var Cancel2, title, true, false);
            using (progress)
                try
                {
                    await SomeMetod(progress, Cancel).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    _notificationManager.Show("Операция отменена", "", TimeSpan.FromSeconds(3));
                }
                
     public Task CalcAsync(IProgress<(int, string,string,bool?)> progress, CancellationToken cancel) =>
            Task.Run(async () =>
            {
                for (var i = 0; i <= 100; i++)
                {
                    cancel.ThrowIfCancellationRequested();
                    progress.Report((i, $"Процесс {i}",null, null));
                    await Task.Delay(TimeSpan.FromSeconds(0.1), cancel);
                }
            }, cancel);            
```                
#### Simple text with OnClick & OnClose actions:
```C#
notificationManager.Show("String notification", onClick: () => Console.WriteLine("Click"),
               onClose: () => Console.WriteLine("Closed!"));
```
### Notifi with button:
```C#
notificationManager.ShowAction("2 button","This is 2 button on form","",TimeSpan.MaxValue,
     (o, args) => _notificationManager.Show("Left button click","",TimeSpan.FromSeconds(3)),"Left Button",
     (o, args) => _notificationManager.Show("Right button click", "", TimeSpan.FromSeconds(3)), "Right Button"); 

notificationManager.ShowAction("2 button","This is 2 button on form","",TimeSpan.MaxValue,
     (o, args) => _notificationManager.Show("Left button click","",TimeSpan.FromSeconds(3)),null,
     (o, args) => _notificationManager.Show("Right button click", "", TimeSpan.FromSeconds(3)), null);

notificationManager.ShowAction("1 right button","This is 2 button on form","",TimeSpan.MaxValue,
     (o, args) => _notificationManager.Show("Right button click", "", TimeSpan.FromSeconds(3)));
```
- Result:

![Demo](http://i.imgur.com/G1ZU2ID.gif)
![Demo](https://github.com/Platonenkov/Notifications.Wpf/blob/master/Files/button_notifi.png)
![Demo](https://github.com/Platonenkov/Notifications.Wpf/blob/master/Files/notifi.png)
