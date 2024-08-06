﻿using System.ComponentModel.DataAnnotations;

namespace PokemonService;

public class CreatePokemonDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public int Price { get; set; }
    [Required]
    public string Seller { get; set; }
    public string Type { get; set; }
    [Required]
    public int HealthPower { get; set; }
    [Required]
    public string Rarity { get; set; }
    [Required]
    public bool Holographic { get; set; } 
    [Required]
    public string ImageUrl { get; set; }
    [Required]
    public List<AttackDTO> Attacks { get; set; }
}
