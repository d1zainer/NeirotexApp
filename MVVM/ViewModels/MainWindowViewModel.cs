using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using NeirotexApp.App;
using NeirotexApp.MVVM.Models;
using NeirotexApp.Services;
using NeirotexApp.UI;
using NeirotexApp.UI.Managers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Threading;

namespace NeirotexApp.MVVM.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private static readonly Lazy<MainWindowViewModel> _mainWindowViewModelInstance = new Lazy<MainWindowViewModel>(() => new MainWindowViewModel());
        public static MainWindowViewModel Instance => _mainWindowViewModelInstance.Value;

        public static Action<LanguageManager.InfoMessageType, MessageType, object[]> InformationStringAction = delegate { }; // Измененный тип события

        [ObservableProperty]
        private string? _message;
        [ObservableProperty]
        private IBrush? _messageBrush;

        private List<string> _filePaths = new();

        private BOSMeth? _bosMethObject ;

        [ObservableProperty]
        private ObservableCollection<SignalViewModel> _channelViewModels;

        private readonly Dictionary<string, SignalViewModel> _channelDictionary = new ();

        private ThreadManager _threadController = ThreadManager.Instance;

        private LanguageManager.InfoMessageType? _currentMessage;
        private MessageType _currentMessageTypeBrush;
        private object[]? _currentMessageArgs; 
        private string _bosMethPath = string.Empty;

        public MainWindowViewModel()
        {
            LanguageManager.Instance.LanguageChanged += UpdateMessageLanguage;

            SetInformationText(LanguageManager.InfoMessageType.WelcomeMessage, MessageType.Info);
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

        public void GetBosMeth(string path)
        {
            try
            {
                _bosMethObject = XMLService.LoadBosMethFromXml(path);
                _bosMethPath = Path.GetDirectoryName(path); //присваиваем путь, где хранится файл xml
                SetInformationText(LanguageManager.InfoMessageType.FileLoadedMessage, MessageType.Info, path);
                ChannelViewModels.Clear();
                _filePaths.Clear(); // Очистка списка путей
                _channelDictionary.Clear(); // Очистка словаря
                foreach (var channel in _bosMethObject?.Channels?.ChannelList)
                {
                    var viewModel = new SignalViewModel
                    {
                        SignalFileName = channel.SignalFileName,
                        UnicNumber = channel.UnicNumber,

                        EffectiveFd = channel.EffectiveFd
                    };
                    viewModel.SetType(channel.Type); // Устанавливаем тип

                    ChannelViewModels.Add(viewModel);
                    if (channel.SignalFileName != null)
                    {
                        _filePaths.Add(channel.SignalFileName);
                        _channelDictionary.Add(channel.SignalFileName, viewModel);
                    }
                }
            } //если поля не заполнены
            catch (NullReferenceException)
            {
                SetInformationText(LanguageManager.InfoMessageType.ErrorBOSMeth, MessageType.Error);
            } //если неверный формат
            catch (Exception)
            {
                SetInformationText(LanguageManager.InfoMessageType.ErrorXMLDocument, MessageType.Error);
            }
        }
        /// <summary>
        /// начинаем чтение
        /// </summary>
        public async Task StartReadProccesingAsync()
        {

            if (_filePaths.Count == 0)
            {
                SetInformationText(LanguageManager.InfoMessageType.NoFileLoadedMessage, MessageType.Error);
                return;
            }

            if (!FilesExistInDirectory(_bosMethPath, _filePaths))
            {
                SetInformationText(LanguageManager.InfoMessageType.ErrorFilesBCF, MessageType.Error, FileDialog.FolderPath);
                _bosMethPath = string.Empty; 
                return;
            }

            _threadController = new ThreadManager(ChannelViewModels.ToList());
            await _threadController.StartProcessingAsync();
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                UpdateUi(_threadController.Results);
                if (_bosMethObject.TemplateGuid != null)
                    SetInformationText(LanguageManager.InfoMessageType.DataProcessedMessage, MessageType.Info,
                        _bosMethObject.TemplateGuid);
            });
           

        }
        /// <summary>
        /// обновляем вьюмодели при получении значений
        /// </summary>
        /// <param name="keyValuePairs"></param>
        private void UpdateUi(ConcurrentDictionary<SignalViewModel, (double, double, double)> keyValuePairs)
        {
            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Key.SignalFileName != null && _channelDictionary.TryGetValue(kvp.Key.SignalFileName, out var existingViewModel))
                {
                    existingViewModel.MathValue = Math.Round(kvp.Value.Item1, 3);
                    existingViewModel.MaxValue = Math.Round(kvp.Value.Item3, 3);
                    existingViewModel.MinValue = Math.Round(kvp.Value.Item2, 3);
                }
            }
        }

        /// <summary>
        /// выводит информацию в Информационное окно
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageBrushType"></param>
        /// <param name="args"></param>
        private void SetInformationText(LanguageManager.InfoMessageType messageType, MessageType messageBrushType, params object[]? args)
        {
            _currentMessage = messageType;
            _currentMessageTypeBrush = messageBrushType;
            _currentMessageArgs = args; // Сохранение аргументов сообщения
            Message = string.Empty;
            Message = LanguageManager.Instance.GetMessage(messageType, args);
            MessageBrush = ForegroundTextController.GetBrushForMessageType(messageBrushType);
        }




        /// <summary>
        /// проверяет есть ли файлы в папке
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        private bool FilesExistInDirectory(string directoryPath, List<string> fileNames)
        {
            string[] filesInDirectory = Directory.GetFiles(directoryPath);

            foreach (string fileName in fileNames)
            {
                if (filesInDirectory.Any(filePath => Path.GetFileName(filePath) == fileName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
