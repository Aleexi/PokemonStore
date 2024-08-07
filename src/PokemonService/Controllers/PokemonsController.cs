using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PokemonService;

[ApiController]
[Route("api/pokemons")]
public class PokemonsController : ControllerBase
{
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public PokemonsController(IPokemonRepository pokemonRepository, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<PokemonDto>>> GetPokemons()
    {
       var pokemons = await _pokemonRepository.GetPokemonsAsync();

       if (pokemons == null) return BadRequest("There exists no pokemons in the database...");

       return Ok(pokemons);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PokemonDto>> GetPokemonById(Guid id)
    {
        var pokemon = await _pokemonRepository.GetPokemonByIdAsync(id);

        if (pokemon == null) return NotFound();

        return Ok(pokemon);
    }

    // [Authorize]
    [HttpPost]
    public async Task<ActionResult<PokemonDto>> AddPokemon([FromBody] CreatePokemonDto createPokemonDto)
    {
        var pokemon = _mapper.Map<Pokemon>(createPokemonDto);

        pokemon.Id = Guid.NewGuid();

        foreach (var attack in pokemon.Attacks)
        {
            attack.Id = Guid.NewGuid();
        }

        _pokemonRepository.AddPokemon(pokemon);

        var successfull = await _pokemonRepository.SaveChangesAsync();

        if (!successfull) return BadRequest("Couldn't create pokemon...");

        await _publishEndpoint.Publish(_mapper.Map<PokemonCreated>(pokemon));

        return CreatedAtAction(nameof(GetPokemonById), new {pokemon.Id}, _mapper.Map<PokemonDto>(pokemon));
    }

    // [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<PokemonDto>> UpdatePokemon([FromBody] UpdatePokemonDto updatePokemonDto, Guid id)
    {
        var pokemon = await _pokemonRepository.GetPokemonEntiytByIdAsync(id);

        if (pokemon == null) return NotFound();

        pokemon.Price = updatePokemonDto.Price > 0 ? updatePokemonDto.Price : pokemon.Price;

        var successfull = await _pokemonRepository.SaveChangesAsync();

        if (!successfull) return BadRequest("Couldn't update pokemon...");

        await _publishEndpoint.Publish(_mapper.Map<PokemonUpdated>(pokemon));

        return Ok(_mapper.Map<PokemonDto>(pokemon));
    }

    // [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePokemon(Guid id)
    {
        var pokemon = await _pokemonRepository.GetPokemonEntiytByIdAsync(id);

        if (pokemon == null) return NotFound();

        _pokemonRepository.RemovePokemon(pokemon);

        var successfull = await _pokemonRepository.SaveChangesAsync();

        if (!successfull) return BadRequest("Failed to remove pokemon entity from database...");

        await _publishEndpoint.Publish(_mapper.Map<PokemonDeleted>(pokemon));

        return Ok();
    }
}
