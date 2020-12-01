using Microsoft.TeamFoundation.Build.Workflow.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.MapFactories.MapExternalDTOToLocalDTO
{
    internal class MapExternalApplicationOnPremiseSettingToStringList : MappingBase
    {
        internal override TOutput Get<TInput, TOutput>(TInput source)
        {
            if (source == null) { return default(TOutput); }

            var applications = source as List<Vueling.Build.Contracts.ServiceLibrary.DTO.ApplicationOnPremiseSetting>;

            if (applications == null) { throw new InvalidCastException(typeof(TInput).Name); }

            StringList applicationToServerMatching = new StringList();

            foreach (var app in applications)
            {
                applicationToServerMatching.Add(app.Name + "|" + app.Server + "|" + "NONE" + "|" + app.Site);
            }

            return applicationToServerMatching as TOutput;
        }
    }
}