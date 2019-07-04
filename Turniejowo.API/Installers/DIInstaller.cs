using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;

namespace Turniejowo.API.Installers
{
    public class DIInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
