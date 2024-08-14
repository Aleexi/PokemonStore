using AutoMapper;
using Contracts;

namespace PokemonService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // Map Attack to AttackDTO
        CreateMap<Attack, AttackDto>();

        // Map Pokemon to PokemonDto, and tell automapper how to map attacks
        CreateMap<Pokemon, PokemonDto>()
            .ForMember(dest => dest.Attacks, opt => opt.MapFrom(src => src.Attacks));

        // Mapping between CreatePokemonDto and Pokemon
        CreateMap<CreatePokemonDto, Pokemon>()
            .ForMember(dest => dest.Attacks, opt => opt.MapFrom(src => src.Attacks));

        // Mapping between AttackDto and Attack
        CreateMap<AttackDto, Attack>();

        /* Mapping profiles for Contracts */ 
        CreateMap<Pokemon, PokemonCreated>()
            .ForMember(dest => dest.Attacks, opt => opt.Ignore()); // Ignore Attacks property
        CreateMap<Pokemon, PokemonUpdated>();
        CreateMap<Pokemon, PokemonDeleted>();
    }
}
