using Commons;
using Commons.Extensions;
using HttpServices;
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
    }

    public IServiceProvider Services { get; }











    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();


        var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
        services.RunBackendModuleInitializers(assemblies);


        return services.BuildServiceProvider();
    }









}

