using MediatR;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;

public class UnassignUserRoleCommand(string userEmail, string roleName) : IRequest
{
	public string UserEmail { get; set; } = userEmail;
	public string RoleName { get; set; } = roleName;
}
