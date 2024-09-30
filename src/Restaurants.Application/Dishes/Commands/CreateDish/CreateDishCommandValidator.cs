using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(dish => dish.Name)
            .Length(3, 70)
            .WithMessage("Name of the dish should be between 3 and 70 charachters.");

        RuleFor(dish => dish.Description)
            .Length(10, 150)
            .WithMessage("Description of the dish should be between 10 and 150 characters.");

        RuleFor(dish => dish.Price)
            .NotEmpty()
            .WithMessage("Price field is required.");


        RuleFor(dish => dish.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");


        RuleFor(dish => dish.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("KiloCalories cannot be lower than 0");
    }
}
