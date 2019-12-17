using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Helpers.Manager;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;

namespace Turniejowo.API.Services
{
    public class ScheduleGeneratorService : IScheduleGeneratorService
    {
        private readonly IScheduleGeneratorManager scheduleGeneratorManager;
        private readonly ITeamRepository teamRepository;
        private readonly ITournamentRepository tournamentRepository;

        public ScheduleGeneratorService(IScheduleGeneratorManager scheduleGeneratorManager, ITeamRepository teamRepository, ITournamentRepository tournamentRepository)
        {
            this.scheduleGeneratorManager = scheduleGeneratorManager;
            this.teamRepository = teamRepository;
            this.tournamentRepository = tournamentRepository;
        }

        public async Task GenerateScheduleAsync(GeneratorScheduleOutlines outlines)
        {
            var possibleMatchDateTimes = await scheduleGeneratorManager.GetPossibleMatchDateTimesAsync(outlines);

            if (!possibleMatchDateTimes.Any())
            {
                throw new ArgumentOutOfRangeException();
            }

            var teams = await teamRepository.FindAsync(t => t.TournamentId == outlines.TournamentId);

            var matchDict = await scheduleGeneratorManager.GetPossibleMatchMatrix(teams.ToList());

            var tournament = await tournamentRepository.FindSingleAsync(t => t.TournamentId == outlines.TournamentId);

            var generatedMatchesList =
                await scheduleGeneratorManager.GenerateSchedule(tournament.IsBracket, possibleMatchDateTimes,
                    matchDict);
        }
    }
}
