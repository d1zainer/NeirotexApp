using System;
using System.Collections.Generic;
using System.Resources;
using System.Threading;
using NeirotexApp.MVVM.Views;

namespace NeirotexApp.App;

public class LangController
{
    public enum InfoMessageType
    {
        WelcomeMessage,
        FileLoadedMessage,
        NoFileLoadedMessage,
        DataProcessedMessage,
        ErrorMessage
    }

    private static readonly Lazy<LangController> _langControllerInstance = new(() => new LangController());

    private static readonly ResourceManager ResourceManager =
        new("NeirotexApp.ResourcesLang.Resources", typeof(MainWindow).Assembly);


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

    public static LangController Instance => _langControllerInstance.Value;

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
}