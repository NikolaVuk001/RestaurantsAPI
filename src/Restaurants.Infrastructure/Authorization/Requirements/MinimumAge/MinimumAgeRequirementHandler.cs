using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimumAge;

public class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger,
	IUserContext userContext)
		: AuthorizationHandler<MinimumAgeRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
		MinimumAgeRequirement requirement)
	{
		var currentUser = userContext.GetCurrentUser()
			?? throw new UnauthorizedException();




		logger.LogInformation("User: {Email}, date of birth: {dateOfBirth} - Handiling MinimumAgeRequirement",
			currentUser.Email,
			currentUser.DateOfBirth);


		if (currentUser.DateOfBirth == null)
		{
			logger.LogWarning("User date of birth is null");
			context.Fail();
			return Task.CompletedTask;
		}

		if (currentUser.DateOfBirth.Value.AddYears(requirement.MinumomAge) <= DateOnly.FromDateTime(DateTime.Today))
		{
			logger.LogInformation("Authorization succeded");
			context.Succeed(requirement);
		}
		else
		{
			context.Fail();
		}

		return Task.CompletedTask;
	}
}
