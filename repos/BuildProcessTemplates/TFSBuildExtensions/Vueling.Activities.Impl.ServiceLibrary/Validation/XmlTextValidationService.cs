using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.Validation;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Impl.ServiceLibrary.Validation
{
    [RegisterServiceAttribute]
    public class XmlTextValidationService : BaseActivityService, IXmlTextValidationService
    {
        private string forbiddenCharacter = "'";
        private string tagToSearch = "label";
        private string attributeToSearch = "text";
        private string fileXmlExtension = ".xml";
        private string errorLabel = "LABEL ERROR {0}";
        private string errorFile = "ERROR IN FILE: {0}, LABEL cannot contains {1} character. Put your text with special characters in <[[CDATA]]";
        private string startTagText = "text=\"";
        private string finalTagText = "\"";
        private string parentFolder = "\\Views\\";
        private string xmlFilePath;
        private string currentLine = "";

        public void Initialize(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
        }

        public override void InternalExecute()
        {
            try
            {
                if (File.Exists(xmlFilePath) && Path.GetExtension(xmlFilePath) == fileXmlExtension && xmlFilePath.Contains(parentFolder))
                {
                    string line;
                    string nameFile = Path.GetFileName(xmlFilePath);
                    System.IO.StreamReader file = new System.IO.StreamReader(xmlFilePath);
                    var label = new AttributeLabel();

                    while ((line = file.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            currentLine = label.Text += line;

                            var indexStartText = label.Text.IndexOf(startTagText);
                            var totalFullTextLength = startTagText.Length + indexStartText;
                            var indexFinalText = (label.Text.Length < totalFullTextLength) ? -1 : label.Text.IndexOf(finalTagText, totalFullTextLength);

                            if (indexFinalText > 0)
                            {
                                label.Complete = true;
                            }

                            if (label.Complete && label.Text.Contains(tagToSearch) && label.Text.Contains(attributeToSearch))
                            {
                                var attribute = label.Text.Substring(totalFullTextLength, indexFinalText - totalFullTextLength);

                                if (attribute.Contains(forbiddenCharacter))
                                {
                                    if (this.ErrorMessageList == null)
                                        this.ErrorMessageList = new List<string>();

                                    this.ErrorMessageList.Add(string.Format(errorFile, nameFile, forbiddenCharacter));
                                    this.ErrorMessageList.Add(string.Format(errorLabel, label.Text));
                                }
                            }

                            if (label.Complete || !label.Text.Contains(tagToSearch))
                            {
                                label.Complete = false;
                                label.Text = "";
                            }
                        }
                    }

                    if (this.ErrorMessageList != null && this.ErrorMessageList.Any())
                    {
                        this.Result = 1;
                    }
                    else
                    {
                        this.Result = 0;
                        this.InformationMessageList = new List<string>();
                        this.InformationMessageList.Add("Process to validate text in xml completed sucessfully");
                    }
                }
            }
            catch (Exception ex)
            {
                if (this.ErrorMessageList == null)
                    this.ErrorMessageList = new List<string>();

                this.Result = 1;
                this.ErrorMessageList.Add(string.Format("An exception occurs, trying to validate text in xml:{0}", ex.Message));
                this.ErrorMessageList.Add(string.Format("File: {0}; Line: {1}", this.xmlFilePath, currentLine));
            }
        }
    }

    public class AttributeLabel
    {
        public string Text { get; set; }
        public bool Complete { get; set; }
    }
}

