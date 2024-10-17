using Commons.Interfaces;
using Elara.wpf.Services;
using Elara.wpf.View;
using Elara.wpf.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Service.Abstractions;

namespace Elara.wpf
{
    public class FrontendModuleInitializer : IFrontendModuleInitializer
    {


        public void Initialize(IServiceCollection services)
        {
            services.AddTransient<LoginWindowViewModel>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<IDialogService, DialogService>();


        }
    }
}
