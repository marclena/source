using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Build.Common;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFSBuildExtensions.LockBuilds
{
    public class SetPermissionsBuildDefinition : BaseCodeActivity
    {
        [Description("Define if allows or denies permissions")]
        [RequiredArgument]
        public InArgument<string> PermissionSetting { get; set; }

        [Description("Identities to apply permissions")]
        [RequiredArgument]
        public InArgument<string[]> Identities { get; set; }

        /*
         * Available permissions. Extracted from library Assembly: Microsoft.TeamFoundation.Build.Common, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
            // Runtime: v2.0.50727
            // Architecture: MSIL
         * public static class BuildPermissions
	        {
		        public static readonly int ViewBuilds = 1;
		        public static readonly int EditBuildQuality = 2;
		        public static readonly int RetainIndefinitely = 4;
		        public static readonly int DeleteBuilds = 8;
		        public static readonly int ManageBuildQualities = 16;
		        public static readonly int DestroyBuilds = 32;
		        public static readonly int UpdateBuildInformation = 64;
		        public static readonly int QueueBuilds = 128;
		        public static readonly int ManageBuildQueue = 256;
		        public static readonly int StopBuilds = 512;
		        public static readonly int ViewBuildDefinition = 1024;
		        public static readonly int EditBuildDefinition = 2048;
		        public static readonly int DeleteBuildDefinition = 4096;
		        public static readonly int OverrideBuildCheckInValidation = 8192;
		        public static readonly int AdministerBuildPermissions = 16384;
	        }
        */
        [Description("List of build permissions to apply")]
        public InArgument<List<int>> Permissions { get; set; }

        [Description("Pattern to query builds. E.g.: '.PRO.'")]
        public InArgument<IEnumerable<string>> Pattern { get; set; }

        [Description("Build definition names excluded from pattern.")]
        public InArgument<List<string>> BuildExclusions { get; set; }

        [Description("Return True or False whether permissiones were applied successfully or not.")]
        public InOutArgument<bool> PermissionsAppliedSuccessfully { get; set; }

        private TFSBuildExtensions.TeamFoundationServer.TeamFoundationServerConnection tfsInstance;

        protected override void InternalExecute()
        {
            tfsInstance = new TFSBuildExtensions.TeamFoundationServer.TeamFoundationServerConnection();
            ApplyPermissionsToIdentities();
            this.PermissionsAppliedSuccessfully.Set(this.ActivityContext, true);
        }

        private void ApplyPermissionsToIdentities()
        {
            List<SecurityChange> changes = new List<SecurityChange>();
            string[] _identities = this.Identities.Get(this.ActivityContext);

            IBuildServer buildServer = (IBuildServer)tfsInstance.Server.GetService(typeof(IBuildServer));
            ISecurityService securityService = tfsInstance.Server.GetService<ISecurityService>();
            ICommonStructureService cssService = tfsInstance.Server.GetService<ICommonStructureService>();
            IIdentityManagementService imService = tfsInstance.Server.GetService<IIdentityManagementService>();

            var builds = buildServer.QueryBuildDefinitions(tfsInstance.TeamProject, QueryOptions.Definitions);

            foreach (IBuildDefinition build in builds)
            {

                if (IsPatternCompliance(build.Name) || IsABuildExclusion(build.Name))
                {
                    foreach(string identity in this.Identities.Get(this.ActivityContext))
                    {
                        TeamFoundationIdentity groupIdentity = imService.ReadIdentity(
                                                IdentitySearchFactor.AccountName, identity,
                                                MembershipQuery.Direct, ReadIdentityOptions.None);

                        SecureBuild(build, groupIdentity, securityService, cssService);

                    }
                }
            }
        }

        private void SecureBuild(IBuildDefinition build, TeamFoundationIdentity groupIdentity,
                                 ISecurityService securityService, ICommonStructureService cssService)
        {
            SecurityNamespace buildSecurity = securityService.GetSecurityNamespace(BuildSecurity.BuildNamespaceId);

            // Check for null 
            // (build security would only be null if we are talking to an older server)
            if (buildSecurity != null)
            {
                // Team project level permissions use this token 
                // (these permissions are found on the Security menu off the Builds node in Team Explorer)
                String tokenTeamProject = LinkingUtilities.DecodeUri(
                    cssService.GetProjectFromName(build.TeamProject).Uri).ToolSpecificId;

                // Definition level permissions use this token (Security menu on a BuildDefinition in Team Explorer)
                String tokenDefinition =
                    tokenTeamProject + "/" +
                    LinkingUtilities.DecodeUri(build.Uri.AbsoluteUri).ToolSpecificId;

                foreach(var permission in this.Permissions.Get(this.ActivityContext))
                {
                    if (this.PermissionSetting.Get(this.ActivityContext).Equals("Allow"))
                    {
                        buildSecurity.SetPermissions(
                            tokenDefinition, groupIdentity.Descriptor,
                            permission, 0, true);
                    }
                    else
                    {
                        buildSecurity.SetPermissions(
                            tokenDefinition, groupIdentity.Descriptor, 0,
                            permission, false);
                    }

                    StringBuilder buildMessage = new StringBuilder();
                    buildMessage.Append("Permissions to build " + build.Name);
                    buildMessage.Append(" for user ");
                    buildMessage.Append(groupIdentity.DisplayName);
                    buildMessage.Append(" set to ");
                    buildMessage.Append(this.PermissionSetting.Get(this.ActivityContext));
                    buildMessage.Append(".");
                    LogBuildMessage(buildMessage.ToString());
                }
            }
        }

        private bool IsPatternCompliance(string buildDefinitionName)
        {
            foreach (var pattern in this.Pattern.Get(this.ActivityContext))
            {
                if (pattern.ToString().Equals("*"))
                {
                    return true;
                }
                if (buildDefinitionName.Contains(pattern.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsABuildExclusion(string buildName)
        {
            if (BuildExclusions.Get(this.ActivityContext) != null)
            {
                List<string> buildExclusions = BuildExclusions.Get(this.ActivityContext);

                foreach (var buildExclusion in buildExclusions)
                {
                    if (buildName.Contains(buildExclusion))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
