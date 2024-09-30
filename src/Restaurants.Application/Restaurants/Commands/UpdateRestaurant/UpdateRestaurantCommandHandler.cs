using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger,	
	IRestaurantRepository restaurantRepository,
	IRestauranAuthorizationService restauranAuthorizationService) : IRequestHandler<UpdateRestaurantCommand>
{
	public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Updating restaurant with id: {@requestID} with values {@restaurant}", request.Id, request);	

		var originalRestaurant = await restaurantRepository.GetByIdAsync(request.Id);
		if(originalRestaurant is null)
		{
			throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
		}

		if (!restauranAuthorizationService.Authorize(originalRestaurant, ResourceOperation.Update))
			throw new ForbidException();

		if (request.Name is not null)
		{
			originalRestaurant.Name = request.Name;
		}
		if(request.Description is not null)
		{
			originalRestaurant.Description = request.Description;
		}
		if(request.HasDelivery is not null)
		{
			originalRestaurant.HasDelivery = (bool)request.HasDelivery;
		}

		await restaurantRepository.UpdateAsync(originalRestaurant);
		
		
	}
}
