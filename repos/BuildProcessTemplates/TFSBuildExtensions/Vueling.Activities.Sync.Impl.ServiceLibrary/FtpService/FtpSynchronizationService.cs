using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TFSBuildExtensions.Library.SynchronizeContent;
using Vueling.Activities.Sync.Contracts.ServiceLibrary.Synchronization;
using Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService.Models;
using Vueling.Activities.Sync.Impl.ServiceLibrary.Helpers;
using Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.Compare;
using Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.FtpDiscovery;
using Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.LocalDiscovery;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService
{
    [RegisterServiceAttribute]
    public class FtpSynchronizationService : ISynchronizationService
    {
        private SynchronizeContentConfiguration synchronizeContentConfiguration;

        IFtpDirectoryService ftpDirectoryService;
        IFileDirectoryService fileDirectoryService;
        ICompareDirectoryService compareDirectoryService;

        public FtpSynchronizationService(SynchronizeContentConfiguration _synchronizeContentConfiguration,
                                            IFtpDirectoryService _ftpDirectoryService, IFileDirectoryService _fileDirectoryService, ICompareDirectoryService _compareDirectoryService)
        {
            synchronizeContentConfiguration = _synchronizeContentConfiguration;

            ftpDirectoryService = _ftpDirectoryService;
            fileDirectoryService = _fileDirectoryService;
            compareDirectoryService = _compareDirectoryService;
        }

        public void RunSynchronization()
        {
            if (!ftpDirectoryService.ExistsStartingFolder(synchronizeContentConfiguration.TargetSettings.FtpSettings))
            {
                CreateStartingFolder(synchronizeContentConfiguration.TargetSettings.FtpSettings);
            }

            var sourceContent = fileDirectoryService.GetDirectoryInfo(synchronizeContentConfiguration.SourceSettings.RootPath, synchronizeContentConfiguration.ExcludeContent);
            var targetContent = ftpDirectoryService.GetDirectoryInfo(synchronizeContentConfiguration.TargetSettings.FtpSettings, synchronizeContentConfiguration.ExcludeContent);

            var actions = compareDirectoryService.GetActions(sourceContent, targetContent);

            Apply(actions, synchronizeContentConfiguration.TargetSettings.FtpSettings);
        }

        private void Apply(SyncActions actions, FtpSettings ftpSettings)
        {
            if (synchronizeContentConfiguration.TargetSettings.FtpSettings.FtpSyncType == FtpSyncTypeEnum.REMOTE_WITH_DELETE)
            {
                Delete(actions.ToDelete, ftpSettings);
            }

            Create(actions.ToCreate, ftpSettings);

            //update
            Delete(actions.ToUpdate.Where(x => !x.IsDirectory).ToList(), ftpSettings);
            Create(actions.ToUpdate.Where(x => !x.IsDirectory).ToList(), ftpSettings);
        }

        private void Create(List<DirectoryContent> content, FtpSettings ftpSettings)
        {
            if (!content.Any()) { return; }

            var toCreateOrdered = GetContentOrderedByDepth(content);

            CreateDirectories(ftpSettings, toCreateOrdered);

            UploadFiles(content, ftpSettings);
        }

        private List<FtpContentDeleteModel> GetContentOrderedByDepth(List<DirectoryContent> content)
        {
            var toCreateOrdered = new List<FtpContentDeleteModel>();

            foreach (var item in content.Where(x => x.IsDirectory))
            {
                toCreateOrdered.Add(new FtpContentDeleteModel
                {
                    Depth = Regex.Matches(item.RelativePath, "/", RegexOptions.Compiled).Count,
                    FtpContent = item
                });
            }
            toCreateOrdered = toCreateOrdered.OrderBy(x => x.Depth).ToList();

            return toCreateOrdered;
        }

        private void UploadFiles(List<DirectoryContent> content, FtpSettings ftpSettings)
        {
            ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = System.Environment.ProcessorCount };

            Parallel.ForEach(content.Where(x => !x.IsDirectory), options, file =>
            {
                FtpWebRequest request = BuildFtpRequest(ftpSettings, file.RelativePath, WebRequestMethods.Ftp.UploadFile);

                //TODO: do something with response
                var response = ExecuteUploadAction(request, file);
            });
        }

        private void CreateDirectories(FtpSettings ftpSettings, List<FtpContentDeleteModel> toCreateOrdered)
        {
            foreach (var toCreate in toCreateOrdered)
            {
                FtpWebRequest request = BuildFtpRequest(ftpSettings, toCreate.FtpContent.RelativePath, WebRequestMethods.Ftp.MakeDirectory);

                //TODO: do something with response
                var response = ExecuteAction(request);
            }
        }

        private void CreateStartingFolder(FtpSettings ftpSettings)
        {
            FtpWebRequest request = BuildFtpRequest(ftpSettings, "", WebRequestMethods.Ftp.MakeDirectory);

            //TODO: do something with response
            var response = ExecuteAction(request);
        }


        private void Delete(List<DirectoryContent> ftpContent, FtpSettings ftpSettings)
        {
            if (!ftpContent.Any()) { return; }

            var toDeleteOrdered = new List<FtpContentDeleteModel>();
            //order by path depth
            foreach (var item in ftpContent.Where(x => x.IsDirectory))
            {
                toDeleteOrdered.Add(new FtpContentDeleteModel
                {
                    Depth = Regex.Matches(item.RelativePath, "/", RegexOptions.Compiled).Count,
                    FtpContent = item
                });
            }
            toDeleteOrdered = toDeleteOrdered.OrderByDescending(x => x.Depth).ToList();

            //delete files
            foreach (var item in ftpContent.Where(x => !x.IsDirectory))
            {
                FtpWebRequest request = BuildFtpRequest(ftpSettings, item.RelativePath, WebRequestMethods.Ftp.DeleteFile);

                var response = ExecuteAction(request);
            }

            foreach (var toDelete in toDeleteOrdered)
            {
                FtpWebRequest request = BuildFtpRequest(ftpSettings, toDelete.FtpContent.RelativePath, WebRequestMethods.Ftp.RemoveDirectory);

                var response = ExecuteAction(request);
            }
        }

        private string ExecuteUploadAction(FtpWebRequest request, DirectoryContent content)
        {
            request.UseBinary = true;

            using (Stream requestStream = request.GetRequestStream())
            {
                var fs = new FileStream(content.AccessPath, FileMode.Open, FileAccess.Read);

                fs.CopyTo(requestStream);

                fs.Flush();
            }

            string response;
            using (FtpWebResponse ftpResponse = (FtpWebResponse)request.GetResponse())
            {
                response = ftpResponse.StatusDescription;
            }

            return response;
        }

        private FtpWebRequest BuildFtpRequest(FtpSettings ftpSettings, string path, string ftpMethod)
        {
            var uriBuilder = new UriBuilder("ftp", ftpSettings.ServerAddress.Replace("ftp://", ""), 21, ftpSettings.StartingPath + path);

            var requestUri = uriBuilder.Uri;

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(requestUri);
            request.Method = ftpMethod;
            request.Credentials = new NetworkCredential(ftpSettings.User, ftpSettings.Password);
            request.EnableSsl = ftpSettings.EnableSSL;
            request.UsePassive = true;
            request.Proxy = null;

            request.UseBinary = true;

            return request;
        }

        private string ExecuteAction(FtpWebRequest request)
        {
            string response = string.Empty;

            FtpWebResponse ftpResponse = Retry.Do<FtpWebResponse>(() => (FtpWebResponse)request.GetResponse(), TimeSpan.FromSeconds(5), 3);

            var lines = new List<string>();

            var directoriesInfo = new List<DirectoryContent>();

            using (Stream responseStream = ftpResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);

                response = reader.ReadToEnd();
            }

            return response;
        }
    }
}
