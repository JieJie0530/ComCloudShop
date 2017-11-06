using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.Utility.Helper
{
    public class HttpPG
    {
        public static string GetGetShuJu(string url) {
            string strRet = "";
            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.UTF8);
                strRet = ser.ReadToEnd();
            }
            catch (Exception ex)
            {
                strRet = "";
            }

            return strRet;
        }

    }
}
