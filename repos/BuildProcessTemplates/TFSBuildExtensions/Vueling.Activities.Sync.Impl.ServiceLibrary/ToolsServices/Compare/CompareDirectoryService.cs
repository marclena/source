using Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.Compare
{
    [RegisterService]
    public class CompareDirectoryService : ICompareDirectoryService
    {
        public SyncActions GetActions(List<DirectoryContent> source, List<DirectoryContent> target)
        {
            var actions = new SyncActions();

            actions.ToCreate = source.Except(target, new DirectoryContentByFullPathComparer()).ToList();
            actions.ToDelete = target.Except(source, new DirectoryContentByFullPathComparer()).ToList();

            AddElementsForUpdate(source, target, actions);

            return actions;
        }

        private void AddElementsForUpdate(List<DirectoryContent> source, List<DirectoryContent> target, SyncActions actions)
        {
            var sourceToCheckForUpdate = source.Except(actions.ToCreate).ToList();
            var targetToCheckForUpdate = target.Except(actions.ToDelete).ToList();

            var toUpdate = new ConcurrentBag<DirectoryContent>();

            ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = System.Environment.ProcessorCount };

            Parallel.ForEach(sourceToCheckForUpdate, options, sourceItem =>
            {
                var targetItem = targetToCheckForUpdate.FirstOrDefault(x => x.RelativePath == sourceItem.RelativePath);

                if (sourceItem.LastWrite > targetItem.LastWrite) { toUpdate.Add(sourceItem); }
            });

            if (!toUpdate.Any()) { return; }

            actions.ToUpdate = toUpdate.ToList();
        }
    }
}
