using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Contracts.ServiceLibrary
{
    public interface IBaseActivityService
    {

        void InternalExecute();

        int Result { get; set; }

        List<string> InformationMessageList { get; set; }

        List<string> WarningMessageList { get; set; }

        List<string> ErrorMessageList { get; set; }
    }
}
