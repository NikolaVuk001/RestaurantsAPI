using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,	
	IRestaurantRepository restaurantRepository,
	IRestauranAuthorizationService restauranAuthorizationService) : IRequestHandler<DeleteRestaurantCommand>
{
	public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Deleting Restaurant with Id: {@requestID}", request.Id);

		var restaurant = await restaurantRepository.GetByIdAsync(request.Id)
			?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

		if (!restauranAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
			throw new ForbidException();

		var isDeleted = await restaurantRepository.DeleteAsync(request.Id);
		if(!isDeleted)
		{
			throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
		}		

		
	}
}
