using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestauranAuthorizationService(ILogger<RestauranAuthorizationService> logger,
	IUserContext userContext) : IRestauranAuthorizationService
{
	public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
	{
		var user = userContext.GetCurrentUser();

		logger.LogInformation("Authorizing user {UserEmail}, to {operation} for restaurant {RestaurantName}",
			user.Email,
			resourceOperation,
			restaurant.Name);

		if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
		{
			logger.LogInformation("Create/read operation - successful authorization");
			return true;
		}

		if (resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
		{
			logger.LogInformation("Admin user, delete operation - successful authorization");
			return true;
		}

		if (resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update
			&& user.Id == restaurant.OwnerId)
		{
			logger.LogInformation("Restaurant owner - successful authorization");
			return true;
		}

		return false;
	}
}
