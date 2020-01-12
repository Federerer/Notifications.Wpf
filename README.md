# Notifications.Wpf
WPF toast notifications.

![Demo](http://i.imgur.com/UvYIVFV.gif)
### Installation:
```
Install-Package Notifications.Wpf
```
### Usage:

#### Notification over the taskbar:
```C#
var notificationManager = new NotificationManager();

notificationManager.Show(new NotificationContent
           {
               Title = "Sample notification",
               Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
               Type = NotificationType.Information
           });
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
notificationManager.Show(
                new NotificationContent {Title = "Notification", Message = "Notification in window!"},
                areaName: "WindowArea");
```

#### Simple text with OnClick & OnClose actions:
```C#
notificationManager.Show("String notification", onClick: () => Console.WriteLine("Click"),
               onClose: () => Console.WriteLine("Closed!"));
```

- Result:

![Demo](http://i.imgur.com/G1ZU2ID.gif)
