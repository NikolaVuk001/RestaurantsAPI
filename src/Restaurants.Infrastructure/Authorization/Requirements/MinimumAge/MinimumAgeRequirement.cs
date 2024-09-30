using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimumAge;

public class MinimumAgeRequirement(int minumumAge) : IAuthorizationRequirement
{
    public int MinumomAge { get; } = minumumAge;
}
