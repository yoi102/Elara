using Commons.Extensions;
using Commons.Helpers;
using Elara.wpf.View;
using Frontend.Shared.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Threading;

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
        LaunchApplicationFlow();

        // 注册全局异常处理器
        DispatcherUnhandledException += OnDispatcherUnhandledException;
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        //UI线程异常

        if (e.Exception is ForceLogoutException)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            MainWindow.Close();
            MainWindow = null;

            LaunchApplicationFlow();
            e.Handled = true;
        }
    }

    private void LaunchApplicationFlow()
    {
        var loginWindow = new LoginWindowView();
        var result = loginWindow.ShowDialog();
        if (result == true)
        {
            //TODO：一写配置加载、cache加载
            MainWindow = new MainWindowView();
            MainWindow.Show();
            MainWindow.Closing += MainWindow_Closing;
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
        else
        {
            Application.Current.Shutdown();
        }
    }

    private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        //TODO：关闭前一些设置保存
    }

    #region Services

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

    #endregion Services
}
