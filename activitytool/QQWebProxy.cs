using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace activitytool
{
    public class QQWebProxy
    {
        public CookieContainer myCookieContainer = null;
        public Dictionary<string, string> Value_Dictionary = new Dictionary<string, string>();
        public string QQ { get { return QQ_value; } }
        private string QQ_value="";
        public QQWebProxy(CookieContainer cookie)
        {
            Uri u = new Uri("qq.com",true);
            Value_Dictionary["{cookie}"] =cookie.GetCookieHeader(u);
            myCookieContainer=cookie;
            Value_Dictionary["{pt2gguin}"] = GetCookie("pt2gguin", myCookieContainer);
            QQ_value = int.Parse(Value_Dictionary["{pt2gguin}"].Substring(1, Value_Dictionary["{pt2gguin}"].Length - 1)).ToString();
            Value_Dictionary["{QQ}"] = QQ_value;
            Value_Dictionary["{skey}"] = GetCookie("skey", myCookieContainer);
            Value_Dictionary["{gtk}"] = GetToKen(Value_Dictionary["{skey}"]);
            Value_Dictionary["{ametk}"] = ameCSRFToken(Value_Dictionary["{skey}"]);
 
        }
        public QQWebProxy(string cookie)
        {
            Value_Dictionary["{cookie}"] = cookie;
            //String 的Cookie　要转成　Cookie型的　并放入CookieContainer中  
            string[] cookstr = cookie.Split(';');
            myCookieContainer = new CookieContainer();
            foreach (string str in cookstr)
            {
                string[] cookieNameValue = str.Split('=');
                Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                ck.Domain = "qq.com";//必须写对  
                myCookieContainer.Add(ck);

                if (cookieNameValue[0].Trim().ToString() == "pt2gguin")
                    Value_Dictionary["{pt2gguin}"] = cookieNameValue[1].Trim().ToString();
                if (cookieNameValue[0].Trim().ToString() == "skey")
                {
                    Value_Dictionary["{skey}"] = cookieNameValue[1].Trim().ToString();
                    Value_Dictionary["{gtk}"] = GetToKen(cookieNameValue[1].Trim().ToString());
                    Value_Dictionary["{ametk}"] = ameCSRFToken(cookieNameValue[1].Trim().ToString());
                }
            }
            QQ_value = long.Parse(Value_Dictionary["{pt2gguin}"].Substring(1, Value_Dictionary["{pt2gguin}"].Length - 1)).ToString();
            Value_Dictionary["{QQ}"] = QQ_value;
            

        }
        /// <summary>
        /// 获取Cookie的值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cc">Cookie集合对象</param>
        /// <returns>返回Cookie名称对应值</returns>
        public static string GetCookie(string cookieName, CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c1 in colCookies) lstCookies.Add(c1);
            }
            var model = lstCookies.Find(p => p.Name == cookieName);
            if (model != null)
            {
                return model.Value;
            }
            return string.Empty;
        }

        public static string StrToMD5(string str)
        {
            byte[] data = Encoding.GetEncoding("GB2312").GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] OutBytes = md5.ComputeHash(data);

            string OutString = "";
            for (int i = 0; i < OutBytes.Length; i++)
            {
                OutString += OutBytes[i].ToString("x2");
            }
            // return OutString.ToUpper();
            return OutString.ToLower();
        }
        public string GetToKen(string key)
        {
            string skey = key;
            int salt = 5381, ASCIICode;
            string md5key = "tencentQQVIP123443safde&!%^%1282";
            List<string> hash = new List<string>();
            hash.Add((salt << 5).ToString());
            for (int i = 0, len = skey.Length; i < len; ++i)
            {
                ASCIICode = (short)skey.ToCharArray().ElementAt(i);
                hash.Add(((salt << 5) + ASCIICode).ToString());
                salt = ASCIICode;
            }
            string md5str = string.Join("", hash) + md5key;
            return StrToMD5(md5str);
        }
        public string ameCSRFToken(string key)
        {
            //var sAMEStr = milo.cookie.get("skey") || "a1b2c3";
            string skey = key;
            int hash = 5381, ASCIICode;

            for (int i = 0, len = skey.Length; i < len; ++i)
            {
                ASCIICode = (short)skey.ToCharArray().ElementAt(i);
                hash += (hash << 5) + ASCIICode;
            }
            string rul = (hash & 2147483647).ToString();
            return rul;
        }

        public void pt_logout_getCookie(string name)
        {
            Match m=new Regex("(^|;\\s*)" + name + "=([^;]*)(;|$)").Match(Value_Dictionary["{cookie}"]);
             //= Regex.Match(, );
            if(m.Success)
            {
                
                string result = "";
                string o = name;
                for (; o != System.Web.HttpUtility.UrlDecode(o); )
                    o = System.Web.HttpUtility.UrlDecode(o);
                string[] t = { "<", ">", "'", "\"", "%3c", "%3e", "%27", "%22", "%253c", "%253e", "%2527", "%2522" };
                string[] n = { "&#x3c;", "&#x3e;", "&#x27;", "&#x22;", "%26%23x3c%3B", "%26%23x3e%3B", "%26%23x27%3B", "%26%23x22%3B", "%2526%2523x3c%253B", "%2526%2523x3e%253B", "%2526%2523x27%253B", "%2526%2523x22%253B" };
                for (int e = 0; e < t.Count(); e++)
                    o = o.Replace(new Regex(t[e],RegexOptions.IgnoreCase).ToString(),n[e]);
            }

        }

        public static string SendDataByPost(string Url, string postDataStr)
        {
            CookieContainer tcookie = new CookieContainer();
            return SendDataByPost(Url, postDataStr, ref tcookie);
        }
        #region 同步通过POST方式发送数据
        /// <summary>
        /// 通过POST方式发送数据
        /// </summary>
        /// <param name="Url">url</param>
        /// <param name="postDataStr">Post数据</param>
        /// <param name="cookie">Cookie容器</param>
        /// <returns></returns>
        public static string SendDataByPost(string Url, string postDataStr, ref CookieContainer cookie, string Host = "", string Referer = "", string User_Agent = "", string addcookies = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }
            if (Host != "")
                request.Host = Host;
            if (Referer != "")
                request.Referer = Referer;
            if (User_Agent != "")
                request.UserAgent = User_Agent;
            if (addcookies != "")
            {
                string[] cookstr = addcookies.Split(';');
                foreach (string str in cookstr)
                {
                    string[] cookieNameValue = str.Split('=');
                    Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                    ck.Domain = Host;//必须写对  
                    request.CookieContainer.Add(ck);
                }
            }

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        #endregion
        public static string SendDataByGET(string Url, string postDataStr)
        {
            CookieContainer tcookie = new CookieContainer();
            return SendDataByGET(Url, postDataStr,ref tcookie);
        }
        #region 同步通过GET方式发送数据
        /// <summary>
        /// 通过GET方式发送数据
        /// </summary>
        /// <param name="Url">url</param>
        /// <param name="postDataStr">GET数据</param>
        /// <param name="cookie">GET容器</param>
        /// <returns></returns>
        public static string SendDataByGET(string Url, string postDataStr, ref CookieContainer cookie, string Host = "", string Referer = "", string User_Agent = "", string addcookies="")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }
            request.Accept = "*/*";
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            //request.Host = "apps.game.qq.com";
            //request.Referer = "http://dnf.qq.com/act/a20130805weixin/cdkey.htm?bg=per";
            if (Host != "")
                request.Host = Host;
            if (Referer != "")
                request.Referer = Referer;
            if (User_Agent != "")
                request.UserAgent = User_Agent;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        #endregion


        /// <summary>   
        /// 下载验证码图片并保存到本地   
        /// </summary>   
        /// <param name="Url">验证码URL</param>   
        /// <param name="cookCon">Cookies值</param>   
        /// <param name="savePath">保存位置/文件名</param>   
        public static Bitmap DowloadCheckImg(string Url, CookieContainer cookCon)
        {
            Bitmap sourcebm = null;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);
            //属性配置   
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            webRequest.MaximumResponseHeadersLength = -1;
            webRequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "GET";
            webRequest.Headers.Add("Accept-Language", "zh-cn");
            webRequest.Headers.Add("Accept-Encoding", "gzip,deflate");
            webRequest.KeepAlive = true;
            webRequest.CookieContainer = cookCon;
            try
            {
                //获取服务器返回的资源   
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (Stream sream = webResponse.GetResponseStream())
                    {
                        //List<byte> list = new List<byte>();
                        //while (true)
                        //{
                        //    int data = sream.ReadByte();
                        //    if (data == -1)
                        //        break;
                        //    list.Add((byte)data);
                        //}
                        //File.WriteAllBytes(savePath, list.ToArray());
                        sourcebm = new Bitmap(sream);
                    }
                }
            }
            catch (WebException ex)
            {

            }
            catch (Exception ex)
            {

            }
            return sourcebm;
        }
        /// <summary>
        /// unicode转中文（符合js规则的）
        /// </summary>
        /// <returns></returns>
        public static string unicode_js_1(string str)
        {
            string outStr = "";
            Regex reg = new Regex(@"(?i)\\u([0-9a-f]{4})");
            outStr = reg.Replace(str, delegate(Match m1)
            {
                return ((char)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString();
            });
            return outStr;
        }
        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

    }
}
