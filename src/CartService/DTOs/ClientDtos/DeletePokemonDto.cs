using System.ComponentModel.DataAnnotations;

namespace CartService.DTOs.ClientDtos;

public class DeletePokemonDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Buyer { get; set; }
}

