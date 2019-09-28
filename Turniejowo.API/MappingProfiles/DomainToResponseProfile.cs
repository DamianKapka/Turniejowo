using AutoMapper;

using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Models;

namespace Turniejowo.API.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Team, TableEntry>().ForMember(t => t.TeamName, opt => opt.MapFrom(src => src.Name));
            CreateMap<Match, MatchResponse>().ForMember(dest => dest.HomeTeamName, opt => opt.MapFrom(src => src.HomeTeam.Name))
                                             .ForMember(dest => dest.GuestTeamName, opt => opt.MapFrom(src => src.GuestTeam.Name));

        }
    }
}
