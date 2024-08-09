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
        private readonly FileDialog _fileDialogService = new FileDialog();
        private readonly MainWindowViewModel _viewModel = MainWindowViewModel.Instance;
        private SettingControl _mySettingControl;


        public static Action UpdateUi = delegate { };


        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;

            this.PointerPressed += OnWindowPointerPressed;

            InitSettings();
            UpdateUi += UpdateStrings;

            SettingButton.Click += SettingButton_Click;

            _mySettingControl = new SettingControl()
            {
                Margin = new Thickness(60, 10, 0, 0),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
                SettingPanel =
                {
                    IsVisible = false
                }
            };
            
            MyGrid.Children.Add(_mySettingControl);


        }
        
        private void SettingButton_Click(object? sender, RoutedEventArgs e)
        {
            _mySettingControl.SettingPanel.IsVisible = !_mySettingControl.SettingPanel.IsVisible;
        }


        /// <summary>
        /// ������������� ���������� - ���� � ���� ����
        /// </summary>
        private void InitSettings()
        {
            LangController.Instance.LoadCulture();
            ThemeController.Instance.LoadTheme();
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
            _viewModel.StartReadProccesingAsync();
        }

        private void OnWindowPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                return;

           
            var pointerPosition = e.GetPosition(this); 
            var settingPanelBounds = _mySettingControl.Bounds; 

            if (settingPanelBounds.Contains(pointerPosition))
            {
                return;
            }
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
