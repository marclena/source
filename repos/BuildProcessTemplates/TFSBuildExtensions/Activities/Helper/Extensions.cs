using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.Helper
{
    public static class Extensions
    {
        public static string listToString(this string[] listStrings)
        {
            bool _first = true;
            StringBuilder _string = new StringBuilder();

            foreach (string _str in listStrings)
            {
                if (!_first)
                {
                    _string.Append(",");
                }
                _string.Append(_str);
                _first = false;

            }

            return _string.ToString();
        }

        public static string GetComponentName(string buildDefinitionName)
        {
            if (buildDefinitionName.ToLower().StartsWith("vueling."))
            {
                return buildDefinitionName.Substring(0, buildDefinitionName.IndexOf(".", 8));
            }
            else
            {
                return buildDefinitionName.Substring(0, buildDefinitionName.IndexOf(".", 0));
            }
        }
    }
}
