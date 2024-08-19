using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NeirotexApp.UI.Managers;
using System.Threading;
using Material.Styles.Assists;
using NeirotexApp.Services;


namespace NeirotexApp.MVVM.Views;

public partial class SettingControl : UserControl
{
    public SettingControl()
    {

        InitializeComponent();
        InitControlls();
        ThemeToggleSwitch.IsCheckedChanged += ThemeToggleSwitchChecked;
        
        LanguageComboBox.SelectionChanged += OnLanguageSelectionChanged;

        
    }

    /// <summary>
    /// инициализация контроллов
    /// </summary>
    public void InitControlls()
    {
       
        LanguageComboBox.SelectedIndex = Thread.CurrentThread.CurrentCulture.ToString().StartsWith("ru") ? 1 : 0; // Русский
        ThemeToggleSwitch.IsChecked = Application.Current?.ActualThemeVariant.ToString() != "Light";
        
    }

    /// <summary>
    /// обновление текста контролов в зависимоти от языка
    /// </summary>
    public void UpdateControlls()
    {
        ComboBoxAssist.SetLabel(LanguageComboBox, LanguageManager.Instance.GetString(LanguageManager.TitleKeys.LanguageComboBox)!);
        ThemeToggleSwitch.Content = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.ThemeToggle);
    }

    private void ThemeToggleSwitchChecked(object? sender, RoutedEventArgs e)
    {
        if (sender is ToggleSwitch toggleSwitch)
        {
            if (toggleSwitch.IsChecked == true)
            {
                SettingService.Instance.SetTheme("Dark");
            }
            else
            {
                SettingService.Instance.SetTheme("Light");
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
                SettingService.Instance.SetCulture(cultureCode);
                // Обновите строки на новой культуре
                MainWindow.UpdateUi?.Invoke();
            }
        }
    }
}