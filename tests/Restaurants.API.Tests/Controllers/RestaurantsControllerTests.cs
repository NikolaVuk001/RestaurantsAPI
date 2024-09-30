using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Net.Http.Json;
using Xunit;

namespace Restaurants.API.Tests.Controllers;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly WebApplicationFactory<Program> _factory;
	private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock = new();

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
		_factory = factory.WithWebHostBuilder(builder =>
		{
			builder.ConfigureTestServices(services =>
			{
				services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
				services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantRepository),
					_ => _restaurantRepositoryMock.Object));
			});
		});
    }

    [Fact()]
	public async Task GetAll_ForValidRequest_Returns200Ok()
	{
		// arrange
		var client = _factory.CreateClient();

		// act
		var restult = await client.GetAsync("api/restaurants?pageNumber=1&pageSize=10");

		// assert
		restult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
	}

	[Fact()]
	public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
	{
		// arrange
		var client = _factory.CreateClient();

		// act
		var restult = await client.GetAsync("api/restaurants");

		// assert
		restult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
	}

	[Fact()]
	public async void GetById_ForNonExistingId_ShouldReturn404NotFound()
	{
		// arrange
		var id = 1123;

		_restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

		var client = _factory.CreateClient();
		

		// act
		var response = await client.GetAsync($"/api/restaurants/{id}");

		// assert
		response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

	}

	[Fact()]
	public async void GetById_ForExistingId_ShouldReturn200Ok()
	{
		// arrange
		var id = 99;

		var restaurant = new Restaurant()
		{
			Id = id,
			Name = "Test",
			Description = "Test Description"
		};

		_restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant);

		var client = _factory.CreateClient();


		// act
		var response = await client.GetAsync($"/api/restaurants/{id}");
		var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

		// assert
		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
		restaurantDto.Should().NotBeNull();
		restaurantDto.Id.Should().Be(restaurant.Id);
		restaurantDto.Name.Should().Be(restaurant.Name);
		restaurantDto.Description.Should().Be(restaurant.Description);
		

	}

	//[Fact()]
	//public void DeleteRestaurantTest()
	//{

	//}

	//[Fact()]
	//public void UpdateRestaurantTest()
	//{

	//}

	//[Fact()]
	//public void CreateRestaurantTest()
	//{

	//}
}