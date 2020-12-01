using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TFSBuildExtensions.Helper;

namespace TFSBuildExtensions.AWS
{
    public class CreateXmlFileDefinition
    {
        private string buildDefinitionName;
        private string buildNumber;
        private string webSiteName;
        private string binariesDirectory;
        private List<string> projectsToIncludeInPackage;
        private bool recreateApplicationParam;

        public CreateXmlFileDefinition(string _buildDefinitionName, string _buildNumber, string _webSite, string _binariesDirectory, List<string> _projectsToIncludeInPackage, bool _recreateApplication)
        {
            buildDefinitionName = _buildDefinitionName;
            buildNumber = _buildNumber;
            webSiteName = _webSite;
            binariesDirectory = _binariesDirectory;
            projectsToIncludeInPackage = _projectsToIncludeInPackage;
            recreateApplicationParam = _recreateApplication;
        }
        
        public void createXmlFile()
        {
            XmlDocument deploy = new XmlDocument();

            XmlNode docNode = deploy.CreateXmlDeclaration("1.0", "UTF-8", null);
            deploy.AppendChild(docNode);

            XmlNode manifest = deploy.CreateElement("Manifest");
            deploy.AppendChild(manifest);

            XmlNode component = deploy.CreateElement("Component");
            component.InnerText = Extensions.GetComponentName(buildDefinitionName);
            manifest.AppendChild(component);

            XmlNode version = deploy.CreateElement("Version");
            version.InnerText = buildNumber;
            manifest.AppendChild(version);

            XmlNode applications = deploy.CreateElement("Applications");

            if (!Directory.Exists(binariesDirectory + @"\AWS"))
            {
                Directory.CreateDirectory(binariesDirectory + @"\AWS");

                foreach (string application in projectsToIncludeInPackage)
                {
                    AddApplicationToXml(deploy, applications, application);
                }

                manifest.AppendChild(applications);

                deploy.Save(Path.Combine(binariesDirectory + @"\AWS", "deploy.xml"));
            }
        }

        internal void AddApplicationToXml(XmlDocument deploy, XmlNode applications, string application)
        {
            if (application.ToLower().StartsWith("skysales"))
            {
                AddSkySales(deploy, applications);
            }
            else if (application.ToLower().StartsWith("vueling.staticcontent"))
            {
                AddStaticContent(deploy, applications);
            }
            else
            {
                switch (application.Substring(application.LastIndexOf(".")).ToLower())
                {
                    case ".config":
                        AddConfiguration(deploy, applications);
                        break;
                    case ".windowsservice":
                        AddWindowsService(deploy, applications, application);
                        break;
                    case ".webservice":
                        AddWebApplication(deploy, applications, application, "webservice");
                        break;
                    case ".webui":
                        AddWebApplication(deploy, applications, application, "webui");
                        break;
                    case ".webapi":
                        AddWebApplication(deploy, applications, application, "webapi");
                        break;
                    case ".database":
                        AddDatabase(deploy, applications, application);
                        break;
                }
            }
        }

        internal void AddConfiguration(XmlDocument deploy, XmlNode applications)
        {
            XmlNode applicationNode = deploy.CreateElement("Application");

            XmlAttribute name = deploy.CreateAttribute("Name");
            name.InnerText = "Vueling.Configuration.Config";
            applicationNode.Attributes.Append(name);

            XmlNode type = deploy.CreateElement("Type");
            type.InnerText = "Configuration";
            applicationNode.AppendChild(type);

            XmlNode staticEndpoints = deploy.CreateElement("StaticEndpoints");

            XmlNode staticEndpoint = deploy.CreateElement("Endpoint");
            XmlAttribute nameEndPoint = deploy.CreateAttribute("Name");
            nameEndPoint.InnerText = "Vueling.StaticContent";
            staticEndpoint.Attributes.Append(nameEndPoint);
            staticEndpoints.AppendChild(staticEndpoint);

            XmlNode staticEndpointBackend = deploy.CreateElement("Endpoint");
            XmlAttribute nameEndPointBackend = deploy.CreateAttribute("Name");
            nameEndPointBackend.InnerText = "Vueling.StaticContentBackend";
            staticEndpointBackend.Attributes.Append(nameEndPointBackend);
            staticEndpoints.AppendChild(staticEndpointBackend);

            XmlNode staticEndpointMICEWEB = deploy.CreateElement("Endpoint");
            XmlAttribute nameEndPointMICEWEB = deploy.CreateAttribute("Name");
            nameEndPointMICEWEB.InnerText = "Vueling.StaticContent.MICEWEB";
            staticEndpointMICEWEB.Attributes.Append(nameEndPointMICEWEB);
            staticEndpoints.AppendChild(staticEndpointMICEWEB);

            applicationNode.AppendChild(staticEndpoints);

            applications.AppendChild(applicationNode); 
        }

