using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using NeirotexApp.App;
using NeirotexApp.MVVM.ViewModels;
using NeirotexApp.Services;
using NeirotexApp.UI;
using System;
using System.Globalization;
using System.Threading;
using Avalonia;
using FileDialog = NeirotexApp.UI.FileDialog;

namespace NeirotexApp.MVVM.Views
{
    public partial class MainWindow : Window
    {
        private readonly FileDialog _fileDialogService = new ();
        private readonly MainWindowViewModel _viewModel = MainWindowViewModel.Instance;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;

            this.PointerPressed += OnWindowPointerPressed;

            SetInitialSetting();

            LanguageComboBox.SelectionChanged += OnLanguageSelectionChanged;

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
            ThemeController.Instance.ApplyTheme("Dark");
        }

        private void OnThemeToggleUnchecked(object sender, RoutedEventArgs e)
        {
            ThemeController.Instance.ApplyTheme("Light");
        }
        #endregion

      
        /// <summary>
        /// ����� ����� ����� ���������
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
                    // ���������� ��������� ����
                    string cultureCode = selectedItem.Content.ToString() == "�������" ? "ru-RU" : "en-US";
                    LangController.Instance.SetCulture(cultureCode);
                    // �������� ������ �� ����� ��������
                    UpdateStrings();
                }
            }
        }

        /// <summary>
        /// ������������� ���������� - ���� � ���� ����
        /// </summary>
        private void SetInitialSetting()
        {
            LangController.Instance.LoadCulture();
            ThemeController.Instance.LoadTheme();

            LanguageComboBox.SelectedIndex = Thread.CurrentThread.CurrentCulture.ToString().StartsWith("ru") ? 1 : 0; // �������
            ThemeToggleSwitch.IsChecked = Application.Current?.ActualThemeVariant.ToString() != "Light";

            UpdateStrings();
        }
       
        /// <summary>
        /// ��������� ������ �� ����� ����
        /// </summary>
        private void UpdateStrings()
        {
            InfoWindowTitleTextBlock.Text = LangController.Instance.GetString(LangController.TitleKeys.InfoPanel);
            LoadFileButton.Content = LangController.Instance.GetString(LangController.TitleKeys.LoadFileBtn);
            StartReadingButton.Content = LangController.Instance.GetString(LangController.TitleKeys.StartReadingBtn);
            //��������� ��������
            foreach (var signalControl in SignalItemsControl.Items)
            {
                if (signalControl is SignalControl sc)
                {
                    sc.UpdateStrings();
                }
            }
            //��������� ���������
            foreach (var item in _viewModel.ChannelViewModels)
            {
                item.UpdateTypeString();
            }

        }



        /// <summary>
        /// ������ ��������� ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnOpenFileButtonClick(object sender, RoutedEventArgs e)
        {
            string? selectedFile = await _fileDialogService.ShowOpenFileDialog(this);
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
                MainWindowViewModel.InformationStringAction?.DynamicInvoke(LangController.InfoMessageType.ErrorMessage,
                    MessageType.Error, ex.Message);
                throw new Exception(ex.Message);

            }
        }

        /// <summary>
        /// ������ ������ ������
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
        /// ��������� ��������
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
