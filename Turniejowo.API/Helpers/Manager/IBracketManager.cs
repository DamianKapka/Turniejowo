using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Helpers.Manager
{
    public interface IBracketManager
    {
        Task<BracketData> FillInBracketWithDataAsync(BracketData data, List<Match> matches);
        Task<BracketData> FillInBracketWithBlankDataAsync(BracketData data);
        Task<int> FindFirstEmptyBracketSlotAsync(ICollection<Match> matches, int teamsQty);
        Task<Match> AutoGenerateBracketMatchAsync(Match correspondingBracketMatch, Match currentlyAddedMatch);
    }
}
