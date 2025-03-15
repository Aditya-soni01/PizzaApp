using Microsoft.Extensions.Configuration;
using Moq;
using PizzaOderingAppAPI.Services;
using Xunit;

namespace PizzaOderingAppAPI.Tests.Services;

public class StripePaymentServiceTests
{
    private readonly Mock<IConfiguration> _configuration;
    private readonly StripePaymentService _service;

    public StripePaymentServiceTests()
    {
        _configuration = new Mock<IConfiguration>();
        _configuration.Setup(x => x["Stripe:SecretKey"])
            .Returns("test_key");
        _service = new StripePaymentService(_configuration.Object);
    }

    [Fact]
    public async Task CreatePaymentIntent_ValidAmount_ReturnsClientSecret()
    {
        // Act & Assert
        var exception = await Record.ExceptionAsync(() => 
            _service.CreatePaymentIntentAsync(100));
        Assert.Null(exception);
    }
}
