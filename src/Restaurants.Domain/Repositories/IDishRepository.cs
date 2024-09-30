using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishRepository
{
	Task<int> CreateAsync(Dish dish);
	Task DeleteAllForRestaurantAsync(int restaurantId);
	Task<IEnumerable<Dish>> GetAllAsync(int restaurantId);
	Task<Dish?> GetByIdAsync(int id, int restaurantId);
}
