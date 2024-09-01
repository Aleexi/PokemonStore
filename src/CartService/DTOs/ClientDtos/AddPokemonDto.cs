using System.ComponentModel.DataAnnotations;

namespace CartService.DTOs.ClientDtos;

public class AddPokemonDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public string Buyer { get; set; }
}

