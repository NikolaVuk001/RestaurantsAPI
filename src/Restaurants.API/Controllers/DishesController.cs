using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Controllers;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Application.Dishes.Queries;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetAllDishesForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;
using Restaurants.Application.Dishes.Commands.DeleteAllDishesForRestaurant;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Microsoft.AspNetCore.Authorization;
using Restaurants.Infrastructure.Authorization;


namespace Restaurants.API.Controllers;
[ApiController]
[Route("api/restaurant/{restaurantId}/dishes")]
[Authorize]
public class DishesController(IMediator mediator) : ControllerBase
{

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Policy = PolicyNames.Atleast20)]
	public async Task<ActionResult<IEnumerable<DishDto>>> GetAllDishesForRestaurant([FromRoute] int restaurantId)
	{
		await mediator.Send(new GetRestaurantByIdQuery(restaurantId));
		var dishes = await mediator.Send(new GetAllDishesForRestaurantQuery(restaurantId));
		return Ok(dishes);

	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<DishDto>> GetDishByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int id)
	{
		await mediator.Send(new GetRestaurantByIdQuery(restaurantId));
		var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(id, restaurantId));
		return Ok(dish);
	}

	[HttpDelete]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeleteAllDishesForRestaurant([FromRoute] int restaurantId)
	{
		await mediator.Send(new GetRestaurantByIdQuery(restaurantId));
		await mediator.Send(new DeleteAllDishesForRestaurantCommand(restaurantId));
		return NoContent();
	}



	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand command)
	{
		await mediator.Send(new GetRestaurantByIdQuery(restaurantId));
		command.RestaurantId = restaurantId;		
		var id = await mediator.Send(command);		
		return CreatedAtAction(nameof(GetDishByIdForRestaurant), new { restaurantId, id }, null);

	}
}
