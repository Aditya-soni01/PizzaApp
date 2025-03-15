namespace PizzaOderingAppAPI.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending"; // Pending, Confirmed, Preparing, Out for Delivery, Delivered, Cancelled
    public decimal TotalAmount { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? Notes { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public Payment? Payment { get; set; }
    public Delivery? Delivery { get; set; }
}
