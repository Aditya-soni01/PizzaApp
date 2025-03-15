using Microsoft.AspNetCore.SignalR;

namespace PizzaOderingAppAPI.Services;

public class OrderHub : Hub
{
    public async Task UpdateOrderStatus(int orderId, string status)
    {
        await Clients.All.SendAsync("OrderStatusUpdated", orderId, status);
    }

    public async Task TrackDelivery(int orderId, string location)
    {
        await Clients.All.SendAsync("DeliveryLocationUpdated", orderId, location);
    }
}
