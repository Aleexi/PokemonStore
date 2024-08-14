using AutoMapper;
using Grpc.Core;
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

    private async Task WaitForChannelReady(GrpcChannel channel, CancellationToken cancellationToken)
    {
        var maxRetryCount = 5;
        var retryCount = 0;
        var delay = TimeSpan.FromSeconds(1);

        while (channel.State != ConnectivityState.Ready && retryCount < maxRetryCount)
        {
            await channel.ConnectAsync(cancellationToken);
            retryCount++;

            if (channel.State == ConnectivityState.Ready)
            {
                break;
            }
            _logger.LogWarning($"Channel not ready, retrying ({retryCount}/{maxRetryCount})...");
            await Task.Delay(delay);
        }

        if (channel.State != ConnectivityState.Ready)
        {
            throw new Exception("Failed to establish a connection to the Pokémon service.");
        }
    }
   
    public Pokemon GetPokemon(string id)
    {
        _logger.LogInformation("Starting GrpcSender...");

        var channel = GrpcChannel.ForAddress(_config["PokemonServiceGrpcUrl"]);
        var client = new GrcpPokemon.GrcpPokemonClient(channel);

        var request = new PokemonRequest { Id = id };

        try
        {
            var response = client.GetPokemonById(request);

            var pokemon = new Pokemon
            {
                ID = response.Pokemon.Id,
                Name = response.Pokemon.Name,
                Price = response.Pokemon.Price,
                Type = response.Pokemon.Type,
                Holographic = response.Pokemon.Holographic,
                Seller = response.Pokemon.Seller,
                CreatedAt = DateTime.Parse(response.Pokemon.CreatedAt),
                HealthPower = response.Pokemon.Healthpower,
                Rarity = response.Pokemon.Rarity,
                ImageUrl = response.Pokemon.Imageurl,
                Attacks = response.Pokemon.Attacks.Select(attack => new Attack
                {
                    Name = attack.Name,
                    Damage = attack.Damage
                }).ToList()
            };
            return pokemon;
        }
        catch (Exception e)
        {
            throw new Exception("Error retrieving pokemon from Pokemon Service", e);
        }
    }

    public async Task<List<Pokemon>> GetPokemons(string date)
    {
        _logger.LogInformation("Starting GrpcSender...");

        var channel = GrpcChannel.ForAddress(_config["PokemonServiceGrpcUrl"]);
        var client = new GrcpPokemon.GrcpPokemonClient(channel);

        var request = new PokemonsRequest
        {
            Date = date,
        };

        // Wait until the channel is ready
        await WaitForChannelReady(channel, new CancellationTokenSource().Token);

        try
        {
            var response = client.GetPokemons(request);

            var pokemons = new List<Pokemon>();

            foreach (var pokemon in response.Pokemons)
            {
                pokemons.Add(new Pokemon
                {
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
