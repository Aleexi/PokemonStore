using MongoDB.Entities;

namespace CartService.Entities;
public class Pokemon : Entity
{
    public string Name { get; set; }
    public string Seller { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
}
