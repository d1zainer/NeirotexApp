using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using NeirotexApp.App;
using NeirotexApp.MVVM.ViewModels;
using NeirotexApp.UI;
using System;
using System.Globalization;
using System.Threading;
using FileDialog = NeirotexApp.UI.FileDialog;

namespace NeirotexApp.MVVM.Views
{
    public partial class MainWindow : Window
    {
        private readonly FileDialog _fileDialogService = new FileDialog();
        private readonly MainWindowViewModel _viewModel = MainWindowViewModel.Instance;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;

            this.PointerPressed += OnWindowPointerPressed;

            SetInitialCulture();

            LanguageComboBox.SelectionChanged += OnLanguageSelectionChanged;

            ThemeToggleSwitch.IsChecked = false;

            SettingButton.Click += SettingButton_Click;
            SettingPanel.IsVisible = false;
        }

        private void SettingButton_Click(object? sender, RoutedEventArgs e)
        { 
                SettingPanel.IsVisible = !SettingPanel.IsVisible;
            
        }
        #region Toggle
        private void OnThemeToggleChecked(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Dark");
        }

        private void OnThemeToggleUnchecked(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Light");
        }
        #endregion

        /// <summary>
        /// сменя светлой и темной темы
        /// </summary>
        /// <param name="themeName"></param>
        /// <exception cref="ArgumentException"></exception>
        private void ApplyTheme(string themeName)
        {
            var app = App.App.Current;
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
            }
        }
        /// <summary>
        /// выбор языка через комбобокс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLanguageSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                var selectedItem = comboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    // Установите выбранный язык
                    string cultureCode = selectedItem.Content.ToString() == "Русский" ? "ru-RU" : "en-US";
                    SetCulture(cultureCode);

                    // Обновите строки на новой культуре
                    UpdateStrings();
                }
            }
        }

        /// <summary>
        /// считываем текущий язык при инициализации
        /// </summary>
        private void SetInitialCulture()
        {
            string initialCulture = CultureInfo.CurrentCulture.Name;
            SetCulture(initialCulture);

            if (initialCulture.StartsWith("ru"))
            {
                LanguageComboBox.SelectedIndex = 1; // Русский
            }
            else
            {
                LanguageComboBox.SelectedIndex = 0; // Английский
            }


        }
        /// <summary>
        /// Сменяей язык
        /// </summary>
        /// <param name="cultureCode"></param>
        private void SetCulture(string cultureCode)
        {
            var cultureInfo = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            UpdateStrings();
            LangController.Instance.LanguageChanged?.Invoke(); //вызываем событие, что произошла смена

        }
        /// <summary>
        /// обновляем строки на новый язык
        /// </summary>
        private void UpdateStrings()
        {
            InfoWindowTitleTextBlock.Text = LangController.Instance.GetString(LangController.TitleKeys.InfoPanel);
            LoadFileButton.Content = LangController.Instance.GetString(LangController.TitleKeys.LoadFileBtn);
            StartReadingButton.Content = LangController.Instance.GetString(LangController.TitleKeys.StartReadingBtn);
            //обновляем контролы
            foreach (var signalControl in SignalItemsControl.Items)
            {
                if (signalControl is SignalControl sc)
                {
                    sc.UpdateStrings();
                }
            }
            //обновляем вьюмодели
            foreach (var item in _viewModel.ChannelViewModels)
            {
                item.UpdateTypeString();
            }

        }



        /// <summary>
        /// кнопка загрузить файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnOpenFileButtonClick(object sender, RoutedEventArgs e)
        {
            string selectedFile = await _fileDialogService.ShowOpenFileDialog(this) ?? throw new InvalidOperationException();
            try
            {
                if (!string.IsNullOrEmpty(selectedFile))
                {
                    _viewModel.GetBosMeth(selectedFile);
                    LoadSignalControls();
                }
            }
            catch (Exception ex)
            {
                MainWindowViewModel.InformationStringAction?.DynamicInvoke(LangController.InfoMessageType.ErrorMessage, MessageType.Error, ex.Message);
                throw new Exception(ex.Message);

            }
        }

        /// <summary>
        /// кнопка начать чтение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReadFromFileClick(object sender, RoutedEventArgs e)
        {
            _viewModel.StartReadProccesing();
        }

        private void OnWindowPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                return;
            else
                BeginMoveDrag(e);
        }
    

        /// <summary>
        /// загружаем контролы
        /// </summary>
        public void LoadSignalControls()
        {
            SignalItemsControl.Items.Clear();
            foreach (var signal in _viewModel.ChannelViewModels)
            {
                var signalControl = new SignalControl
                {
                    DataContext = signal
                };

                SignalItemsControl.Items.Add(signalControl);
            }
        }
    }
}
