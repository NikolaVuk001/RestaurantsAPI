using MediatR;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommand(string userEmail, string roleName) : IRequest
{
    public string UserEmail { get; set; } = userEmail;
    public string RoleName { get; set; } = roleName;
}
