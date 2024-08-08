using System;
using System.Collections.Generic;
using Contracts.PublicClasses;

namespace Contracts;

public class PokemonCreated
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Seller { get; set; }
    public int Price { get; set; }
    public string? Type { get; set; }
    public int HealthPower { get; set; }
    public string? Rarity { get; set; }
    public bool Holographic { get; set; } 
    public DateTime CreatedAt { get; set; }
    public string? ImageUrl { get; set; }

    public List<AttackContract>? Attacks { get; set; }
}
