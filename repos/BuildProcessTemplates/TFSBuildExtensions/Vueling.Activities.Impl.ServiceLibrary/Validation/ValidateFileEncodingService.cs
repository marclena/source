using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.Validation;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Impl.ServiceLibrary.Validation
{
    [RegisterServiceAttribute]
    public class ValidateFileEncodingService : BaseActivityService, IValidateFileEncodingService
    {
        private List<string> filesPath;

        public void Initialize(List<string> _filesPath)
        {
            filesPath = _filesPath;
        }

        public override void InternalExecute()
        {
            var utf8WithBom = new UTF8Encoding(true);

            foreach (var filePath in filesPath)
            {
                if (File.Exists(filePath))
                {
                    using (var reader = new StreamReader(filePath, utf8WithBom))
                    {
                        reader.Read();
                        if (Equals(reader.CurrentEncoding, utf8WithBom))
                        {
                            this.WarningMessageList.Add("Error encoding with file " + filePath + ". Current encoding is  UTF8 with BOM.");
                            this.Result = 0;
                        }
                    }
                }
            }
        }
    }
}
