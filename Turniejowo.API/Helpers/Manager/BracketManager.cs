using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Models;

namespace Turniejowo.API.Helpers.Manager
{
    public class BracketManager : IBracketManager
    {
        private IMapper mapper;

        public BracketManager(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<BracketData> FillInBracketWithData(BracketData data, List<Match> matches)
        {
            return await Task.Run(() =>
            {
                int[] eachRoundMatchQty = ComputeMatchQtyForTournamentRounds(data.Rounds.Count,data.NumberOfTeams);

                data.Rounds.ForEach(r => r.Matches = new List<MatchResponse>());

                foreach (var match in matches)
                {
                    data.Rounds[0].Matches.Add(mapper.Map<MatchResponse>(match));
                }

                for (int i = 0; i < eachRoundMatchQty.Length; i++)
                {
                    while (data.Rounds[i].Matches.Count < eachRoundMatchQty[i])
                    {
                        data.Rounds[i].Matches.Add(new MatchResponse());
                    }
                }

                return data;
            });
        }

        public async Task<BracketData> FillInBracketWithBlankData(BracketData data)
        {
            return await Task.Run(() =>
            {
                for (int i = 1; i <= data.Rounds.Count; i++)
                {
                    var numOfIterationDivide = (int) Math.Pow(2, i);
                    var iterationQty = data.NumberOfTeams / numOfIterationDivide;
                    data.Rounds[i - 1].Matches = new List<MatchResponse>();
                    Parallel.For(0, iterationQty,
                        (index) => { data.Rounds[i - 1].Matches.Add(new MatchResponse());});
                }

                return data;
            });
        }

        private int[] ComputeMatchQtyForTournamentRounds(int roundsQty, int teamQty)
        {
            int[] roundMatchesQty = new int[roundsQty];

            for (int i = 0; i < roundsQty; i++)
            {
                roundMatchesQty[i] = (int)(teamQty / Math.Pow(2, i + 1));
            }

            return roundMatchesQty;
        }
    }
}
