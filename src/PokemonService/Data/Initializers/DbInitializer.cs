using Microsoft.EntityFrameworkCore;

namespace PokemonService;

public class DbInitializer
{
    public static void InitalizeDatabase(WebApplication app)
    {
        // Create scope 
        using var scope = app.Services.CreateScope();

        // Get required service -> pokemonDbContext
        var pokemonDbContext = scope.ServiceProvider.GetRequiredService<PokemonDbContext>();

        // Use seedData
        seedData(pokemonDbContext);
    }

    public static void seedData (PokemonDbContext pokemonDbContext)
    {
        // Apply latest migration 
        pokemonDbContext.Database.Migrate();

        Console.WriteLine("Checking if Pokemons exists in database...");
        // Check if we have data already, if so return
        if (pokemonDbContext.Pokemons.Any()) return;

        Console.WriteLine("Database empty -> Seeding database...");
        // If we don't, seed it with example data
        var pokemons = new List<Pokemon>{
            new Pokemon {
                Id = Guid.Parse("bbab4d5a-8565-48b1-9450-5ac2a5c4a654"),
                Name = "Pikachu",
                Price = 200,
                Type = TypeEnum.Electric,
                HealthPower = 100,
                Rarity = RarityEnum.Common,
                Holographic = false,
                Seller = "Alexander",
                Attacks = new List<Attack>
                {
                    new Attack
                    {
                        Name = "Eletric Shock",
                        Damage = 120
                    },
                    new Attack
                    {
                        Name = "Eletric Blast",
                        Damage = 100
                    }
                },
                ImageUrl = "https://cdn.pixabay.com/photo/2023/04/11/18/35/pikachu-7917962_1280.jpg"
            },
            new Pokemon {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Name = "Mew",
                Price = 800,
                Type = TypeEnum.All,
                HealthPower = 150,
                Rarity = RarityEnum.Legendary,
                Holographic = true,
                Seller = "Alexander",
                Attacks = new List<Attack>
                {
                    new Attack
                    {
                        Name = "Divine Deperture",
                        Damage = 10000
                    },
                    new Attack
                    {
                        Name = "Normal Blast",
                        Damage = 100
                    }
                },
                ImageUrl = "https://cdn.pixabay.com/photo/2024/02/20/08/18/ai-generated-8584912_1280.jpg"
            },
            new Pokemon {
                Id = Guid.Parse("c8c3ec17-01bf-49db-82aa-1ef80b833a9f"),
                Name = "Evee",
                Price = 300,
                Type = TypeEnum.All,
                HealthPower = 100,
                Rarity = RarityEnum.Common,
                Holographic = false,
                Seller = "Alexander",
                Attacks = new List<Attack>
                {
                    new Attack
                    {
                        Name = "Cocon Blast",
                        Damage = 1
                    }
                },
                ImageUrl = "https://cdn.pixabay.com/photo/2016/09/14/17/32/pokemon-1669884_1280.jpg"
            }
        };

        // Start tracking entities 
        pokemonDbContext.Pokemons.AddRange(pokemons);

        // Save changes of entities to database
        var successfull = pokemonDbContext.SaveChanges() > 0;

        if (!successfull)
        {
            Console.WriteLine("Couldn't seed database...");
            return;
        }

        Console.WriteLine($"Successfully seeded database, there exists: {pokemonDbContext.Pokemons.Count()} pokemons");
    }
}
