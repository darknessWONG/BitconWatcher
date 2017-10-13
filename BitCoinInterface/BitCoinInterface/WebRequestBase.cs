using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BitCoinInterface
{
    public class WebRequestBase
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        public static HttpWebRequest _createPublicRequest(string url, string func_name, IDictionary<string, string> parameters = null)
        {
            HttpWebRequest request = null;
            StringBuilder fullUrl = new StringBuilder();

            fullUrl.AppendFormat("{0}/{1}", url, func_name);
            if (parameters != null)
            {
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        fullUrl.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        fullUrl.AppendFormat("?{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
            }

            request = (HttpWebRequest)WebRequest.Create(fullUrl.ToString());
            request.ProtocolVersion = HttpVersion.Version10;
            //request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = DefaultUserAgent;

            return request;
        }

        public static Hashtable _getJsonFromRequest(HttpWebRequest request)
        {
            HttpWebResponse respone = (HttpWebResponse)request.GetResponse();
            Stream stream = respone.GetResponseStream();
            StreamReader responeStreamReader = new StreamReader(stream);
            Hashtable datas = JsonConvert.DeserializeObject<Hashtable>(responeStreamReader.ReadToEnd());
            responeStreamReader.Close();
            stream.Close();
            respone.Close();
            request.Abort();
            return datas;
        }
    }

}
