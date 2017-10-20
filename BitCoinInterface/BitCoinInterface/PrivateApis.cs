using System;
using System.Collections.Generic;
using System.Net;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitCoinInterface
{
    public abstract class PrivateApis
    {

        #region attribute
        public string AccessKey                       { get; set; }
        public string AccessSecert                    { get; set; }
        public string TimeStamp                       { get; protected set; }
        public string Url                             { get; protected set; }
        public string Func_name                       { get; protected set; }
        public Dictionary<string, string> BodyArgs    { get; protected set; }
        public Dictionary<string, string> HeadersArgs { get; protected set; }
        public HttpWebRequest Request                 { get; protected set; }
        public Hashtable Json                         { get; protected set; }
        public string Metod                           { get; protected set; }


        #endregion

        public PrivateApis(string accessKey, string accessSecert)
        {
            BodyArgs = new Dictionary<string, string>();
            HeadersArgs = new Dictionary<string, string>();
            Request = null;
            Json = null;
            SetMember("https://api.bitflyer.jp/", "", accessKey, accessSecert);
        }

        public string CreateTimeStamp()
        {
            TimeStamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
            return TimeStamp;
        }

        public void SetMember(string url, string func_name, string accessKey, string accessSecert)
        {
            Url = url;
            Func_name = func_name;
            AccessKey = accessKey;
            AccessSecert = accessSecert;
        }

        public void SetHeaders(string accessSign)
        {
            HeadersArgs["ACCESS-KEY"] = AccessKey;
            HeadersArgs["ACCESS-TIMESTAMP"] = TimeStamp;
            HeadersArgs["ACCESS-SIGN"] = accessSign;

        }

    }

    public class GetBalanceApi : PrivateApis
    {
        public GetBalanceApi(string accessKey, string accessSecert)
            : base(accessKey,accessSecert)
        {
            SetMember("GET", "/v1/me/getbalance");
        }

        public void SetMember(string metod, string func)
        {
            Metod = metod;
            Func_name = func;
        }
        public void start()
        {
            creatRequest();
            refresh();
        }
        public double getJPYVal()
        {
            try
            {
                foreach(int key in Json.Keys)
                {
                    if(((Hashtable)Json[key])["currency_code"].ToString() == "JPY")
                    {
                        return Convert.ToDouble(((Hashtable)Json[key])["available"]);
                    }
                }
                throw new Exception("No currency_code is JPY");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public double getBTCVal()
        {
            try
            {
                foreach (int key in Json.Keys)
                {
                    if (((Hashtable)Json[key])["currency_code"].ToString() == "BTC")
                    {
                        return Convert.ToDouble(((Hashtable)Json[key])["available"]);
                    }
                }
                throw new Exception("No currency_code is JPY");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void creatRequest()
        {
            string body = "";
            CreateTimeStamp();
            string data = TimeStamp + Metod + Func_name + body;
            string sign = MathTool._SignWithHMACSHA256(data, AccessSecert);
            SetHeaders(sign);

            Request = WebRequestBase._createGetPrivateRequest(Url, Func_name, HeadersArgs);
        }
        private void refresh()
        {
            if (Request != null)
            {
                Json = WebRequestBase._getJsonFromRequest(Request);
            }
        }
    }
}
