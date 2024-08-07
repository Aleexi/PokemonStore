namespace PokemonService;

public interface IPokemonRepository
{
    Task<List<PokemonDto>> GetPokemonsAsync();
    Task<PokemonDto> GetPokemonByIdAsync(Guid id);
    Task<Pokemon> GetPokemonEntiytByIdAsync(Guid id);
    void AddPokemon(Pokemon pokemon);
    void RemovePokemon(Pokemon pokemon);
    Task<bool> SaveChangesAsync();

    Task<List<Pokemon>> GetPokemonsAfterDate(string date);
}
