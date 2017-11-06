using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace ComCloudShop.WeixinOauth.Pay.Helper
{
    public class HttpClientHelper
    {
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetResponse(string url)
        {
            string returnValue = string.Empty;
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
            webReq.Method = "GET";
            webReq.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            returnValue = streamReader.ReadToEnd();
            //关闭信息
            streamReader.Close();
            response.Close();

            return returnValue;
        }


        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        //public static string GetResponse(string url)
        //{
        //    HttpClient httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Accept.Add(
        //       new MediaTypeWithQualityHeaderValue("application/json"));
        //    HttpResponseMessage response = httpClient.GetAsync(url).Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string result = response.Content.ReadAsStringAsync().Result;
        //        return result;
        //    }
        //    return null;
        //}

        //public static T GetResponse<T>(string url)
        //    where T : class,new()
        //{

        //    string returnValue = string.Empty;
        //    HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
        //    webReq.Method = "GET";
        //    webReq.ContentType = "application/json";
        //    HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
        //    StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.Default);
        //    returnValue = streamReader.ReadToEnd();
        //    //关闭信息
        //    streamReader.Close();
        //    response.Close();
        //    T result = default(T);
        //    result = JsonConvert.DeserializeObject<T>(returnValue);
        //    return result;

        //    //HttpClient httpClient = new HttpClient();

        //    //httpClient.DefaultRequestHeaders.Accept.Add(
        //    //   new MediaTypeWithQualityHeaderValue("application/json"));

        //    //HttpResponseMessage response = httpClient.GetAsync(url).Result;

        //    //T result = default(T);

        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    Task<string> t = response.Content.ReadAsStringAsync();
        //    //    string s = t.Result;

        //    //    result = JsonConvert.DeserializeObject<T>(s);
        //    //}
        //    //return result;

        //}

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static string PostResponse(string url, string postData)
        {
            //HttpContent httpContent = new StringContent(postData);
            //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //HttpClient httpClient = new HttpClient();

            //HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            //if (response.IsSuccessStatusCode)
            //{
            //    string result = response.Content.ReadAsStringAsync().Result;
            //    return result;
            //}
            //return null;

            string returnValue = string.Empty;
            byte[] byteData = Encoding.UTF8.GetBytes(postData);
            Uri uri = new Uri(url);
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);
            webReq.Method = "POST";
            webReq.ContentType = "application/json";
            webReq.ContentLength = byteData.Length;
            //定义Stream信息
            Stream stream = webReq.GetRequestStream();
            stream.Write(byteData, 0, byteData.Length);
            stream.Close();
            //获取返回信息
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            returnValue = streamReader.ReadToEnd();
            //关闭信息
            streamReader.Close();
            response.Close();
            stream.Close();
            return returnValue;
        }

        /// <summary>
        /// 发起post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">url</param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        //public static T PostResponse<T>(string url, string postData)
        //    where T : class,new()
        //{
        //    //HttpContent httpContent = new StringContent(postData);
        //    //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    //HttpClient httpClient = new HttpClient();

        //    //T result = default(T);

        //    //HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    Task<string> t = response.Content.ReadAsStringAsync();
        //    //    string s = t.Result;

        //    //    result = JsonConvert.DeserializeObject<T>(s);
        //    //}
        //    string returnValue = string.Empty;
        //    byte[] byteData = Encoding.UTF8.GetBytes(postData);
        //    Uri uri = new Uri(url);
        //    HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);
        //    webReq.Method = "POST";
        //    webReq.ContentType = "application/json";
        //    webReq.ContentLength = byteData.Length;
        //    //定义Stream信息
        //    Stream stream = webReq.GetRequestStream();
        //    stream.Write(byteData, 0, byteData.Length);
        //    stream.Close();
        //    //获取返回信息
        //    HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
        //    StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.Default);
        //    returnValue = streamReader.ReadToEnd();
        //    //关闭信息
        //    streamReader.Close();
        //    response.Close();
        //    stream.Close();
        //    T result = default(T);
        //    result = JsonConvert.DeserializeObject<T>(returnValue);
        //    return result;
        //}

        /// <summary>
        /// V3接口全部为Xml形式，故有此方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T PostXmlResponse<T>(string url, string xmlString)
            where T : class,new()
        {
            string returnValue = string.Empty;
            byte[] byteData = Encoding.UTF8.GetBytes(xmlString);
            Uri uri = new Uri(url);
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);
            webReq.Method = "POST";
            webReq.ContentType = "application/json";
            webReq.ContentLength = byteData.Length;
            //定义Stream信息
            Stream stream = webReq.GetRequestStream();
            stream.Write(byteData, 0, byteData.Length);
            stream.Close();
            //获取返回信息
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            returnValue = streamReader.ReadToEnd();
            //关闭信息
            streamReader.Close();
            response.Close();
            stream.Close();
            T result = default(T);
            result = XmlDeserialize<T>(returnValue);
            return result;

            //HttpContent httpContent = new StringContent(xmlString);
            //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //HttpClient httpClient = new HttpClient();

            //T result = default(T);

            //HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            //if (response.IsSuccessStatusCode)
            //{
            //    Task<string> t = response.Content.ReadAsStringAsync();
            //    string s = t.Result;

            //    result = XmlDeserialize<T>(s);
            //}
            //return result;
        }

        /// <summary>
        /// 反序列化Xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xmlString)
            where T : class,new()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (StringReader reader = new StringReader(xmlString))
                {
                    return (T)ser.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("XmlDeserialize发生异常：xmlString:" + xmlString + "异常信息：" + ex.Message);
            }

        }
    }
}
