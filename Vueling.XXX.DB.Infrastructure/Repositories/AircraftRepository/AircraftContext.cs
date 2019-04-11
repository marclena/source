using System.Data.EntityClient;
using Vueling.XXX.DB.Infrastructure.Configuration;
using Vueling.Extensions.Library.DI;
using System.Globalization;

namespace Vueling.XXX.DB.Infrastructure.Repositories.AircraftRepository
{
    [RegisterContextAttribute]
    public class AircraftContext : ContextBase, IAircraftContext
    {

        public AircraftContext(IXXXInfrastructureConfiguration xXXInfrastructureConfiguration)
        {
            Context = GetEntitiesContext(xXXInfrastructureConfiguration.ConnectionString, xXXInfrastructureConfiguration.DatabaseTimeout);
        }

        protected AircraftDbEntities GetEntitiesContext(string connectionString, int commandTimeout)
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
                                            "res://*/Repositories.AircraftRepository.AircraftDataModel.csdl",
                                            "res://*/Repositories.AircraftRepository.AircraftDataModel.ssdl",
                                            "res://*/Repositories.AircraftRepository.AircraftDataModel.msl");

            // Build Entity Connection
            EntityConnection entityConnection = new EntityConnection(entityConnectionStringBuilder.ToString());

            // Initialize Entity Object
            return new AircraftDbEntities(entityConnection)
            {
                CommandTimeout = commandTimeout
            };

        }

    }
}
