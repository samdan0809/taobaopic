using System;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Specialized;
namespace taobaoSellPic
{
    public class HttpSend
    {
        public HttpSend()
        { 
        }

       
        public static string postSend(string url, string param ,WebHeaderCollection headers,string  encoding)
        {
            System.Text.Encoding myEncode = System.Text.Encoding.GetEncoding(encoding);
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(param);
            
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";             
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = postBytes.Length;
            if (headers != null)
            {
                for (int i = 0; i < headers.Count; i++)
                {
                    req.Headers.Add(headers.Keys[i],headers[i]);
                }
              
            }
            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }

                using (WebResponse res = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), myEncode))
                    {
                        string strResult = sr.ReadToEnd();
                        return strResult;
                    }
                }

            }
            catch (WebException ex)
            {
                return "无法连接到服务器\r\n错误信息：" + ex.Message;
            }

        }
        public static string postSend(string url, string param)
        {
            return postSend(url, param,null, "UTF-8");
        }










        public static string getSend(string url, string param, WebHeaderCollection headers,string encoding)
        {
            System.Text.Encoding myEncode = System.Text.Encoding.GetEncoding(encoding);
             
            string address = url;
            if (param != "")
                address = address + "?" + param;
            Uri uri = new Uri(address);
            WebRequest webReq = WebRequest.Create(uri);
            if (headers != null)
            {
                for (int i = 0; i < headers.Count; i++)
                {
                    webReq.Headers.Add(headers.Keys[i], headers[i]);
                }

            }
            try
            {
                using (HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse())
                {
                    using (Stream respStream = webResp.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(respStream, myEncode))
                        {
                            string strRes = objReader.ReadToEnd();
                            return strRes;
                        }
                    }
                }

            }
            catch (WebException ex)
            {
                return "无法连接到服务器/r/n错误信息：" + ex.Message;
            }
        }      
   
        public static string getSend(string url, string param)
        {
            return HttpSend.getSend(url, param, null, "UTF-8");
        }
    }

}
