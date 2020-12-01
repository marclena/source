using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vueling.StaticContent.Back.JS.Helpers.Impl.ServiceLibrary
{
    public abstract class BaseFolder
    {
        private readonly string _parentPath;

        public string relativePath
        {
            get
            {
                if (!string.IsNullOrEmpty(currentFolder))
                    return _parentPath + "/" + currentFolder;
                else
                {
                    return string.Empty;
                }
            }
        }

        protected abstract string currentFolder { get; }

        protected BaseFolder(string parentPath)
        {
            _parentPath = parentPath;
        }

        virtual protected string GetFullPath(string fileName)
        {
            return relativePath + "/" + fileName;
        }
    }
}
