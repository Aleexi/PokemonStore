using CartService.Entities;
using MongoDB.Driver;
using MongoDB.Entities;

namespace CartService.Data.Initializers;

public class DbInitializer 
{
    public static async Task InitializeDatabase(WebApplication app)
    {
        /* 
        var count = await DB.CountAsync<Cart>();

        if (count > 0)
        {
            Console.WriteLine("There already exists carts in the database, returning...");
        }
        */

        // Connect to carts database
        await DB.InitAsync("carts-db", 
            MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        // Define queryable parameters
        await DB.Index<Cart>()
            .Key(x => x.ID, KeyType.Text)
            .Key(x => x.TotalPrice, KeyType.Text)
            .Key(x => x.Buyer, KeyType.Text)
            .Key(x => x.pokemons, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Cart>();

        Console.WriteLine(count > 0 ? $"There exists {count} carts in database..." :
            "There exists 0 carts in database, attempting to seed...");

        if (count == 0) {
            {
                // Seed data
                var carts = new List<Cart>
                {
                    new Cart {
                        Buyer = "alex",
                        TotalPrice = 2000,
                        pokemons = new List<Pokemon>
                        {
                            new Pokemon
                            {
                                ID = "c8c3ec17-01bf-49db-82aa-1ef80b833a9f",
                                Name = "Evee",
                                Seller = "bob",
                                Price = 300,
                                Quantity = 4,
                                ImageUrl = "https://cdn.pixabay.com/photo/2016/09/14/17/32/pokemon-1669884_1280.jpg"
                            },
                            new Pokemon
                            {
                                ID = "afbee524-5972-4075-8800-7d1f9d7b0a0c",
                                Name = "Mew",
                                Seller = "bob",
                                Price = 800,
                                Quantity = 1,
                                ImageUrl = "https://cdn.pixabay.com/photo/2016/09/14/17/32/pokemon-1669884_1280.jpg"
                            }
                        }
                    },
                    new Cart {
                        Buyer = "alice",
                        TotalPrice = 900,
                        pokemons = new List<Pokemon>
                        {
                            new Pokemon
                            {
                                ID = "c8c3ec17-01bf-49db-82aa-1ef80b833a9f",
                                Name = "Evee",
                                Seller = "bob",
                                Price = 300,
                                Quantity = 3,
                                ImageUrl = "https://cdn.pixabay.com/photo/2016/09/14/17/32/pokemon-1669884_1280.jpg"
                            }
                        }
                    },
                    new Cart {
                        Buyer = "bob",
                        TotalPrice = 600,
                        pokemons = new List<Pokemon>
                        {
                            new Pokemon
                            {
                                ID = "c8c3ec17-01bf-49db-82aa-1ef80b833a9f",
                                Name = "Evee",
                                Seller = "bob",
                                Price = 300,
                                Quantity = 2,
                                ImageUrl = "https://cdn.pixabay.com/photo/2016/09/14/17/32/pokemon-1669884_1280.jpg"
                            }
                        }
                    }
                };

                await carts.SaveAsync();
                Console.WriteLine("Filled database with Carts");
                return;
            }
        }
        else
        {
            Console.WriteLine("There already exists carts in database...");
            return;
        }
    }
}

