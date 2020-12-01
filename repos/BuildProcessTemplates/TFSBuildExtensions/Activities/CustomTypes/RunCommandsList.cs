using System.ComponentModel;
using System.IO;
using System;

namespace TFSBuildExtensions.CustomTypes
{
    public class RunCommandsList
    {
        [DisplayName("Command")]
        [Category("Properties")]
        public string Command { get; set; }

        [DisplayName("Parameters")]
        [Category("Properties")]
        public string Parameters { get; set; }

        [DisplayName("WorkingFolder")]
        [Category("Properties")]
        public string WorkingFolder { get; set; } 

         public override string ToString()
         {
             return string.IsNullOrEmpty(Command) ? "New" : Path.GetFileName(Command);
         }
    }
}
