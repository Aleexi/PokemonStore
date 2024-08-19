using AutoMapper;
using CartService.DTOs.ReturnDtos;
using CartService.Entities;

namespace CartService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Cart, CartDto>().ForMember(dest => dest.pokemons, opt => opt.MapFrom(src => src.pokemons));
        CreateMap<Pokemon, PokemonDto>();
    }
}

