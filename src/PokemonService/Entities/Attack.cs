using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonService;

public class Attack
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Damage { get; set; }
    

    // Navigation Properties for 1-to-1 relationship to Pokemon
    public Pokemon Pokemon { get; set; }
    public Guid PokemonId { get; set; }
}
