using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(ILogger<GetRestaurantByIdQueryHandler> logger,
	IMapper mapper,
	IRestaurantRepository restaurantRepository,
	IBlobStorageService blobStorageService) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
{
	public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Getting restaurant by id: {@requestID}", request.Id);
		var restaurant = await restaurantRepository.GetByIdAsync(request.Id)
			?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

		var restaurantDto = mapper.Map<RestaurantDto>(restaurant);
		//var restaurantDto = RestaurantDto.FromEntity(restaurant); Manual Mapping

		restaurantDto.LogoSasUrl = blobStorageService.GetBlobSasUrl(restaurant.LogoUrl);


		return restaurantDto;
	}
}
