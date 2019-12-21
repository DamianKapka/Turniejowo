using System;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Exceptions;
using Turniejowo.API.Helpers.Manager;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;
using Turniejowo.API.UnitOfWork;

namespace Turniejowo.API.Services
{
    public class ScheduleGeneratorService : IScheduleGeneratorService
    {
        private readonly IScheduleGeneratorManager scheduleGeneratorManager;
        private readonly ITeamRepository teamRepository;
        private readonly ITournamentRepository tournamentRepository;
        private readonly IMatchRepository matchRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScheduleGeneratorService(IScheduleGeneratorManager scheduleGeneratorManager, ITeamRepository teamRepository, ITournamentRepository tournamentRepository, IMatchRepository matchRepository, IUnitOfWork unitOfWork)
        {
            this.scheduleGeneratorManager = scheduleGeneratorManager;
            this.teamRepository = teamRepository;
            this.tournamentRepository = tournamentRepository;
            this.matchRepository = matchRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task GenerateScheduleAsync(GeneratorScheduleOutlines outlines)
        {
            var possibleMatchDateTimes = await scheduleGeneratorManager.GetPossibleMatchDateTimesAsync(outlines);

            if (!possibleMatchDateTimes.Any())
            {
                throw new ArgumentOutOfRangeException();
            }

            var teams = await teamRepository.FindAsync(t => t.TournamentId == outlines.TournamentId);

            if (teams.Count < 2)
            {
                throw new NotFoundInDatabaseException();
            }

            var matchDict = await scheduleGeneratorManager.GetPossibleMatchMatrix(teams.ToList());

            var tournament = await tournamentRepository.FindSingleAsync(t => t.TournamentId == outlines.TournamentId);

            var generatedMatchesList =
                await scheduleGeneratorManager.GenerateSchedule(tournament.IsBracket, possibleMatchDateTimes,
                    matchDict);

            generatedMatchesList.ForEach(gm => matchRepository.Add(gm));

            await unitOfWork.CompleteAsync();
        }
    }
}
