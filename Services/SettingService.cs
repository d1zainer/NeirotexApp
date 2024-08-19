using NeirotexApp.MVVM.Models;
using NeirotexApp.Properties;
using NeirotexApp.Services.Interfaces;
using System.Globalization;
using System.Threading;
using NeirotexApp.UI.Managers;
using Avalonia.Styling;
using System;
using Avalonia;
using NeirotexApp.MVVM.Views;
using System.Resources;

namespace NeirotexApp.Services;

public class SettingService : ILanguageSetting, IThemeSetting
{

    public static SettingService Instance { get; private set; }


    public static void Init()
    {
        if (Instance == null)
        {
            Instance = new SettingService();
        }
    }
    public SettingService()
    {
        FirstSettings = LoadSettings();
        GetCulture(FirstSettings);
        GetTheme(FirstSettings);
        
    }

    public Settings FirstSettings { get; set; }

    /// <summary>
    ///     Загружает настройки приложения
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static Settings LoadSettings()
    {
        var settings = new Settings();
        settings.Language = SettingsApp.Default.Language;
        settings.Theme = SettingsApp.Default.Theme;
        return settings;
    }

    /// <summary>
    ///     Сохраняет настройки приложения
    /// </summary>
    public static void SaveSettings(Settings settings)
    {
        SettingsApp.Default.Language = settings.Language;
        SettingsApp.Default.Theme = settings.Theme;
        SettingsApp.Default.Save();
    }

    /// <summary>
    /// Загрузка сохранённой культуры с использованием XMLService
    /// </summary>
    public void GetCulture(Settings settings)
    {
        if (!string.IsNullOrWhiteSpace(settings.Language))
        {
            SetCulture(settings.Language);
        }
    }

    /// <summary>
    /// Загрузка темы
    /// </summary>
    public void GetTheme(Settings settings)
    {
        if (!string.IsNullOrEmpty(settings.Theme))
        {
            SetTheme(settings.Theme);
        }
    }

    /// <summary>
    /// Сохранение текущей культуры с использованием XMLService
    /// </summary>
    /// <param name="cultureCode"></parSam
    public void SaveCulture(string cultureCode)
    {
        var settings = SettingService.LoadSettings(); // Загрузка текущих настроек
        settings.Language = cultureCode; // Обновление языка
        SaveSettings(settings); // Сохранение обновленных настроек
    }

    /// <summary>
    /// Сохранение текущей культуры с использованием XMLService
    /// </summary>
    /// <param name="themeName"></param>
    public void SaveTheme(string themeName)
    {
        var settings = SettingService.LoadSettings(); // Загрузка текущих настроек
        settings.Theme = themeName; // Обновление языка
        SaveSettings(settings); // Сохранение обновленных настроек
    }

    /// <summary>
    /// Сменяей язык
    /// </summary>
    /// <param name="cultureCode"></param>
    public void SetCulture(string cultureCode)
    {
        var cultureInfo = new CultureInfo(cultureCode);
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        SaveCulture(cultureCode); // Сохранение культуры в XML файл
        LanguageManager.Instance.LanguageChanged?.Invoke(); //вызываем событие, что произошла смена
    }

    public void SetTheme(string themeName)
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
}