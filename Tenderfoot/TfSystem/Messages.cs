using Tenderfoot.Tools.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Tenderfoot.TfSystem
{
    public static class Resources
    {
        public static string GetMessage(string message, params string[] values)
        {
            if (message.IsEmpty())
            {
                return null;
            }

            string messageFile = TfSettings.Configuration().GetSection("SystemResources").GetSection("SystemMessages").Value;
            string defaultMessage = TfSettings.Configuration(messageFile).GetSection(message).Value;

            if (values != null && values.Count() > 0)
            {
                var fields = new List<string>();

                foreach (var value in values)
                {
                    if (value.IsEmpty())
                    {
                        continue;
                    }

                    string fieldName = GetField(value);

                    if (fieldName.IsEmpty())
                    {
                        fields.Add(value);
                    }

                    fields.Add(fieldName);
                }

                return string.Format(defaultMessage, fields.ToArray());
            }

            return
                defaultMessage.IsEmpty() ?
                message :
                defaultMessage;
        }

        public static string GetField(string fieldName)
        {
            string fieldFile = TfSettings.Configuration().GetSection("SystemResources").GetSection("FieldMessages").Value;
            string systemFieldName = TfSettings.Configuration(fieldFile).GetSection(fieldName).Value;
            
            return
                systemFieldName.IsEmpty() ?
                fieldName :
                systemFieldName;
        }
    }
}
