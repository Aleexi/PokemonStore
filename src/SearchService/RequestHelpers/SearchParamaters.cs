namespace SearchService.RequestHelpers;

public class SearchParameters
{
    public string SearchTerm { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 3;

    public string Seller { get; set; }

    public string Rarity { get; set; }

    public string OrderBy { get; set; }

    public string FilterBy { get; set; }
}


