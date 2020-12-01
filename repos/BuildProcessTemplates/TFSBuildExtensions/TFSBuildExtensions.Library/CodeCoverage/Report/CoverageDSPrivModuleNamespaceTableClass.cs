﻿using System;
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
    [System.Runtime.Serialization.DataContractAttribute(Name = "CoverageDSPrivModuleNamespaceTableClass")]
    public partial class CoverageDSPrivModuleNamespaceTableClass
    {

        private string classKeyNameField;

        private string classNameField;

        private int linesCoveredField;

        private int linesNotCoveredField;

        private int linesPartiallyCoveredField;

        private int blocksCoveredField;

        private int blocksNotCoveredField;

        private string namespaceKeyNameField;

        private List<CoverageDSPrivModuleNamespaceTableClassMethod> methodField;

        private static System.Xml.Serialization.XmlSerializer serializer;

        public CoverageDSPrivModuleNamespaceTableClass()
        {
            this.methodField = new List<CoverageDSPrivModuleNamespaceTableClassMethod>();
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClassKeyName
        {
            get
            {
                return this.classKeyNameField;
            }
            set
            {
                this.classKeyNameField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClassName
        {
            get
            {
                return this.classNameField;
            }
            set
            {
                this.classNameField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LinesCovered
        {
            get
            {
                return this.linesCoveredField;
            }
            set
            {
                this.linesCoveredField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LinesNotCovered
        {
            get
            {
                return this.linesNotCoveredField;
            }
            set
            {
                this.linesNotCoveredField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LinesPartiallyCovered
        {
            get
            {
                return this.linesPartiallyCoveredField;
            }
            set
            {
                this.linesPartiallyCoveredField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int BlocksCovered
        {
            get
            {
                return this.blocksCoveredField;
            }
            set
            {
                this.blocksCoveredField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int BlocksNotCovered
        {
            get
            {
                return this.blocksNotCoveredField;
            }
            set
            {
                this.blocksNotCoveredField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NamespaceKeyName
        {
            get
            {
                return this.namespaceKeyNameField;
            }
            set
            {
                this.namespaceKeyNameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Method")]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<CoverageDSPrivModuleNamespaceTableClassMethod> Method
        {
            get
            {
                return this.methodField;
            }
            set
            {
                this.methodField = value;
            }
        }

        private static System.Xml.Serialization.XmlSerializer Serializer
        {
            get
            {
                if ((serializer == null))
                {
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(CoverageDSPrivModuleNamespaceTableClass));
                }
                return serializer;
            }
        }

        #region Serialize/Deserialize
        /// <summary>
        /// Serializes current CoverageDSPrivModuleNamespaceTableClass object into an XML document
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
        /// Deserializes workflow markup into an CoverageDSPrivModuleNamespaceTableClass object
        /// </summary>
        /// <param name="xml">string workflow markup to deserialize</param>
        /// <param name="obj">Output CoverageDSPrivModuleNamespaceTableClass object</param>
        /// <param name="exception">output Exception value if deserialize failed</param>
        /// <returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        public static bool Deserialize(string xml, out CoverageDSPrivModuleNamespaceTableClass obj, out System.Exception exception)
        {
            exception = null;
            obj = default(CoverageDSPrivModuleNamespaceTableClass);
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

        public static bool Deserialize(string xml, out CoverageDSPrivModuleNamespaceTableClass obj)
        {
            System.Exception exception = null;
            return Deserialize(xml, out obj, out exception);
        }

        public static CoverageDSPrivModuleNamespaceTableClass Deserialize(string xml)
        {
            System.IO.StringReader stringReader = null;
            try
            {
                stringReader = new System.IO.StringReader(xml);
                return ((CoverageDSPrivModuleNamespaceTableClass)(Serializer.Deserialize(System.Xml.XmlReader.Create(stringReader))));
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
        /// Serializes current CoverageDSPrivModuleNamespaceTableClass object into file
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
        /// Deserializes xml markup from file into an CoverageDSPrivModuleNamespaceTableClass object
        /// </summary>
        /// <param name="fileName">string xml file to load and deserialize</param>
        /// <param name="obj">Output CoverageDSPrivModuleNamespaceTableClass object</param>
        /// <param name="exception">output Exception value if deserialize failed</param>
        /// <returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        public static bool LoadFromFile(string fileName, out CoverageDSPrivModuleNamespaceTableClass obj, out System.Exception exception)
        {
            exception = null;
            obj = default(CoverageDSPrivModuleNamespaceTableClass);
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

        public static bool LoadFromFile(string fileName, out CoverageDSPrivModuleNamespaceTableClass obj)
        {
            System.Exception exception = null;
            return LoadFromFile(fileName, out obj, out exception);
        }

        public static CoverageDSPrivModuleNamespaceTableClass LoadFromFile(string fileName)
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
