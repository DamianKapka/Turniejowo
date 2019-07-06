using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Turniejowo.API.Helpers;
using Turniejowo.API.Installers;

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
