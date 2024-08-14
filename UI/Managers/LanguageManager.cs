using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using NeirotexApp.MVVM.Models;
using NeirotexApp.MVVM.Views;
using NeirotexApp.Services;

namespace NeirotexApp.UI.Managers;

public class LanguageManager
{
    private static readonly Lazy<LanguageManager> _langControllerInstance = new(() => new LanguageManager());

    private static readonly ResourceManager ResourceManager =
        new("NeirotexApp.ResourcesLang.Resources", typeof(MainWindow).Assembly);
    public static LanguageManager Instance => _langControllerInstance.Value;

    // Закрытый конструктор для синглтона

    /// <summary>
    /// структура в которой находятся ключи для ресурсов
    /// </summary>
    public struct TitleKeys
    {
        public const string InfoPanel = "InfoWindowTitle";
        public const string LoadFileBtn = "LoadFileButton";
        public const string StartReadingBtn = "StartReadingButton";
        public const string SignalFileName = "SignalFileName";
        public const string UnicNumber = "UnicNumber";
        public const string Type = "Type";
        public const string EffectiveFd = "EffectiveFd";
        public const string MathValue = "MathValue";
        public const string MinValue = "MinValue";
        public const string MaxValue = "MaxValue";

        public const string InfoPanelChannel = "InfoWindowChannelTitle";
        public const string ChannelValue = "ChannelValue";

        public const string EEG = "EEG";
        public const string ECG = "ECG";
        public const string EMG = "EMG";
    }

    /// <summary>
    ///     словарь, в котором тип сообщения(Для информационного окна) и ключ
    /// </summary>
    public static readonly Dictionary<InfoMessageType, string> MessageKeys = new()
    {
        { InfoMessageType.WelcomeMessage, "WelcomeMessage" },
        { InfoMessageType.FileLoadedMessage, "FileLoadedMessage" },
        { InfoMessageType.NoFileLoadedMessage, "NoFileLoadedMessage" },
        { InfoMessageType.DataProcessedMessage, "DataProcessedMessage" },
        { InfoMessageType.ErrorMessage, "ErrorMessage" }
    };

    /// <summary>
    ///  событие при смене языка
    /// </summary>
    public Action LanguageChanged = delegate { };
    public enum InfoMessageType
    {
        WelcomeMessage,
        FileLoadedMessage,
        NoFileLoadedMessage,
        DataProcessedMessage,
        ErrorMessage
    }

    /// <summary>
    ///     возращает сообщение по типу
    /// </summary>
    /// <param name="messageType"></param>
    /// <returns></returns>
    public string? GetMessage(InfoMessageType messageType, params object[]? args)
    {
        if (MessageKeys.TryGetValue(messageType, out var key))
        {
            var message = GetString(key);
            return args.Length > 0 ? string.Format(message, args) : message; // Форматируем строку, если есть аргументы
        }

        return string.Empty;
    }

    /// <summary>
    ///  возращает переведенную строку
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string? GetString(string key)
    {
        return ResourceManager.GetString(key, Thread.CurrentThread.CurrentCulture);
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
        Instance.LanguageChanged?.Invoke(); //вызываем событие, что произошла смена
    }

    /// <summary>
    /// Сохранение текущей культуры с использованием XMLService
    /// </summary>
    /// <param name="cultureCode"></param
    /// 
    private void SaveCulture(string cultureCode)
    {
        var settings = SettingService.LoadSettings(); // Загрузка текущих настроек
        settings.Language = cultureCode; // Обновление языка
        SettingService.SaveSettings(settings); // Сохранение обновленных настроек
    }
    /// <summary>
    /// Загрузка сохранённой культуры с использованием XMLService
    /// </summary>
    public void LoadCulture(Settings settings)
    {
        try
        {
            if (settings != null && !string.IsNullOrEmpty(settings.Language))
            {
                SetCulture(settings.Language);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }





}