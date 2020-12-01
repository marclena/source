using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary;

namespace Vueling.Activities.Impl.ServiceLibrary
{
    public abstract class BaseActivityService : IBaseActivityService
    {
        int result;
        List<string> informationMessageList = new List<string>();
        List<string> warningMessageList = new List<string>();
        List<string> errorMessageList = new List<string>();

        public abstract void InternalExecute();

        public int Result
        {
            get { return result; }
            set { result = value; }
        }

        public List<string> InformationMessageList
        {
            get { return informationMessageList; }
            set { informationMessageList = value; }
        }

        public List<string> WarningMessageList
        {
            get { return warningMessageList; }
            set { warningMessageList = value; }
        }

        public List<string> ErrorMessageList
        {
            get { return errorMessageList; }
            set { errorMessageList = value; }
        }
    }
}
