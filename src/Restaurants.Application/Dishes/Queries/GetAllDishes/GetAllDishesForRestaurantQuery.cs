using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetAllDishesForRestaurant;

public class GetAllDishesForRestaurantQuery(int restaurandId) : IRequest<IEnumerable<DishDto>>
{
    public int RestaurantId { get; } = restaurandId;
}

