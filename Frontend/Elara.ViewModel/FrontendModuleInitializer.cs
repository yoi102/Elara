using Commons.Interfaces;
using Elara.wpf.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Elara.ViewModel;

public class FrontendModuleInitializer : IFrontendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddTransient<LoginWindowViewModel>();
        services.AddTransient<ResetPasswordViewModel>();
        services.AddTransient<CreateAccountViewModel>();
        services.AddTransient<MainWindowViewModel>();
    }
}
