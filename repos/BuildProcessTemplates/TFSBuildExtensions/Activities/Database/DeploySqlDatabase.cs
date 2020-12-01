using System.ComponentModel;
using System.Activities;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace TFSBuildExtensions.Database
{
    public class DeploySqlDatabase : BaseCodeActivity
    {
        [Description("Sources directory")]
        [RequiredArgument]
        public InArgument<string> SourcesDirectory { get; set; }

        [Description("Binaries directory")]
        [RequiredArgument]
        public InArgument<string> BinariesDirectory { get; set; }

        [Description("Database name")]
        [RequiredArgument]
        public InArgument<string> DatabaseName { get; set; }

        [Description("Database server")]
        [RequiredArgument]
        public InArgument<string> DatabaseServer { get; set; }

        [Description("Database server port")]
        [DefaultValue("1433")]
        public InArgument<string> DatabaseServerPort { get; set; }

        [Description("User to connect to database")]
        [RequiredArgument]
        public InArgument<string> DeployUser { get; set; }

        [Description("Password to connect to database")]
        [RequiredArgument]
        public InArgument<string> DeployPassword { get; set; }

        [Description("Allow drops")]
        [DefaultValue("True")]
        public InArgument<string> AllowDrops { get; set; }

        [Description("Database deployed successfully or not")]
        public OutArgument<bool> IsDatabaseDeployedSuccessfully { get; set; }

        [Description("Standard log output")]
        public OutArgument<string> StdOutput { get; set; } 

        protected override void InternalExecute()
        {
            deployDatabase();
        }

        private void deployDatabase()
        {
            string pathSqlFile = this.BinariesDirectory.Get(this.ActivityContext) + @"\" + this.DatabaseName.Get(this.ActivityContext).Replace("\"","") + ".deploymanifest";

            if (!File.Exists(pathSqlFile))
            {
                LogBuildMessage("File " + pathSqlFile + " doesn't exists.");
                runSqlPackage();
            }
            else
            {
                LogBuildMessage("File " + pathSqlFile + " exists.");
                runVSDBCMD();
            }

            this.IsDatabaseDeployedSuccessfully.Set(this.ActivityContext, true);
        }

        private void runVSDBCMD()
        {
            string exeVSDBCMDPath = Path.Combine(this.SourcesDirectory.Get(this.ActivityContext) + @"\BuildProcessTemplates\VSDBCMDTemp", "vsdbcmd.exe");
            if (!File.Exists(exeVSDBCMDPath))
            {
                LogBuildError("Could not locate " + exeVSDBCMDPath + ". Please verify workspace mapping of this build definition.");
            }

            string exeVSDBCMDArguments = string.Format(@"/ManifestFile:{0}\{1}.deploymanifest /a:Deploy /cs:" + "\u0022" + "Data Source={2},{3};Integrated Security=false;user={4};password={5}" + "\u0022" + " /p:GenerateDropsIfNotInProject={6} /p:BlockIncrementalDeploymentIfDataLoss=False /p:TargetDatabase={7} /p:IgnorePermissions=True /p:GenerateDeployStateChecks=False /p:SingleUserMode=False /p:CheckNewConstraints=False /p:DeployDatabaseProperties=False /dd:+", this.BinariesDirectory.Get(this.ActivityContext), this.DatabaseName.Get(this.ActivityContext).Replace("\"", string.Empty), this.DatabaseServer.Get(this.ActivityContext), this.DatabaseServerPort.Get(this.ActivityContext), this.DeployUser.Get(this.ActivityContext), this.DeployPassword.Get(this.ActivityContext), this.AllowDrops.Get(this.ActivityContext), this.DatabaseName.Get(this.ActivityContext).Substring(0, this.DatabaseName.Get(this.ActivityContext).Length - 9));

            runProcess(exeVSDBCMDPath, exeVSDBCMDArguments);
        }

        private void runSqlPackage()
        {
            string exeSqlPackagePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFilesX86) + @"\Microsoft SQL Server\110\DAC\bin\", "SqlPackage.exe");
            if (!File.Exists(exeSqlPackagePath))
            {
                LogBuildError("Could not locate " + exeSqlPackagePath + ". Please download and install SQL Server Data Tools - November 2012 update at http://msdn.microsoft.com/en-us/jj650014");
                return;
            }

            string exeSqlPackageArguments = string.Format(@"/Action:Publish /SourceFile:{0}\{1}.dacpac /TargetConnectionString:" + "\u0022" + "Data Source={2},{3};Initial Catalog={4};User ID=l.Hudson;Password=Hudson_1804_L;Pooling=False" + "\u0022" + " /p:IncludeCompositeObjects=True", this.BinariesDirectory.Get(this.ActivityContext), this.DatabaseName.Get(this.ActivityContext).Replace("\"", string.Empty), this.DatabaseServer.Get(this.ActivityContext), this.DatabaseServerPort.Get(this.ActivityContext), this.DatabaseName.Get(this.ActivityContext).Substring(0, this.DatabaseName.Get(this.ActivityContext).Length - 9));

            runProcess(exeSqlPackagePath, exeSqlPackageArguments);
        }

        private void runProcess(string exePath, string exeArguments)
        {
            StringBuilder output = new StringBuilder();

            using (Process proc = new Process())
            {
                proc.StartInfo.FileName = exePath;
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.Arguments = exeArguments;
                
                this.LogBuildMessage("Running command " + proc.StartInfo.FileName);

                proc.Start();

                string errorStream = proc.StandardError.ReadToEnd();
                if (errorStream.Length > 0)
                {
                    output.Append(errorStream);
                    if (errorStream.StartsWith("warning"))
                    {
                        this.LogBuildWarning(errorStream);
                    }
                    else
                    {
                        this.LogBuildError(errorStream);
                    }
                }

                string outputStream = proc.StandardOutput.ReadToEnd();
                if (outputStream.Length > 0)
                {
                    output.Append(outputStream);
                    this.LogBuildMessage(outputStream);
                }

                proc.WaitForExit();

                if (proc.ExitCode != 0)
                {
                    this.LogBuildError(errorStream);
                }

                this.ActivityContext.SetValue(StdOutput, output.ToString());
            }
        }

        private void createFile()
        {
            XDocument xDocument = new XDocument();

            XNamespace aw = "http://schemas.microsoft.com/developer/msbuild/2003";

            xDocument.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            xDocument.Add(
                new XElement(aw + "Project", new XAttribute("ToolsVersion", "4.0"),
                    new XElement("PropertyGroup", 
                        new XElement("IncludeCompositeObjects", "True"),
                        new XElement("TargetDatabaseName", this.DatabaseName.Get(this.ActivityContext)),
                        new XElement("DeployScriptFileName", this.DatabaseName.Get(this.ActivityContext) + ".Database.sql"),
                        new XElement("TargetConnectionString", "Data Source=" + this.DatabaseServer.Get(this.ActivityContext) + ";Persist Security Info=True;User ID=l.Hudson;Pooling=False"),
                        new XElement("ProfileVersionNumber", "1")
            )));

            xDocument.Save(Path.Combine(this.BinariesDirectory.Get(this.ActivityContext),
                                                   (this.DatabaseName.Get(this.ActivityContext) + "." +
                                                    ".publish.xml")));
        }
    }
}
