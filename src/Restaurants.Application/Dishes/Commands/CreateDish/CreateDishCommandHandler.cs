using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler(ILogger<CreateDishCommand> logger,
    IMapper mapper,
    IDishRepository dishRepository,
    IRestaurantRepository restaurantRepository,
    IRestauranAuthorizationService restauranAuthorizationService) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
		logger.LogInformation("Creating new dish {@Dish}", request);
        var dish = mapper.Map<Dish>(request);
        var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId);
		if (!restauranAuthorizationService.Authorize(restaurant!, ResourceOperation.Create))
			throw new ForbidException();

		int id = await dishRepository.CreateAsync(dish);
        return id;
    }
}
