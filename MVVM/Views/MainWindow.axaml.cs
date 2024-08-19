using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using NeirotexApp.MVVM.ViewModels;
using NeirotexApp.UI.Managers;
using FileDialog = NeirotexApp.UI.FileDialog;

namespace NeirotexApp.MVVM.Views;

public partial class MainWindow : Window
{
    public static Action UpdateUi = delegate { };
    private readonly FileDialog _fileDialogService = new();
    private readonly MainWindowViewModel _viewModel = MainWindowViewModel.Instance;
    private readonly SettingControl _mySettingControl;


    public MainWindow()
    {
        InitializeComponent();
        UpdateStrings();
        DataContext = _viewModel;

        PointerPressed += OnWindowPointerPressed;


        UpdateUi += UpdateStrings;

        SettingButton.Click += SettingButton_Click;

        _mySettingControl = new SettingControl
        {
            Margin = new Thickness(60, 10, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
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
    ///     обновляем строки на новый язык
    /// </summary>
    private void UpdateStrings()
    {
        InfoWindowTitleTextBlock.Text = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.InfoPanel);
        LoadFileButton.Content = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.LoadFileBtn);
        StartReadingButton.Content = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.StartReadingBtn);
        //обновляем контролы
        foreach (var signalControl in SignalItemsControl.Items)
            if (signalControl is SignalControl sc)
                sc.UpdateStrings();
        //обновляем вьюмодели
        foreach (var item in _viewModel.ChannelViewModels) item.UpdateTypeString();
    }


    /// <summary>
    ///     кнопка загрузить файл
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnOpenFileButtonClick(object sender, RoutedEventArgs e)
    {
        var selectedFile = await _fileDialogService.ShowOpenFileDialog(this);

        if (!string.IsNullOrEmpty(selectedFile))
        {
            _viewModel.GetBosMeth(selectedFile);
            LoadSignalControls();
        }
    }

    /// <summary>
    ///     кнопка начать чтение
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnReadFromFileClick(object sender, RoutedEventArgs e)
    {
        _viewModel.StartReadProccesingAsync();
    }

    private void OnWindowPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
            return;


        var pointerPosition = e.GetPosition(this);
        var settingPanelBounds = _mySettingControl.Bounds;

        if (settingPanelBounds.Contains(pointerPosition)) return;
        BeginMoveDrag(e);
    }


    /// <summary>
    ///     загружаем контролы
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