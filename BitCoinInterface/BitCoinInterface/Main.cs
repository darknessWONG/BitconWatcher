using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using System.Timers;
using System.Collections;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;

namespace BitCoinInterface
{
    public class BitCoinInterface
    {
        static void Main(String[] args)
        {
            //string metod = "GET";
            //string timestamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();

            //string path = "/v1/me/getbalance";
            //string body = "";
            //string apiSecret = "Gt/V0lPpY5e/DUXmTTHaQCKbOZT0ikT5ahYyjjrGIio=";
            //string apiKey = "8jnAgjEnjZ9gj9YCU8wEir";
            //string data = timestamp + metod + path + body;

            //StringBuilder url = new StringBuilder("https://api.bitflyer.jp/v1/me/getbalance");
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.ToString());
            //var hash = MathTool._SignWithHMACSHA256(data, apiSecret);
            //Console.WriteLine(hash);
            //request.Headers.Add("ACCESS-KEY", apiKey);
            //request.Headers.Add("ACCESS-TIMESTAMP", timestamp);
            //request.Headers.Add("ACCESS-SIGN", hash);

            //HttpWebResponse respone = (HttpWebResponse)request.GetResponse();
            //Stream stream = respone.GetResponseStream();
            //StreamReader responeStreamReader = new StreamReader(stream);
            ////string re = responeStreamReader.ReadToEnd();
            //JArray datas = JsonConvert.DeserializeObject<JArray>(responeStreamReader.ReadToEnd());

            //Hashtable aaa = datas.ToHashtable();
            //foreach(int key in aaa.Keys)
            //{
            //    Console.WriteLine(((Hashtable)aaa[key])["currency_code"]);
            //}

            BitCoinStock a = new BitCoinStock();
            a.MainLoop();



            Console.ReadKey();




        }



    }

}
