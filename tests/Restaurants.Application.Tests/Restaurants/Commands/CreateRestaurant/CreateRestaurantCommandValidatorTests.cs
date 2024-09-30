using FluentAssertions;
using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidatorTests
{
	[Fact()]
	public void Validator_ForValidCommand_ShoudNotHaveValidationErrors()
	{
		// arange

		var command = new CreateRestaurantCommand()
		{
			Name = "Test",
			Category = "Italian",
			ContactEmail = "test@test.com",
			PostalCode = 12345 
		};

		var validator = new CreateRestaurantCommandValidator();

		validator.Validate(command);

		// act

		var result = validator.TestValidate(command);

		// assert

		result.ShouldNotHaveAnyValidationErrors();
	}

	public void Validator_ForInvalidCommand_ShoudHaveValidationErrors()
	{
		// arange

		var command = new CreateRestaurantCommand()
		{
			Name = "Te",
			Category = "Ita",
			ContactEmail = "@test.com",
			PostalCode = 12345
		};

		var validator = new CreateRestaurantCommandValidator();

		validator.Validate(command);

		// act

		var result = validator.TestValidate(command);

		// assert

		result.ShouldHaveAnyValidationError();
	}


	[Theory()]
	[InlineData("Italian")]
	[InlineData("Mexican")]
	[InlineData("Chinese")]
	[InlineData("Fast Food")]
	[InlineData("Indian")]
	public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
	{
		// arrange 
		var validator = new CreateRestaurantCommandValidator();
		var command = new CreateRestaurantCommand { Category = category };

		// act

		var result = validator.TestValidate(command);

		//assert

		result.ShouldNotHaveValidationErrorFor(c => c.Category);
	}

	[Theory()]
	[InlineData("American")]
	[InlineData("Serbian")]
	public void Validator_ForInvalidCategory_ShouldHaveValidationErrorsForCategoryProperty(string category)
	{
		// arange
		var validator = new CreateRestaurantCommandValidator();
		var command = new CreateRestaurantCommand { Category = category };

		// act
		var result = validator.TestValidate(command);

		// assert 

		result.ShouldHaveValidationErrorFor(c => c.Category)
			.WithErrorMessage("Invalid category. Please choose from the valid categories.Valid Categories: Italian, Mexican, Fast Food, Indian, Chinese");
	}
}