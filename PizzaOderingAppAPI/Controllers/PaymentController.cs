using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOderingAppAPI.Services;
using PizzaOderingAppAPI.Models;
using PizzaOderingAppAPI.Interfaces;

namespace PizzaOderingAppAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IRepository<Payment> _paymentRepository;

    public PaymentController(IPaymentService paymentService, IRepository<Payment> paymentRepository)
    {
        _paymentService = paymentService;
        _paymentRepository = paymentRepository;
    }

    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentDto request)
    {
        var clientSecret = await _paymentService.CreatePaymentIntentAsync(request.Amount);
        return Ok(new { ClientSecret = clientSecret });
    }

    [HttpPost("confirm/{paymentIntentId}")]
    public async Task<IActionResult> ConfirmPayment(string paymentIntentId)
    {
        var success = await _paymentService.ConfirmPaymentAsync(paymentIntentId);
        return Ok(new { Success = success });
    }
}

public class PaymentIntentDto
{
    public decimal Amount { get; set; }
}
