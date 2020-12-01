using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.Library.SynchronizeContent
{
    public enum FtpSyncTypeEnum
    {
        MIRROR,
        LOCAL_WITH_DELETE,
        LOCAL_WITHOUT_DELETE,
        REMOTE_WITH_DELETE,
        REMOTE_WITHOUT_DELETE
    }
}
