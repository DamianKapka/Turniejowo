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

        public ScheduleGeneratorService(IScheduleGeneratorManager scheduleGeneratorManager, ITeamRepository teamRepository)
        {
            this.scheduleGeneratorManager = scheduleGeneratorManager;
            this.teamRepository = teamRepository;
        }

        public async Task GenerateScheduleAsync(GeneratorScheduleOutlines outlines)
        {
            var possibleMatchDateTimes = await scheduleGeneratorManager.GetPossibleMatchDateTimesAsync(outlines);

            if (!possibleMatchDateTimes.Any())
            {
                throw new ArgumentOutOfRangeException();
            }

            var teams = await teamRepository.FindAsync(t => t.TournamentId == outlines.TournamentId);

            var possibleMatches = await scheduleGeneratorManager.GetPossibleMatchScenarios(teams.ToList());

            if (possibleMatches.Count > possibleMatchDateTimes.Count)
            {
                throw new DataMisalignedException();
            }


        }
    }
}
