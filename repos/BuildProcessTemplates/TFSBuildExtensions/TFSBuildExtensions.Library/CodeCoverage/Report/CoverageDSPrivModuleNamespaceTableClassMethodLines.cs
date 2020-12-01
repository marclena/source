using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TFSBuildExtensions.Library.CodeCoverage.Report
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Runtime.Serialization.DataContractAttribute(Name = "CoverageDSPrivModuleNamespaceTableClassMethodLines")]
    public partial class CoverageDSPrivModuleNamespaceTableClassMethodLines
    {

        private int lnStartField;

        private int colStartField;

        private int lnEndField;

        private int colEndField;

        private int coverageField;

        private int sourceFileIDField;

        private int lineIDField;

        private static System.Xml.Serialization.XmlSerializer serializer;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LnStart
        {
            get
            {
                return this.lnStartField;
            }
            set
            {
                this.lnStartField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ColStart
        {
            get
            {
                return this.colStartField;
            }
            set
            {
                this.colStartField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LnEnd
        {
            get
            {
                return this.lnEndField;
            }
            set
            {
                this.lnEndField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ColEnd
        {
            get
            {
                return this.colEndField;
            }
            set
            {
                this.colEndField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Coverage
        {
            get
            {
                return this.coverageField;
            }
            set
            {
                this.coverageField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SourceFileID
        {
            get
            {
                return this.sourceFileIDField;
            }
            set
            {
                this.sourceFileIDField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LineID
        {
            get
            {
                return this.lineIDField;
            }
            set
            {
                this.lineIDField = value;
            }
        }

        private static System.Xml.Serialization.XmlSerializer Serializer
        {
            get
            {
                if ((serializer == null))
                {
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(CoverageDSPrivModuleNamespaceTableClassMethodLines));
                }
                return serializer;
            }
        }

        #region Serialize/Deserialize
        /// <summary>
        /// Serializes current CoverageDSPrivModuleNamespaceTableClassMethodLines object into an XML document
        /// </summary>
        /// <returns>string XML value</returns>
        public virtual string Serialize()
        {
            System.IO.StreamReader streamReader = null;
            System.IO.MemoryStream memoryStream = null;
            try
            {
                memoryStream = new System.IO.MemoryStream();
                Serializer.Serialize(memoryStream, this);
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                streamReader = new System.IO.StreamReader(memoryStream);
                return streamReader.ReadToEnd();
            }
            finally
            {
                if ((streamReader != null))
                {
                    streamReader.Dispose();
                }
                if ((memoryStream != null))
                {
                    memoryStream.Dispose();
                }
            }
        }

        /// <summary>
        /// Deserializes workflow markup into an CoverageDSPrivModuleNamespaceTableClassMethodLines object
        /// </summary>
        /// <param name="xml">string workflow markup to deserialize</param>
        /// <param name="obj">Output CoverageDSPrivModuleNamespaceTableClassMethodLines object</param>
        /// <param name="exception">output Exception value if deserialize failed</param>
        /// <returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        public static bool Deserialize(string xml, out CoverageDSPrivModuleNamespaceTableClassMethodLines obj, out System.Exception exception)
        {
            exception = null;
            obj = default(CoverageDSPrivModuleNamespaceTableClassMethodLines);
            try
            {
                obj = Deserialize(xml);
                return true;
            }
            catch (System.Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        public static bool Deserialize(string xml, out CoverageDSPrivModuleNamespaceTableClassMethodLines obj)
        {
            System.Exception exception = null;
            return Deserialize(xml, out obj, out exception);
        }

        public static CoverageDSPrivModuleNamespaceTableClassMethodLines Deserialize(string xml)
        {
            System.IO.StringReader stringReader = null;
            try
            {
                stringReader = new System.IO.StringReader(xml);
                return ((CoverageDSPrivModuleNamespaceTableClassMethodLines)(Serializer.Deserialize(System.Xml.XmlReader.Create(stringReader))));
            }
            finally
            {
                if ((stringReader != null))
                {
                    stringReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Serializes current CoverageDSPrivModuleNamespaceTableClassMethodLines object into file
        /// </summary>
        /// <param name="fileName">full path of outupt xml file</param>
        /// <param name="exception">output Exception value if failed</param>
        /// <returns>true if can serialize and save into file; otherwise, false</returns>
        public virtual bool SaveToFile(string fileName, out System.Exception exception)
        {
            exception = null;
            try
            {
                SaveToFile(fileName);
                return true;
            }
            catch (System.Exception e)
            {
                exception = e;
                return false;
            }
        }

        public virtual void SaveToFile(string fileName)
        {
            System.IO.StreamWriter streamWriter = null;
            try
            {
                string xmlString = Serialize();
                System.IO.FileInfo xmlFile = new System.IO.FileInfo(fileName);
                streamWriter = xmlFile.CreateText();
                streamWriter.WriteLine(xmlString);
                streamWriter.Close();
            }
            finally
            {
                if ((streamWriter != null))
                {
                    streamWriter.Dispose();
                }
            }
        }

        /// <summary>
        /// Deserializes xml markup from file into an CoverageDSPrivModuleNamespaceTableClassMethodLines object
        /// </summary>
        /// <param name="fileName">string xml file to load and deserialize</param>
        /// <param name="obj">Output CoverageDSPrivModuleNamespaceTableClassMethodLines object</param>
        /// <param name="exception">output Exception value if deserialize failed</param>
        /// <returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        public static bool LoadFromFile(string fileName, out CoverageDSPrivModuleNamespaceTableClassMethodLines obj, out System.Exception exception)
        {
            exception = null;
            obj = default(CoverageDSPrivModuleNamespaceTableClassMethodLines);
            try
            {
                obj = LoadFromFile(fileName);
                return true;
            }
            catch (System.Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        public static bool LoadFromFile(string fileName, out CoverageDSPrivModuleNamespaceTableClassMethodLines obj)
        {
            System.Exception exception = null;
            return LoadFromFile(fileName, out obj, out exception);
        }

        public static CoverageDSPrivModuleNamespaceTableClassMethodLines LoadFromFile(string fileName)
        {
            System.IO.FileStream file = null;
            System.IO.StreamReader sr = null;
            try
            {
                file = new System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read);
                sr = new System.IO.StreamReader(file);
                string xmlString = sr.ReadToEnd();
                sr.Close();
                file.Close();
                return Deserialize(xmlString);
            }
            finally
            {
                if ((file != null))
                {
                    file.Dispose();
                }
                if ((sr != null))
                {
                    sr.Dispose();
                }
            }
        }
        #endregion
    }
}
