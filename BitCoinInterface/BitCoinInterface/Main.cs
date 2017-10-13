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
            BitCoinStock a = new BitCoinStock("config.xml");
            a.mainLoop();


        }
    }

}
