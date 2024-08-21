using System.ComponentModel.DataAnnotations;

namespace CartService.DTOs.ClientDtos;

public class UpdatePokemonDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public int UpdatedQuantity { get; set; }
    [Required]
    public string Buyer { get; set; }
}

