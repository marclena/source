using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Contracts.ServiceLibrary.Compression
{
    public interface IZipService : IBaseActivityService
    {
        void Initialize(string compressedFile, List<string> includeFiles, List<string> ignoreFiles);
        //void Initialize(string sourcesDirectory, string project, string sourceFolderBinariesDirectory, List<string> ignoreFilesFilters);
    }
}
