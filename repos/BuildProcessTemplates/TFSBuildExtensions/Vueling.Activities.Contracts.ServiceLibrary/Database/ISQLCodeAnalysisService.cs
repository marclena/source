using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Contracts.ServiceLibrary.Database
{
    public interface ISQLCodeAnalysisService
    {
        void Initialize(string pathScripFiles);

        double Complexity { get; set; }

        void InternalExecute();

    }
}
