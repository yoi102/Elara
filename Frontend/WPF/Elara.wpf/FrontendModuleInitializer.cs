using Commons.Interfaces;
using Elara.wpf.Services;
using Elara.wpf.ViewModel;
using InteractionServices.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Elara.wpf;

public class FrontendModuleInitializer : IFrontendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddTransient<LoginWindowViewModel>();
        services.AddTransient<ResetPasswordViewModel>();
        services.AddTransient<CreateAccountViewModel>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<IDialogService, DialogService>();
        services.AddTransient<ISnackbarService, SnackbarService>();
        services.AddTransient<ICultureSettingService, CultureSettingService>();
        services.AddTransient<IThemeSettingService, ThemeSettingService>();
    }
}
