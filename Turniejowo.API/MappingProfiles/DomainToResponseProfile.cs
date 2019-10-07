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
            CreateMap<Match, MatchResponse>()
                .ForMember(dest => dest.HomeTeamName, opt => opt.MapFrom(src => src.HomeTeam.Name))
                .ForMember(dest => dest.GuestTeamName, opt => opt.MapFrom(src => src.GuestTeam.Name));
            CreateMap<Player, PlayerResponse>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name));
            CreateMap<Team, TeamResponse>();
            CreateMap<User, UserResponse>();
            CreateMap<Tournament, TournamentResponse>()
                .ForMember(dest => dest.Discipline, opt => opt.MapFrom(src => src.Discipline.Name))
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.FullName))
                .ForMember(dest => dest.CreatorContact, opt => opt.MapFrom(src => src.Creator.Phone));
        }
    }
}