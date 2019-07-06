using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;
using Turniejowo.API.Models.Repositories;
using Turniejowo.API.Models.UnitOfWork;

namespace Turniejowo.API.Services
{
    public class MatchService : IMatchService
    {
        private readonly ITeamRepository teamRepository;
        private readonly IMatchRepository matchRepository;
        private readonly IUnitOfWork unitOfWork;

        public MatchService(IMatchRepository matchRepository, IUnitOfWork unitOfWork, ITeamRepository teamRepository)
        {
            this.matchRepository = matchRepository;
            this.unitOfWork = unitOfWork;
            this.teamRepository = teamRepository;
        }

        public void AddNewMatch(Match match)
        {
            throw new NotImplementedException();
        }

        public void DeleteMatch(int id)
        {
            throw new NotImplementedException();
        }

        public void EditMatch(Match match)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Match>> GetAllMatches()
        {
            var matches = await matchRepository.GetAll();

            return matches;
        }

        public async Task<Match> GetMatchById(int id)
        {
            var match = await matchRepository.GetById(id);

            return match;
        }
    }
}
