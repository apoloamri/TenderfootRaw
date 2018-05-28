using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem.Diagnostics;
using Tenderfoot.Tools;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Net
{
    public class TfApi
    {
        private string URL { get; set; }
        private string Actions { get; set; }

        public TfApi(string URL, params string[] Actions)
        {
            this.URL = URL;

            if (Actions != null)
            {
                this.Actions = string.Join("/", Actions);
            }
        }

        public HttpMethod HttpMethod { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public Dictionary<string, string> Response { get; set; } = new Dictionary<string, string>();

        public void Request()
        {
            var dataString = string.Empty;
            var apiUrl = $"{this.URL}/{this.Actions}";
            HttpWebRequest request = null;

            if (this.HttpMethod == HttpMethod.GET)
            {
                dataString = JsonTools.DictionaryToQuery(this.Parameters);
                request = (HttpWebRequest)WebRequest.Create($"{apiUrl}?{dataString}");
                request.Method = this.HttpMethod.GetString();
            }
            else
            {
                dataString = JsonTools.DictionaryToJson(this.Parameters);
                request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = this.HttpMethod.GetString();
                request.ContentType = "application/json";
                request.ContentLength = dataString.Length;
                using (Stream webStream = request.GetRequestStream())
                {
                    using (StreamWriter requestWriter = new StreamWriter(webStream, Encoding.ASCII))
                    {
                        requestWriter.Write(dataString);
                    }
                }
            }
            
            if (request == null)
            {
                return;
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            this.Response = responseReader.ReadToEnd().JsonGetDictionary();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TfDebug.WriteLog(
                    ex,
                    $"URL: {apiUrl}",
                    $"Method: {this.HttpMethod.GetString()}",
                    $"Paremeters: {dataString}");
                this.Response = new Dictionary<string, string>() { { "Error", ex.Message } };
            }
        }
    }
}
