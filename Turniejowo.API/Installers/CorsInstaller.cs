using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Turniejowo.API.Helpers;

namespace Turniejowo.API.Installers
{
    public class CorsInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicySettings.CORSPolicyName, builder =>
                {
                    builder.WithOrigins().AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }
    }
}
