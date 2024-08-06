using AutoMapper;

namespace PokemonService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // Map Attack to AttackDTO
        CreateMap<Attack, AttackDTO>();

        // Map Pokemon to PokemonDTO, and tell automapper how to map attacks
        CreateMap<Pokemon, PokemonDTO>()
            .ForMember(dest => dest.Attacks, opt => opt.MapFrom(src => src.Attacks));

        // Mapping between CreatePokemonDTO and Pokemon
        CreateMap<CreatePokemonDTO, Pokemon>()
            .ForMember(dest => dest.Attacks, opt => opt.MapFrom(src => src.Attacks));

        // Mapping between AttackDTO and Attack
        CreateMap<AttackDTO, Attack>();

    }
}
