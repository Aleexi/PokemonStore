using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Entities;
using SearchService.Services;

namespace SearchService.Data.Initializer;

public class DbInitializer
{
    public static async Task InitDatabase(WebApplication app)
    {
        // Initializes database with given database name, need connection string (from appsettings.development.json for development)
        await DB.InitAsync("search-db", 
            MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Pokemon>()
            .Key(x => x.Price, KeyType.Text)
            .Key(x => x.HealthPower, KeyType.Text)
            .Key(x => x.Rarity, KeyType.Text)
            .Key(x => x.Holographic, KeyType.Text)
            .Key(x => x.Type, KeyType.Text)
            .Key(x => x.Seller, KeyType.Text)
            .Key(x => x.Name, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Pokemon>();

        Console.WriteLine(count > 0 ? $"There exists {count} pokemons, exiting..." 
               : "No pokemons exists, retrieving all pokemons from PokemonService...");

        // Retrieve all pokemons from Pokemon Service 
        
        if (count == 0)
        {   
            using var scope = app.Services.CreateScope();

            var sender = scope.ServiceProvider.GetRequiredService<GrpcSender>();

            var pokemons = await sender.GetPokemons(null);

            if (pokemons.Count == 0) return;

            await DB.SaveAsync(pokemons);
            Console.WriteLine("Filled the MongoDb with pokemons");
        } 
    }
}
