using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TFSBuildExtensions.Library.CodeMetrics.Report;
using TFSBuildExtensions.Library.RepositoryContracts;

namespace Vueling.Activitites.DB.Infrastructure.ADORepositories
{
    public class RepositoryCodeMetricsResults : IRepositoryCodeMetricsResults
    {
        private string connectionString = "Data source=" + Vueling.Activities.Configuration.Configuration.tfsdatatier + ";Initial Catalog=Tfs_Warehouse;User ID=" + Vueling.Activities.Configuration.Configuration.tfsuserdb + ";Password=" + Vueling.Activities.Configuration.Configuration.tfspassworddb;

        public RepositoryCodeMetricsResults()
        {
        }

        public bool addBuildCodeMetricsResult(Build build)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = connection.CreateCommand();

                sqlCommand.Parameters.Add("@BuildDefinition", System.Data.SqlDbType.NChar).Value = build.Name.Substring(0, build.Name.LastIndexOf("_"));
                sqlCommand.Parameters.Add("@BuildName", System.Data.SqlDbType.NChar).Value = build.Name;

                foreach (Metric metric in build.Metrics)
                {
                    sqlCommand.Parameters.Add("@" + metric.Name, System.Data.SqlDbType.Int).Value = Convert.ToInt64(Convert.ToDouble(metric.Value));
                }

                sqlCommand.CommandText = @"INSERT INTO VuelingSolutionCodeMetricsResults(BuildDefinition,BuildName,MaintainabilityIndex,CyclomaticComplexity,ClassCoupling,DepthOfInheritance,LinesOfCode) VALUES(@BuildDefinition,@BuildName,@MaintainabilityIndex,@CyclomaticComplexity,@ClassCoupling,@DepthOfInheritance,@LinesOfCode)";

                sqlCommand.ExecuteNonQuery();
            }

            return true;
        }

        public bool addApplicationCodeMetricsResult(Module application, string buildName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = connection.CreateCommand();

                sqlCommand.Parameters.Add("@BuildName", System.Data.SqlDbType.NChar).Value = buildName;
                sqlCommand.Parameters.Add("@ApplicationName", System.Data.SqlDbType.NChar).Value = application.Name.Substring(0, application.Name.LastIndexOf(".dll"));

                foreach (Metric metric in application.Metrics)
                {
                    sqlCommand.Parameters.Add("@" + metric.Name, System.Data.SqlDbType.Int).Value = Convert.ToInt64(Convert.ToDouble(metric.Value));
                }

                sqlCommand.CommandText = @"INSERT INTO VuelingApplicationCodeMetricsResults(BuildName,ApplicationName,MaintainabilityIndex,CyclomaticComplexity,ClassCoupling,DepthOfInheritance,LinesOfCode) VALUES(@BuildName,@ApplicationName,@MaintainabilityIndex,@CyclomaticComplexity,@ClassCoupling,@DepthOfInheritance,@LinesOfCode)";

                sqlCommand.ExecuteNonQuery();
            }

            return true;
        }
    }
}
