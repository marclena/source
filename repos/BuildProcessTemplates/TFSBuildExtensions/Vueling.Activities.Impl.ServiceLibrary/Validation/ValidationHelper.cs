using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Vueling.Activities.Impl.ServiceLibrary.Validation
{
    public static class ValidationHelper
    {
        public static bool isLibAssemblyExceptionForThisProject(XDocument libAssemblyExceptions, string project, string assemblyName)
        {
            IEnumerable<XElement> genericAssemblyExceptions = libAssemblyExceptions.Element("AssemblyExceptions").Element("GenericExclusions").Elements("Assembly").Where(f => f.Value.ToLower() == assemblyName.ToLower());

            if (genericAssemblyExceptions.Count() > 0)
            {
                return true;
            }

            IEnumerable<XElement> elementProject = libAssemblyExceptions.Element("AssemblyExceptions").Element("Projects").Elements("Project").Where(r => r.Attribute("Name").Value == project);

            IEnumerable<XElement> elementAssembly = elementProject.Elements("Assembly").Where(f => f.Value.ToLower() == assemblyName.ToLower());

            if (elementAssembly.Count() > 0)
            {
                return true;
            }

            return false;
        }

        public static bool IsException(IEnumerable<string> assemblyExceptions, string project)
        {
            foreach (var exception in assemblyExceptions)
            {
                if (exception.ToString().Equals(project))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
