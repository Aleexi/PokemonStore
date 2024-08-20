using AutoMapper;
using CartService.Entities;
using Grpc.Core;
using Grpc.Net.Client;
using PokemonService;

namespace CartService.Services;

public class GrpcSender
{
    private readonly ILogger _logger;
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

    public async Task<Pokemon> GetPokemonById(string id)
    {
        var channel = GrpcChannel.ForAddress(_config["PokemonServiceUrlGrpc"]);
        var client = new GrcpPokemon.GrcpPokemonClient(channel);

        var request = new PokemonRequest { Id = id };

        await WaitForChannelReady(channel, new CancellationTokenSource().Token);

        try
        {
            var response = client.GetPokemonById(request);

            var pokemon = new Pokemon
            {
                ID = response.Pokemon.Id,
                Name = response.Pokemon.Name,
                Seller = response.Pokemon.Seller,
                Price = response.Pokemon.Price,
                ImageUrl = response.Pokemon.Imageurl,
            };

            return pokemon;
        }
        catch (Exception e)
        {

            _logger.LogError($"{e.Message}");
            return null;
        }
    }
}

