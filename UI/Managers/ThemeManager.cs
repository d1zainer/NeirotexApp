﻿using Avalonia.Styling;
using NeirotexApp.Services;
using System;
using Avalonia;
using Avalonia.Controls.Selection;

namespace NeirotexApp.UI.Managers
{
    public class ThemeManager
    {
        private static readonly Lazy<ThemeManager> _themeControllerInstance = new(() => new ThemeManager());
        public static ThemeManager Instance => _themeControllerInstance.Value;

        /// <summary>
        /// смена светлой и темной темы
        /// </summary>
        /// <param name="themeName"></param>
        /// <exception cref="ArgumentException"></exception>
        public void ApplyTheme(string themeName)
        {
            var app = Application.Current;
            if (app != null)
            {
                switch (themeName)
                {
                    case "Dark":
                        app.RequestedThemeVariant = ThemeVariant.Dark;
                        break;
                    case "Light":
                        app.RequestedThemeVariant = ThemeVariant.Light;
                        break;
                    default:
                        throw new ArgumentException("Invalid theme name", nameof(themeName));
                }

                SaveTheme(themeName);
            }
        }

        /// <summary>
        /// Сохранение текущей культуры с использованием XMLService
        /// </summary>
        /// <param name="themeName"></param>
        private void SaveTheme(string themeName)
        {
            var settings = SettingService.LoadSettings(); // Загрузка текущих настроек
            settings.Theme = themeName; // Обновление языка
            SettingService.SaveSettings(settings); // Сохранение обновленных настроек
        }

        /// <summary>
        /// Загрузка темы
        /// </summary>
        public void LoadTheme(NeirotexApp.MVVM.Models.Settings settings)
        {
            try
            {
                if (settings != null && !string.IsNullOrEmpty(settings.Theme))
                {
                    ApplyTheme(settings.Theme);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