        internal void AddSkySales(XmlDocument deploy, XmlNode applications)
        {
            XmlNode applicationNode = deploy.CreateElement("Application");

            XmlAttribute name = deploy.CreateAttribute("Name");
            name.InnerText = "SkySales";
            applicationNode.Attributes.Append(name);

            XmlNode type = deploy.CreateElement("Type");
            type.InnerText = "WebUI";
            applicationNode.AppendChild(type);

            XmlNode website = deploy.CreateElement("WebSite");
            website.InnerText = "SkySales";
            applicationNode.AppendChild(website);

            applications.AppendChild(applicationNode);
        }

        internal void AddStaticContent(XmlDocument deploy, XmlNode applications)
        {
            XmlNode applicationNode = deploy.CreateElement("Application");

            XmlAttribute name = deploy.CreateAttribute("Name");
            name.InnerText = "StaticContent";
            applicationNode.Attributes.Append(name);

            XmlNode type = deploy.CreateElement("Type");
            type.InnerText = "StaticContent";
            applicationNode.AppendChild(type);

            XmlNode website = deploy.CreateElement("WebSite");
            website.InnerText = webSiteName;
            applicationNode.AppendChild(website);

            applications.AppendChild(applicationNode);
        }

        internal void AddWindowsService(XmlDocument deploy, XmlNode applications, string application)
        {
            XmlNode applicationNode = deploy.CreateElement("Application");

            XmlAttribute name = deploy.CreateAttribute("Name");
            name.InnerText = application;
            applicationNode.Attributes.Append(name);

            XmlNode type = deploy.CreateElement("Type");
            type.InnerText = "WindowsService";
            applicationNode.AppendChild(type);

            applications.AppendChild(applicationNode);
        }

        internal void AddWebApplication(XmlDocument deploy, XmlNode applications, string application, string typeApplication)
        {
            XmlNode applicationNode = deploy.CreateElement("Application");

            XmlAttribute name = deploy.CreateAttribute("Name");
            name.InnerText = application;
            applicationNode.Attributes.Append(name);

            XmlNode type = deploy.CreateElement("Type");
            type.InnerText = typeApplication;
            applicationNode.AppendChild(type);

            XmlNode website = deploy.CreateElement("WebSite");
            website.InnerText = (webSiteName.Equals("Vueling.Corporative.WebUI")?"Vueling.Corporative.WebUI":webSiteName);
            applicationNode.AppendChild(website);

            XmlNode recreateApplication = deploy.CreateElement("RecreateApplication");
            recreateApplication.InnerText = (recreateApplicationParam ? "True" : "False");
            applicationNode.AppendChild(recreateApplication);

            applications.AppendChild(applicationNode);
        }

        internal void AddDatabase(XmlDocument deploy, XmlNode applications, string application)
        {
            XmlNode applicationNode = deploy.CreateElement("Application");

            XmlAttribute name = deploy.CreateAttribute("Name");
            name.InnerText = application;
            applicationNode.Attributes.Append(name);

            XmlNode type = deploy.CreateElement("Type");
            type.InnerText = "Database";
            applicationNode.AppendChild(type);

            applications.AppendChild(applicationNode);
        }
    }
}
