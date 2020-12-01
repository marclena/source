// Copied from https://github.com/aspnet/AspNetCore/blob/9f202feafc335dd70ae640f2053b465be3879e82/src/Middleware/HealthChecks/src/HealthCheckResponseWriters.cs
// with modifications.

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Owin;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System;
using Newtonsoft.Json;

namespace ATC.HealthCheck.NET
{
    internal static class HealthCheckResponseWriters
    {
        public static Task WriteMinimalPlaintext(IOwinContext owinContext, HealthReport result)
        {
            HealthCheckResponse response = new HealthCheckResponse();
            response.machineName = Environment.MachineName;
            
            response.status = result.Status.ToString();
            string status;
            owinContext.Response.ContentType = "application/json";
           
            Dictionary<string, string> entries = new Dictionary<string, string>();
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (string a  in result.Entries.Keys)
            {
                if (result.Entries[a].Status == HealthStatus.Healthy)
                    status = "OK";
                else
                    status = "KO";
                entries.Add(a, status);
            }
            response.healthy = entries;
            properties.Add("ApplicationVersion", typeof(HealthCheckResponseWriters).Assembly.GetName().Version.ToString());
            response.properties = properties;
          
            var json=JsonConvert.SerializeObject(response);
            return owinContext.Response.WriteAsync(json);
        }
    }
}