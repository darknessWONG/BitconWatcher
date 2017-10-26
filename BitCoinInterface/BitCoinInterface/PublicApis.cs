using System;
using System.Collections.Generic;
using System.Net;
using System.Collections;

namespace BitCoinInterface
{
    public abstract class PublicApis
    {
        #region attribute
        public string Url { get; protected set;}
        public string Func_name { get; protected set;}
        public Dictionary<string, string> Args { get; protected set;}
        public HttpWebRequest Request { get; protected set;}
        public Hashtable Json { get; protected set;}
        #endregion

        public PublicApis()
        {
            Url = "https://api.bitflyer.jp/v1/";
            Func_name = "";
            Args = new Dictionary<string, string>();
            Request = null;
            Json = null;
        }
    }

    public class TickerApi : PublicApis
    {
        public TickerApi()
        {
            Func_name = "ticker";

        }

        public void start()
        {
            creatRequest();
            refresh();
        }
        public double getLtpVal()
        {
            try
            {
                return Convert.ToDouble(Json["ltp"]);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private void creatRequest()
        {
            Request = WebRequestBase._createPublicRequest(Url, Func_name);
        }
        private void refresh()
        {
            if (Request != null)
            {
                Json = (Hashtable)WebRequestBase._getJsonFromRequest(Request);
            }
        }
    }
}