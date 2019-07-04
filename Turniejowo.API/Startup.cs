using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Turniejowo.API.Helpers;
using Turniejowo.API.Installers;
using Turniejowo.API.Models;
using Turniejowo.API.Models.GenericRepository;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;

namespace Turniejowo.API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var installers = typeof(Startup).Assembly.ExportedTypes.Where(t => typeof(IInstaller).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                            .Select(Activator.CreateInstance)
                            .Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.Install(services, _config));                           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var swaggerOptions = new SwaggerOptions();
            _config.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(options => { options.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(options => { options.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });
            app.UseCors(CorsPolicySettings.CORSPolicyName);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.Run(async context =>
            {
                context.Response.Redirect("swagger/index.html");
            });
        }
    }
}
