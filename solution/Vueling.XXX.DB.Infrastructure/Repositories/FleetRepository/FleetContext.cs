using System.Data.EntityClient;
using System.Globalization;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.DB.Infrastructure.Configuration;

namespace Vueling.XXX.DB.Infrastructure.Repositories.FleetRepository
{
    [RegisterContextAttribute]
    public class FleetContext : ContextBase, IFleetContext
    {

        public FleetContext(IXXXInfrastructureConfiguration xXXInfrastructureConfiguration)
        {
            Context = GetEntitiesContext(xXXInfrastructureConfiguration.ConnectionString, xXXInfrastructureConfiguration.DatabaseTimeout);
        }

        protected FleetDbEntities GetEntitiesContext(string connectionString, int commandTimeout)
        {
            // Initialize Entity Connection String Builder
            EntityConnectionStringBuilder entityConnectionStringBuilder = new EntityConnectionStringBuilder();

            // Set the provider name
            entityConnectionStringBuilder.Provider = "System.Data.SqlClient";

            // Set the provider-specific connection string
            entityConnectionStringBuilder.ProviderConnectionString = connectionString;
            entityConnectionStringBuilder.ProviderConnectionString += ";MultipleActiveResultSets=True";

            // Set the Metadata location
            entityConnectionStringBuilder.Metadata = string.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}",
                                            "res://*/Repositories.FleetRepository.FleetDataModel.csdl",
                                            "res://*/Repositories.FleetRepository.FleetDataModel.ssdl",
                                            "res://*/Repositories.FleetRepository.FleetDataModel.msl");

            // Build Entity Connection
            EntityConnection entityConnection = new EntityConnection(entityConnectionStringBuilder.ToString());

            // Initialize Entity Object
            return new FleetDbEntities(entityConnection)
            {
                CommandTimeout = commandTimeout
            };

        }

    }
}
