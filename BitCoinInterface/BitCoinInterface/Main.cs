using System;
using System.Threading;
using System.Xml;

namespace BitCoinInterface
{
    public class BitCoinInterface
    {
        static void Main(String[] args)
        {
            //var request = WebRequestBase._createPublicRequest("https://api.bitflyer.jp/v1/", "ticker");
            //var datas = WebRequestBase._getJsonFromRequest(request);
            //Console.WriteLine(datas["ltp"]);
            //Console.ReadKey();
            //var obj = new Ticker();
            //obj.start();
            //Console.WriteLine(obj.getLtpVal());
            ////Thread.Sleep(15000);
            //obj.start();
            //Console.WriteLine(obj.getLtpVal());
            //Console.ReadKey();
            XmlDocument xDoc = new XmlDocument();
            XmlElement xEle = xDoc.CreateElement("config");
            XmlElement xChile = xDoc.CreateElement("UpThreshold");
            xChile.InnerText = "0.04";
            xEle.AppendChild(xChile);
            xDoc.AppendChild(xEle);
            xDoc.Save("config.xml");


        }
    }

}
