using Contracts;
using MassTransit;
using Newtonsoft.Json;

namespace PokemonService.Consumers;

public class PokemonCreatedConsumer : IConsumer<PokemonCreated>
{
    public async Task Consume(ConsumeContext<PokemonCreated> context)
    {
        string jsonString = JsonConvert.SerializeObject(context.Message, Formatting.Indented);
        Console.WriteLine($"PokemonService received ==> {jsonString}");
        
        // Console.WriteLine($"PokemonService received ==> {context.Message.Id}");
        await Task.Delay(1);
    }
}
