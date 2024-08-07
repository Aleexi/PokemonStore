namespace PokemonService;

public class Attack
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Damage { get; set; }
    

    // Navigation Properties for 1-to-many relationship to Pokemon
    public Pokemon Pokemon { get; set; }
    public Guid PokemonId { get; set; }
}
