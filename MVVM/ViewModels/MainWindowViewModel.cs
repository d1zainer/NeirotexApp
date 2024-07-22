using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using NeirotexApp.App;
using NeirotexApp.MVVM.Models;
using NeirotexApp.ResourcesLang;
using NeirotexApp.Services;
using NeirotexApp.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NeirotexApp.MVVM.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private static readonly Lazy<MainWindowViewModel> _mainWindowViewModelInstance = new Lazy<MainWindowViewModel>(() => new MainWindowViewModel());
        public static MainWindowViewModel Instance => _mainWindowViewModelInstance.Value;

        public static Action<LangController.InfoMessageType, MessageType, object[]> InformationStringAction = delegate { }; // Измененный тип события

        [ObservableProperty]
        private string _message;
        [ObservableProperty]
        private IBrush _messageBrush;

        private List<string> _filePaths = new List<string>();

        private BOSMeth BOSMethObject;

        [ObservableProperty]
        private ObservableCollection<SignalViewModel> _channelViewModels;

        private Dictionary<string, SignalViewModel> _channelDictionary = new Dictionary<string, SignalViewModel>();

        private ThreadController _threadController = ThreadController.Instance;

        private LangController.InfoMessageType? _currentMessage;
        private MessageType _currentMessageTypeBrush;
        private object[] _currentMessageArgs; // Добавлено для хранения аргументов сообщения

        public MainWindowViewModel()
        {
            LangController.Instance.LanguageChanged += UpdateMessageLanguage;

            SetInformationText(LangController.InfoMessageType.WelcomeMessage, MessageType.Info);
            InformationStringAction += SetInformationText;
            ChannelViewModels = new ObservableCollection<SignalViewModel>();
        }
        /// <summary>
        /// обновляем сттроку
        /// </summary>
        public void UpdateMessageLanguage()
        {
            if (_currentMessage.HasValue)
            {
                SetInformationText(_currentMessage.Value, _currentMessageTypeBrush, _currentMessageArgs);
            }
        }

        public void GetBOSMeth(string path)
        {
            try
            {
                BOSMethObject = XMLService.LoadBOSMethFromXml(path);
                if (BOSMethObject != null)
                {
                    SetInformationText(LangController.InfoMessageType.FileLoadedMessage, MessageType.Info, path);
                    ChannelViewModels.Clear();
                    _filePaths.Clear(); // Очистка списка путей
                    _channelDictionary.Clear(); // Очистка словаря
                    foreach (var channel in BOSMethObject.Channels.ChannelList)
                    {
                        var viewModel = new SignalViewModel
                        {
                            SignalFileName = channel.SignalFileName,
                            UnicNumber = channel.UnicNumber,
                           
                            EffectiveFd = channel.EffectiveFd
                        };
                        viewModel.SetType(channel.Type); // Устанавливаем тип

                        ChannelViewModels.Add(viewModel);
                        _filePaths.Add(channel.SignalFileName);
                      
                            _channelDictionary.Add(channel.SignalFileName, viewModel);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                SetInformationText(LangController.InfoMessageType.ErrorMessage, MessageType.Error, ex.Message);
            }
        }
        /// <summary>
        /// начинаем чтение
        /// </summary>
        public void StartReadProccesing()
        {
            if (_filePaths.Count == 0)
            {
                SetInformationText(LangController.InfoMessageType.NoFileLoadedMessage, MessageType.Error);
                return;
            }
            else
            {
                _threadController = new ThreadController(ChannelViewModels.ToList());
                _threadController.StartProcessing();
                UpdateUI(_threadController._results);
                SetInformationText(LangController.InfoMessageType.DataProcessedMessage, MessageType.Info);
            }
        }
        /// <summary>
        /// обновляем вьюмодели при получении значений
        /// </summary>
        /// <param name="keyValuePairs"></param>
        private void UpdateUI(ConcurrentDictionary<SignalViewModel, (double, double, double)> keyValuePairs)
        {
            foreach (var kvp in keyValuePairs)
            {
                if (_channelDictionary.TryGetValue(kvp.Key.SignalFileName, out var existingViewModel))
                {
                    existingViewModel.MathValue = Math.Round(kvp.Value.Item1, 3);
                    existingViewModel.MaxValue = Math.Round(kvp.Value.Item3, 3);
                    existingViewModel.MinValue = Math.Round(kvp.Value.Item2, 3);
                }
            }
        }

        private void SetInformationText(LangController.InfoMessageType messageType, MessageType messageBrushType, params object[] args)
        {
            _currentMessage = messageType;
            _currentMessageTypeBrush = messageBrushType;
            _currentMessageArgs = args; // Сохранение аргументов сообщения
            Message = string.Empty;
            Message = LangController.Instance.GetMessage(messageType, args);
            MessageBrush = ForegroundTextController.GetBrushForMessageType(messageBrushType);
        }
    }
}
