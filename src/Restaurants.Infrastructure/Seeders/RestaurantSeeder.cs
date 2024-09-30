using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace Restaurants.Infrastructure.Seeders;



internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
	public async Task Seed()
	{
		if(dbContext.Database.GetPendingMigrations().Any())
		{
			await dbContext.Database.MigrateAsync();
		}

		if (await dbContext.Database.CanConnectAsync())
		{
			if (!dbContext.Restaurants.Any())
			{
				var restaurants = GetRestaurants();
				dbContext.Restaurants.AddRange(restaurants);
				await dbContext.SaveChangesAsync();
			}

			if(!dbContext.Roles.Any())
			{
				var roles = GetRoles();
				dbContext.Roles.AddRange(roles);
				await dbContext.SaveChangesAsync();
			}
		}
	}


	private IEnumerable<IdentityRole> GetRoles()
	{
		List<IdentityRole> roles =
			[
				new IdentityRole(UserRoles.User)
				{
					NormalizedName = UserRoles.User.ToUpper()
				},
				new IdentityRole(UserRoles.Owner)
				{
					NormalizedName = UserRoles.Owner.ToUpper()
				},
				new IdentityRole(UserRoles.Admin){
					NormalizedName = UserRoles.Admin.ToUpper()
				},
			];

		return roles;	
	}

	private IEnumerable<Restaurant> GetRestaurants()
	{
		User owner = new User()
		{
			Email = "seed-user@test.com"
		};

		List<Restaurant> restaurants = [
			new Restaurant()
			{
				Owner = owner,
				Name = "KFC",
				Category = "Fast Food",
				Description =
					"KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
				ContactEmail = "contact@kfc.com",
				HasDelivery = true,
				Dishes =
				[
					new()
					{
						Name = "Nashville Hot Chicken",
						Description = "Nashville Hot Chicken (10 pcs.)",
						Price = 10.30M,
					},
					new()
					{
						Name = "Chicken Nuggets",
						Description = "Chicken Nuggets (5 pcs.)",
						Price = 5.30M,
					},
				],
				Adress = new()
				{
					City = "London",
					Street = "Cork St 5",
					PostalCode = 11000
				}
			},
			new Restaurant()
			{
				Owner = owner,
				Name = "McDonald",
				Category = "Fast Food",
				Description =
					"McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
				ContactEmail = "contact@mcdonald.com",
				HasDelivery = true,
				Adress = new Adress()
				{
					City = "London",
					Street = "Boots 193",
					PostalCode = 11000
				}
			}
		];

		return restaurants;

	}
}
