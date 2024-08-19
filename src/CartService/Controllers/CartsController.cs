using AutoMapper;
using CartService.DTOs.ReturnDtos;
using CartService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace CartService.Controllers;

[ApiController]
[Route("api/carts")]
public class CartsController : ControllerBase
{
    private readonly IMapper _mapper;

    public CartsController(IMapper mapper)
    {
        _mapper = mapper;
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

        // Need to authenticate user with Buyer parameter
        // Add Jwt to be able to use identity correctly

        var cart = await DB.Find<Cart>().Match(x => x.Buyer == Buyer).ExecuteFirstAsync();

        return Ok(_mapper.Map<CartDto>(cart));
    }
}