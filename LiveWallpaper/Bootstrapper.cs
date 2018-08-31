﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LiveWallpaper.ViewModels;
using LiveWallpaper.Services;

namespace LiveWallpaper
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer container;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            //自定义消息拦截
            container = new SimpleContainer();

            container.Instance(container)
            .Singleton<IWindowManager, WindowManager>()
            .Singleton<ContextMenuViewModel>(nameof(ContextMenuViewModel))
            .Singleton<MainViewModel>(nameof(MainViewModel))
            .PerRequest<CreateWallpaperViewModel>()
            .PerRequest<SettingViewModel>();
        }

        private object GetCefSource(ActionExecutionContext arg)
        {
            return arg.Source;
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            logger.Info("OnStartup");
            //logger.Info(Environment.CurrentDirectory);
            //logger.Info(AppService.AppDir);
            //logger.Info(AppService.SettingPath);

            //todo 不是开机启动
            //if (Environment.CurrentDirectory == AppService.AppDir)
            DisplayRootViewFor<MainViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}
