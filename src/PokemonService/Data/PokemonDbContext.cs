using Microsoft.EntityFrameworkCore;

namespace PokemonService;

public class PokemonDbContext : DbContext
{
    public PokemonDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Pokemon> Pokemons {set; get;}
    public DbSet<Attack> Attacks {set; get;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pokemon>()
            .HasMany(p => p.Attacks)
            .WithOne(x => x.Pokemon)
            .HasForeignKey(x => x.PokemonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
