using System.Collections.Generic;
using System.Linq;

namespace XX.Template.Core.Extensions
{
    public class OperationResult<TResult>
    {
        public OperationResult()
        {
            Errors = new List<ErrorResult>();
        }
        public TResult Result { get; set; }
        public List<ErrorResult> Errors { get; set; }
        public bool HasErrors => Errors != null && Errors.Any();

    }
}