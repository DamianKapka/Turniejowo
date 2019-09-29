using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Turniejowo.API.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("turniejowo", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Turniejowo API",
                    Version = "v1.37"
                });
            });
        }
    }
}
