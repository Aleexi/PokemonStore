using Grpc.Core;

namespace PokemonService.Services;

public class GrpcListener : GrcpPokemon.GrcpPokemonBase
{
    private readonly ILogger<GrpcListener> _logger;
    private readonly IPokemonRepository _pokemonRepository;

    public GrpcListener(ILogger<GrpcListener> logger, IPokemonRepository pokemonRepository)
    {
        _logger = logger;
        _pokemonRepository = pokemonRepository;
    }

    public override async Task<PokemonsResponse> GetPokemons(PokemonsRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"==> Grpc-Server Received Request with date: {request.Date}");

        var pokemons = await _pokemonRepository.GetPokemonsAfterDate(request.Date);

        if (pokemons.Count == 0) throw new RpcException (new Status(StatusCode.NotFound, "Not Found"));

        // Map pokemons into PokemonsResponse
        var response = new PokemonsResponse();

        response.Pokemons.AddRange(pokemons.Select(pokemon => new PokemonModel {
            Id = pokemon.Id.ToString(),
            Name = pokemon.Name,
            Seller = pokemon.Seller,
            Price = pokemon.Price,
            Type = pokemon.Type.ToString(),
            Healthpower = pokemon.HealthPower,
            Rarity = pokemon.Rarity.ToString(),
            Holographic = pokemon.Holographic,
            CreatedAt = pokemon.CreatedAt.ToString(),
            Imageurl = pokemon.ImageUrl,
            // Use select to transform attack to a AttackModel
            Attacks = { 
                // Initialize the repeated field 
                pokemon.Attacks.Select(attack => new AttackModel
                {
                    Name = attack.Name,
                    Damage = attack.Damage
                }).ToList()
            }
        }));

        return response;
    }

}
