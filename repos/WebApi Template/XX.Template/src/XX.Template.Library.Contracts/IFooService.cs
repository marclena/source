using System.Threading.Tasks;
using XX.Template.Core.Extensions;
using XX.Template.Library.Contracts.Dto;

namespace XX.Template.Library.Contracts
{
    /// <summary>
    /// </summary>
    public interface IFooService
    {
        /// <summary>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<OperationResult<FooRsDto>> DoSomethingAsync(FooRqDto data);
    }
}