using Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Vueling.Activities.Sync.Impl.ServiceLibrary.Helpers;
using Vueling.Extensions.Library.DI;
using System;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.LocalDiscovery
{
    [RegisterService]
    public class FileDirectoryService : IFileDirectoryService
    {
        public List<DirectoryContent> GetDirectoryInfo(string path, List<string> excludeContent)
        {
            var content = new List<DirectoryContent>();

            if (path.IsMatch(excludeContent)) { return content; }

            IterateRecursivelyOnFolders(path, content, excludeContent);

            NormalizeComparePath(path, content);

            return content;
        }

        private void IterateRecursivelyOnFolders(string path, List<DirectoryContent> content, List<string> excludeContent)
        {
            DirectoryInfo diTop = new DirectoryInfo(path);

            if (diTop.FullName.IsMatch(excludeContent)) { return; }

            ExtractFiles(path, content, diTop, excludeContent);

            ExtractDirectories(path, content, diTop, excludeContent);
        }

        private void ExtractFiles(string path, List<DirectoryContent> content, DirectoryInfo diTop, List<string> excludeContent)
        {
            foreach (var file in diTop.EnumerateFiles())
            {
                var newContent = new DirectoryContent();

                if (file.Name.IsMatch(excludeContent)) { continue; }

                content.Add(new DirectoryContent
                {
                    AccessPath = string.Format("{0}\\{1}", path, file.Name),
                    FileSize = file.Length.ToString(),
                    RelativePath = file.FullName,
                    IsDirectory = false,
                    LastWrite = GetUniversalDateTimeWithoutSeconds(file.LastWriteTime),
                });
            }
        }

        private void ExtractDirectories(string path, List<DirectoryContent> content, DirectoryInfo diTop, List<string> excludeContent)
        {
            foreach (var item in diTop.EnumerateDirectories())
            {
                if (item.FullName.IsMatch(excludeContent)) { continue; }

                content.Add(new DirectoryContent
                {
                    AccessPath = string.Format("{0}\\{1}", path, item.Name),
                    FileSize = "0",
                    RelativePath = item.FullName,
                    IsDirectory = true,
                    LastWrite = GetUniversalDateTimeWithoutSeconds(item.LastWriteTime)
                });

                IterateRecursivelyOnFolders(item.FullName, content, excludeContent);
            }
        }

        private DateTime GetUniversalDateTimeWithoutSeconds(DateTime dateTime)
        {
            var universal = dateTime.ToUniversalTime();

            return new DateTime(universal.Year, universal.Month, universal.Day, universal.Hour, universal.Minute, 0);
        }

        private static void NormalizeComparePath(string path, List<DirectoryContent> content)
        {
            ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = System.Environment.ProcessorCount };

            Parallel.ForEach(content, options, item =>
            {
                item.RelativePath = item.RelativePath.Replace(path, "").Replace(@"\", "/");
            });
        }
    }
}
