namespace PokemonService;

public class Pokemon
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Seller { get; set; }
    public int Price { get; set; }
    public TypeEnum Type { get; set; }
    public int HealthPower { get; set; }
    public RarityEnum Rarity { get; set; }
    public bool Holographic { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string ImageUrl { get; set; }

    
    public List<Attack> Attacks { get; set; }
}