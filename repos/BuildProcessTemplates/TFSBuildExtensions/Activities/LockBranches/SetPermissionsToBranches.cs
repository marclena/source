using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using System.ComponentModel;
using System.Activities;
using Microsoft.TeamFoundation.VersionControl.Client;
using TFSBuildExtensions.Helper;

namespace TFSBuildExtensions.EnvironmentManagement.LockBranches
{

    [BuildActivity(HostEnvironmentOption.All)]
    public class SetPermissionsToBranches : BaseCodeActivity
    {
        #region Properties
        [Description("Define if allows or denies permissions")]
        [RequiredArgument]
        public InArgument<string> PermissionSetting { get; set; }

        [Description("Environment where permissions are applied (INT, PRE, PRO)")]
        [RequiredArgument]
        public InArgument<string> EnvironmentBranch { get; set; }

        [Description("List of permissions applied")]
        [RequiredArgument]
        public InArgument<string[]> Permissions { get; set; }

        [Description("VersionControlServer")]
        [RequiredArgument]
        public InArgument<VersionControlServer> VersionControlServer { get; set; }

        [Description("Identities to apply permissions")]
        [RequiredArgument]
        public InArgument<string[]> Identities { get; set; }

        [Description("List of directories excluded from apply those permissions")]
        public InArgument<List<string>> DirectoryListExclusions { get; set; }

        [Description("List of directories to apply those permissions")]
        public InArgument<string> DirectorytoApplyPermissions { get; set; }

        #endregion

        protected override void InternalExecute()
        {
            ApplyPermissions();
        }

        private void ApplyPermissions()
        {
            VersionControlServer _vcs = this.VersionControlServer.Get(this.ActivityContext);
            var names = new List<string>();
            ItemSet items = null;

            if (String.IsNullOrEmpty(DirectorytoApplyPermissions.Get(this.ActivityContext)))
            {
                items = _vcs.GetItems(@"$\Vueling\*", RecursionType.None);
            } else
            {
                items = _vcs.GetItems(@"$\Vueling\" + DirectorytoApplyPermissions.Get(this.ActivityContext), RecursionType.None);                
            }

            foreach (var projects in items.Items.Select(item => _vcs.GetItems(item.ServerItem + @"\" + this.EnvironmentBranch.Get(this.ActivityContext), RecursionType.None)))
            {
                names.AddRange(from proj in projects.Items select proj.ServerItem);
            }

            foreach (var path in names)
            {
                if (!IsAnException(path))
                {
                    ApplyPermissionsToIdentities(path, _vcs);
                }
            }

        }

        private void ApplyPermissionsToIdentities(string path, VersionControlServer _vcs)
        {
            List<SecurityChange> changes = new List<SecurityChange>();
            string[] _permissions = this.Permissions.Get(this.ActivityContext);
            string[] _identities = this.Identities.Get(this.ActivityContext);

            foreach (string _identity in _identities)
            {
                changes.Clear();
                if (this.PermissionSetting.Get(this.ActivityContext).Equals("Allow"))
                {
                    changes.Add(new PermissionChange(path, _identity, _permissions, null, null));
                    LogBuildMessage("Permissions allowance " + _permissions.listToString() + " has been applied to branch " + path + " for identity " + _identity + " successfully.", BuildMessageImportance.Normal);
                }
                else if (this.PermissionSetting.Get(this.ActivityContext).Equals("Deny"))
                {
                    changes.Add(new PermissionChange(path, _identity, null, _permissions, null));
                    LogBuildMessage("Permissions deny " + _permissions.listToString() + " has been applied to branch " + path + " for identity " + _identity + " successfully.", BuildMessageImportance.Normal);
                }
                SecurityChange[] actualChanges = _vcs.SetPermissions(changes.ToArray());
            }
        }


        private bool IsAnException(string serverItem)
        {
            if (DirectoryListExclusions.Get(this.ActivityContext) != null)
            {
                List<string> dirExclusions = DirectoryListExclusions.Get(this.ActivityContext);

                foreach (var dirExclusion in dirExclusions)
                {
                    if (serverItem.Contains(dirExclusion))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
