using Avalonia.Controls;
using NeirotexApp.MVVM.ViewModels;
using NeirotexApp.UI.Managers;
using System;

namespace NeirotexApp.MVVM.Views;

public partial class SignalControl : UserControl
{
    public SignalControl()
    {
        InitializeComponent();
        DataContext = new SignalViewModel();
        UpdateStrings();


    }

    /// <summary>
    /// обновляем строки внутри контроля
    /// </summary>
    public void UpdateStrings()
    {
        if (InfoHeaderTextBlock != null)
            InfoHeaderTextBlock.Text = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.InfoPanelChannel);
        if (InfoHeaderTextBlock != null)
            InfoHeaderTextBlock.Text = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.InfoPanelChannel);

        if (ValuesHeaderTextBlock != null)
            ValuesHeaderTextBlock.Text = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.ChannelValue);

        if (NameLabelTextBlock != null)
            NameLabelTextBlock.Text = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.SignalFileName);
      
        if (NumberLabelTextBlock != null)
            NumberLabelTextBlock.Text = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.UnicNumber);

        if (TypeLabelTextBlock != null)
            TypeLabelTextBlock.Text = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.Type);

        if (FrequencyLabelTextBlock != null)
            FrequencyLabelTextBlock.Text = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.EffectiveFd);

        if (MathValueLabelTextBlock != null)
            MathValueLabelTextBlock.Text = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.MathValue);

        if (MinValueLabelTextBlock != null)
            MinValueLabelTextBlock.Text = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.MinValue);

        if (MaxValueLabelTextBlock != null)
            MaxValueLabelTextBlock.Text = LanguageManager.Instance.GetString(LanguageManager.TitleKeys.MaxValue);
    }

}