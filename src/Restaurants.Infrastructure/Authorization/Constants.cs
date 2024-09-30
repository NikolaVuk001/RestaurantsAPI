namespace Restaurants.Infrastructure.Authorization;

public static class PolicyNames
{
	public const string HasNationality = "HasNationality";
	public const string Atleast20 = "Atleast20";
	public const string HasRestaurants = "HasRestaurants";
}

public static class AppClaimTypes
{
	public const string Nationality = "Nationality";
	public const string DateOfBirth = "DateofBirth";
}
