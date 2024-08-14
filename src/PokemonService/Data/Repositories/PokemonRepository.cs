
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

    public async Task<PokemonDto> GetPokemonByIdAsync(Guid id)
    {
        var pokemon = await _pokemonDbContext.Pokemons.Include(x => x.Attacks).FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<PokemonDto>(pokemon);
    }

    public async Task<Pokemon> GetPokemonEntiytByIdAsync(Guid id)
    {
        return await _pokemonDbContext.Pokemons.Include(x => x.Attacks).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<PokemonDto>> GetPokemonsAsync()
    {
        var pokemons = await _pokemonDbContext.Pokemons.Include(x => x.Attacks).ToListAsync();

        return pokemons.Select(_mapper.Map<PokemonDto>).ToList();
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

    public async Task<List<Pokemon>> GetPokemonsAfterDate(string date)
    {
        // If there is no date return all entities in Pokemons 
        if (date == null) return await _pokemonDbContext.Pokemons.Include(x => x.Attacks).ToListAsync();

        return await _pokemonDbContext.Pokemons.Include(x => x.Attacks).Where(x => x.CreatedAt > DateTime.Parse(date)).ToListAsync();
    }
}
