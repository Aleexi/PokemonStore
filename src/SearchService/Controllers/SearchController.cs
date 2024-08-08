using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SearchService;

[Route("api/search")]
[ApiController]
public class SearchController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok("First Controller!");
    } 
}

