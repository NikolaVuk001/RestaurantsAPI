using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements.HasRestaurants;
using Xunit;

namespace Restaurants.Infrastructure.Tests.Authorization.Requirements.HasRestaurants;

public class HasRestaurantsRequirmentHandlerTests
{
	[Fact()]
	public async Task HandleRequirmentAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()	
	{
		// arrange

		var userContextMock = new Mock<IUserContext>();
		
		var currentUser = new CurrentUser("1", "test@test.com", ["User", "Owner"], null, null);

		userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

		var restaurants = new List<Restaurant>()
		{
			new()
			{
				OwnerId = currentUser.Id
			},
			new()
			{
				OwnerId = currentUser.Id
			}
		};
		var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
		restaurantRepositoryMock.Setup(repo => repo.GetOwnerRestaurantsAsync(currentUser.Id)).ReturnsAsync(restaurants);

		var loggerMock = new Mock<ILogger<HasRestaurantsRequirmentHandler>>();

		var requirment = new HasRestaurantsRequirment(2);
		var handler = new HasRestaurantsRequirmentHandler(loggerMock.Object
			, userContextMock.Object,
			restaurantRepositoryMock.Object);

		var context = new AuthorizationHandlerContext([requirment], null, null);

		// act
		await handler.HandleAsync(context);

		// assert
		context.HasSucceeded.Should().BeTrue();
		context.HasFailed.Should().BeFalse();
	}

	[Fact()]
	public async Task HandleRequirmentAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
	{
		// arrange

		var userContextMock = new Mock<IUserContext>();

		var currentUser = new CurrentUser("1", "test@test.com", ["User", "Owner"], null, null);

		userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

		var restaurants = new List<Restaurant>()
		{
			new()
			{
				OwnerId = currentUser.Id
			},
		};
		var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
		restaurantRepositoryMock.Setup(repo => repo.GetOwnerRestaurantsAsync(currentUser.Id)).ReturnsAsync(restaurants);

		var loggerMock = new Mock<ILogger<HasRestaurantsRequirmentHandler>>();

		var requirment = new HasRestaurantsRequirment(2);
		var handler = new HasRestaurantsRequirmentHandler(loggerMock.Object
			, userContextMock.Object,
			restaurantRepositoryMock.Object);

		var context = new AuthorizationHandlerContext([requirment], null, null);

		// act
		await handler.HandleAsync(context);

		// assert
		context.HasSucceeded.Should().BeFalse();
		context.HasFailed.Should().BeTrue();
	}
}