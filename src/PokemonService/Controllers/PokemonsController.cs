using AutoMapper;
using MassTransit.Futures.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokemonService;

[ApiController]
[Route("api/pokemons")]
public class PokemonsController : ControllerBase
{
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;

    public PokemonsController(IPokemonRepository pokemonRepository, IMapper mapper)
    {
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<PokemonDTO>>> GetPokemons()
    {
       var pokemons = await _pokemonRepository.GetPokemonsAsync();

       if (pokemons == null) return BadRequest("There exists no pokemons in the database...");

       return Ok(pokemons);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PokemonDTO>> GetPokemonById(Guid id)
    {
        var pokemon = await _pokemonRepository.GetPokemonByIdAsync(id);

        if (pokemon == null) return NotFound();

        return Ok(pokemon);
    }

    // [Authorize]
    [HttpPost]
    public async Task<ActionResult<PokemonDTO>> AddPokemon([FromBody] CreatePokemonDTO createPokemonDTO)
    {
        var pokemon = _mapper.Map<Pokemon>(createPokemonDTO);

        pokemon.Id = Guid.NewGuid();

        foreach (var attack in pokemon.Attacks)
        {
            attack.Id = Guid.NewGuid();
        }

        _pokemonRepository.AddPokemon(pokemon);

        var successfull = await _pokemonRepository.SaveChangesAsync();

        if (!successfull) return BadRequest("Couldn't create pokemon...");

        // Publish to RabbitMQ bus an PokemonCreated Event emitt!
        // ...

        return CreatedAtAction(nameof(GetPokemonById), new {pokemon.Id}, _mapper.Map<PokemonDTO>(pokemon));
    }

    // [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<PokemonDTO>> UpdatePokemon([FromBody] UpdatePokemonDTO updatePokemonDTO, Guid id)
    {
        var pokemon = await _pokemonRepository.GetPokemonEntiytByIdAsync(id);

        if (pokemon == null) return NotFound();

        pokemon.Price = updatePokemonDTO.Price > 0 ? updatePokemonDTO.Price : pokemon.Price;

        var successfull = await _pokemonRepository.SaveChangesAsync();

        if (!successfull) return BadRequest("Couldn't update pokemon...");

        // Publish to RabbitMQ UpdatePokemon Event emitt!
        // ...

        return Ok(_mapper.Map<PokemonDTO>(pokemon));
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

        // Publish to RabbitMQ deletePokemon Event emitt!
        // ...

        return Ok();
    }
}
