using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;
using Turniejowo.API.Services;

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

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IMatchService, MatchService>();
        }
    }
}
