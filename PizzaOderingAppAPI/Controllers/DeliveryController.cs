using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PizzaOderingAppAPI.Models;
using PizzaOderingAppAPI.Services;
using PizzaOderingAppAPI.Interfaces;

namespace PizzaOderingAppAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeliveryController : ControllerBase
{
    private readonly IRepository<Delivery> _deliveryRepository;
    private readonly IHubContext<OrderHub> _orderHub;

    public DeliveryController(IRepository<Delivery> deliveryRepository, IHubContext<OrderHub> orderHub)
    {
        _deliveryRepository = deliveryRepository;
        _orderHub = orderHub;
    }

    [HttpPost("assign")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignDelivery(int orderId, int deliveryPersonId)
    {
        var delivery = new Delivery
        {
            OrderId = orderId,
            DeliveryPersonId = deliveryPersonId,
            Status = "Assigned",
            TrackingNumber = Guid.NewGuid().ToString("N")
        };

        await _deliveryRepository.AddAsync(delivery);
        await _orderHub.Clients.All.SendAsync("DeliveryAssigned", orderId, deliveryPersonId);
        return Ok(delivery);
    }

    [HttpPut("{id}/location")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationUpdateDto location)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(id);
        if (delivery == null) return NotFound();

        await _orderHub.Clients.All.SendAsync("LocationUpdated", delivery.OrderId, location);
        return Ok();
    }
}

public class LocationUpdateDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Address { get; set; }
}
