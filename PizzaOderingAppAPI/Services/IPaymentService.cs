namespace PizzaOderingAppAPI.Services;

public interface IPaymentService
{
    Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "usd");
    Task<bool> ConfirmPaymentAsync(string paymentIntentId);
}
