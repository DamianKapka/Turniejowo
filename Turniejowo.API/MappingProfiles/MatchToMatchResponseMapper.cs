using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Models;
using Turniejowo.API.Services;

namespace Turniejowo.API.MappingProfiles
{
    public class MatchToMatchResponseMapper : IMatchToMatchResponseMapper
    {
        private readonly ITeamService teamService;

        public MatchToMatchResponseMapper(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public async Task<List<MatchResponse>> Map(Match[] objToMap)
        {
            var matchResponses = new List<MatchResponse>();

            foreach (var match in objToMap)
            {
                var homeTeam = await teamService.GetTeamByIdAsync((int)match.HomeTeamId);
                var guestTeam = await teamService.GetTeamByIdAsync((int)match.GuestTeamId);

                matchResponses.Add(new MatchResponse()
                {
                    MatchId = match.MatchId,
                    MatchDateTime = match.MatchDateTime,
                    IsFinished = match.IsFinished,
                    GuestTeamId = match.GuestTeamId,
                    GuestTeamName = guestTeam.Name,
                    GuestTeamPoints = match.GuestTeamPoints,
                    HomeTeamId = match.HomeTeamId,
                    HomeTeamName = homeTeam.Name,
                    HomeTeamPoints = match.HomeTeamPoints
                });
            }

            return matchResponses;
        }
    }
}
