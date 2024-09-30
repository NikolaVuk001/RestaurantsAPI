﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Authorization.Requirements.HasRestaurants;
using Restaurants.Infrastructure.Authorization.Requirements.MinimumAge;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Configuration;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Infrastructure.Storage;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("RestaurantsDb");
		services.AddDbContext<RestaurantsDbContext>(options => 
		options
			.UseSqlServer(connectionString)
			.EnableSensitiveDataLogging());

		services.AddIdentityApiEndpoints<User>()
			.AddRoles<IdentityRole>()
			.AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
			.AddEntityFrameworkStores<RestaurantsDbContext>();
			
		
		services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
		services.AddScoped<IRestaurantRepository, RestaurantsRepository>();
		services.AddScoped<IDishRepository, DishRepository>();
		

		services.AddAuthorizationBuilder()
			.AddPolicy(PolicyNames.HasNationality, builder => builder.RequireClaim(AppClaimTypes.Nationality, "SRBENDA", "SRBIN"))
			.AddPolicy(PolicyNames.Atleast20, builder => builder.AddRequirements(new MinimumAgeRequirement(20)))
			.AddPolicy(PolicyNames.HasRestaurants, builder => builder.AddRequirements(new HasRestaurantsRequirment(2)));

		services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
		services.AddScoped<IAuthorizationHandler, HasRestaurantsRequirmentHandler>();
		services.AddScoped<IRestauranAuthorizationService, RestauranAuthorizationService>();

		services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
		services.AddScoped<IBlobStorageService, BlobStorageService>();
		
	}
}
