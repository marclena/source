using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Contracts.ServiceLibrary.Validation
{
    public interface IAssemblyVersionValidationService
    {
        void Initialize(List<string> _projectList,
                                                string _sourcesDirectory,
                                                string _environment,
                                                IEnumerable<string> _assemblyExceptions,
                                                IEnumerable<string> _assembliesToProcess,
                                                string _fullPathLibAssemblyExceptions,
                                                string _fullPathFilteredAssemblies);

        void InternalExecute();

        bool AreVersionValid { get; set; }

        System.Collections.Generic.List<string> LogErrorList { get; }
        
        System.Collections.Generic.List<string> LogInformationList { get; }
        
        System.Collections.Generic.List<string> LogWarningList { get; }
    }
}
