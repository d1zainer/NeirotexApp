using Avalonia.Controls;
using NeirotexApp.App;
using NeirotexApp.MVVM.ViewModels;
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
            InfoHeaderTextBlock.Text = LangController.Instance.GetString(LangController.TitleKeys.InfoPanelChannel);
        if (InfoHeaderTextBlock != null)
            InfoHeaderTextBlock.Text = LangController.Instance.GetString(LangController.TitleKeys.InfoPanelChannel);

        if (ValuesHeaderTextBlock != null)
            ValuesHeaderTextBlock.Text = LangController.Instance.GetString(LangController.TitleKeys.ChannelValue);

        if (NameLabelTextBlock != null)
            NameLabelTextBlock.Text = LangController.Instance.GetString(LangController.TitleKeys.SignalFileName);
      
        if (NumberLabelTextBlock != null)
            NumberLabelTextBlock.Text = LangController.Instance.GetString(LangController.TitleKeys.UnicNumber);

        if (TypeLabelTextBlock != null)
            TypeLabelTextBlock.Text = LangController.Instance.GetString(LangController.TitleKeys.Type);

        if (FrequencyLabelTextBlock != null)
            FrequencyLabelTextBlock.Text = LangController.Instance.GetString(LangController.TitleKeys.EffectiveFd);

        if (MathValueLabelTextBlock != null)
            MathValueLabelTextBlock.Text = LangController.Instance.GetString(LangController.TitleKeys.MathValue);

        if (MinValueLabelTextBlock != null)
            MinValueLabelTextBlock.Text = LangController.Instance.GetString(LangController.TitleKeys.MinValue);

        if (MaxValueLabelTextBlock != null)
            MaxValueLabelTextBlock.Text = LangController.Instance.GetString(LangController.TitleKeys.MaxValue);
    }

}