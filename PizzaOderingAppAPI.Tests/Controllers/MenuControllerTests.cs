using Microsoft.AspNetCore.Mvc;
using Moq;
using PizzaOderingAppAPI.Controllers;
using PizzaOderingAppAPI.Interfaces;
using PizzaOderingAppAPI.Models;
using Xunit;

namespace PizzaOderingAppAPI.Tests.Controllers;

public class MenuControllerTests
{
    private readonly Mock<IRepository<MenuItem>> _menuItemRepository;
    private readonly Mock<IRepository<FoodCategory>> _categoryRepository;
    private readonly MenuController _controller;

    public MenuControllerTests()
    {
        _menuItemRepository = new Mock<IRepository<MenuItem>>();
        _categoryRepository = new Mock<IRepository<FoodCategory>>();
        _controller = new MenuController(_menuItemRepository.Object, _categoryRepository.Object);
    }

    [Fact]
    public async Task GetAllItems_ReturnsOkResult()
    {
        // Arrange
        var menuItems = new List<MenuItem> 
        { 
            new() { Id = 1, Name = "Test Pizza" } 
        };
        _menuItemRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(menuItems);

        // Act
        var result = await _controller.GetAllItems();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<MenuItem>>(okResult.Value);
        Assert.Single(returnValue);
    }
}
