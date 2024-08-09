using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;
using SearchService.Services;

namespace SearchService.Consumers
{
    public class PokemonUpdatedConsumer : IConsumer<PokemonUpdated>
    {
        private readonly ILogger<PokemonUpdatedConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;

        public PokemonUpdatedConsumer(ILogger<PokemonUpdatedConsumer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public async Task Consume(ConsumeContext<PokemonUpdated> context)
        {
            _logger.LogInformation($"Received PokemonUpdated Event -> id: {context.Message.Id}");

            // If Updated Pokemon doesn't exists, need to retrieve from Pokemon Service
            var pokemon = await DB.Find<Pokemon>().OneAsync(context.Message.Id);

            if (pokemon == null)
            {
                _logger.LogInformation("Update pokemon wasn't found in search database, Calling RPC to retrieve");

                using var scope = _serviceProvider.CreateScope();

                var client = scope.ServiceProvider.GetRequiredService<GrpcSender>();

                pokemon = client.GetPokemon(context.Message.Id.ToString());

                if (pokemon == null)
                {
                    _logger.LogInformation("Pokemon doesn't exists in PokemonService either");
                    return;
                }
                _logger.LogInformation("Pokemon retrieved from Pokemon Service, saving...");
                await DB.SaveAsync(pokemon);
            }
            else
            {
                await DB.Update<Pokemon>()
                    .MatchID(context.Message.Id)
                    .Modify(a => a.Price, context.Message.Price)
                    .ExecuteAsync();

                _logger.LogInformation("Update complete...");
            }
        }
    }
}
