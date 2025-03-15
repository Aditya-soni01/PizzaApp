namespace PizzaOderingAppAPI.Models;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public string PaymentMethod { get; set; } = null!; // Credit Card, PayPal, etc.
    public string Status { get; set; } = "Pending";
    public string? TransactionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
}
