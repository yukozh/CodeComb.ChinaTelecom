﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;

namespace CodeComb.ChinaTelecom.Puller
{
    public class HttpHelper
    {
        public CookieContainer cookie = new CookieContainer();
        public string HttpPost(string Url, List<Argument> args)
        {
            string ret = string.Empty;
            try
            {
                string Param = string.Empty;
                foreach (var arg in args)
                {
                    Param += String.Format("{0}={1}&", HttpUtility.UrlEncode(arg.Key), HttpUtility.UrlEncode(arg.Value));
                }
                Param = Param.TrimEnd('&');
                byte[] byteArray = Encoding.UTF8.GetBytes(Param);
                //byte[] byteArray = Encoding.Default.GetBytes(Param);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(Url));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.CookieContainer = cookie;
                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                response.Cookies = webReq.CookieContainer.GetCookies(new Uri(Url));

                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                //StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch
            {
            }
            return ret;
        }
        public string HttpGet(string Url)
        {
            string ret = string.Empty;
            try
            {
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(Url));
                webReq.Method = "GET";
                webReq.ContentType = "text/html;charset=utf-8";
                webReq.CookieContainer = cookie;
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                //cookie = webReq.CookieContainer.GetCookies(webReq, Url);
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                //StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
            }
            catch
            {
            }
            return ret;
        }
    }
    public class Argument
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
