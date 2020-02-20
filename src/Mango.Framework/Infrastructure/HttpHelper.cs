using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
namespace Mango.Framework.Infrastructure
{
    public class HttpHelper
    {
        public static string Post(string requestUri,string dataContent)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpContent httpContent = new StringContent(dataContent);
                    httpClient.PostAsync(requestUri, httpContent);
                    return null;
                }
            }
            catch 
            {
                return null;
            }
        }
        public static string Get(string requestUri)
        {
            string httpResult = string.Empty;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var httpResponseMessage= httpClient.GetAsync(requestUri).Result;
                    httpResult= httpResponseMessage.Content.ReadAsStringAsync().Result;
                    return httpResult;
                }
            }
            catch
            {
                return httpResult;
            }
        }
    }
}
