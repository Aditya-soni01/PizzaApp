namespace PizzaOderingAppAPI.Models;

public class Delivery
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int? DeliveryPersonId { get; set; }
    public ApplicationUser? DeliveryPerson { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime? PickupTime { get; set; }
    public DateTime? DeliveredTime { get; set; }
    public string? TrackingNumber { get; set; }
}
