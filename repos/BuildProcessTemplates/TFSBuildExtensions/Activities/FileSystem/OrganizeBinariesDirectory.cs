using Microsoft.TeamFoundation.Build.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.FileSystem
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class OrganizeBinariesDirectory : BaseCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> BinariesDirectory { get; set; }


        protected override void InternalExecute()
        {
            string binariesDirectory = this.BinariesDirectory.Get(this.ActivityContext);

            DirectoryInfo directoryInfo = new DirectoryInfo(binariesDirectory);

            var webDirectories = directoryInfo.EnumerateDirectories().Where(x => x.Name.EndsWith("WebService") || x.Name.EndsWith("WebUI") || x.Name.EndsWith("WebAPI"));

            if(webDirectories.Count() > 0)
            {
                Directory.CreateDirectory(binariesDirectory + "\\_PublishedWebsites");
            }

            foreach (DirectoryInfo folder in webDirectories)
            {
                Directory.Move(folder.FullName + "\\_PublishedWebsites\\" + folder.Name, binariesDirectory + "\\_PublishedWebsites\\" + folder.Name);
                Directory.Move(folder.FullName + "\\_PublishedWebsites\\" + folder.Name + "_Package", binariesDirectory + "\\_PublishedWebsites\\" + folder.Name + "_Package");
            }
        }
    }
}
