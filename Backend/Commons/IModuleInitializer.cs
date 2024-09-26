using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Commons
{
    public interface IModuleInitializer
    {
        void Initialize(IServiceCollection services);
    }
}