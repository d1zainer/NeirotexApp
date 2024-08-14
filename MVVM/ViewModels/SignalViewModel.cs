using CommunityToolkit.Mvvm.ComponentModel;
using NeirotexApp.UI.Managers;

namespace NeirotexApp.MVVM.ViewModels
{
    public partial class SignalViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _signalFileName;

        [ObservableProperty]
        private int _effectiveFd;

        [ObservableProperty]
        private int _type; // Числовое значение типа

        [ObservableProperty]
        private int _unicNumber;

        [ObservableProperty]
        private double _mathValue;

        [ObservableProperty]
        private double _minValue;

        [ObservableProperty]
        private double _maxValue;

        // Свойство для отображения типа
        [ObservableProperty]
        private string? _typeString;


        /// <summary>
        /// Метод для обновления 
        /// </summary>
        public void UpdateTypeString()
        {
            TypeString = GetSignalTypeString(_type);
        }

        private string? GetSignalTypeString(int signalType)
        {
            return signalType switch
            {
                1 => LanguageManager.Instance.GetString(LanguageManager.TitleKeys.EEG),
                2 => LanguageManager.Instance.GetString(LanguageManager.TitleKeys.ECG),
                3 => LanguageManager.Instance.GetString(LanguageManager.TitleKeys.EMG),
                _ => signalType.ToString()
            };
        }
       

        // Метод для установки типа в начале, с учетом языка
        public void SetType(int newType)
        {
            if (SetProperty(ref _type, newType))
            {
                UpdateTypeString();
            }
        }
    }
}
