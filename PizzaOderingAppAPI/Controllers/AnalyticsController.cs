using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaOderingAppAPI.Data;
using System;

namespace PizzaOderingAppAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AnalyticsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AnalyticsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);

        var stats = new
        {
            TotalOrders = await _context.Orders.CountAsync(),
            MonthlyOrders = await _context.Orders
                .Where(o => o.OrderDate >= startOfMonth)
                .CountAsync(),
            TotalRevenue = await _context.Orders
                .Where(o => o.Status == "Delivered")
                .SumAsync(o => o.TotalAmount),
            ActiveUsers = await _context.Users
                .Where(u => u.IsActive)
                .CountAsync(),
            PopularItems = await _context.OrderItems
                .GroupBy(oi => oi.MenuItemId)
                .Select(g => new
                {
                    MenuItemId = g.Key,
                    ItemName = g.First().MenuItem.Name,
                    OrderCount = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(x => x.OrderCount)
                .Take(5)
                .ToListAsync()
        };

        return Ok(stats);
    }
}
