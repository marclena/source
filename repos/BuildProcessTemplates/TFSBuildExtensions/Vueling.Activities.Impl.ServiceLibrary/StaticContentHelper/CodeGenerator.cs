using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Impl.ServiceLibrary.StaticContentHelper
{
    public class CodeGenerator
    {
        private static string DefaultClassName = "StaticResources";
        CodeCompileUnit targetUnit;
        CodeTypeDeclaration targetClass;
        private List<string> files { get; set; }
        private List<string> folders { get; set; }
        private string folder { get; set; }
        private string className { get; set; }
        private HashSet<string> InvalidClassNameCharacters;
        private string DefaultSpaceString;
        private string StaticContentNamespace;

        private string GetFileName(string fullPath)
        {
            return fullPath.Substring(fullPath.LastIndexOf("\\") + 1);
        }

        private string GetNamespaceFromPath(string path)
        {
            path = path.Substring(0, path.LastIndexOf("\\"));
            path = path.Replace("\\", ";");
            path = FormatName(path, false);
            path = path.Replace(";", ".");
            return (!string.IsNullOrEmpty(path) && path.StartsWith("."))
                ? StaticContentNamespace + ".Helpers" + path
                : StaticContentNamespace + ".Helpers";
        }

        private string FormatName(string name, bool capitalizeFirst)
        {
            if (string.IsNullOrWhiteSpace(name))
                return DefaultClassName;
            foreach (var invalidChar in InvalidClassNameCharacters)
            {
                name = name.Replace(invalidChar, DefaultSpaceString);
            }
            if (capitalizeFirst)
                name = char.ToUpper(name[0]) + name.Substring(1);
            if (name == "Vueling")
                return "VuelingFolder";
            return name;
        }

        public CodeGenerator(string folder, List<string> filesList, List<string> foldersList, HashSet<string> invalidClassNameCharacters, string defaultSpaceString, string staticContentNamespace, string defaultClassName = null)
        {
            InvalidClassNameCharacters = invalidClassNameCharacters;
            DefaultSpaceString = defaultSpaceString;
            StaticContentNamespace = staticContentNamespace;
            if (!string.IsNullOrEmpty(defaultClassName))
                DefaultClassName = defaultClassName;

            this.folder = !string.IsNullOrWhiteSpace(folder) ? folder : DefaultClassName;
            files = filesList;
            folders = foldersList;

            targetUnit = new CodeCompileUnit();
            CodeNamespace samples = new CodeNamespace(GetNamespaceFromPath(folder));
            samples.Imports.Add(new CodeNamespaceImport("System"));
            className = FormatName(GetFileName(folder), true);
            if (className == "Vueling")
                className = "VuelingFolder";

            if (className == DefaultClassName)
            {
                targetClass = new CodeTypeDeclaration(className)
                {
                    Attributes = MemberAttributes.Static | MemberAttributes.Public
                };
                targetClass.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "\r\n[System.Diagnostics.CodeAnalysis.SuppressMessage(\"CustomRules.Maintenability\", \"VY1004:GlobalNoUseParamlessConstructor\")]\r\nstatic"));
                targetClass.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, String.Empty));
            }
            else
            {
                targetClass = new CodeTypeDeclaration(className);
                targetClass.TypeAttributes = TypeAttributes.Public;
                string namespaceBase = GetNamespaceFromPath(folder);
                namespaceBase = namespaceBase.Substring(0, namespaceBase.LastIndexOf(".Helpers"));
                targetClass.BaseTypes.Add(new CodeTypeReference(namespaceBase + ".BaseFolder"));
            }
            targetClass.IsClass = true;
            samples.Types.Add(targetClass);
            targetUnit.Namespaces.Add(samples);
        }

        public void AddBaseFileFields()
        {
            string sNamespace = GetNamespaceFromPath(folder);
            if (sNamespace.StartsWith("."))
                sNamespace = sNamespace.Substring(1);

            CodeSnippetTypeMember currentRelPath = new CodeSnippetTypeMember();
            currentRelPath.Text = "        private const string currentRelPath = \"/\";\r\n";
            targetClass.Members.Add(currentRelPath);

            CodeSnippetTypeMember constructor = new CodeSnippetTypeMember();

            constructor.Text = "        static " + className + "()  \r\n        {";

            foreach (var fold in folders)
            {
                var propertyNameAndType = FormatName(GetFileName(fold), true);
                constructor.Text += "\r\n                " + propertyNameAndType + " = new " + sNamespace + "." + propertyNameAndType +
                                    "(currentRelPath);";
            }

            constructor.Text += "\r\n        }\r\n";

            targetClass.Members.Add(constructor);

            foreach (var fold in folders)
            {
                var propertyNameAndType = FormatName(GetFileName(fold), true);
                CodeSnippetTypeMember folderProperty = new CodeSnippetTypeMember();
                folderProperty.Text = "        public static " + sNamespace + "." + propertyNameAndType + " " + propertyNameAndType +
                                      " { get; set; }\r\n";
                targetClass.Members.Add(folderProperty);
            }
            foreach (var file in files)
            {
                var fileName = FormatName(GetFileName(file), true);
                var fileNameWithoutFormat = GetFileName(file);

                CodeMemberField fileNameField = new CodeMemberField();
                fileNameField.Attributes = MemberAttributes.Const | MemberAttributes.Private;
                fileNameField.Name = fileName + "_FileName";
                fileNameField.Type = new CodeTypeReference(typeof(string));
                fileNameField.InitExpression = new CodePrimitiveExpression(fileNameWithoutFormat);
                targetClass.Members.Add(fileNameField);

                CodeSnippetTypeMember publicFileProperty = new CodeSnippetTypeMember();
                publicFileProperty.Text = "        public string " + fileName + " { get { return GetFullPath(" +
                                          fileNameField.Name + "); } }";

                targetClass.Members.Add(publicFileProperty);
            }
        }

        public void AddFields()
        {
            string sNamespace = GetNamespaceFromPath(folder);
            if (sNamespace.StartsWith("."))
                sNamespace = sNamespace.Substring(1);

            CodeSnippetTypeMember constructor = new CodeSnippetTypeMember();
            constructor.Text = "        public " + className + "(string parentPath) : base(parentPath) \r\n        {";

            foreach (var fold in folders)
            {
                var propertyNameAndType = FormatName(GetFileName(fold), true);
                constructor.Text += "\r\n                " + propertyNameAndType + " = new " + sNamespace + "." + FormatName(GetFileName(folder), false) + "." + propertyNameAndType +
                                    "(relativePath);";
            }

            constructor.Text += "\r\n        }\r\n";

            targetClass.Members.Add(constructor);

            CodeSnippetTypeMember currentFolder = new CodeSnippetTypeMember();
            currentFolder.Text = "        protected override string currentFolder { get { return \"" + GetFileName(folder) + "\"; } }\r\n";
            targetClass.Members.Add(currentFolder);

            foreach (var fold in folders)
            {
                var propertyNameAndType = FormatName(GetFileName(fold), true);
                CodeSnippetTypeMember folderProperty = new CodeSnippetTypeMember();
                folderProperty.Text = "        public " + sNamespace + "." + FormatName(GetFileName(folder), false) + "." + propertyNameAndType + " " + propertyNameAndType +
                                      " { get; set; }\r\n";
                targetClass.Members.Add(folderProperty);
            }
            foreach (var file in files)
            {
                var fileName = FormatName(GetFileName(file), true);
                var fileNameWithoutFormat = GetFileName(file);

                CodeMemberField fileNameField = new CodeMemberField();
                fileNameField.Attributes = MemberAttributes.Const | MemberAttributes.Private;
                fileNameField.Name = fileName + "_FileName";
                fileNameField.Type = new CodeTypeReference(typeof(string));
                fileNameField.InitExpression = new CodePrimitiveExpression(fileNameWithoutFormat);
                targetClass.Members.Add(fileNameField);

                CodeSnippetTypeMember publicFileProperty = new CodeSnippetTypeMember();
                publicFileProperty.Text = "        public string " + fileName + " { get { return GetFullPath(" +
                                          fileNameField.Name + "); } }\r\n";

                targetClass.Members.Add(publicFileProperty);
            }
        }

        public void GenerateCSharpCode(string fileName)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (StreamWriter sourceWriter = new StreamWriter(fileName))
            {
                provider.GenerateCodeFromCompileUnit(
                    targetUnit, sourceWriter, options);
            }
        }
    }

}
