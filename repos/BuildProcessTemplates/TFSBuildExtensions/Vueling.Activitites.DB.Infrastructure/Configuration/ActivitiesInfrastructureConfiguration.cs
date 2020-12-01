using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.Extensions.Library.DI;
using Vueling.Extensions.Library;
using Vueling.Activitites.DB.Infrastructure.Exceptions;
using System.Diagnostics;

namespace Vueling.Activitites.DB.Infrastructure.Configuration
{
    [RegisterConfigurationAttribute]
    public class ActivitiesInfrastructureConfiguration : IActivitiesInfrastructureConfiguration
    {
        private string databaseConnection_Vueling_TfsOperations = "Data source=" + Vueling.Activities.Configuration.Configuration.sqldatatier + ";Initial Catalog=Vueling_TfsOperations;User ID=" + Vueling.Activities.Configuration.Configuration.tfsuserdb + ";Password=" + Vueling.Activities.Configuration.Configuration.tfspassworddb;
        private string databaseConnection_Tfs_Warehouse = "Data source=" + Vueling.Activities.Configuration.Configuration.tfsdatatier + ";Initial Catalog=Tfs_Warehouse;User ID=" + Vueling.Activities.Configuration.Configuration.tfsuserdb + ";Password=" + Vueling.Activities.Configuration.Configuration.tfspassworddb;

        public string DatabaseConnection_Vueling_TfsOperations
        {
            get { return databaseConnection_Vueling_TfsOperations; }
        }

        public string DatabaseConnection_Tfs_Warehouse
        {
            get { return databaseConnection_Tfs_Warehouse; }
        }
    }
}