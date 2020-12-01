using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Vueling.Activities.Configuration;

namespace TFSBuildExtensions.TeamFoundationServer
{

    public class TeamFoundationServerConnection
    {
        private TfsTeamProjectCollection _server;
        private string tfsUrl;
        private string tfsUser;
        private string tfsPassword;
        private string domain;
        private string teamProject;

        public TeamFoundationServerConnection()
        {
            tfsUrl = Configuration.tfsurl;
            tfsUser = Configuration.tfsuser;
            tfsPassword = Configuration.tfspassword;
            domain = Configuration.domain;
            teamProject = Configuration.teamProject;

            ConnectToTFS();
        }

        private void ConnectToTFS()
        {
            NetworkCredential myNetCredentials = new NetworkCredential(tfsUser, tfsPassword, domain);
            ICredentials myCredentials = (ICredentials)myNetCredentials;

            Uri tfsUri = new Uri(tfsUrl);
            _server = new TfsTeamProjectCollection(tfsUri, myCredentials);
            _server.EnsureAuthenticated();
        }

        public TfsTeamProjectCollection Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public string TeamProject
        {
            get { return teamProject; }
            set { teamProject = value; }
        }
    }
}
