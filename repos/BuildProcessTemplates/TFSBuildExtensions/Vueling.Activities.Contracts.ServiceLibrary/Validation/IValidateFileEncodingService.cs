using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Contracts.ServiceLibrary.Validation
{
    public interface IValidateFileEncodingService : IBaseActivityService
    {
        void Initialize(List<string> filesPath);
    }
}
