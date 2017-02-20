using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using Notifications.Wpf.Caliburn.Micro.Sample.ViewModels;

namespace Notifications.Wpf.Caliburn.Micro.Sample
{
    class Bootstrapper : BootstrapperBase
    {
        SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            base.Configure();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<INotificationManager, NotificationManager>();
            _container.Singleton<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootViewFor<ShellViewModel>();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            var timer = new Timer {Interval = 12000};
            timer.Elapsed += (o, args) => IoC.Get<INotificationManager>().Show("String from Bootstrapper!");
            timer.Start();
        }
    }
}
