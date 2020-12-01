using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Contracts.ServiceLibrary.StaticContentHelper
{
    public interface IStaticContentHelperGenerator : IBaseActivityService
    {
        void Initialize(string sourceDirectory, string targetDirectory, string staticContentWebUINamespace);
    }
}
