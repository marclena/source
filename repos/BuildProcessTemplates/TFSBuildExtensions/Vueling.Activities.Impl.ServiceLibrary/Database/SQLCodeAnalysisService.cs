using SqlCodeGuardAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.Database;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Impl.ServiceLibrary.Database
{
    [RegisterService]
    public class SQLCodeAnalysisService : BaseActivityService, ISQLCodeAnalysisService
    {
        private string pathScripFiles;

        private double complexity;


        public void Initialize(string _pathScripFiles)
        {
            pathScripFiles = _pathScripFiles;
        }

        public override void InternalExecute()
        {
            string[] sqlFiles = Directory.GetFiles(pathScripFiles, "*.sql");

            foreach (var file in sqlFiles)
            {
                string sql;
                using (StreamReader sr = new StreamReader(file))
                {
                    sql = sr.ReadToEnd();
                }
                API apiSqlCodeGuard = new API();

                apiSqlCodeGuard.IncludeIssue("BP005"); //* in SELECT list
                apiSqlCodeGuard.IncludeIssue("BP018"); //UPDATE statement without WHERE clause
                apiSqlCodeGuard.IncludeIssue("MI008"); //CURSOR string in a statement
                apiSqlCodeGuard.MI008Statements.Add("cursor-declaration");
                apiSqlCodeGuard.IncludeIssue("EI028"); //Adding NOT NULL column without default value
                apiSqlCodeGuard.IncludeIssue("BP017"); //DELETE statement without WHERE clause
                apiSqlCodeGuard.IncludeIssue("ST011"); //Consider using table variable instead of temporary table
                apiSqlCodeGuard.IncludeIssue("PE003"); //Creation of table by SELECT ... INTO ... statement
                apiSqlCodeGuard.IncludeIssue("BP004"); //INSERT without column list 
                apiSqlCodeGuard.IncludeIssue("BP006"); //Usually TOP without ORDER BY is meaningless
                apiSqlCodeGuard.IncludeIssue("EI019"); //BEGIN TRANSACTION without ROLLBACK TRANSACTION

                bool unparsed;
                List<Issue> listIssuesAnalysis = apiSqlCodeGuard.GetIssues(sql, out unparsed);

                if (unparsed)
                {
                    this.ErrorMessageList.Add("Error parsing sql script " + file);

                    Console.WriteLine("Unparsed! Issue list may be incomplete or wrong");
                }

                foreach (Issue issue in listIssuesAnalysis)
                {
                    this.ErrorMessageList.Add(String.Format("({0}){1} at {2}:{3} ({4})", issue.ErrorCode, issue.ErrorText, issue.Column, issue.Line, issue.ErrorMessage));
                }

                double complexity; int statementCount;

                apiSqlCodeGuard.GetComplexity(sql, out unparsed, out complexity, out statementCount);
            }
        }

        public double Complexity
        {
            get
            {
                return complexity;
            }
            set
            {
                complexity = value;
            }
        }
    }
}
