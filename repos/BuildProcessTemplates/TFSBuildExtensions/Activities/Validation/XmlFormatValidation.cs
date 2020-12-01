using System;
using System.Activities;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace TFSBuildExtensions.Validation
{
    public class XmlFormatValidation : BaseCodeActivity
    {
        [Description("File to validate xml format")]
        [RequiredArgument]
        public InArgument<string> FilePathToValidate { get; set; }

        [Description("Validate if file is valid")]
        public OutArgument<bool> IsXmlFileFormatValid { get; set; } 

        protected override void InternalExecute()
        {
            this.IsXmlFileFormatValid.Set(this.ActivityContext, isFileValid());
        }

        private bool isFileValid()
        {

            try
            {
                var filename = this.FilePathToValidate.Get(this.ActivityContext);

                if (Path.GetExtension(filename).EndsWith("xml") || Path.GetExtension(filename).EndsWith("config")
                    || Path.GetExtension(filename).EndsWith("naml") || Path.GetExtension(filename).EndsWith("xslt"))
                {
                    XDocument.Load(filename);
                }
                else
                {
                    LogBuildWarning("File " + filename + " is not a valid xml file.");
                }
            }
            catch (XmlException)
            {
                return false;
            }
            catch(Exception)
            {
                throw;
            }
            return true;
        }
    }
}
