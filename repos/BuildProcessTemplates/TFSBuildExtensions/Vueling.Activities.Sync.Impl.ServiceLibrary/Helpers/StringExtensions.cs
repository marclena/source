using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.Helpers
{
    public static class StringExtensions
    {
        public static bool IsMatch(this string input, List<string> excludeContent)
        {
            if (!excludeContent.Any()) { return false; }

            return excludeContent.FirstOrDefault(x => Regex.IsMatch(input, StringExtensions.WildCardToRegular(x), RegexOptions.Compiled)) != null;
        }

        private static string WildCardToRegular(string value)
        {
            return "^" + Regex.Escape(value).Replace("\\.", "[.]").Replace("\\*", ".*") + "$";
        }

    }
}
