using NeirotexApp.MVVM.ViewModels;
using NeirotexApp.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;

namespace NeirotexApp.ResourcesLang
{
    public class LangController 
    {

        private static readonly Lazy<LangController> _langControllerInstance = new Lazy<LangController>(() => new LangController());
        public static LangController Instance => _langControllerInstance.Value;

        /// <summary>
        /// событие при смене языка
        /// </summary>
        public  Action LanguageChanged = delegate { };

        private static readonly ResourceManager ResourceManager =
           new ResourceManager("NeirotexApp.ResourcesLang.Resources", typeof(MainWindow).Assembly);


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
            public  const string Type = "Type";
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
        /// словарь, в котором тип сообщения(Для информационного окна) и ключ
        /// </summary>

        public static readonly Dictionary<InfoMessageType, string> MessageKeys = new Dictionary<InfoMessageType, string>
        {
            { InfoMessageType.WelcomeMessage, "WelcomeMessage" },
            { InfoMessageType.FileLoadedMessage, "FileLoadedMessage" },
            { InfoMessageType.NoFileLoadedMessage, "NoFileLoadedMessage" },
            { InfoMessageType.DataProcessedMessage, "DataProcessedMessage" },
            { InfoMessageType.ErrorMessage, "ErrorMessage"}
        };

        /// <summary>
        /// возращает сообщение по типу
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public string GetMessage(InfoMessageType messageType, params object[] args)
        {
            if (MessageKeys.TryGetValue(messageType, out var key))
            {
                var message = GetString(key); 
                return args.Length > 0 ? string.Format(message, args) : message; // Форматируем строку, если есть аргументы
            }
            return string.Empty; 
        }

        public enum InfoMessageType
        {
            WelcomeMessage,
            FileLoadedMessage,
            NoFileLoadedMessage,
            DataProcessedMessage,
            ErrorMessage

        }
        
        /// <summary>
        /// возращает переведенную строку
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            return ResourceManager.GetString(key, Thread.CurrentThread.CurrentCulture);
        }

    }
}
