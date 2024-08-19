using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NeirotexApp.UI.Managers;
using System.Threading;
using NeirotexApp.Services;

namespace NeirotexApp.MVVM.Views;

public partial class SettingControl : UserControl
{
    public SettingControl()
    {

        InitializeComponent();
        ThemeToggleSwitch.Checked += ThemeToggleSwitchOnChecked; 
        ThemeToggleSwitch.Unchecked += ThemeToggleSwitchOnUnchecked;
        LanguageComboBox.SelectionChanged += OnLanguageSelectionChanged;

        InitControlls();
    }

    /// <summary>
    /// инициализация контроллов
    /// </summary>
    public void InitControlls()
    {
        LanguageComboBox.SelectedIndex = Thread.CurrentThread.CurrentCulture.ToString().StartsWith("ru") ? 1 : 0; // Русский
        ThemeToggleSwitch.IsChecked = Application.Current?.ActualThemeVariant.ToString() != "Light";
    }

    private void ThemeToggleSwitchOnUnchecked(object? sender, RoutedEventArgs e)
    {
        SettingService.Instance.SetTheme("Light");
    }

    private void ThemeToggleSwitchOnChecked(object? sender, RoutedEventArgs e)
    {
        SettingService.Instance.SetTheme("Dark");
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