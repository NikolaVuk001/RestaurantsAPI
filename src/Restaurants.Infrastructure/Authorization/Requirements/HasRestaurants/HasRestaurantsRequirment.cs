using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements.HasRestaurants;

public class HasRestaurantsRequirment(int count) : IAuthorizationRequirement
{
	public int Count { get; } = count;
}
