
namespace CartService.DTOs.ReturnDtos;

public class CartDto
{
        public string Id { get; set; }

        public int TotalPrice { get; set; } = 0;

        public string Buyer { get; set; }

        public List<PokemonDto> pokemons { get; set; }
}

