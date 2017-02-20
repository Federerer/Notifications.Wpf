# Notifications.Wpf
WPF toast notifications.

![Demo](http://i.imgur.com/UvYIVFV.gif)
### Installation:
```
Install-Package Notifications.Wpf -Pre
```
### Usage:

####Notification over the taskbar:
```C#
var notificationManager = new NotificationManager();

notificationManager.Show(new NotificationContent
           {
               Title = "Sample notification",
               Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
               Type = NotificationType.Info
           });
```

####Notification inside application window:
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

####Simple text with OnClick & OnClose actions:
```C#
notificationManager.Show("String notification", onClick: () => Console.WriteLine("Click"),
               onClose: () => Console.WriteLine("Closed!"));
```
###Caliburn.Micro MVVM support:
- App.xaml:
```XAML
xmlns:controls="clr-namespace:Notifications.Wpf.Controls;assembly=Notifications.Wpf"

<Application.Resources>
    [...]
    <Style TargetType="controls:Notification">
        <Style.Resources>
            <DataTemplate DataType="{x:Type micro:PropertyChangedBase}">
                <ContentControl cal:View.Model="{Binding}"/>
            </DataTemplate>
        </Style.Resources>
    </Style>
</Application.Resources>
```
- ShellViewModel:
```C#
var content = new NotificationViewModel(_manager)
{
    Title = "Custom notification.",
    Message = "Click on buttons!"
};

_manager.Show(content, expirationTime: TimeSpan.FromSeconds(30));
```
- NotificationView:
```XAML
<DockPanel LastChildFill="False">
    <!--Using CloseOnClick attached property to close notification when button is pressed-->
    <Button x:Name="Ok" Content="Ok" DockPanel.Dock="Right" controls:Notification.CloseOnClick="True"/>
    <Button x:Name="Cancel" Content="Cancel" DockPanel.Dock="Right" Margin="0,0,8,0" controls:Notification.CloseOnClick="True"/>
</DockPanel>
```
- Result:

![Demo](http://i.imgur.com/G1ZU2ID.gif)
