using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using NeirotexApp.MVVM.Views;


namespace NeirotexApp.App
{
    public partial class App : Application
    {
        

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                // Установка текущей культуры приложения
                


                desktop.MainWindow = new MainWindow
                {


                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}