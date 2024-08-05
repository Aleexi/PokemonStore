namespace PokemonService.Entities;

public class Pokemon
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public TypeEnum Type { get; set; }
    public int HealthPower { get; set; }
    public RarityEnum Rarity { get; set; }
    public bool Holographic { get; set; } 
    public List<Attack> Attacks { get; set; }
    public DateTime createdAt { get; set; } = DateTime.UtcNow;
}