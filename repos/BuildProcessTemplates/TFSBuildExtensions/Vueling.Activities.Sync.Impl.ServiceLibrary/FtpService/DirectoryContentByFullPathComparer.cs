using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService.Models;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService
{
    public class DirectoryContentByFullPathComparer : IEqualityComparer<DirectoryContent>
    {
        public bool Equals(DirectoryContent source, DirectoryContent target)
        {
            return source.RelativePath.Equals(target.RelativePath);
        }

        public int GetHashCode(DirectoryContent obj)
        {
            return 0;
        }
    }
}
