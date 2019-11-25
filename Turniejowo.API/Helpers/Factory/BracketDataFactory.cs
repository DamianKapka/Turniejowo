using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Helpers.Factory
{
    public class BracketDataFactory
    {
        public static async Task<BracketData> CreateBracketTemplate(int numberOfTeams)
        {
            return await Task.Run(() =>
            {
                var bracketData = new BracketData
                {
                    NumberOfTeams = numberOfTeams
                };
                bracketData.Rounds = new List<BracketRound>();

                int numOfRounds = (int) Math.Log(numberOfTeams,2);

                for (int i = 0; i < numOfRounds; i++)
                {
                    bracketData.Rounds.Add(new BracketRound
                    {
                        RoundIndex = i+1
                    });
                }

                return bracketData;
            });
        }
    }
}
