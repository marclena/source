using System.ServiceModel;

namespace Vueling.XXX.WCF.WebService
{
    [ServiceContract]
    public interface IAutomaticTasklingProcesses
    {
        [OperationContract]
        void CreationBlocksExample();
    }
}
