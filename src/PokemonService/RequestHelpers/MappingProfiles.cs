using AutoMapper;
using PokemonService.Entities;

namespace PokemonService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Pokemon, PokemonDTO>();
    }
}
