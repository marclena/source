using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddJsonFiles(this IConfigurationBuilder builder,
            WebHostBuilderContext builderContext)
        {
            var configFiles = Directory
                .GetFiles(Path.Combine(builderContext.HostingEnvironment.ContentRootPath, "Configuration"),
                    "settings.*").ToList();
            var filteredConfigFiles = configFiles.Where(x => !x.Contains(".development.")).ToList();

            if (builderContext.HostingEnvironment.IsDevelopment() ||
                builderContext.HostingEnvironment.IsEnvironment("test"))
                filteredConfigFiles.AddRange(configFiles.Where(x => x.Contains(".development.")).ToList());


            foreach (var configFile in filteredConfigFiles) builder.AddJsonFile(configFile, false, true);


            return builder;
        }

        public static IConfigurationBuilder If(this IConfigurationBuilder builder, Func<bool> validate,
            Action<IConfigurationBuilder> action)
        {
            if (validate.Invoke()) action.Invoke(builder);

            return builder;
        }
    }
}