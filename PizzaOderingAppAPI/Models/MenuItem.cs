namespace PizzaOderingAppAPI.Models;

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsVegetarian { get; set; }
    public bool IsSpicy { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int FoodCategoryId { get; set; }
    public FoodCategory FoodCategory { get; set; } = null!;
}
