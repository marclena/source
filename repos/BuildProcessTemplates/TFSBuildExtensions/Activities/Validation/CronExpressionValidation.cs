using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using System.ComponentModel;
using System.Activities;
using System.Xml;

namespace TFSBuildExtensions.EnvironmentManagement.Validation
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class CronExpressionValidation : BaseCodeActivity
    {
        #region Properties
        [Description("Path where xml jobs are located")]
        [RequiredArgument]
        public InArgument<string> CronXmlFilesPath { get; set; }

        [Description("Result: True: Validation succeeded. False: Validation Failed.")]
        public OutArgument<bool> Result { get; set; }
        #endregion

        protected override void InternalExecute()
        {
            Validate();
        }

        private void Validate()
        {
            bool valid = true;
            if (System.IO.File.Exists(this.ActivityContext.GetValue(CronXmlFilesPath)))
            {
                try
                {
                    var newConfigSectionDoc = new XmlDocument();
                    newConfigSectionDoc.Load(this.ActivityContext.GetValue(CronXmlFilesPath));

                    XmlNodeList newJobs = newConfigSectionDoc.GetElementsByTagName("job");
                    foreach (XmlNode node in newJobs)
                    {
                        //Comprobación del cron-expression
                        if (!CheckCronExpression(node)) valid = false;

                    }
                }
                catch (Exception ex)
                {
                    this.LogBuildMessage(ex.Message, BuildMessageImportance.High);
                    valid = false;
                }
            }

            this.ActivityContext.SetValue(Result,valid);
        }

        private static bool CheckCronExpression(XmlNode node)
        {
            bool bRet = true;
            try
            {
                var package = node as XmlElement;
                if (package == null) return false;

                foreach (var cnode in package.GetElementsByTagName("cron-expression"))
                {
                    if (!Quartz.CronExpression.IsValidExpression((((System.Xml.XmlElement)(cnode))).InnerText))
                    {
                        bRet = false;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                bRet = false;
            }

            return bRet;
        }
    }
}
