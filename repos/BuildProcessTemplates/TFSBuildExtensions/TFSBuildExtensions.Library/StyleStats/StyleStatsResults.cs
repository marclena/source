using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.Library.StyleStats
{
    public class StyleStatsResults
    {
        public string published { get; set; }
        public List<string> paths { get; set; }
        public int stylesheets { get; set; }
        public int size { get; set; }
        public int dataUriSize { get; set; }
        public int rules { get; set; }
        public int selectors { get; set; }
        public double simplicity { get; set; }
        public double averageOfIdentifier { get; set; }
        public int mostIdentifier { get; set; }
        public string mostIdentifierSelector { get; set; }
        public double averageOfCohesion { get; set; }
        public int lowestCohesion { get; set; }
        public List<string> lowestCohesionSelector { get; set; }
        public int totalUniqueFontSizes { get; set; }
        public List<string> uniqueFontSizes { get; set; }
        public int totalUniqueFontFamilies { get; set; }
        public List<string> uniqueFontFamilies { get; set; }
        public int totalUniqueColors { get; set; }
        public List<string> uniqueColors { get; set; }
        public int idSelectors { get; set; }
        public int universalSelectors { get; set; }
        public int unqualifiedAttributeSelectors { get; set; }
        public int javascriptSpecificSelectors { get; set; }
        public int importantKeywords { get; set; }
        public int floatProperties { get; set; }
        public List<PropertiesCount> propertiesCount { get; set; }
        public int mediaQueries { get; set; }
    }

    public class PropertiesCount
    {
        public string property { get; set; }
        public int count { get; set; }
    }
}
