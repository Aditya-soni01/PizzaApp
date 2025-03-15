using FluentValidation;
using PizzaOderingAppAPI.Models;

namespace PizzaOderingAppAPI.Validators;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TotalAmount).GreaterThan(0);
        RuleFor(x => x.OrderItems).NotEmpty().WithMessage("Order must contain at least one item");
        RuleFor(x => x.DeliveryAddress).NotEmpty().When(x => x.Status != "Cancelled");
        RuleForEach(x => x.OrderItems).SetValidator(new OrderItemValidator());
    }
}

public class OrderItemValidator : AbstractValidator<OrderItem>
{
    public OrderItemValidator()
    {
        RuleFor(x => x.MenuItemId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThan(0);
    }
}
