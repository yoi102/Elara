using Commons.Interfaces;
using Elara.wpf.View;
using Elara.wpf.ViewModel;
using HttpServices.Services;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.wpf
{
    public class FrontendModuleInitializer : IFrontendModuleInitializer
    {


        public void Initialize(IServiceCollection services)
        {
            services.AddTransient<LoginWindowViewModel>();
            services.AddSingleton<MainWindowView>();
            services.AddTransient<MainWindowViewModel>();


        }
    }
}
