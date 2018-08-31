﻿using Caliburn.Micro;
using DZY.DotNetUtil.Helpers;
using JsonConfiger;
using JsonConfiger.Models;
using LiveWallpaper.Settings;
using LiveWallpaper.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LiveWallpaper.ViewModels
{
    public class SettingViewModel : Screen
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        JCrService _jcrService = new JCrService();
        protected override async void OnInitialize()
        {
            var config = await JsonHelper.JsonDeserializeFromFileAsync<dynamic>(AppService.SettingPath);
            string descPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Res\\setting.desc.json");
            var descConfig = await JsonHelper.JsonDeserializeFromFileAsync<dynamic>(descPath);
            JsonConfierViewModel = _jcrService.GetVM(config, descConfig);
            base.OnInitialize();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
        }

        #region properties

        #region JsonConfierViewModel

        /// <summary>
        /// The <see cref="JsonConfierViewModel" /> property's name.
        /// </summary>
        public const string JsonConfierViewModelPropertyName = "JsonConfierViewModel";

        private JsonConfierViewModel _JsonConfierViewModel;

        /// <summary>
        /// JsonConfierViewModel
        /// </summary>
        public JsonConfierViewModel JsonConfierViewModel
        {
            get { return _JsonConfierViewModel; }

            set
            {
                if (_JsonConfierViewModel == value) return;

                _JsonConfierViewModel = value;
                NotifyOfPropertyChange(JsonConfierViewModelPropertyName);
            }
        }

        #endregion

        #endregion

        public async void Save()
        {
            var data = _jcrService.GetData(JsonConfierViewModel.Nodes);
            await JsonHelper.JsonSerializeAsync(data, AppService.SettingPath);
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }

        public void OpenConfigFolder()
        {
            try
            {
#if UWP
                //https://stackoverflow.com/questions/48849076/uwp-app-does-not-copy-file-to-appdata-folder
                var appData = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "Roaming\\LiveWallpaper");
#else
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                appData = Path.Combine(appData, "LiveWallpaper");
#endif
                Process.Start(appData);
            }
            catch (Exception ex)
            {
                logger.Warn("OpenConfigFolder:" + ex);
            }
        }
    }
}
