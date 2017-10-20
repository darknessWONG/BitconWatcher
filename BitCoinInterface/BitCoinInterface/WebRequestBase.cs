using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
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
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = DefaultUserAgent;

            return request;
        }

        public static HttpWebRequest _createPostPrivateRequest(string url, string func_name, Dictionary<string, string> bodyParameters = null, 
            Dictionary<string, string> headerParameters = null)
        {
            HttpWebRequest request = null;
            StringBuilder fullUrl = new StringBuilder();
            StringBuilder bodyData = new StringBuilder();
            StringBuilder headerData = new StringBuilder();

            request = (HttpWebRequest)WebRequest.Create(fullUrl.ToString());

            fullUrl.AppendFormat("{0}/{1}", url, func_name);
            if(bodyParameters != null)
            {
                int i = 0;
                foreach(string key in bodyParameters.Keys)
                {
                    if (i > 0)
                    {
                        bodyData.AppendFormat("&{0}={1}", key, bodyParameters[key]);
                    }
                    else
                    {
                        bodyData.AppendFormat("?{0}={1}", key, bodyParameters[key]);
                    }
                    i++;
                }
                byte[] data = Encoding.GetEncoding("UTF-8").GetBytes(bodyData.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            if(headerParameters != null)
            {
                int i = 0;
                foreach(string key in headerParameters.Keys)
                {
                    if (i > 0)
                    {
                        headerData.AppendFormat(",{0}:{1}", key, headerParameters[key]);
                    }
                    else
                    {
                        headerData.AppendFormat("{0}:{1}", key, headerParameters[key]);
                    }
                    i++;
                }
                WebHeaderCollection header = request.Headers;
                header.Add(header.ToString());
            }


            request.ProtocolVersion = HttpVersion.Version10;
            //request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = DefaultUserAgent;
            


            return request;
        }

        public static HttpWebRequest _createGetPrivateRequest(string url, string func_name, Dictionary<string, string> headerParameters = null)
        {
            HttpWebRequest request = null;
            StringBuilder fullUrl = new StringBuilder();
            StringBuilder bodyData = new StringBuilder();
            StringBuilder headerData = new StringBuilder();

            fullUrl.AppendFormat("{0}/{1}", url, func_name);
            request = (HttpWebRequest)WebRequest.Create(fullUrl.ToString());


            if (headerParameters != null)
            {
                foreach (string key in headerParameters.Keys)
                {
                    request.Headers.Add(key, headerParameters[key]);
                }
            }

            

            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = DefaultUserAgent;



            return request;
        }

        public static Hashtable _getJsonFromRequest(HttpWebRequest request)
        {
            HttpWebResponse respone = (HttpWebResponse)request.GetResponse();
            Stream stream = respone.GetResponseStream();
            StreamReader responeStreamReader = new StreamReader(stream);
            var json = JsonConvert.DeserializeObject(responeStreamReader.ReadToEnd());
            //Hashtable datas = JsonConvert.DeserializeObject<JArray>(responeStreamReader.ReadToEnd()).ToHashtable();
            Hashtable datas;
            string type = json.GetType().ToString();
            if(type == "Newtonsoft.Json.Linq.JArray")
            {
                datas = ((JArray)json).ToHashtable();
            }
            else
            {
                datas = ((JObject)json).ToHashtable();
            }
            //Hashtable datas = json.ToHashtable();
            responeStreamReader.Close();
            stream.Close();
            respone.Close();
            request.Abort();
            return datas;
        }

        public static string _CreateSign(string timeStamp, string method, string path, string body, string secret)
        {
            string original = timeStamp + method + path + body;
            return MathTool._SignWithHMACSHA256(original, secret);
        }
    }

}
