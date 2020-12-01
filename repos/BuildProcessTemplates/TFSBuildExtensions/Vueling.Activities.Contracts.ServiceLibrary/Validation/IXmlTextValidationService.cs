using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Contracts.ServiceLibrary.Validation
{
    public interface IXmlTextValidationService : IBaseActivityService
    {
        void Initialize(string xmlFilePath);
    }
}