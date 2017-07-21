using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;

namespace actini
{
    public partial class iniForm : Form
    {
        public List<actinfo> tmpactinfoList;
        public List<int> tmpactinfoList_C;
        public iniForm()
        {
            InitializeComponent();
        }
        private atcDate atc = new atcDate();
        private void button1_Click(object sender, EventArgs e)
        {
            AddAtc("【DNF】男法师觉醒，赢超值福利", 171790, 1490630400, 1493567996, "iyouxi.vip.qq.com", "http://vip.qq.com/club/act/2017/2000179524/6637060b9c.html", "雷米×10", 1);
            AddAtc("10分钟", 171790, 1490630400, 1493567996, "iyouxi.vip.qq.com", "http://vip.qq.com/club/act/2017/2000179524/6637060b9c.html", "雷米×10", 1, 0);
            //AddAtc("10分钟", 171790, 1490630400, 1493567996, "iyouxi.vip.qq.com", "http://vip.qq.com/club/act/2017/2000179524/6637060b9c.html", "雷米×10", 1, "0,0");
            //AddAtc("10分钟", 171790, 1490630400, 1493567996, "iyouxi.vip.qq.com", "http://vip.qq.com/club/act/2017/2000179524/6637060b9c.html", "雷米×10", 1, "0,0,0");
        }
        public void AddAtc(string atcname, int actid, int start_time, int end_time, string Host, string Referer, string giftname, int model, int index = -1)
        {
            AddAtc(new actinfo() { actname = atcname, actid = actid, start_time = start_time, end_time = end_time, Host = Host, Referer = Referer, giftname = giftname, model = model }, index);
        }
        public void AddAtc(actinfo info, int index = -1)
        {
            if (index == -1)
            {
                atc.Date.Add(info);
                listBox1.Items.Add(info.actname);
                refresh();
            }
            else
            {
                if (tmpactinfoList[index].atcExt == null)
                {
                    tmpactinfoList[index].atcExt = new List<actinfo>();
                }
                tmpactinfoList[index].atcExt.Add(info);
                //string itstr = " └" + info.actname;
                //if (tmpactinfoList_C[index] > 0)
                //{
                //    for (int i = 0; i < tmpactinfoList_C[index]; i++) itstr = " " + itstr;

                //}

                //listBox1.Items.Add(itstr);
                refresh();
                //string[] indexs = index.Split(new char[] { ',' });
                //List<actinfo> tmp = atc.Date;
                //int tmpindex = 0;
                //tmpindex = int.Parse(indexs[0]);
                //if (tmp[tmpindex].atcExt == null)
                //{
                //    tmp[tmpindex].atcExt = new List<actinfo>();
                //    tmp = tmp[tmpindex].atcExt;

                //}
                //else
                //{
                //    tmp = tmp[tmpindex].atcExt;
                //}
                //string itstr = " └" + info.actname;
                //for (int i = 1; i < indexs.Length; i++)
                //{
                //    tmpindex = int.Parse(indexs[i]);
                //    if (tmp[tmpindex].atcExt == null)
                //    {
                //        tmp[tmpindex].atcExt = new List<actinfo>();
                //        tmp = tmp[tmpindex].atcExt;

                //    }
                //    else
                //    {
                //        tmp = tmp[tmpindex].atcExt;
                //    }
                //    itstr = " " + itstr;

                //}

                //tmp.Add(info);
                //listBox1.Items.Add(itstr);

            }
        }

