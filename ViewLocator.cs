using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using NeirotexApp.UI.Managers;

namespace NeirotexApp
{
    public class ViewLocator : IDataTemplate
    {

        public Control? Build(object? data)
        {
            if (data is null)
                return null;

            var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                var control = (Control)Activator.CreateInstance(type)!;
                control.DataContext = data;
                return control;
            }

            return new TextBlock { Text = LanguageManager.Instance.GetTitleByCulture(LanguageManager.TitleType.ViewLocator) + name };
        }

        public bool Match(object? data)
        {
            return data is ObservableObject;
        }
    }
}
