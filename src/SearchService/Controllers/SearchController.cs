using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Entities;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<List<Pokemon>>> SearchPokemons([FromQuery] SearchParameters searchParameters)
    {
        // Create query
        var query = DB.PagedSearch<Pokemon, Pokemon>();

        // Check if user has sent any specific search terms
        if (!string.IsNullOrEmpty(searchParameters.SearchTerm))
        {
            query.Match(Search.Full, searchParameters.SearchTerm).SortByTextScore();
        }
        else Console.WriteLine("No SearchTerm provided...");

        // Check if OrderBy provided 
        query = searchParameters.OrderBy switch
        {
            "Price" => query.Sort(x => x.Descending(x => x.Price)),
            _ => query.Sort(x => x.Descending(x => x.HealthPower)) // Default sorting by Id if no OrderBy provided

        };

        // Check if FilterBy provided
        query = searchParameters.FilterBy switch
        {
            "holographic" => query.Match(x => x.Holographic == true),
            _ => query
        };

        // Check if Seller, provided
        query = !string.IsNullOrEmpty(searchParameters.Seller) ? query.Match(x => x.Seller == searchParameters.Seller) : query;
        query = !string.IsNullOrEmpty(searchParameters.Rarity) ? query.Match(x => x.Rarity == searchParameters.Rarity) : query;

        query.PageNumber(searchParameters.PageNumber).PageSize(searchParameters.PageSize);

        var result = await query.ExecuteAsync();

        var pagedResult = new
        {
            totalNumberPokemons = result.TotalCount,
            numberOfPages = result.PageCount,
            result = result.Results
        };

        return Ok(pagedResult);
    } 
    
    
}

