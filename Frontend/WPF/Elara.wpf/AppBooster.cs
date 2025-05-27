using DataProviders.Abstractions;
using Elara.wpf.View;
using ExceptionHandling;
using Frontend.Shared.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Windows;

namespace Elara.wpf;

public class AppBooster
{
    private readonly IServiceProvider _serviceProvider;

    public AppBooster(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Run()
    {
        RegisterGlobalExceptionHandlers();
        StartLoginFlow();
    }

    /// <summary>
    /// 注册全局 UI 线程异常处理
    /// </summary>
    private void RegisterGlobalExceptionHandlers()
    {
        Application.Current.DispatcherUnhandledException += async (sender, e) =>
        {
            if (e.Exception is ForceLogoutException)
            {
                RestartSession();
                e.Handled = true;
            }
            else
            {
                // ❗ 先标记 Handled，避免 await 造成异常泄露
                e.Handled = true;
                var handled = await _serviceProvider.GetService<IExceptionDispatcher>()!.DispatchAsync(e.Exception);

                if (!handled)
                {
                    // 如果没有处理，则显示默认的错误对话框
                    // fallback 处理：记录日志、显示MessageBox等
                    MessageBox.Show("未处理的异常: " + e.Exception.Message);
                    //Debug模式的话就强制中断
                    Debugger.Break();
                }
            }
        };
    }

    /// <summary>
    /// 启动登录流程并根据结果展示主窗口或关闭应用
    /// </summary>
    private void StartLoginFlow()
    {
        var loginWindow = new LoginWindowView();
        bool? loginResult = loginWindow.ShowDialog();

        if (loginResult == true)
        {
            var mainWindow = new MainWindowView();
            App.Current.MainWindow = mainWindow;
            mainWindow.Closing += OnMainWindowClosing;
            mainWindow.Show();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
        else
        {
            Application.Current.Shutdown();
        }
    }

    /// <summary>
    /// 注销用户数据并重启登录流程
    /// </summary>
    private void RestartSession()
    {
        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

        App.Current.MainWindow?.Close();

        var userDataProvider = _serviceProvider.GetService<IUserDataProvider>();
        userDataProvider?.CleanUserData();

        StartLoginFlow();
    }

    private void OnMainWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        // TODO: 保存当前用户的设置、释放资源等
    }
}
