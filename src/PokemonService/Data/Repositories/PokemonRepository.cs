
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace PokemonService;

public class PokemonRepository : IPokemonRepository
{
    private readonly PokemonDbContext _pokemonDbContext;
    private readonly IMapper _mapper;

    public PokemonRepository(PokemonDbContext pokemonDbContext, IMapper mapper)
    {
        _pokemonDbContext = pokemonDbContext;
        _mapper = mapper;
    }

    public async Task<PokemonDTO> GetPokemonByIdAsync(Guid id)
    {
        var pokemon = await _pokemonDbContext.Pokemons.Include(x => x.Attacks).FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<PokemonDTO>(pokemon);
    }

    public async Task<Pokemon> GetPokemonEntiytByIdAsync(Guid id)
    {
        return await _pokemonDbContext.Pokemons.Include(x => x.Attacks).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<PokemonDTO>> GetPokemonsAsync()
    {
        var pokemons = await _pokemonDbContext.Pokemons.Include(x => x.Attacks).ToListAsync();

        return pokemons.Select(_mapper.Map<PokemonDTO>).ToList();
    }

    public void AddPokemon(Pokemon pokemon)
    {
        _pokemonDbContext.Add(pokemon);
    }


    public void RemovePokemon(Pokemon pokemon)
    {
        _pokemonDbContext.Remove(pokemon);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _pokemonDbContext.SaveChangesAsync() > 0;
    }
}
