using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
	IMapper mapper,
	IRestaurantRepository restaurantRepository,
	IUserContext userContext) : IRequestHandler<CreateRestaurantCommand, int>
{
	public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
	{
		var currentUser = userContext.GetCurrentUser();

		logger.LogInformation("{UserEmail} [{UsedId}] Creating a new restaurant {@Restaurant}",
			request,
			currentUser!.Email,
			currentUser!.Id);


		var restaurant = mapper.Map<Restaurant>(request);

		restaurant.OwnerId = currentUser.Id;

		int id = await restaurantRepository.CreateAsync(restaurant);
		return id;		
	}
}
