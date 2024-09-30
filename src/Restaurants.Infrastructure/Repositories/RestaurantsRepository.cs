using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantRepository
{
	public async Task<int> CreateAsync(Restaurant restaurant)
	{
		dbContext.Restaurants.Add(restaurant);
		await dbContext.SaveChangesAsync();
		return restaurant.Id;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var restaurant = dbContext.Restaurants.FirstOrDefault(x => x.Id == id);
		if(restaurant == null)
		{			
			return false;
		}
		dbContext.Remove(restaurant);
		await dbContext.SaveChangesAsync();
		return true;

	}

	public async Task<IEnumerable<Restaurant>> GetAllAsync()
	{
		var restaurants = await dbContext.Restaurants.ToListAsync();
		return restaurants;
	}
	

	public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
	{
		var lowerSearchPhrase = searchPhrase?.ToLower();

		var baseQuery = dbContext.Restaurants.Where(
			r =>
			lowerSearchPhrase == null ||
			r.Name.ToLower() == lowerSearchPhrase ||
			r.Description.ToLower() == lowerSearchPhrase);

		var totalCount = await baseQuery.CountAsync();

		if(sortBy != null)
		{
			var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
			{
				{nameof(Restaurant.Name), r => r.Name },
				{nameof(Restaurant.Description), r => r.Description },
				{nameof(Restaurant.Category), r => r.Category}
			};

			var selectedColumn = columnsSelector[sortBy];
			baseQuery = sortDirection == SortDirection.Ascending
				? baseQuery.OrderBy(selectedColumn)
				: baseQuery.OrderByDescending(selectedColumn);
		}

		var restaurants = await baseQuery
			.Skip(pageSize * (pageNumber - 1))
			.Take(pageSize)
			.ToListAsync();

		return (restaurants, totalCount);
	}

	public async Task<Restaurant?> GetByIdAsync(int id)
	{		
		var restaurant = await dbContext.Restaurants.Include(r => r.Dishes).FirstOrDefaultAsync(r => r.Id == id);
		return restaurant;
	}

	public async Task<IEnumerable<Restaurant>> GetOwnerRestaurantsAsync(string ownerId)
	{
		var restaurants = await dbContext.Restaurants.Where(r => r.OwnerId == ownerId).ToListAsync();
		return restaurants;
	}

	public Task SaveChangesAsync() => dbContext.SaveChangesAsync();

	public async Task UpdateAsync(Restaurant restaurant)
	{
		dbContext.Update(restaurant);
		await dbContext.SaveChangesAsync();
	}
}
