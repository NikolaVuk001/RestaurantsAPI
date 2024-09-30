using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandlerTests
{
	private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
	private readonly Mock<IRestaurantRepository> _repositoryMock;
	private readonly Mock<IRestauranAuthorizationService> _restaurantAuthorizationServiceMock;

	private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
		_loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
		_repositoryMock = new Mock<IRestaurantRepository>();
		_restaurantAuthorizationServiceMock = new Mock<IRestauranAuthorizationService>();

		_handler = new UpdateRestaurantCommandHandler(
			_loggerMock.Object,
			_repositoryMock.Object,
			_restaurantAuthorizationServiceMock.Object);

    }

    [Fact()]
	public async Task Handle_WithValidRequest_ShouldUpdateRestaurants()
	{
		// arange 
		var restaurantId = 1;
		var command = new UpdateRestaurantCommand()
		{
			Id = restaurantId,
			Name = "Update Test Name",
			Description = "New Description",
			HasDelivery = true
		};

		var restaurant = new Restaurant()
		{
			Id = restaurantId,
			Name = "Test",
			Description = "Test"
		};

		_repositoryMock.Setup(repo => repo
		.GetByIdAsync(restaurantId))
		.ReturnsAsync(restaurant);

		_restaurantAuthorizationServiceMock.Setup(m =>
		m.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
			.Returns(true);

		// act
		await _handler.Handle(command, CancellationToken.None);

		// assert
		_repositoryMock.Verify(r => r.UpdateAsync(restaurant), Times.Once);		
	}

	[Fact()]
	public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
	{
		// arrange
		var restaurantId = 2;
		var request = new UpdateRestaurantCommand()
		{
			Id = restaurantId
		};		
;

		_repositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId))
			.ReturnsAsync((Restaurant?)null);

		// act
		Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);


		// assert
		await act.Should().ThrowAsync<NotFoundException>()
			.WithMessage($"Restaurant with id: {restaurantId} doesn't exist.");
	}

	[Fact()]
	public async Task Handle_WithUnauthorizedUser_ShouldTrowForbidException()
	{
		// arrange
		var restaurantId = 1;

		var command = new UpdateRestaurantCommand()
		{
			Id = restaurantId
		};

		var restaurant = new Restaurant()
		{
			Id = restaurantId
		};

		_repositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId))
			.ReturnsAsync(restaurant);

		_restaurantAuthorizationServiceMock.
			Setup(m => m.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
				.Returns(false);

		// act 
		Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

		// assert

		await act.Should().ThrowAsync<ForbidException>();


	}
}