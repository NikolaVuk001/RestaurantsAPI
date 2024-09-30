using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantRepository
{
	Task<IEnumerable<Restaurant>> GetAllAsync();

	Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);

	Task<Restaurant?> GetByIdAsync(int id);

	Task<int> CreateAsync(Restaurant restaurant);

	Task<bool> DeleteAsync(int id);
	Task UpdateAsync(Restaurant restaurant);

	Task<IEnumerable<Restaurant>> GetOwnerRestaurantsAsync(string ownerId);
	Task SaveChangesAsync();
}
