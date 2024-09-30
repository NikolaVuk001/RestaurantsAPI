using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class DishRepository(RestaurantsDbContext dbContext) : IDishRepository
{
	public async Task<int> CreateAsync(Dish dish)
	{
		dbContext.Dishes.Add(dish);
		await dbContext.SaveChangesAsync();
		return dish.Id;
	}

	public async Task DeleteAllForRestaurantAsync(int restaurantId)
	{
		var dishes = await GetAllAsync(restaurantId);
		dbContext.Dishes.RemoveRange(dishes);
		await dbContext.SaveChangesAsync();
	}

	public async Task<IEnumerable<Dish>> GetAllAsync(int restaurantId)
	{
		return await dbContext.Dishes.Where(d => d.RestaurantId == restaurantId).ToListAsync();
	}

	public async Task<Dish?> GetByIdAsync(int id, int restaurantId)
	{
		var dish = await dbContext.Dishes.Where(d => d.RestaurantId == restaurantId).FirstOrDefaultAsync(d => d.Id == id);
		return dish;
	}
}
