using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Bson;
using MongoDB.Entities;
using SearchService.Entities;
using System.Text.Json;

namespace SearchService.Consumers
{
    public class PokemonCreatedConsumer : IConsumer<PokemonCreated>
    {
        private readonly ILogger<PokemonCreatedConsumer> _logger;
        private readonly IMapper _mapper;

        public PokemonCreatedConsumer(ILogger<PokemonCreatedConsumer> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<PokemonCreated> context)
        {
            _logger.LogInformation($"Received PokemonCreated Event with -> id: {context.Message.Id}");

            var pokemon = _mapper.Map<Pokemon>(context.Message);

            _logger.LogInformation(JsonSerializer.Serialize(pokemon, new JsonSerializerOptions { WriteIndented = true }));


            // Check that pokemon doesn't already exists, shouldn't be
            var existingPokemon = await DB.Find<Pokemon>().OneAsync(context.Message.Id);

            if (existingPokemon != null) _logger.LogInformation("Pokemon already exists...");

            await DB.SaveAsync(pokemon);

        }
    }
}
