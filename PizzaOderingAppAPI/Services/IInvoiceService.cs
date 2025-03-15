namespace PizzaOderingAppAPI.Services;

public interface IInvoiceService
{
    Task<byte[]> GenerateInvoiceAsync(int orderId);
}
