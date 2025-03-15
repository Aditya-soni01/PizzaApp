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
[Produces("application/json")]
public class OrderController : ControllerBase
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IHubContext<OrderHub> _orderHub;

    public OrderController(IRepository<Order> orderRepository, IHubContext<OrderHub> orderHub)
    {
        _orderRepository = orderRepository;
        _orderHub = orderHub;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderRepository.GetAllAsync();
        return Ok(orders);
    }

    /// <summary>
    /// Creates a new order
    /// </summary>
    /// <param name="order">Order details</param>
    /// <returns>The created order</returns>
    /// <response code="201">Returns the newly created order</response>
    /// <response code="400">If the order is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder(Order order)
    {
        var newOrder = await _orderRepository.AddAsync(order);
        await _orderHub.Clients.All.SendAsync("NewOrder", newOrder);
        return CreatedAtAction(nameof(GetOrders), new { id = newOrder.Id }, newOrder);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return NotFound();

        order.Status = status;
        await _orderRepository.UpdateAsync(order);
        await _orderHub.Clients.All.SendAsync("OrderStatusUpdated", id, status);
        
        return Ok(new { message = "Order status updated" });
    }
}
