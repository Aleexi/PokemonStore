using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Consumers
{
    public class PokemonDeletedConsumer : IConsumer<PokemonDeleted>
    {
        private readonly ILogger _logger;

        public PokemonDeletedConsumer(ILogger<PokemonDeletedConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PokemonDeleted> context)
        {
            _logger.LogInformation($"Received PokemonCreated Event with -> id: {context.Message.Id}");

            var pokemon = DB.Find<Pokemon>().OneAsync(context.Message.Id);

            if (pokemon == null)
            {
                _logger.LogInformation($"Pokemon with id: {context.Message.Id} doesn't exists in SearchService database, no actions needed");
                return;
            }

            _logger.LogInformation($"Deleting pokemon with id {context.Message.Id}...");
            await DB.DeleteAsync<Pokemon>(pokemon);
        }
    }
}
