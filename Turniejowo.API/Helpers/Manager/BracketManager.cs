using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Models;

namespace Turniejowo.API.Helpers.Manager
{
    public class BracketManager : IBracketManager
    {
        public async Task<BracketData> FillInBracketWithData(BracketData data, List<Match> matches)
        {
            throw new NotImplementedException();
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
    }
}
