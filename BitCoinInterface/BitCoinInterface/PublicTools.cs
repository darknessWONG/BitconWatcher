using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Security;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BitCoinInterface
{
    /* 价格状态
     * 用以表示价格升跌状态的枚举类型。
     * noStatus只有在初始化是才会使用。
     **/
    public enum StockStatus
    {
        noStatus,
        up,
        down
    }
    public enum ExchangeStatus
    {
        doNothing,
        buy,
        sell
    }

    public class ErrorCounter
    {
        private bool[] counter;
        private int counterNum;
        private int toleranceNum;
        private int currentPointer;

        public ErrorCounter(int counterNum, int toleranceNum)
        {
            if (counterNum < toleranceNum)
            {
                throw new ErrorCounterException();
            }
            this.counterNum = counterNum;
            this.toleranceNum = toleranceNum;
            currentPointer = 0;
            counter = Enumerable.Repeat<bool>(true, counterNum).ToArray();
        }

        public bool AddRecord(bool Record)
        {
            counter[currentPointer] = Record;
            currentPointer = ++currentPointer % counterNum;
            return GetNowStatus();
        }
        public bool GetNowStatus()
        {
            int count = 0;
            for (int i = 0; i < counterNum; i++)
            {
                if (!counter[i])
                {
                    count++;
                }
            }
            if (count >= toleranceNum)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public static class XmlConfigHandler
    {
        public static void _SaveConfigXml(Hashtable datas, string fileName)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlElement xEle = xDoc.CreateElement("config");
            xDoc.AppendChild(_createXmlFromHashtable(xDoc, xEle, datas));
            xDoc.Save(fileName);
        }
        public static Hashtable _LoadConfigXml(string xmlFile)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlFile);
            XmlNode xNode = xDoc.SelectSingleNode("config");
            return _createHashtableFromXml(xNode, new Hashtable());
        }

        private static XmlElement _createXmlFromHashtable(XmlDocument xDoc, XmlElement parent, Hashtable datas)
        {
            foreach (string key in datas.Keys)
            {
                if (datas[key].GetType().Name == "text".GetType().Name)
                {
                    XmlElement xChild = xDoc.CreateElement(key);
                    xChild.InnerText = (string)datas[key];
                    parent.AppendChild(xChild);
                }
                else if (datas[key].GetType().Name == new Hashtable().GetType().Name)
                {
                    XmlElement xChild = xDoc.CreateElement(key);
                    parent.AppendChild(_createXmlFromHashtable(xDoc, xChild, (Hashtable)datas[key]));
                }
            }
            return parent;
        }
        private static Hashtable _createHashtableFromXml(XmlNode xNode, Hashtable datas)
        {
            foreach (XmlNode node in xNode)
            {
                if (node.HasChildNodes && node.FirstChild.Name != "#text")
                {
                    datas[node.Name] = _createHashtableFromXml(node, new Hashtable());
                }
                else
                {
                    datas[node.Name] = node.InnerText;
                }
            }
            return datas;
        }
    }

    public static class MathTool
    {
        public static double _RetainDecimal(double num, int decNum)
        {
            int a = (int)(num * Math.Pow(10, decNum));
            return a / Math.Pow(10, decNum);
        }

        public static string _SignWithHMACSHA256(string data, string secret)
        {
            using (var encoder = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                var hash = encoder.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }
    }



}

namespace Newtonsoft.Json.Linq
{
    public static class BJArray
    {
        public static Hashtable ToHashtable(this JArray datas)
        {
            Hashtable returnValue = new Hashtable();
            int i = 0;
            foreach (JObject data in datas)
            {
                Hashtable tmp = new Hashtable();
                foreach (JProperty pro in data.Properties())
                {
                    tmp[pro.Name] = data[pro.Name].ToString();
                }
                returnValue[i++] = tmp;
            }
            return returnValue;
        }
    }

    public static class BJObject
    {
        public static Hashtable ToHashtable(this JObject data)
        {
            Hashtable returnValue = new Hashtable();
            foreach (JProperty pro in data.Properties())
            {
                returnValue[pro.Name] = data[pro.Name].ToString();
            }
            return returnValue;
        }
    }
}

