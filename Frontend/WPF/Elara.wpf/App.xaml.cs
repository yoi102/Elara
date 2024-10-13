using Commons;
using Commons.Extensions;
using Elara.wpf.View;
using Elara.wpf.ViewModel;
using HttpServices;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Xml;

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
        var loginWindow = new LoginWindowView();
        if (loginWindow == null)
        {
            throw new ApplicationException();
        }
        var result = loginWindow.ShowDialog();

        if (result == true)
        {
            var mainWindow = Services.GetService<MainWindowView>();
            if (mainWindow == null)
            {
                throw new ApplicationException();
            }
            mainWindow.Show();
        }
        else
        {
            Environment.Exit(0);
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

