using AutoMapper;
using Grpc.Net.Client;
using PokemonService;
using SearchService.Entities;
using System.Runtime.Serialization;
using System.Text.Json;

namespace SearchService.Services;

public class GrpcSender
{
    private readonly ILogger<GrpcSender> _logger;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public GrpcSender(ILogger<GrpcSender> logger, IConfiguration config, IMapper mapper)
    {
        _logger = logger;
        _config = config;
        _mapper = mapper;
    }
    public List<Pokemon> GetPokemons(string date)
    {
        _logger.LogInformation("Starting GrpcSender...");

        var channel = GrpcChannel.ForAddress(_config["PokemonServiceGrpcUrl"]);
        var client = new GrcpPokemon.GrcpPokemonClient(channel);

        var request = new PokemonsRequest
        {
            Date = date,
        };

        try
        {
            var response = client.GetPokemons(request);

            var pokemons = new List<Pokemon>();

            foreach (var pokemon in response.Pokemons)
            {
                pokemons.Add(new Pokemon
                {
                    // Id property is managed by the base class and should not be manually assigned
                    ID = pokemon.Id,
                    Name = pokemon.Name,
                    Price = pokemon.Price,
                    Type = pokemon.Type,
                    Holographic = pokemon.Holographic,
                    Seller = pokemon.Seller,
                    CreatedAt = DateTime.Parse(pokemon.CreatedAt),
                    HealthPower = pokemon.Healthpower,
                    Rarity = pokemon.Rarity,
                    ImageUrl = pokemon.Imageurl,
                    Attacks = pokemon.Attacks.Select(attack => new Attack
                    {
                        Name = attack.Name,
                        Damage = attack.Damage
                    }).ToList()
                });
            }

            return pokemons;

        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"Error when trying to receive pokemons from GrpcServer");
            return null;
        }
    }
}
