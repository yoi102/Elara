using Commons;
using Commons.Extensions;
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
        Services = ConfigureServices();
        this.InitializeComponent();
        ShowWindow();
    }

    private void ShowWindow()
    {
        MainWindow = new MainWindowView();
        //MainWindow.Show();

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

