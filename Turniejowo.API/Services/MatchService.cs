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

        public async Task AddNewMatchAsync(Match match)
        {
            if (await teamRepository.FindSingleAsync(x => x.TeamId == match.HomeTeamId) == null || await teamRepository.FindSingleAsync(y => y.TeamId == match.GuestTeamId) == null)
            {
                throw new NotFoundInDatabaseException();
            }

            matchRepository.Add(match);
            await unitOfWork.CompleteAsync();
        }
        
        public async Task DeleteMatchAsync(int id)
        {
            var matchToDel = await matchRepository.FindSingleAsync(x => x.MatchId == id);

            if (matchToDel == null)
            {
                throw new NotFoundInDatabaseException();
            }

            matchRepository.Delete(matchToDel);
            await unitOfWork.CompleteAsync();
        }

        public async Task EditMatchAsync(Match match)
        {
            var matchToEdit = await matchRepository.FindSingleAsync(x => x.MatchId == match.MatchId);

            if (matchToEdit == null)
            {
                throw new NotFoundInDatabaseException();
            }

            if (await teamRepository.FindSingleAsync(x => x.TeamId == match.HomeTeamId) == null || await teamRepository.FindSingleAsync(y => y.TeamId == match.GuestTeamId) == null)
            {
                throw new NotFoundInDatabaseException();
            }

            matchRepository.ClearEntryState(matchToEdit);

            matchRepository.Update(match);
            await unitOfWork.CompleteAsync();
        }

        public async Task<ICollection<Match>> GetAllMatchesAsync()
        {
            var matches = await matchRepository.GetAllAsync();

            if (matches.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            return matches;
        }

        public async Task<Match> GetMatchByIdAsync(int id)
        {
            var match = await matchRepository.GetByIdAsync(id);

            if (match == null)
            {
                throw new NotFoundInDatabaseException();
            }

            return match;
        }
    }
}
