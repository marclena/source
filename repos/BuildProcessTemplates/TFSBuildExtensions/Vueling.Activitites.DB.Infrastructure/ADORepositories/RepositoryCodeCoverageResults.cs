using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TFSBuildExtensions.Library.RepositoryContracts;

namespace Vueling.Activitites.DB.Infrastructure.ADORepositories
{
    public class RepositoryCodeCoverageResults : IRepositoryCodeCoverageResults
    {
        private string connectionString = "Data source=" + Vueling.Activities.Configuration.Configuration.tfsdatatier + ";Initial Catalog=Tfs_Warehouse;User ID=" + Vueling.Activities.Configuration.Configuration.tfsuserdb + ";Password=" + Vueling.Activities.Configuration.Configuration.tfspassworddb;

        public RepositoryCodeCoverageResults()
        {
        }

        public bool addBuildCodeCoverageResult(string buildDefinition, string buildName, decimal codeCoverageIndex)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = connection.CreateCommand();

                sqlCommand.Parameters.Add("@BuildDefinition", System.Data.SqlDbType.NChar).Value = buildDefinition;
                sqlCommand.Parameters.Add("@BuildName", System.Data.SqlDbType.NChar).Value = buildName;
                sqlCommand.Parameters.Add("@CodeCoverageIndex", System.Data.SqlDbType.Decimal).Value = codeCoverageIndex;
                sqlCommand.CommandText = @"INSERT INTO VuelingSolutionCodeCoverageResults(BuildDefinition, BuildName, CodeCoverageIndex) Values(@BuildDefinition, @BuildName, @CodeCoverageIndex)";

                sqlCommand.ExecuteNonQuery();
            }

            return true;
        }

        public bool addApplicationCodeCoverageResult(string buildName, string applicationName, decimal codeCoverageIndex)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = connection.CreateCommand();

                sqlCommand.Parameters.Add("@BuildName", System.Data.SqlDbType.NChar).Value = buildName;
                sqlCommand.Parameters.Add("@ApplicationName", System.Data.SqlDbType.NChar).Value = applicationName;
                sqlCommand.Parameters.Add("@CodeCoverageIndex", System.Data.SqlDbType.Decimal).Value = codeCoverageIndex;
                sqlCommand.CommandText = @"INSERT INTO VuelingApplicationCodeCoverageResults(BuildName, ApplicationName, CodeCoverageIndex) Values(@BuildName, @ApplicationName, @CodeCoverageIndex)";

                sqlCommand.ExecuteNonQuery();
            }

            return true;
        }
    }
}
