using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NeirotexApp.MVVM.Models;

namespace NeirotexApp.Services
{
    public class SettingService
    {
        public  Settings FirstSettings { get; set; }
        public SettingService()
        {
            FirstSettings = LoadSettings();
        }




        /// <summary>
        /// Загружает настройки приложения
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Settings LoadSettings()
        {
           Settings settings = new Settings();
           settings.Language = Properties.Settings.Default.Language;
           settings.Theme = Properties.Settings.Default.Theme;
           return settings;
        }

        /// <summary>
        /// Сохраняет настройки приложения
        /// </summary>
        public static void SaveSettings(Settings settings)
        {
            Properties.Settings.Default.Language = settings.Language;
            Properties.Settings.Default.Theme = settings.Theme;
            Properties.Settings.Default.Save();

        }
    }
}
