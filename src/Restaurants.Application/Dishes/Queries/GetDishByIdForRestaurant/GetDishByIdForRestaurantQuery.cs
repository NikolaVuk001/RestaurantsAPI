using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;

public class GetDishByIdForRestaurantQuery(int id, int restaurantId) : IRequest<DishDto>
{
    public int Id { get; } = id;
    public int RestaurantId { get; } = restaurantId;
}
