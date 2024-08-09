using AutoMapper;
using Contracts;
using Contracts.PublicClasses;
using SearchService.Entities;

namespace SearchService.RequestHelpers
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            CreateMap<AttackContract, Attack>();
            CreateMap<PokemonCreated, Pokemon>()
                .ForMember(dest => dest.Attacks, opt => opt.MapFrom(src => src.Attacks));
        }
    }
}
