using Commons.Interfaces;
using Elara.ViewModel.Chat;
using Elara.ViewModel.Contact;
using Elara.wpf.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Elara.ViewModel;

public class FrontendModuleInitializer : IFrontendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddTransient<ChatShellViewModel>();
        services.AddTransient<ContactInfoViewModel>();
        services.AddTransient<LoginWindowViewModel>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ResetPasswordViewModel>();
        services.AddTransient<SignUpViewModel>();
    }
}
