using NeirotexApp.MVVM.Models;
using NeirotexApp.Properties;

namespace NeirotexApp.Services;

public class SettingService
{
    public SettingService()
    {
        FirstSettings = LoadSettings();
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
}