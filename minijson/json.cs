using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace MiniJson
{
    public class node
    {

        public node()
        {
            val = "";
        }
        public node(string value)
        {
            val = value;
        }
        public string val { get; set; }
        public void Trim()
        {
            if (val.IndexOf(" ") > -1)
                val = val.Replace(" ", "");
        }
        public node GetNode(string name, bool child=false)
        {
            try
            {
                int wz = 0, tz = 0;
                string attrName = "\"" + name + "\":";
                wz = val.IndexOf(attrName);
                if (wz == -1)
                    return null;
                string tmp;
                int rb=0, lb = 0;
                do{
                    if (rb != lb) wz = val.IndexOf(attrName,tz);
                tmp = val.Substring(wz + attrName.Length, 1);
                if (tmp != "[" && tmp != "{")
                    if (tmp == "\"")
                        tz = val.IndexOfAny(new char[] { '\"' }, wz + attrName.Length + 1);
                    else
                        tz = val.IndexOfAny(new char[] { ',', '}' }, wz);
                else
                    tz = indeOfBracket(val, tmp, wz + attrName.Length + 1) + 1;
                if (tz == -1)
                    return null;
                if (!child) 
                    break;
                lb=val.Substring(0,wz).Count(t=>t=='{');
                rb=val.Substring(0,wz).Count(t=>t=='}');
                if(val[0]=='{')
                    lb--;
                } while (rb != lb);
                //if (tmp != "\"")
                tmp = val.Substring(wz + attrName.Length, tz - attrName.Length - wz);
                //else tmp = val.Substring(wz + attrName.Length + 1, tz - attrName.Length - wz - 2);
                return new node(tmp);
            }
            catch
            {
                return null;
            }

        }
        public static int indeOfBracket(string jsonstr, string bk, int beginindex, int endindex = 0)
        {
            Stack st = new Stack();
            int wz = beginindex;
            if (bk.Length > 1)
                return -1;
            st.Push(bk);
            for (int i = 0; st.Count > 0; i++)
            {
                wz = jsonstr.IndexOfAny(new char[] { '{', '}', '[', ']' }, wz + 1);
                if (wz == -1)
                    return wz;
                if (endindex > 0 && wz > endindex)
                    break;
                string tmp = jsonstr.Substring(wz, 1);
                if (tmp != st.Peek().ToString())
                {
                    if (st.Peek().ToString() == "{" && tmp == "}")
                        st.Pop();
                    else
                        if (st.Peek().ToString() == "[" && tmp == "]")
                            st.Pop();

                }
                else
                    st.Push(tmp);
                if (i > 10000)
                    break;
            }
            if (st.Count == 0)
                return wz;
            else return -1;


        }
        public string toString()
        {
            //string result = "";
            //if (!string.IsNullOrEmpty(str))
            //{
            //    string[] strlist = str.Replace("\\u", "").Split('u');
            //    try
            //    {
            //        for (int i = 1; i < strlist.Length; i++)
            //        {
            //            //将unicode字符转为10进制整数，然后转为char中文字符  
            //            result += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
            //        }
            //    }
            //    catch (FormatException ex)
            //    {
            //        result = ex.Message;
            //    }
            //}

            if (val.IndexOf("\"") > -1)
            {
                string str = val.Replace("\"", "");
                return Regex.Unescape(str);
            }
            else return val;
        }
        public int toInt()
        {

            if (val.IndexOf("\"") > -1)
            {
                string str = val.Replace("\"", "");
                return int.Parse(str);
            }
            return int.Parse(val);
        }
        public double toDouble()
        {
            if (val.IndexOf("\"") > -1)
            {
                string str = val.Replace("\"", "");
                return double.Parse(str);
            }
            return double.Parse(val);
        }
        public DateTime toDateTime()
        {
            return DateTime.Parse(val);
        }
        public DateTime TimeStamptoDateTime()
        {
            DateTime r = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(toInt());
            return r;
        }
        public Listnode toListnode()
        {
            List<node> rlList = new List<node>();
            string[] strlist;
            List<string> strList = new List<string>();
            int st = 0, et = 0;
            string tmp = "";
            tmp = val.Substring(0, 1);
            if (tmp == "[")
                st = 1;
            else return null;
            if (val.Substring(1, 1) != "{")
            {
                tmp = val.Substring(1, val.Length - 2);
                strlist = tmp.Split(new char[] { ',' });
                new Listnode(strlist);
            }
            do
            {

                et = _MJson.indeOfBracket(val, "{", st);
                if (et == -1)
                    break;
                strList.Add(val.Substring(st, et - st + 1));
                st = val.IndexOf("{", et);
                if (st > -1)
                    et = st + 1;

            }
            while (st < et);
            return new Listnode(strList);
        }
    }
    public class Listnode
    {
        public List<node> val { get; set; }
        public Listnode(List<node> value)
        {
            val = value;
        }
        public Listnode(List<string> value)
        {
            List<node> tmp = new List<node>();
            foreach (var t in value)
            {
                tmp.Add(new node(t));
            }
            val = tmp;
        }
        public Listnode(string[] value)
        {
            List<node> tmp = new List<node>();
            foreach (var t in value)
            {
                tmp.Add(new node(t));
            }
            val = tmp;
        }


    }
    public class _MJson : node
    {
        public string json { get; set; }
        public _MJson(string jsonstr)
        {
            json = jsonstr;
            val = jsonstr;


        }
        public node IndexNode;


        public node GetNode(string name)
        {
            return base.GetNode(name);
            //int wz = 0, tz = 0;
            //string attrName = "\"" + name + "\":";
            //wz = json.IndexOf(attrName);
            //if (wz == -1)
            //    return null;
            //string tmp;
            //tmp = json.Substring(wz + attrName.Length, 1);
            //if (tmp != "[" && tmp != "{")
            //    tz = json.IndexOfAny(new char[] { ',', '}' }, wz);
            //else
            //    tz = indeOfBracket(json, tmp, wz + attrName.Length + 1) + 1;
            //if (tz == -1)
            //    return null;
            ////if (tmp != "\"")
            //try
            //{
            //    tmp = json.Substring(wz + attrName.Length, tz - attrName.Length - wz);
            //    //else tmp = json.Substring(wz + attrName.Length + 1, tz - attrName.Length - wz - 2);
            //    IndexNode = new node(tmp);
            //}
            //catch (Exception e)
            //{
            //    return null;
            //}
            //return IndexNode;

        }
        public static string getJsonByObject(Object obj)
        {
            //实例化DataContractJsonSerializer对象，需要待序列化的对象类型
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            //实例化一个内存流，用于存放序列化后的数据
            MemoryStream stream = new MemoryStream();
            //使用WriteObject序列化对象
            serializer.WriteObject(stream, obj);
            //写入内存流中
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            //通过UTF8格式转换为字符串
            return Encoding.UTF8.GetString(dataBytes);
        }
    }
}
