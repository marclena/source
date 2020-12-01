using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AWS.Logger.Core
{
    public static class WriteLogMessageHelper
    {
        private const string EXCEPTION_TAG = ",\"Exception\":";
        private const string MESSAGE_TAG = "\",\"Message\":";
        private const string LEVEL_TAG = "{\"Level\":\"";
        private const string PROPS_TAG = ",\"Metadata\":{";
        private const string APP_TAG = "\",\"Application\":\"";
        private const string CLOSE_TAG = "}";
        private const string OPEN_BRACKET = "[";
        private const string CLOSE_BRACKET = "]";

        public static void WriteTimeStampStartLine(
            this TextWriter output,
            LogMessage logEvent)
        {
            var strDate = logEvent.timestamp;
            var timestamp = $"{OPEN_BRACKET}{strDate}{CLOSE_BRACKET} ";
            output.Write(timestamp);
        }

        public static void WriteException(
            this TextWriter output,
            LogMessage logEvent)
        {
            var exception = logEvent.Properties.FirstOrDefault(x => x.Key.Equals("Exception"));
            if (exception.Key != null || logEvent.Exception != null)
            {
                output.Write(EXCEPTION_TAG);
                JsonValueFormatter.WriteQuotedJsonString(exception.Value?.ToString() ??
                    logEvent.Exception.ToString(),
                    output);
            }
        }

        public static void WriteMessage(
            this TextWriter output,
            LogEvent logEvent,
            string prefix = "")
        {
            output.Write(MESSAGE_TAG);
            JsonValueFormatter.WriteQuotedJsonString(prefix +
                logEvent.MessageTemplate.Text, output);
        }

        public static void WriteTruncateMessage(
            this TextWriter output,
            string message)
        {
            output.Write(MESSAGE_TAG);
            JsonValueFormatter.WriteQuotedJsonString(message, output);
        }

        public static void WriteLevel(
            this TextWriter output,
            LogEvent logEvent)
        {
            output.Write(LEVEL_TAG);
            output.Write(logEvent.Level);
        }


        public static void WriteProperties(
            this TextWriter output,
            LogMessage logEvent,
            JsonValueFormatter valueFormatter)
        {
            output.Write(PROPS_TAG);
            var delimiter = "";

            foreach (var property in logEvent.Metadata)
            {
                if (IgnoreProperties(property))
                    continue;

                var name = ParseName(property);
                output.Write(delimiter);
                JsonValueFormatter.WriteQuotedJsonString(name, output);
                output.Write(':');
                valueFormatter.Format(property.Value, output);
                delimiter = ",";
            }
            output.Write(CLOSE_TAG);
        }

        public static void WriteApplication(
            this TextWriter output)
        {
            var entry = Assembly.GetEntryAssembly().GetName().Name;
            output.Write(APP_TAG);
            output.Write(entry);
        }

        public static void WriteJsonClosure(
            this TextWriter output)
        {
            output.Write(CLOSE_TAG);
        }

        private static bool IgnoreProperties(
            KeyValuePair<string, LogEventPropertyValue> prop)
        {
            // Obviate boilerplate properties for now
            return prop.Key.Equals("Exception") ||
                   prop.Key.Equals("maximumAllowedBytes");
        }

        private static string ParseName(KeyValuePair<string, LogEventPropertyValue> property)
        {
            var name = property.Key;

            if (name.Length > 0 && name[0] == '@')
            {
                name = '@' + name;
            }
            return name;
        }
    }
}
