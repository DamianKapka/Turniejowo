using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Turniejowo.API.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration config)
        {
            services.AddMvc();
        }
    }
}
