using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService.Models
{
    public class FtpContentDeleteModel
    {
        public DirectoryContent FtpContent { get; set; }
        public int Depth { get; set; }
    }
}
