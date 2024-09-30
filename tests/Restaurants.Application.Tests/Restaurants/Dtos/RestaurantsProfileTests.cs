using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Dtos;

public class RestaurantsProfileTests
{
	private IMapper _mapper;

    public RestaurantsProfileTests()
    {
		var configuration = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile<RestaurantsProfile>();
		});

		_mapper = configuration.CreateMapper();
	}

    [Fact()]
	public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
	{
		// arrange
		

		var restaurant = new Restaurant()
		{
			Id = 1,
			Name = "Test restaurant",
			Description = "Test descripton",
			Category = "Test category",
			HasDelivery = true,
			ContactEmail = "test@test.com",
			ContactNumber = "123456789",
			Adress = new Adress
			{
				City = "Test City",
				Street = "Test Street",
				PostalCode = 12345
			}
		};

		// act

		var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);


		// assert 

		restaurantDto.Should().NotBeNull();
		restaurantDto.Id.Should().Be(restaurant.Id);
		restaurantDto.Name.Should().Be(restaurant.Name);
		restaurantDto.Description.Should().Be(restaurant.Description);
		restaurantDto.Category.Should().Be(restaurant.Category);
		restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
		restaurantDto.City.Should().Be(restaurant.Adress.City);
		restaurantDto.Street.Should().Be(restaurant.Adress.Street);
		restaurantDto.PostalCode.Should().Be(restaurant.Adress.PostalCode);
		
		
	}

	[Fact()]
	public void CreateMap_ForCreateRestuarantCommandToRestaurant_MapsCorrectly()
	{
		// arrange

		

		

		var command = new CreateRestaurantCommand()
		{			
			Name = "Test restaurant",
			Description = "Test descripton",
			Category = "Test category",
			HasDelivery = true,
			ContactEmail = "test@test.com",
			ContactNumber = "123456789",			
			City = "Test City",
			Street = "Test Street",
			PostalCode = 12345
			
		};

		// act

		var restaurant = _mapper.Map<Restaurant>(command);


		// assert 

		restaurant.Should().NotBeNull();		
		restaurant.Name.Should().Be(command.Name);
		restaurant.Description.Should().Be(command.Description);
		restaurant.Category.Should().Be(command.Category);
		restaurant.HasDelivery.Should().Be(command.HasDelivery);
		restaurant.ContactEmail.Should().Be(command.ContactEmail);
		restaurant.ContactNumber.Should().Be(command.ContactNumber);
		restaurant.Adress.Should().NotBeNull();
		restaurant.Adress.City.Should().Be(command.City);
		restaurant.Adress.Street.Should().Be(command.Street);
		restaurant.Adress.PostalCode.Should().Be(command.PostalCode);


	}

	[Fact()]
	public void CreateMap_ForUpdateRestuarantCommandToRestaurant_MapsCorrectly()
	{
		// arrange

		var command = new UpdateRestaurantCommand()
		{
			Id = 1,
			Name = "Test restaurant",
			Description = "Test descripton",			
			HasDelivery = false,
		};

		// act

		var restaurant = _mapper.Map<Restaurant>(command);


		// assert 

		
		restaurant.Should().NotBeNull();
		restaurant.Id.Should().Be(command.Id);
		restaurant.Name.Should().Be(command.Name);
		restaurant.Description.Should().Be(command.Description);
		restaurant.HasDelivery.Should().Be(command.HasDelivery.Value);


	}
}