        private void iniForm_Load(object sender, EventArgs e)
        {

            atc.ver = "1.1";
            atc.Date = new List<actinfo>();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            atc.ver = textBox1.Text;
            atc.adlink = textBox2.Text;
            FileStream fs = new FileStream("atc.ini", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            string aa = JsonConvert.SerializeObject(atc);
            sw.Write(aa);
            sw.Flush();

            //清空缓冲区

            //关闭流
            sw.Close();
            fs.Close();
            MessageBox.Show("保存成功！！！");

        }



        private void button6_Click(object sender, EventArgs e)
        {
            string str = File.ReadAllText(Application.StartupPath + "\\atc.ini");
            atc = JsonConvert.DeserializeObject<atcDate>(str);
            textBox1.Text = atc.ver;
            textBox2.Text = atc.adlink;
            refresh();
        }
        static int c = 0;
        public void refresh()
        {
            listBox1.Items.Clear();
            tmpactinfoList = new List<actinfo>();
            tmpactinfoList_C = new List<int>();
            refresh(atc.Date);

        }
        public void refresh(List<actinfo> tmp, string q = "")
        {


            if (q != "")
                q = "  " + q;
            if (tmp == null)
                return;
            foreach (var v in tmp)
            {
                listBox1.Items.Add(q + v.actname);
                tmpactinfoList.Add(v);
                tmpactinfoList_C.Add(c);

                if (v.atcExt != null)
                {
                    c++;
                    if (q == "")
                        refresh(v.atcExt, "  └");
                    else
                        refresh(v.atcExt, q);
                    c--;
                }
            }


        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return;
            Form2 f2 = new Form2(tmpactinfoList[listBox1.SelectedIndex], 2, listBox1.SelectedIndex);
            f2.ShowDialog(this);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                Form2 f2 = new Form2();
                f2.ShowDialog(this);
            }
            else
            {
                Form2 f2 = new Form2(tmpactinfoList[listBox1.SelectedIndex], 0);
                f2.ShowDialog(this);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;
            Form2 f2 = new Form2(tmpactinfoList[listBox1.SelectedIndex], 1, listBox1.SelectedIndex);
            f2.ShowDialog(this);
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;
            Form2 f2 = new Form2(tmpactinfoList[listBox1.SelectedIndex], 2);
            f2.ShowDialog(this);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;
            int i = -1;
            i = atc.Date.IndexOf(tmpactinfoList[listBox1.SelectedIndex]);
            if (i > -1)
            {
                atc.Date.RemoveAt(i);
                refresh();
                return;
            }
            foreach (var v in tmpactinfoList)
            {
                if (v.atcExt == null)
                    continue;
                i = v.atcExt.IndexOf(tmpactinfoList[listBox1.SelectedIndex]);
                if (i > -1)
                {
                    v.atcExt.RemoveAt(i);
                    refresh();
                    return;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;
            int i = -1;
            int index = listBox1.SelectedIndex;
            i = atc.Date.IndexOf(tmpactinfoList[listBox1.SelectedIndex]);
            if (i > -1)
            {
                if (i > 0)
                {
                    atc.Date.RemoveAt(i);
                    atc.Date.Insert(i - 1, tmpactinfoList[listBox1.SelectedIndex]);
                    refresh();
                    listBox1.SelectedIndex = index - 1;
                }
                return;
            }
            foreach (var v in tmpactinfoList)
            {
                if (v.atcExt == null)
                    continue;
                i = v.atcExt.IndexOf(tmpactinfoList[listBox1.SelectedIndex]);
                if (i > -1)
                {
                    if (i > 0)
                    {
                        v.atcExt.RemoveAt(i);
                        v.atcExt.Insert(i - 1, tmpactinfoList[listBox1.SelectedIndex]);
                        refresh();
                        listBox1.SelectedIndex = index - 1;
                    }
                    return;
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;
            int i = -1;
            int index = listBox1.SelectedIndex;
            i = atc.Date.IndexOf(tmpactinfoList[listBox1.SelectedIndex]);
            if (i > -1)
            {
                if (i < atc.Date.Count - 1)
                {
                    atc.Date.RemoveAt(i);
                    atc.Date.Insert(i + 1, tmpactinfoList[listBox1.SelectedIndex]);
                    refresh();
                    listBox1.SelectedIndex = index + 1;
                }
                return;
            }
            foreach (var v in tmpactinfoList)
            {
                if (v.atcExt == null)
                    continue;
                i = v.atcExt.IndexOf(tmpactinfoList[listBox1.SelectedIndex]);
                if (i > -1)
                {
                    if (i < v.atcExt.Count - 1)
                    {
                        v.atcExt.RemoveAt(i);
                        v.atcExt.Insert(i + 1, tmpactinfoList[listBox1.SelectedIndex]);
                        refresh();
                        listBox1.SelectedIndex = index + 1;
                    }
                    return;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            atc.ver = textBox1.Text;
            atc.adlink = textBox2.Text;
            string aa = JsonConvert.SerializeObject(atc);
            if(Post("http://www.tx5d.com/api/p.ashx",aa)=="ok")
                MessageBox.Show("上传成功！！！");
            else MessageBox.Show("错误！！！");
                


        }

        public string Post(string Url, string jsonParas)
        {
            string strURL = Url;

            //创建一个HTTP请求  
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            //Post请求方式  
            request.Method = "POST";
            //内容类型
            request.ContentType = "application/x-www-form-urlencoded";

            //设置参数，并进行URL编码 

            string paraUrlCoded = jsonParas;//System.Web.HttpUtility.UrlEncode(jsonParas);   

            byte[] payload;
            //将Json字符串转化为字节  
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            //设置请求的ContentLength   
            request.ContentLength = payload.Length;
            //发送请求，获得请求流 

            Stream writer;
            try
            {
                writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
            }
            catch (Exception)
            {
                writer = null;
                Console.Write("连接服务器失败!");
            }
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            writer.Close();//关闭请求流

            String strValue = "";//strValue为http响应所返回的字符流
            HttpWebResponse response;
            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }

            Stream s = response.GetResponseStream();


            //Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(s);
            string postContent = sRead.ReadToEnd();
            sRead.Close();


            return postContent;//返回Json数据
        }

    }
}
