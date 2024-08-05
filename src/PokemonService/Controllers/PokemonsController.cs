using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonService.Entities;

namespace PokemonService.Controllers;

[ApiController]
[Route("Pokemons")]
public class PokemonsController : ControllerBase
{
    private readonly IMapper _mapper;

    public PokemonsController(IMapper mapper)
    {
        _mapper = mapper;
    }
    [HttpGet]
    public ActionResult<int> Get()
    {
        var pokemons = new List<Pokemon>{
            new Pokemon {
                Id = Guid.NewGuid(),
                Name = "Darkrai",
                Price = 1000,
                Type = TypeEnum.Dark,
                HealthPower = 150,
                Rarity = RarityEnum.Legendary,
                Holographic = true,
                Attacks = new List<Attack>
                {
                    new Attack
                    {
                        Name = "Nightmare",
                        Damage = 120
                    },
                    new Attack
                    {
                        Name = "Dark Blast",
                        Damage = 100
                    }
                }
            },
            new Pokemon {
                Id = Guid.NewGuid(),
                Name = "Arceus",
                Price = 800,
                Type = TypeEnum.Normal,
                HealthPower = 200,
                Rarity = RarityEnum.Legendary,
                Holographic = true,
                Attacks = new List<Attack>
                {
                    new Attack
                    {
                        Name = "Normal Blast",
                        Damage = 100
                    },
                    new Attack
                    {
                        Name = "Divine Departure",
                        Damage = 150
                    }
                }
            }
        };
        return Ok(pokemons.Select(_mapper.Map<PokemonDTO>).ToList());
    }
}
