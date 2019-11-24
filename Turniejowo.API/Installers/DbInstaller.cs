using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Turniejowo.API.Models;

namespace Turniejowo.API.Installers
{
    public class DbInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<TurniejowoDbContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString("TurniejowoDirectDB")));
        }
    }
}
