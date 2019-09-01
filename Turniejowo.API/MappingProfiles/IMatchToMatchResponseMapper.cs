using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Models;

namespace Turniejowo.API.MappingProfiles
{
    public interface IMatchToMatchResponseMapper : ICustomMappingProfile<Match,MatchResponse>
    {
    }
}
