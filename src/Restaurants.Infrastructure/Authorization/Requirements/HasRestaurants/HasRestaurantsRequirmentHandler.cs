using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements.HasRestaurants;

public class HasRestaurantsRequirmentHandler(ILogger<HasRestaurantsRequirmentHandler> logger,
	IUserContext userContext,
	IRestaurantRepository restaurantRepository) : AuthorizationHandler<HasRestaurantsRequirment>
{
	protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, HasRestaurantsRequirment requirement)
	{
		
		var currentUser = userContext.GetCurrentUser()
			?? throw new UnauthorizedException();

		logger.LogInformation("User: {Email}, date of birth: {dateOfBirth} - Handiling MinimumAgeRequirement",
			currentUser.Email,
			currentUser.DateOfBirth);

		if(!currentUser.Roles.Contains(UserRoles.Owner))
		{
			logger.LogWarning("User role is not {Role}", UserRoles.Owner);
			context.Fail();
			return Task.CompletedTask;
		}

		var ownerRestaurants = await restaurantRepository.GetOwnerRestaurantsAsync(currentUser.Id);
		int ownerRestaurantsCount = ownerRestaurants.Count();

		if (ownerRestaurantsCount < requirement.Count)
		{
			logger.LogWarning("Owner has: {restaurantCont} but the minimum requirment is: {requirment}",
				ownerRestaurantsCount,
				requirement.Count);
			context.Fail();
			
		}
		else
		{
			logger.LogInformation("Authorization for MinimumRestaurants succeded");
			context.Succeed(requirement);
		}

		return Task.CompletedTask;

	}
}
