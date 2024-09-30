using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommandHandler(ILogger<UploadRestaurantLogoCommandHandler> logger,
	IRestaurantRepository restaurantRepository,
	IRestauranAuthorizationService restauranAuthorizationService,
	IBlobStorageService blobStorageService
	) : IRequestHandler<UploadRestaurantLogoCommand>
{
	public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Uploading restaurant logo for id: {RestaurantId}", request.RestaurantId);

		var originalRestaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId);
		if(originalRestaurant is null)
		{
			throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
		}
		if(!restauranAuthorizationService.Authorize(originalRestaurant, ResourceOperation.Update)) 
		{
			throw new ForbidException();
		}

		var logoUrl = await blobStorageService.UploadToBlobAsync(request.File, request.FileName);

		originalRestaurant.LogoUrl = logoUrl;

		await restaurantRepository.SaveChangesAsync();
	}
	
}
