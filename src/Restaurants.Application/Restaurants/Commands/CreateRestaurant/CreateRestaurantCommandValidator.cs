using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{

    private readonly List<string> validCategories = ["Italian", "Mexican", "Fast Food", "Indian", "Chinese"];
    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100)
            .WithMessage("Name length must be between 3 and 100 characters.");

        RuleFor(dto => dto.Category)
            .Must(category => validCategories.Contains(category))
            .WithMessage("Invalid category. Please choose from the valid categories." +
        "Valid Categories: Italian, Mexican, Fast Food, Indian, Chinese");

        // Custom Implementation down below
        //      .Custom((value,context) =>
        //      {
        //          var isValidCategory = validCategories.Contains(value);
        //          if(!isValidCategory)
        //          {
        //              context.AddFailure("Category", "Invalid category. Please choose from the valid categories." +
        //"\nValid Categories: Italian, Mexican, Fast Food, Indian, Chinese");
        //          }
        //      });

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide valid email adress.");

        RuleFor(dto => dto.ContactNumber)
            .Matches("^(?:[\\d-\\/]*\\d){9,10}$")
            .WithMessage("Contact Number needs two have 9-10 digits and can only contain '-' and '/' characters");

        //RuleFor(dto => dto.ContactNumber)
        //    .Matches("Regex ovde");

    }
}
