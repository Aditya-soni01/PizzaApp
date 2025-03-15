using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOderingAppAPI.Models;
using PizzaOderingAppAPI.Interfaces;

namespace PizzaOderingAppAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IRepository<MenuItem> _menuItemRepository;
    private readonly IRepository<FoodCategory> _categoryRepository;

    public MenuController(IRepository<MenuItem> menuItemRepository, IRepository<FoodCategory> categoryRepository)
    {
        _menuItemRepository = menuItemRepository;
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems()
        => Ok(await _menuItemRepository.GetAllAsync());

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateMenuItem([FromBody] MenuItem menuItem)
    {
        var newItem = await _menuItemRepository.AddAsync(menuItem);
        return CreatedAtAction(nameof(GetAllItems), new { id = newItem.Id }, newItem);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItem menuItem)
    {
        if (id != menuItem.Id) return BadRequest();
        await _menuItemRepository.UpdateAsync(menuItem);
        return NoContent();
    }
}
