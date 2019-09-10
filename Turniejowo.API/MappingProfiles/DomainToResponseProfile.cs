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
        }
    }
}
