using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;

public class UnassignUserRoleCommandHandler(ILogger<UnassignUserRoleCommandHandler> logger,
	UserManager<User> userManager, 
	RoleManager<IdentityRole> roleManager) : IRequestHandler<UnassignUserRoleCommand>
{
	public async Task Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Unassinging User Role: {@request}", request);

		var user = await userManager.FindByEmailAsync(request.UserEmail)
			?? throw new NotFoundException(nameof(User), request.UserEmail);

		var role = await roleManager.FindByNameAsync(request.RoleName)
			?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);

		if(await userManager.IsInRoleAsync(user, role.Name!))
		{
			await userManager.RemoveFromRoleAsync(user, role.Name!);
		}
		

		

	}
}
