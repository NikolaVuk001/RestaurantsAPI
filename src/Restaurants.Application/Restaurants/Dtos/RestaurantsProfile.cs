using AutoMapper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
		CreateMap<CreateRestaurantCommand, Restaurant>()
			.ForMember(d => d.Adress, opt => opt.MapFrom(
				src => new Adress
				{
					City = src.City,
					PostalCode = src.PostalCode,
					Street = src.Street,
				}));

		CreateMap<Restaurant, RestaurantDto>()
			.ForMember(d => d.City, opt =>
				opt.MapFrom(src => src.Adress == null ? null : src.Adress.City))
			.ForMember(d => d.PostalCode, opt =>
				opt.MapFrom(src => src.Adress == null ? null : src.Adress.PostalCode))
			.ForMember(d => d.Street, opt =>
				opt.MapFrom(src => src.Adress == null ? null : src.Adress.Street))
			.ForMember(d => d.Dishes, opt => opt.MapFrom(src => src.Dishes));

		CreateMap<UpdateRestaurantCommand, Restaurant>();
			

		


	}
}
