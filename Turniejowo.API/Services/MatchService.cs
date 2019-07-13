using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Exceptions;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;
using Turniejowo.API.UnitOfWork;

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

        public async Task AddNewMatch(Match match)
        {
            if (await teamRepository.FindSingle(x => x.TeamId == match.HomeTeamId) == null || await teamRepository.FindSingle(y => y.TeamId == match.GuestTeamId) == null)
            {
                throw new NotFoundInDatabaseException();
            }

            matchRepository.Add(match);
            await unitOfWork.CompleteAsync();
        }
        
        public async Task DeleteMatch(int id)
        {
            var matchToDel = await matchRepository.FindSingle(x => x.MatchId == id);

            if (matchToDel == null)
            {
                throw new NotFoundInDatabaseException();
            }

            matchRepository.Delete(matchToDel);
            await unitOfWork.CompleteAsync();
        }

        public async Task EditMatch(Match match)
        {
            var matchToEdit = await matchRepository.FindSingle(x => x.MatchId == match.MatchId);

            if (matchToEdit == null)
            {
                throw new NotFoundInDatabaseException();
            }

            if (await teamRepository.FindSingle(x => x.TeamId == match.HomeTeamId) == null || await teamRepository.FindSingle(y => y.TeamId == match.GuestTeamId) == null)
            {
                throw new NotFoundInDatabaseException();
            }

            matchRepository.ClearEntryState(matchToEdit);

            matchRepository.Update(match);
            await unitOfWork.CompleteAsync();
        }

        public async Task<ICollection<Match>> GetAllMatches()
        {
            var matches = await matchRepository.GetAll();

            if (matches.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            return matches;
        }

        public async Task<Match> GetMatchById(int id)
        {
            var match = await matchRepository.GetById(id);

            if (match == null)
            {
                throw new NotFoundInDatabaseException();
            }

            return match;
        }
    }
}
