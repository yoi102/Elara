using Commons.Extensions;
using Commons.Helpers;
using Elara.wpf.View;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Elara.wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        string lang = System.Globalization.CultureInfo.CurrentCulture.Name;
        var culture = new System.Globalization.CultureInfo(lang);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        I18NExtension.Culture = culture;

        Services = ConfigureServices();
        this.InitializeComponent();
        ShowWindow();
    }

    private void ShowWindow()
    {
        MainWindow = new MainWindowView();

        var loginWindow = new LoginWindowView();
        var result = loginWindow.ShowDialog();

        if (result == true)
        {
            MainWindow.Show();
        }
        else
        {
            Application.Current.Shutdown();
        }
    }

    public IServiceProvider Services { get; }

    /// <summary>
    /// Gets the current <see cref="App"/> instance in use
    /// </summary>
    public new static App Current => (App)Application.Current;

    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
        services.RunFrontendModuleInitializers(assemblies);

        return services.BuildServiceProvider();
    }
}
