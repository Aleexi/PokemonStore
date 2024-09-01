using MongoDB.Entities;

namespace CartService.Entities;

public class Cart : Entity
{

    public int TotalPrice { get; set; } = 0;

    public string? Buyer { get; set; }

    public List<Pokemon>? pokemons { get; set; }
}

