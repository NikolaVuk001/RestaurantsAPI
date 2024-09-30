using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetAllDishesForRestaurant;

public class GetAllDishesForRestaurantQueryHandler(ILogger<GetAllDishesForRestaurantQueryHandler> logger,
    IMapper mapper,
    IDishRepository dishRepository) : IRequestHandler<GetAllDishesForRestaurantQuery, IEnumerable<DishDto>>
{
    public async Task<IEnumerable<DishDto>> Handle(GetAllDishesForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all dishes for restuarant with ID: {@restaurantID}", request.RestaurantId);
        var disehes = await dishRepository.GetAllAsync(request.RestaurantId);
        var dishesDto = mapper.Map<IEnumerable<DishDto>>(disehes);
        return dishesDto;

    }
}
