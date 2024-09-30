using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAllDishesForRestaurant;

public class DeleteAllDishesForRestaurantCommandHandler(ILogger<DeleteAllDishesForRestaurantCommandHandler> logger,
	IDishRepository dishRepository,
	IRestaurantRepository restaurantRepository,
	IRestauranAuthorizationService restauranAuthorizationService) : IRequestHandler<DeleteAllDishesForRestaurantCommand>
{
	public async Task Handle(DeleteAllDishesForRestaurantCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Deleting all the dishes for restaurant with Id: {@id}", request.RestaurantId);

		var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId);

		if (!restauranAuthorizationService.Authorize(restaurant!, ResourceOperation.Delete))
			throw new ForbidException();

		await dishRepository.DeleteAllForRestaurantAsync(request.RestaurantId);
	}
}
