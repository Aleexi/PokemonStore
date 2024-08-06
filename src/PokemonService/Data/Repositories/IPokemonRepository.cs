namespace PokemonService;

public interface IPokemonRepository
{
    Task<List<PokemonDTO>> GetPokemonsAsync();
    Task<PokemonDTO> GetPokemonByIdAsync(Guid id);
    Task<Pokemon> GetPokemonEntiytByIdAsync(Guid id);
    void AddPokemon(Pokemon pokemon);
    void RemovePokemon(Pokemon pokemon);
    Task<bool> SaveChangesAsync();
}
