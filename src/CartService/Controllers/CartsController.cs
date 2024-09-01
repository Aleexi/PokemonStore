using AutoMapper;
using CartService.DTOs.ClientDtos;
using CartService.DTOs.ReturnDtos;
using CartService.Entities;
using CartService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Entities;

namespace CartService.Controllers;

[ApiController]
[Route("api/carts")]
public class CartsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IServiceProvider _service;

    public CartsController(IMapper mapper, IServiceProvider service)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Cart>>> GetCarts()
    {
        var carts = await DB.Find<Cart>().ExecuteAsync();
        
        return Ok(carts.Select(_mapper.Map<CartDto>).ToList());
    }

    [HttpGet("{buyer}")]
    [Authorize]
    public async Task<ActionResult<Cart>> GetCartByName(string Buyer)
    {
        if (string.IsNullOrWhiteSpace(Buyer)) return BadRequest();

        if (Buyer != User.Identity.Name) return Forbid();

        var cart = await DB.Find<Cart>().Match(x => x.Buyer == Buyer).ExecuteFirstAsync();

        return Ok(_mapper.Map<CartDto>(cart));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Cart>> AddPokemonToCart([FromBody] AddPokemonDto addpokemonDto)
    {
        if (addpokemonDto.Buyer != User.Identity.Name) return BadRequest("Buyer and Identity is not the same...");

        // Make a Grpc communicating with PokemonService to retrieve pokemon of interest
        using var scope = _service.CreateScope();
        var grpc = scope.ServiceProvider.GetRequiredService<GrpcSender>();

        var pokemon = await grpc.GetPokemonById(addpokemonDto.Id.ToString());

        // Check if pokemon was found 
        if (pokemon == null) return NotFound();

        pokemon.Quantity = addpokemonDto.Quantity;

        // Check if the user already has a cart
        var cart = await DB.Find<Cart>().Match(x => x.Buyer == addpokemonDto.Buyer).ExecuteFirstAsync();

        // check if user has cart
        if (cart == null) 
        {
            // If not cart exists, create it, add pokemon and save it 
            Console.WriteLine($"Creating Cart for {User.Identity.Name}...");

            cart = new Cart();
            cart.Buyer = User.Identity.Name;
            cart.pokemons = new List<Pokemon>{ pokemon };
            cart.TotalPrice = pokemon.Quantity * pokemon.Price;

            await DB.SaveAsync(cart);
            return CreatedAtAction(nameof(GetCartByName), new { cart.ID }, _mapper.Map<CartDto>(cart));
        }
        else 
        {
            Console.WriteLine($"Cart exists for {User.Identity.Name}...");
            
            // Check if pokemon already exists in cart, then update quantity and totalprice
            if (cart.pokemons.Any(p => p.ID.Equals(addpokemonDto.Id.ToString())))
            {
                Console.WriteLine("Pokemon already exists in cart, updating...");
                var pokemonToUpdate = cart.pokemons.FirstOrDefault(x => x.ID == addpokemonDto.Id.ToString());


                // Update first the Total Price by removing the price affected by the pokemon
                cart.TotalPrice -= pokemonToUpdate.Quantity * pokemonToUpdate.Price;

                pokemonToUpdate.Quantity = pokemonToUpdate.Quantity + addpokemonDto.Quantity;
                cart.TotalPrice += pokemonToUpdate.Quantity * pokemonToUpdate.Price; 

                await DB.SaveAsync(cart);
                return Ok(_mapper.Map<CartDto>(cart));
            }

            else
            {
                Console.WriteLine("Pokemon doesn't exists in cart, adding...");
                cart.pokemons.Add(pokemon);
                cart.TotalPrice += pokemon.Quantity * pokemon.Price;

                await DB.SaveAsync(cart);
                return Ok(_mapper.Map<CartDto>(cart));
            }
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<Cart>> UpdatePokemonInCart([FromBody] UpdatePokemonDto updatePokemonDto)
    {
        if (User.Identity.Name != updatePokemonDto.Buyer) return BadRequest("Buyer and Identity is not the same...");

        // Check if user has cart
        var cart = await DB.Find<Cart>().Match(x => x.Buyer == updatePokemonDto.Buyer).ExecuteFirstAsync();

        if (cart == null) return BadRequest("User doesn't have a cart in the cartService...");

        // Check if pokemon is in cart
        var pokemon = cart.pokemons.FirstOrDefault(x => x.ID == updatePokemonDto.Id.ToString());

        // If pokemon doesn't exists
        if (pokemon == null) return BadRequest("Pokemon didn't exist in cart, returning...");

        if (updatePokemonDto.UpdatedQuantity < pokemon.Quantity)
        {
            cart.TotalPrice -= (pokemon.Quantity - updatePokemonDto.UpdatedQuantity) * pokemon.Price;
        }
        else
        {
            cart.TotalPrice += (updatePokemonDto.UpdatedQuantity - pokemon.Quantity) * pokemon.Price;
        }

        pokemon.Quantity = updatePokemonDto.UpdatedQuantity;

        await DB.SaveAsync(cart);

        return Ok(_mapper.Map<CartDto>(cart));
    }

    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> DeletePokemonInCart([FromBody] DeletePokemonDto deletePokemonDto)
    {
        if (User.Identity.Name != deletePokemonDto.Buyer) return BadRequest("User identity and buyer name not the same...");

        // Check if user has cart
        Cart cart = await DB.Find<Cart>().Match(x => x.Buyer == deletePokemonDto.Buyer).ExecuteFirstAsync();

        if (cart == null) return BadRequest($"Cart for user {deletePokemonDto.Buyer} doesn't exists");

        // Check if pokemon exists within cart

        Pokemon pokemon = cart.pokemons.FirstOrDefault(x => x.ID == deletePokemonDto.Id.ToString());

        if (pokemon == null) return BadRequest("Pokemon didn't exists in cart");

        cart.TotalPrice -= pokemon.Quantity * pokemon.Price;

        cart.pokemons.Remove(pokemon);

        await cart.SaveAsync();

        return Ok();
    }
}