using PokemonService.Entities;

namespace PokemonService;

public class PokemonDTO
{
    public string Name { get; set; }
    public int Price { get; set; }
    public TypeEnum Type { get; set; }
    public int HealthPower { get; set; }
    public RarityEnum Rarity { get; set; }
    public bool Holographic { get; set; } 
    public List<Attack> Attacks { get; set; }
}
