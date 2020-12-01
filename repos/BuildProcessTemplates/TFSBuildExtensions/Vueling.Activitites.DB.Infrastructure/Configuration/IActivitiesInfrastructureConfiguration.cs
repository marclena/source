using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vueling.Activitites.DB.Infrastructure.Configuration
{
    public interface IActivitiesInfrastructureConfiguration
    {
        string DatabaseConnection_Vueling_TfsOperations { get; }

        string DatabaseConnection_Tfs_Warehouse { get; }
    }
}
