using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Helpers.Manager
{
    public interface IBracketManager
    {
        Task<BracketData> FillInBracketWithData(BracketData data, List<Match> matches);
        Task<BracketData> FillInBracketWithBlankData(BracketData data);
        Task<int> FindFirstEmptyBracketSlot(ICollection<Match> matches, int teamsQty);
        Task<Match> AutoGenerateBracketMatch();
    }
}
