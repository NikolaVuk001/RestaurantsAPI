using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommand : IRequest<int>
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public string Category { get; set; } = default!;
	public bool HasDelivery { get; set; }
	public string? ContactEmail { get; set; }
	[Phone(ErrorMessage = "Please provide a valid phone number")]
	public string? ContactNumber { get; set; }
	public string? City { get; set; }
	public string? Street { get; set; }
	//[RegularExpression(@"^\d{2}-\d{3}$",
	//	ErrorMessage = "Pleaste provide a valid postal code (XX-XXX).")]
	public int? PostalCode { get; set; }
}
