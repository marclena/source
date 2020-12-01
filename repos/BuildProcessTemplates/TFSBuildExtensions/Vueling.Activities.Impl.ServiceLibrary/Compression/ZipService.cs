using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.Compression;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Impl.ServiceLibrary.Compression
{
    [RegisterService]
    public class ZipService : BaseActivityService, IZipService
    {
        private string _resultFile;
        private List<string> _includeFiles;
        private List<string> _ignoreFilesFilters;

        public void Initialize(string compressedFile,
            List<string> includeFiles,
            List<string> ignoreFiles)
        {
            _resultFile = compressedFile;
            _includeFiles = includeFiles;
            _ignoreFilesFilters = ignoreFiles;
        }
        public override void InternalExecute()
        {
            ZipFolder(_resultFile, _includeFiles, _ignoreFilesFilters);
        }

        private bool IsDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return attr.HasFlag(FileAttributes.Directory);
        }

        private List<FileToCompress> GetAllFilesToCompressRecursive(string pathToCompress, List<string> ignoreFiles, string originalPath)
        {
            List<FileToCompress> allFilesToCompress = new List<FileToCompress>();
            if (IsDirectory(pathToCompress))
            {
                var files = Directory.EnumerateFiles(pathToCompress);
                foreach (var ignore in ignoreFiles)
                {
                    var ignored = Directory.EnumerateFiles(pathToCompress, ignore);
                    files = files.Except(ignored);
                }
                allFilesToCompress.AddRange(files.Select(f => new FileToCompress(f, originalPath)).ToList());
                var directories = Directory.EnumerateDirectories(pathToCompress);
                foreach (var ignore in ignoreFiles)
                {
                    var ignored = Directory.EnumerateDirectories(pathToCompress, ignore);
                    directories = directories.Except(ignored);
                }
                if (directories.Any())
                {
                    foreach (var directory in directories)
                    {
                        allFilesToCompress.AddRange(GetAllFilesToCompressRecursive(directory, ignoreFiles, originalPath));
                    }
                }
            }
            else
            {
                allFilesToCompress.Add(new FileToCompress(pathToCompress, originalPath));
            }
            return allFilesToCompress;
        }

        private List<FileToCompress> GetAllFilesToCompress(List<string> filesToCompress, List<string> ignoreFiles)
        {
            List<FileToCompress> allFilesToCompress = new List<FileToCompress>();
            foreach (var pathToCompress in filesToCompress)
            {
                if (!File.Exists(pathToCompress) && !Directory.Exists(pathToCompress))
                {
                    int lastIndexOfPath = pathToCompress.LastIndexOf(@"\");
                    if (lastIndexOfPath >= 0 && lastIndexOfPath < pathToCompress.Length - 1)
                    {
                        string path = pathToCompress.Substring(0, lastIndexOfPath);
                        string wildCard = pathToCompress.Substring(lastIndexOfPath + 1);
                        string[] files = Directory.GetFiles(path, wildCard, SearchOption.TopDirectoryOnly);
                        string[] folders = Directory.GetDirectories(path, wildCard, SearchOption.TopDirectoryOnly);
                        if (files != null && files.Any())
                        {
                            foreach (var file in files)
                            {
                                allFilesToCompress.Add(new FileToCompress(file, path));
                            }
                        }
                        if (folders != null && folders.Any())
                        {
                            foreach (var folder in folders)
                            {
                                allFilesToCompress.AddRange(GetAllFilesToCompressRecursive(folder, ignoreFiles, path));
                            }
                        }
                    }
                }
                else
                {
                    if (IsDirectory(pathToCompress))
                    {
                        allFilesToCompress.AddRange(GetAllFilesToCompressRecursive(pathToCompress, ignoreFiles, pathToCompress));
                    }
                    else
                    {
                        string folderPath = null;
                        int lastIndexOfPath = pathToCompress.LastIndexOf(@"\");
                        if (lastIndexOfPath >= 0)
                        {
                            folderPath = pathToCompress.Substring(0, lastIndexOfPath);
                        }
                        allFilesToCompress.Add(new FileToCompress(pathToCompress, folderPath));
                    }
                }
            }
            return allFilesToCompress;
        }

        private void EnsureZipFolder(string zipPath)
        {
            string folder = "";
            int indexOfPath = zipPath.LastIndexOf("\\");
            if (indexOfPath >= 0)
            {
                folder = zipPath.Substring(0, indexOfPath);
                Directory.CreateDirectory(folder);
            }
        }

        private void ZipFolder(string outPathZipName, List<string> filesToCompress, List<string> ignoreFiles)
        {

            EnsureZipFolder(outPathZipName);
            FileStream fsOut = File.Create(outPathZipName);
            ZipOutputStream zipStream = new ZipOutputStream(fsOut);

            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
            // This setting will strip the leading part of the folder path in the entries, to
            // make the entries relative to the starting folder.
            // To include the full path for each entry up to the drive root, assign folderOffset = 0.

            var allFilesToCompress = GetAllFilesToCompress(filesToCompress, ignoreFiles);
            foreach (var fileToCompress in allFilesToCompress)
            {
                CompressFolder(fileToCompress.FullPath, zipStream, fileToCompress.FolderOffset, ignoreFiles);
            }
            zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
            zipStream.Close();

            InformationMessageList.Add(string.Format("Compressed {0} files into {1}.", allFilesToCompress.Count, outPathZipName));
        }

        // Recurses down the folder structure
        //
        private void CompressFolder(string filename, ZipOutputStream zipStream, int folderOffset, List<string> ignoreFiles)
        {
            FileInfo fi = new FileInfo(filename);

            string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
            entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
            ZipEntry newEntry = new ZipEntry(entryName);
            newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity
            newEntry.Size = fi.Length;

            zipStream.PutNextEntry(newEntry);

            // Zip the file in buffered chunks
            // the "using" will close the stream even if an exception occurs
            byte[] buffer = new byte[4096];
            using (FileStream streamReader = File.OpenRead(filename))
            {
                StreamUtils.Copy(streamReader, zipStream, buffer);
            }
            zipStream.CloseEntry();
        }

        private class FileToCompress
        {
            public FileToCompress(string fullPath, string originalPath)
            {
                FullPath = fullPath;

                if (!string.IsNullOrEmpty(originalPath))
                {
                    FolderOffset = originalPath.Length;
                }
                else
                {
                    int lastIndexOfPath = FullPath.LastIndexOf(@"\");
                    FolderOffset = lastIndexOfPath >= 0 ? lastIndexOfPath : 0;
                }
                RelativePath = FullPath.Substring(FolderOffset);
            }
            public string FullPath { get; set; }
            public string RelativePath { get; set; }
            public int FolderOffset { get; set; }
        }
    }
}
