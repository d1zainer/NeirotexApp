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


        public const string ThemeToggle = "ThemeToggle";
        public const string LanguageComboBox = "LanguageComboBox";

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
        { InfoMessageType.ErrorMessage, "ErrorMessage" },
        { InfoMessageType.ReadingFileError, "ReadingFileError" },
        { InfoMessageType.ErrorXMLDocument ,"ErrorXMLDocument"},
        { InfoMessageType.ErrorBOSMeth ,"ErrorBOSMeth"},
        { InfoMessageType.ErrorFilesBCF ,"ErrorFilesBCF"}
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
        ErrorMessage,
        ReadingFileError,
        ErrorXMLDocument,
        ErrorBOSMeth,
        ErrorFilesBCF
    }

    /// <summary>
    /// возращает сообщение по типу
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
    /// Возвращает строку с текущим языком в зависимости от типа заголовка.
    /// </summary>
    /// <param name="titleType">Тип заголовка.</param>
    /// <returns>Строка заголовка на текущем языке.</returns>
    public string GetTitleByCulture(TitleType titleType)
    {
        var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

        switch (titleType)
        {
            case TitleType.FileDialog:
                return GetFileDialogTitleByCulture(currentCulture);

            case TitleType.ViewLocator:
                return GetViewLocatorTitleByCulture(currentCulture);

            default:
                throw new ArgumentOutOfRangeException(nameof(titleType), $"Unknown TitleType: {titleType}");
        }
    }

    /// <summary>
    /// Возвращает строку для диалога выбора файла в зависимости от культуры.
    /// </summary>
    private string GetFileDialogTitleByCulture(string cultureName)
    {
        return cultureName switch
        {
            "ru-RU" => "Выберите файл",
            "en-US" => "Select a file",
            _ => "Select a file"
        };
    }

    /// <summary>
    /// Возвращает строку для ViewLocator в зависимости от культуры.
    /// </summary>
    private string GetViewLocatorTitleByCulture(string cultureName)
    {
        return cultureName switch
        {
            "ru-RU" => "Не найдено:",
            "en-US" => "Not found:",
            _ => "Not found:"
        };
    }



    public enum TitleType
    {
        FileDialog,
        ViewLocator
    }
    





}