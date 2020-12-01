using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using System.IO;

namespace TFSBuildExtensions.EnvironmentManagement.Validation
{
    public class FileSizeLimitValidation : BaseCodeActivity
    {
        private InArgument<long> size;
        private InArgument<IEnumerable<string>> patternFiles;
        private InArgument<string> rootFolder;
        private InArgument<bool> recursively;
        private OutArgument<bool> isExceedingLimit;

        [Description("Maximum size allowed for evaluated files in Kb")]
        [RequiredArgument]
        public InArgument<long> Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        [Description("Files from which size evaluated. Default pattern is all files; *. E.g.1: *.js. E.g.2:foo*. E.g.3:foo.cs")]
        public InArgument<IEnumerable<string>> PatternFiles
        {
            get { return this.patternFiles; }
            set { this.patternFiles = value; }
        }

        [Description("Root folder to find files to evaluate")]
        [RequiredArgument]
        public InArgument<string> RootFolder
        {
            get { return rootFolder; }
            set { this.rootFolder = value; }
        }

        [Description("Try to find files with defined pattern to folders recursively")]
        public InArgument<bool> Recursively
        {
            get { return this.recursively; }
            set { this.recursively = value; }
        }

        [Description("Store if evaluated files exceeded size threshold or not")]
        public OutArgument<bool> IsExceedingLimit
        {
            get { return this.isExceedingLimit; }
            set { this.isExceedingLimit = value; }
        }

        protected override void InternalExecute()
        {
            Initialize();
            EvaluateSizeLimit();
        }

        /// <summary>
        /// Initialize optional variables
        /// </summary>
        private void Initialize()
        {
            if (patternFiles.Get(this.ActivityContext) == null || patternFiles.Get(this.ActivityContext).Count() == 0)
            {
                patternFiles.Set(this.ActivityContext, new List<string>() {"*"});
            }

            this.isExceedingLimit.Set(this.ActivityContext, false);
        }


        private void EvaluateSizeLimit()
        {
            var patterns = patternFiles.Get(this.ActivityContext).ToList();
            SearchOption searchOption = (recursively.Get(this.ActivityContext) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            foreach(string pattern in patterns)
            {
                var filePaths = Directory.GetFiles(rootFolder.Get(this.ActivityContext), pattern, searchOption).ToList();

                foreach(var filePath in filePaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    var fileSizeLimit = (fileInfo.Length / 1024);
                    if (fileSizeLimit > size.Get(this.ActivityContext))
                    {
                        LogBuildError(String.Format(" File {0} is exceding size limit threshold ({1})", fileInfo.FullName, size.Get(this.ActivityContext)));
                        this.isExceedingLimit.Set(this.ActivityContext, true);
                        return;
                    }
                }
            }
        }
    }
}
