using System.Security.Principal;

namespace Restaurants.Infrastructure.Configuration;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; }
    public string LogosContainerName { get; set; }

    public string AccountKey { get; set; }
    public string AccountName { get; set; }
}
