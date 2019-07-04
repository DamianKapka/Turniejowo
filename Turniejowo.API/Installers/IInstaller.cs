using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turniejowo.API.Installers
{
    public interface IInstaller
    {
        void Install(IServiceCollection services, IConfiguration config);
    }
}
