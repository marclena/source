using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService.Models
{
    public class SyncActions
    {
        public SyncActions()
        {
            ToCreate = new List<DirectoryContent>();
            ToDelete = new List<DirectoryContent>();
            ToUpdate = new List<DirectoryContent>();
        }

        public List<DirectoryContent> ToCreate { get; set; }
        public List<DirectoryContent> ToDelete { get; set; }
        public List<DirectoryContent> ToUpdate { get; set; }
    }
}
