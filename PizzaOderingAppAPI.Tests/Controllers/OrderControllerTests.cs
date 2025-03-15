using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using PizzaOderingAppAPI.Controllers;
using PizzaOderingAppAPI.Models;
using PizzaOderingAppAPI.Services;
using Xunit;

namespace PizzaOderingAppAPI.Tests.Controllers;

public class OrderControllerTests
{
    private readonly Mock<IRepository<Order>> _orderRepository;
    private readonly Mock<IHubContext<OrderHub>> _orderHub;
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _orderRepository = new Mock<IRepository<Order>>();
        _orderHub = new Mock<IHubContext<OrderHub>>();
        _controller = new OrderController(_orderRepository.Object, _orderHub.Object);
    }

    [Fact]
    public async Task CreateOrder_ValidOrder_ReturnsCreatedResponse()
    {
        // Arrange
        var order = new Order { Id = 1, UserId = 1, TotalAmount = 50 };
        _orderRepository.Setup(repo => repo.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync(order);

        // Act
        var result = await _controller.CreateOrder(order);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<Order>(createdResult.Value);
        Assert.Equal(order.Id, returnValue.Id);
    }

    [Fact]
    public async Task UpdateOrderStatus_ValidStatus_ReturnsOkResult()
    {
        // Arrange
        var order = new Order { Id = 1, Status = "Pending" };
        _orderRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(order);

        // Act
        var result = await _controller.UpdateOrderStatus(1, "Confirmed");

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Confirmed", order.Status);
    }
}
