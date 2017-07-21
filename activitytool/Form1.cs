using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Threading;
using MiniJson;

namespace activitytool
{
    public partial class Form1 : Form
    {


        string cookieStr = "";
        DNFRoleInfo userinfo = null;
        Listnode u_node = null;
        //在WebBrowser中登录cookie保存在WebBrowser.Document.Cookie中      
        CookieContainer myCookieContainer = null;
        List<node> actnodeList = null;
        List<int> actnodeList_C = null;
        string userid = "";
        string gtk = "";
        string ametk = "";
        Dictionary<string, string> sSDIDList = new Dictionary<string, string>();
        string sSDID = "";
        List<int> atcid = null;
        int index = -1;
        string checkparam = "";
        string md5str = "";
        List<activitytool.DNFHelper.ServerSelect> svlist = null;
        string area = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        /// 设置cookie
        ///
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
        ///
        /// 获取cookie
        ///
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetGetCookie(string url, string name, StringBuilder data, ref int dataSize);

        delegate void SetTextCallback(string text);
        delegate void SetgroupBoxCallback(bool c);
        delegate void SetpicCallback(Bitmap c);

        public void SetText(string text)
        {
            if (this.textBox3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox3.Text = text;
            }
        }
        public void SetgroupBox(bool c)
        {
            if (this.groupBox2.InvokeRequired)
            {
                SetgroupBoxCallback d = new SetgroupBoxCallback(SetgroupBox);
                this.Invoke(d, new object[] { c });
            }
            else
            {
                this.groupBox2.Enabled = c;
            }
        }
        public void SetPIC(Bitmap c)
        {
            if (this.pictureBox1.InvokeRequired)
            {
                SetpicCallback d = new SetpicCallback(SetPIC);
                this.Invoke(d, new object[] { c });
            }
            else
            {
                this.pictureBox1.Image = c;
            }
        }

        public string http_GET(string strURL, CookieContainer CookieContainer)
        {
            System.Net.HttpWebRequest request;
            // 创建一个HTTP请求
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.CookieContainer = CookieContainer;
            //request.Method="get";
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            myreader.Close();
            return responseText;

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
        private void button1_Click(object sender, EventArgs e)
        {
            if (cookieStr == "")
            {
                MessageBox.Show("请先登录！！！");
                return;
            }


            string aa = http_GET("http://iyouxi.vip.qq.com/ams3.0.php?_c=queryRoleInfo&gamename=dnf&area="+area+"&g_tk=" + gtk, myCookieContainer);
            //string aa = "json14848016790871({\"ret\":0,\"data\":[{\"role_id\":\"16705750\",\"nick\":\"Asura\\u4e36\\u4e44\"},{\"role_id\":\"17521661\",\"nick\":\"\\u9694\\u58c1[\\u738b\\u5927\\u67aa]\"},{\"role_id\":\"26980759\",\"nick\":\"\\u5929\\u9009\\u4e44\\u7f14\\u9020\"},{\"role_id\":\"28160539\",\"nick\":\"\\u6218\\u6597\\u5723\\u7075\"},{\"role_id\":\"28460688\",\"nick\":\"\\u529b\\u5c48\\u5929\\u4e0b\"},{\"role_id\":\"28549736\",\"nick\":\"\\u6613\\u6b66\\u8bed\"},{\"role_id\":\"28604163\",\"nick\":\"\\u4e73\\u6c41\\u306e\\u9a91\\u58eb\"},{\"role_id\":\"28894325\",\"nick\":\"\\u6c14\\u4e44\\u6b7b\\u4f60\"},{\"role_id\":\"29047949\",\"nick\":\"\\u8d64\\u4e36\\u72ac\"},{\"role_id\":\"29165134\",\"nick\":\"+38\\u7684[\\u51ef\\u4e3d]\"},{\"role_id\":\"29799370\",\"nick\":\"\\u540e\\u8857\\u516c\\u4e3b\"},{\"role_id\":\"30117053\",\"nick\":\"\\u5723\\u804c\\u8005\\u662f\\u6211\"},{\"role_id\":\"30117056\",\"nick\":\"\\u5723\\u804c\\u8005\\u771f\\u662f\\u6211\"},{\"role_id\":\"30119531\",\"nick\":\"\\u6211\\u771f\\u662f\\u5723\\u804c\\u8005\"},{\"role_id\":\"30166714\",\"nick\":\"\\u7b2c\\u4e94\\u5251\\u5723\\u00b7\\u7edd\"},{\"role_id\":\"30173360\",\"nick\":\"\\u4f26\\u5bb6\\u4e5f\\u80fd\\u5c04\"},{\"role_id\":\"30305005\",\"nick\":\"\\u9020\\u7891\\u8005\\u00b7\\u5239\"},{\"role_id\":\"30307230\",\"nick\":\"\\u6211\\u662f\\u9ed1\\u6b66\"},{\"role_id\":\"30436728\",\"nick\":\"\\u9b54\\u4e44\\u9053\"},{\"role_id\":\"30843469\",\"nick\":\"\\u72d7\\u6b87\\u786a\\u4f60\\u4e0d\\u914d\"},{\"role_id\":\"31209434\",\"nick\":\"\\u821e\\u6c14\\u5927\\u6e7f\"},{\"role_id\":\"31283367\",\"nick\":\"\\u5361\\u5361\\u7f57\\u7279\\u00b7\\u56f8\"},{\"role_id\":\"31373443\",\"nick\":\"\\u4f60\\u6cd5\\u7237\"},{\"role_id\":\"31427057\",\"nick\":\"\\u65e0\\u6cd5\\u51b0\\u51bb\"},{\"role_id\":\"31506914\",\"nick\":\"\\u620f\\u8c82\\u8749\"},{\"role_id\":\"31627023\",\"nick\":\"\\u5730\\u72f1\\u00b7\\u53ec\\u5524\"},{\"role_id\":\"31935605\",\"nick\":\"[\\u5c0f\\u67d2]\"},{\"role_id\":\"32071049\",\"nick\":\"\\u9ea6\\u57ce\\u5173\\u7fbd\"},{\"role_id\":\"33685115\",\"nick\":\"\\u65e0\\u540d\\u7684\\u673a\\u68b0\\u5e08\"}],\"time\":\"1484805186\",\"msg\":\"success\"});";
            //int sta = aa.IndexOf("({") + 1;
            //string json = aa.Substring(sta, aa.IndexOf("});") - sta + 1);
            //userinfo = JsonConvert.DeserializeObject<DNFRoleInfo>(aa);
            userinfo = new DNFRoleInfo();
            _MJson m = new _MJson(aa);
            u_node = m.GetNode("data").toListnode();

            StringBuilder tmp = new StringBuilder();
            tmp.Append("dnf%257Cyes%257C" + userid + "%257C" + area + "%257C");

            foreach (var v in u_node.val)
            {
                comboBox1.Items.Add(v.GetNode("nick").toString());
                tmp.Append(v.GetNode("role_id").toString() + "*");
                //comboBox1.Items.Add(v.nick);
                //tmp.Append(v.role_id + "*");
            }
            if (comboBox1.Items.Count > 1)
            {
                comboBox1.SelectedIndex = 0;
                checkparam = tmp.ToString();
                //MessageBox.Show("获取角色成功");
            }
            else MessageBox.Show("该区找不到角色，或者出现错误");

            //MessageBox.Show(aa);
            //MessageBox.Show(cookieStr);


        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.Url.ToString().IndexOf("http://imgcache.qq.com/qv/file/qq/login.loginProxy_1.x.x.html") == 0)
            {
                webBrowser1.Navigate("http://dnf.qq.com/act/a20130805weixin/cdkey.htm?bg=pe");
                groupBox1.Visible = true;
                cookieStr = webBrowser1.Document.Cookie;
                //MessageBox.Show(cookieStr);
                myCookieContainer = new CookieContainer();
                //String 的Cookie　要转成　Cookie型的　并放入CookieContainer中  
                string[] cookstr = cookieStr.Split(';');

                foreach (string str in cookstr)
                {
                    string[] cookieNameValue = str.Split('=');
                    Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                    if (cookieNameValue[0].Trim().ToString() == "pt2gguin")
                        userid = cookieNameValue[1].Trim().ToString();
                    if (cookieNameValue[0].Trim().ToString() == "skey")
                    {
                        gtk = GetToKen(cookieNameValue[1].Trim().ToString());
                        ametk = ameCSRFToken(cookieNameValue[1].Trim().ToString());
                    }
                    ck.Domain = "qq.com";//必须写对  
                    myCookieContainer.Add(ck);
                }
                userid = int.Parse(userid.Substring(1, userid.Length - 1)).ToString();
                label1.Text = userid;
                if (File.Exists(userid+".ini"))
                {
                    string[] tmpstr = File.ReadAllLines(userid + ".ini");
                    comboBox2.SelectedIndex = int.Parse(tmpstr[0]);
                    comboBox3.SelectedIndex = int.Parse(tmpstr[1]);
 
                }
                //comboBox3_SelectedIndexChanged(null,null);
                //button1_Click(null, null);
                webBrowser1.ScriptErrorsSuppressed = true;
                //webBrowser1.Visible = false;

            }

            if (webBrowser1.Url.ToString() == "http://dnf.qq.com/act/a20130805weixin/cdkey.htm?bg=pe")
            {
                cookieStr = webBrowser1.Document.Cookie;
                //MessageBox.Show(cookieStr);
                myCookieContainer = new CookieContainer();
                //String 的Cookie　要转成　Cookie型的　并放入CookieContainer中  
                string[] cookstr = cookieStr.Split(';');

                foreach (string str in cookstr)
                {
                    string[] cookieNameValue = str.Split('=');
                    Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                    ck.Domain = "qq.com";//必须写对  
                    myCookieContainer.Add(ck);
                }
                //webBrowser1.Visible = false;
                //pictureBox1.Image = web.DowloadCheckImg("http://captcha.qq.com/getimage?aid=21000104", myCookieContainer);
                //string query_role_result = web.SendDataByGET("http://apps.game.qq.com/comm-cgi-bin/content_admin/activity_center/query_role.cgi?game=dnf&area="+area+"&sServiceDepartment=x6m5", "", ref myCookieContainer, "apps.game.qq.com", "http://dnf.qq.com/act/a20130805weixin/cdkey.htm?bg=per");
                //if (query_role_result.IndexOf("msg:'ok'") > 0)
                //{
                //    md5str = query_role_result.Substring(query_role_result.IndexOf("md5str:'") + 8, 32);

                //}
                //else
                //{
                //    MessageBox.Show("获取角色加密信息失败，CDK可能无法正常兑换");
                //}
                //string ams_actdesc = web.SendDataByGET("http://amp.guanjia.qq.com/comm-htdocs/js/ams/v0.2R02/act/107853/act.desc.js", "", ref myCookieContainer, "amp.guanjia.qq.com", "http://amp.guanjia.qq.com/act/brand/201703dnf/index.html");
                //_MJson m = new _MJson(ams_actdesc);
                //sSDID = m.GetNode("sSDID").toString();
            }




        }

        private void button2_Click(object sender, EventArgs e)
        {
            cookieStr = "";
            userinfo = null;
            myCookieContainer = null;
            userid = "";
            sSDID = "";
            gtk = "";
            comboBox1.Items.Clear();
            label1.Text = "未登录";
            webBrowser1.Navigate("http://ui.ptlogin2.qq.com/cgi-bin/login?appid=8000201&s_url=http%3A%2F%2Fimgcache.qq.com%2Fqv%2Ffile%2Fqq%2Flogin.loginProxy_1.x.x.html");
            groupBox1.Visible = false;
        }

        private string getgift(int actid, string roleid, string area)
        {
            string url = "http://iyouxi.vip.qq.com/ams3.0.php?_c=page&actid=" + actid + "&roleid={roleid}&area={area}&g_tk={g_tk}";
            return getgift(url, roleid, area);

        }
        private string getgift(string gifurlUrl, string roleid, string area)
        {
            string gifurl = gifurlUrl.Replace("{g_tk}", gtk).Replace("{area}", area).Replace("{roleid}", roleid);
            string aa = http_GET(gifurl, myCookieContainer);
            //var a = JsonConvert.DeserializeObject<smsg>(aa);

            //MessageBox.Show(gifurl);
            return aa;
        }
        private string getgift(node act, string roleid, string area)
        {
            string gifurl = act.GetNode("subURL").toString().Replace("{g_tk}", gtk).Replace("{area}", area).Replace("{roleid}", roleid).Replace("{ametk}", ametk).Replace("{actid}", act.GetNode("actid").toString()).Replace("{flowid}", act.GetNode("flowid").toString());
            string postdata = act.GetNode("subDate").toString().Replace("{g_tk}", gtk).Replace("{area}", area).Replace("{roleid}", roleid).Replace("{ametk}", ametk).Replace("{actid}", act.GetNode("actid").toString()).Replace("{flowid}", act.GetNode("flowid").toString());
            //string aa = http_GET(gifurl, myCookieContainer);
            if (act.GetNode("model").toString() == "2")
            {
                string ext1 = act.GetNode("Ext1").toString();
                if (!sSDIDList.ContainsKey(ext1))
                {
                    string ams_actdesc = web.SendDataByGET(ext1, "", ref myCookieContainer, act.GetNode("Ext2").toString(), act.GetNode("Ext1").toString());
                    _MJson m = new _MJson(ams_actdesc);
                    sSDIDList[ext1] = m.GetNode("sSDID").toString();
 
                }

                gifurl = gifurl.Replace("{checkparam}", checkparam).Replace("{md5str}", md5str).Replace("{ametk}", ametk).Replace("{sSDID}", sSDIDList[ext1]);
                postdata = postdata.Replace("{checkparam}", checkparam).Replace("{md5str}", md5str).Replace("{ametk}", ametk).Replace("{sSDID}", sSDIDList[ext1]);
            }

            string aa = "";
            if (act.GetNode("subMethod").toString() == "Post")
                aa = web.SendDataByPost(gifurl, postdata, ref myCookieContainer, act.GetNode("Host").toString(), act.GetNode("Referer").toString());
            else
                aa = web.SendDataByGET(gifurl, postdata, ref myCookieContainer, act.GetNode("Host").toString(), act.GetNode("Referer").toString());
            //var a = JsonConvert.DeserializeObject<smsg>(aa);

            //MessageBox.Show(gifurl);
            return aa;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //string r = "{\"ret\":0,\"data\":{\"act\":{\"start_time\":1484582400,\"end_time\":1486310396,\"tlimit\":\"nolimit\",\"qqlimit_step\":\"d\",\"numlimit_step\":\"d\",\"op\":\"online_cdk\",\"qqlimit_num\":1,\"qqlimit_totalnum\":20,\"numlimit_num\":1,\"numlimit_per_num\":50000,\"numlimit_totalnum\":970000},\"join\":{\"time\":1484833645,\"level\":1,\"info\":\"ol|DTDLUAAAjGeGWxxw\",\"diamonds\":0},\"rule\":{\"rolexval\":\"5879\"},\"op\":{\"cdkey\":\"DTDLUAABHBDxRGYd\",\"cdkavailtime\":\"\",\"type\":\"1\",\"mid\":\"MA20170106164104022\"},\"hook\":{\"recordGift\":{\"actid\":160620,\"status\":0,\"type\":1,\"is_life_coupon\":\"0\",\"qqvipCardId\":0,\"time\":1484879153,\"name\":\"\\u5728\\u7ebf20\\u5206\\u949f\\u793c\\u5305\",\"level\":-1,\"info\":\"DTDLUAABHBDxRGYd\"}},\"actname\":\"\\u5728\\u7ebf20\\u5206\\u949f\\u793c\\u5305\",\"game\":\"Dnf\"},\"rettype\":0,\"time\":\"1484879153\",\"actid\":160620,\"msg\":\"success\"}";

            if (userid == "")
            {
                MessageBox.Show("请先登录！！！");
                return;
            }
            if (index < 0)
            {
                MessageBox.Show("请选择奖励再点击！！！");
                return;
            }
            if (atcid[index] == 0)
            {
                MessageBox.Show("当前选中项无奖励！！！");
                return;
            }

            string r = getgift(atcid[index], u_node.val[comboBox1.SelectedIndex].GetNode("role_id").toString(), area);
            _MJson m = new _MJson(r);

            //var a = JsonConvert.DeserializeObject<amsResponse>(r);
            if (m.GetNode("msg").toString() == "success")
            {

                if (m.GetNode("op").toString() == "online_cdk")
                    textBox1.AppendText("\r\n======CDK=======\r\n" + m.GetNode("cdkey").toString() + "\r\n================\r\n");
                textBox1.AppendText("\r\n" + DateTime.Now.ToString() + ",领取【" + m.GetNode("actname").toString() + "】成功");
                if (!textBox1.Focused) textBox1.SelectionStart = textBox1.Text.Length;
            }
            else 
                textBox1.AppendText("\r\n" + DateTime.Now.ToString() + ",领取【" + m.GetNode("actname").toString() + "】失败，原因：" + m.GetNode("msg").toString() + "\r\n");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadonlineini();
            //loadnewini();
            loadServer();
            //loadini();
            groupBox1.Visible = false;

            StringBuilder cookie = new StringBuilder(new String(' ', 2048));
            int datasize = cookie.Length;
            bool b = InternetGetCookie("http://qq.com", null, cookie, ref datasize);
            //MessageBox.Show(cookie.ToString());

        }

        private void loadServer()
        {
            svlist = DNFHelper.GETServerSelect();
            comboBox2.Items.AddRange(svlist.Select(x => x.t).ToArray());
        }
        private void loadini()
        {
            string str = File.ReadAllText(Application.StartupPath + "\\atc.ini", System.Text.Encoding.GetEncoding("GB2312"));
            string[] atc = str.Split('\n');
            atcid = new List<int>();
            foreach (var a in atc)
            {
                string[] atcde = a.Split('|');
                ListViewItem lvi = new ListViewItem();
                int did = 0;
                int.TryParse(atcde[2], out did);
                atcid.Add(did);
                switch (atcde[0])
                {
                    case "1": lvi.Text = atcde[1]; lvi.SubItems.Add(atcde[3]); listView1.Items.Add(lvi); break;
                    case "2": lvi.Text = "   └" + atcde[1]; lvi.SubItems.Add(atcde[3]); listView1.Items.Add(lvi); break;
                    case "3": lvi.Text = "      └" + atcde[1]; lvi.SubItems.Add(atcde[3]); listView1.Items.Add(lvi); break;
                }


            }

        }

        private void loadonlineini()
        {


            string str = http_GET("http://www.tx5d.com/api/g.ashx",null);
            _MJson m = new _MJson(str);

            atcid = new List<int>();
            Listnode Lnode = m.GetNode("Date").toListnode();
            label2.Text =  m.GetNode("ver").toString();
            AD_webBrowser.Navigate(m.GetNode("adlink").toString());
            actnodeList = new List<node>();
            actnodeList_C = new List<int>();
            loadListnode(Lnode);
        }
        private void loadnewini()
        {
            string str = File.ReadAllText(Application.StartupPath + "\\atc.ini", System.Text.Encoding.GetEncoding("utf-8"));
            _MJson m = new _MJson(str);

            atcid = new List<int>();
            Listnode Lnode = m.GetNode("Date").toListnode();
            label2.Text =   m.GetNode("ver").toString();
            AD_webBrowser.Navigate(m.GetNode("adlink").toString());
            actnodeList = new List<node>();
            actnodeList_C = new List<int>();
            loadListnode(Lnode);
        }
        static int c = 0;
        private void loadListnode(Listnode tmp, string q = "")
        {


            if (q != "")
                q = "  " + q;
            if (tmp == null)
                return;
            foreach (var v in tmp.val)
            {
                actnodeList.Add(v);
                actnodeList_C.Add(c);
                //listBox1.Items.Add(q + v.actname);
                //tmpactinfoList.Add(v);
                //tmpactinfoList_C.Add(c);
                ListViewItem lvi = new ListViewItem();
                lvi.Text = q + v.GetNode("actname").toString();
                lvi.SubItems.Add(v.GetNode("giftname").toString());
                listView1.Items.Add(lvi);
                if (c == 0) atcid.Add(c);
                else
                {
                    int did = 0;
                    int.TryParse(v.GetNode("actid").toString(), out did);
                    atcid.Add(did);
                }

                if (v.GetNode("atcExt").toString() != "null")
                {
                    c++;
                    if (q == "")
                        loadListnode(v.GetNode("atcExt").toListnode(), "  └");
                    else
                        loadListnode(v.GetNode("atcExt").toListnode(), q);
                    c--;
                }
            }


        }
        public void Write(string d)
        {
            try
            {
                FileStream fs = new FileStream(Application.StartupPath + "\\log.txt", FileMode.Append);
                //FileStream fs = new FileStream("D:\\log.txt", FileMode.Append);
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes(d);
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();

            }
            catch (Exception ex)
            { }

        }

        private void listView_Validated(object sender, EventArgs e)
        {
            if (listView1.FocusedItem != null)
            {
                listView1.FocusedItem.BackColor = SystemColors.Highlight;
                listView1.FocusedItem.ForeColor = Color.White;
            }
        }

        private void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            e.Item.ForeColor = Color.Black;
            e.Item.BackColor = SystemColors.Window;
            if (listView1.SelectedItems.Count > 0)
                index = e.ItemIndex;
            else index = -1;
            //if (listView1.FocusedItem != null)
            //{
            //    listView1.FocusedItem.Selected = true;
            //}
        }

        private string CDKexchange(string CDK, string Code, string roleid, string area)
        {
            StringBuilder Date = new StringBuilder();
            Date.Append("sCdKey=" + CDK);
            Date.Append("&iUin=" + userid);
            Date.Append("&sVerifyCode=" + Code + "&isVerifyCode=1");
            Date.Append("&iVerifyId=21000104&sArea=" + area + "&sPartition=&sRoleId=" + roleid + "&sServiceType=dnf&md5str=" + md5str);
            Date.Append("&ams_checkparam=" + checkparam + "&checkparam=" + checkparam);
            Date.Append("&iActivityId=10502&iFlowId=87571&g_tk=" + ametk + "&e_code=0&g_code=0&sServiceDepartment=x6m5");
            //string a = web.SendDataByPost("http://x6m5.ams.game.qq.com/ams/ame/ame.php?ameVersion=0.3&sServiceType=dnf&iActivityId=10502&sServiceDepartment=x6m5&sSDID=undefined&_=1486607335019", "sCdKey=DRWZQAJEQkREJVsu&iUin=1324158917&sVerifyCode=" + Code + "&isVerifyCode=1&iVerifyId=21000104&sArea=" + area + "&sPartition=&sRoleId=" + roleid + "&sServiceType=dnf&md5str=0ECE5A0A17A368A032DE24803599C24B&ams_checkparam=" + checkparam + "&checkparam=" + checkparam + "&iActivityId=10502&iFlowId=87571&g_tk=" + ametk + "&e_code=0&g_code=0&eas_url=http%253A%252F%252Fdnf.qq.com%252Fact%252Fa20130805weixin%252Fcdkey.htm&eas_refer=&sServiceDepartment=x6m5", ref myCookieContainer);
            string a = web.SendDataByPost("http://x6m5.ams.game.qq.com/ams/ame/ame.php?ameVersion=0.3&sServiceType=dnf&iActivityId=10502&sServiceDepartment=x6m5&sSDID=undefined&_=1486607335019", Date.ToString(), ref myCookieContainer);

            return a;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //string aaaa = web.SendDataByGET("http://dnf.qq.com/act/a20130805weixin/cdkey.htm?bg=pe", "", ref myCookieContainer);
            string a = CDKexchange(textBox2.Text, textBox3.Text, u_node.val[comboBox1.SelectedIndex].GetNode("role_id").toString(), area);
            _MJson m = new _MJson(a);
            if (checkBox1.Checked == true)
            {
                Thread t = new Thread(getvcode);
                t.Start();
            }
            else
            {
                pictureBox1.Image = web.DowloadCheckImg("http://captcha.qq.com/getimage?aid=21000104", myCookieContainer);
            }
            node w = m.GetNode("modRet");
            w.Trim();
            MessageBox.Show(w.GetNode("sMsg").toString());

        }
        private void getvcode()
        {
            SetgroupBox(false);
            string returnMess = "";
            Bitmap pic = web.DowloadCheckImg("http://captcha.qq.com/getimage?aid=21000104", myCookieContainer);
            SetPIC(pic);
            SetText("识别中……");
            string username = "mjx_dm";
            string pwd = "123456dm";
            string softKey = "";
            //获取用户信息 
            string userInfo = VerCode.GetUserInfo(username, pwd);
            //上传字节集验证码
            returnMess = VerCode.RecByte_A(pic, 4, username, pwd, softKey);
            SetText(returnMess.Substring(0, 4));
            SetgroupBox(true);


        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Thread t = new Thread(getvcode);
                t.Start();
            }
            else
            {
                pictureBox1.Image = web.DowloadCheckImg("http://captcha.qq.com/getimage?aid=21000104", myCookieContainer);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Thread t = new Thread(getvcode);
                t.Start();
            }
            else
            {
                pictureBox1.Image = web.DowloadCheckImg("http://captcha.qq.com/getimage?aid=21000104", myCookieContainer);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (userid == "")
            {
                MessageBox.Show("请先登录！！！");
                return;
            }
            if (comboBox2.SelectedIndex < 0)
            {
                MessageBox.Show("请选择大区！！！");
                return;
            }
            if (comboBox3.SelectedIndex < 0)
            {
                MessageBox.Show("请选择角色！！！");
                return;
            }
            AD_webBrowser.Visible = false;
            string r = "";
            _MJson m;
            int i = 0;
            foreach (int id in atcid)
            {
                if (id == 0)
                { i++; continue; }

                r = getgift(actnodeList[i], u_node.val[comboBox1.SelectedIndex].GetNode("role_id").toString(), area);

                m = new _MJson(r);

                if (actnodeList[i].GetNode("model").toString() == "2")
                {
                    if (m.GetNode("modRet") == null) textBox1.AppendText("\r\n" + DateTime.Now.ToString() + "领取失败:" + m.GetNode("sMsg").toString() + "\r\n" );
                    else textBox1.AppendText("\r\n" + DateTime.Now.ToString() + ":" + m.GetNode("modRet").GetNode("sMsg").toString() + "\r\n");

                }

                else
                    if (m.GetNode("msg").toString() == "success")
                    {

                        if (m.GetNode("op").toString() == "online_cdk")
                            textBox1.AppendText("\r\n======CDK=======\r\n" + m.GetNode("cdkey").toString() + "\r\n================\r\n");
                        textBox1.AppendText("\r\n" + DateTime.Now.ToString() + ",领取【" + m.GetNode("actname").toString() + "】成功");
                        if (!textBox1.Focused) textBox1.SelectionStart = textBox1.Text.Length;
                    }
                    else
                        textBox1.AppendText("\r\n" + DateTime.Now.ToString() + ",领取【" + m.GetNode("actname").toString() + "】失败，原因：" + m.GetNode("msg").toString() + "\r\n");
        
                i++;
            }


        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (userid == "")
            {
                MessageBox.Show("请先登录！！！");
                return;
            }
            if (index < 0)
            {
                MessageBox.Show("请选择奖励再点击！！！");
                return;
            }
            if (atcid[index] == 0)
            {
                MessageBox.Show("当前选中项无奖励！！！");
                return;
            }
            if (comboBox2.SelectedIndex < 0)
            {
                MessageBox.Show("请选择大区！！！");
                return;
            }
            if (comboBox3.SelectedIndex < 0)
            {
                MessageBox.Show("请选择角色！！！");
                return;
            }
            AD_webBrowser.Visible = false;

            string r = getgift(actnodeList[index], u_node.val[comboBox1.SelectedIndex].GetNode("role_id").toString(), area);

            _MJson m = new _MJson(r);

            if (actnodeList[index].GetNode("model").toString() == "2")
            {
                if (m.GetNode("modRet") == null) textBox1.AppendText("\r\n" + DateTime.Now.ToString() + "领取失败:" + m.GetNode("sMsg").toString() + "\r\n");
                else textBox1.AppendText("\r\n" + DateTime.Now.ToString() + ":" + m.GetNode("modRet").GetNode("sMsg").toString() + "\r\n");

            }

            else
                if (m.GetNode("msg").toString() == "success")
                {

                    if (m.GetNode("op").toString() == "online_cdk")
                        textBox1.AppendText("\r\n======CDK=======\r\n" + m.GetNode("cdkey").toString() + "\r\n================\r\n");
                    textBox1.AppendText("\r\n" + DateTime.Now.ToString() + ",领取【" + m.GetNode("actname").toString() + "】成功");
                    if (!textBox1.Focused) textBox1.SelectionStart = textBox1.Text.Length;
                }
                else
                    textBox1.AppendText("\r\n" + DateTime.Now.ToString() + ",领取【" + m.GetNode("actname").toString() + "】失败，原因：" + m.GetNode("msg").toString() + "\r\n");
        

            //string ams_actdesc = web.SendDataByGET("http://amp.guanjia.qq.com/comm-htdocs/js/ams/v0.2R02/act/107853/act.desc.js", "", ref myCookieContainer, "amp.guanjia.qq.com", "http://amp.guanjia.qq.com/act/brand/201703dnf/index.html");
            //_MJson m = new _MJson(ams_actdesc);
            //sSDID = m.GetNode("sSDID").toString();
            //string aa = web.SendDataByPost("http://apps.game.qq.com/ams/ame/ame.php?ameVersion=0.3&sServiceType=ampguanjia&iActivityId=107853&sServiceDepartment=group_7&set_info=group_7&sSDID=" + sSDID + "&isXhrPost=true", "gameId=&sArea=&iSex=&sRoleId=&iGender=&sServiceType=ampguanjia&objCustomMsg=&areaname=&roleid=&rolelevel=&rolename=&areaid=&iActivityId=107853&iFlowId=352167&g_tk=" + ametk + "&e_code=0&g_code=0&eas_url=http%253A%252F%252Famp.guanjia.qq.com%252Fact%252Fbrand%252F201703dnf%252F&eas_refer=&xhr=1&sServiceDepartment=group_7&xhrPostKey=xhr_1490862711364", ref myCookieContainer);
            //MessageBox.Show(new _MJson(aa).GetNode("msg").toString());

        }

        private void button_gourl_Click(object sender, EventArgs e)
        {
            if (index < 0)
            {
                MessageBox.Show("请选择要前往的活动！！！");
                return;
            }
            System.Diagnostics.Process.Start(actnodeList[index].GetNode("actURL").toString());


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex < 0)
                return;
            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(svlist[comboBox2.SelectedIndex].opt_data_array.Select(x=>x.t).ToArray());

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cookieStr == "")
            {
                MessageBox.Show("请先登录！！！");
                return;
            }
            if (comboBox2.SelectedIndex < 0)
                return;
            if (comboBox3.SelectedIndex < 0)
                return;
            area = svlist[comboBox2.SelectedIndex].opt_data_array[comboBox3.SelectedIndex].v;
            string aa = http_GET("http://iyouxi.vip.qq.com/ams3.0.php?_c=queryRoleInfo&gamename=dnf&area=" + area + "&g_tk=" + gtk, myCookieContainer);
            //string aa = "json14848016790871({\"ret\":0,\"data\":[{\"role_id\":\"16705750\",\"nick\":\"Asura\\u4e36\\u4e44\"},{\"role_id\":\"17521661\",\"nick\":\"\\u9694\\u58c1[\\u738b\\u5927\\u67aa]\"},{\"role_id\":\"26980759\",\"nick\":\"\\u5929\\u9009\\u4e44\\u7f14\\u9020\"},{\"role_id\":\"28160539\",\"nick\":\"\\u6218\\u6597\\u5723\\u7075\"},{\"role_id\":\"28460688\",\"nick\":\"\\u529b\\u5c48\\u5929\\u4e0b\"},{\"role_id\":\"28549736\",\"nick\":\"\\u6613\\u6b66\\u8bed\"},{\"role_id\":\"28604163\",\"nick\":\"\\u4e73\\u6c41\\u306e\\u9a91\\u58eb\"},{\"role_id\":\"28894325\",\"nick\":\"\\u6c14\\u4e44\\u6b7b\\u4f60\"},{\"role_id\":\"29047949\",\"nick\":\"\\u8d64\\u4e36\\u72ac\"},{\"role_id\":\"29165134\",\"nick\":\"+38\\u7684[\\u51ef\\u4e3d]\"},{\"role_id\":\"29799370\",\"nick\":\"\\u540e\\u8857\\u516c\\u4e3b\"},{\"role_id\":\"30117053\",\"nick\":\"\\u5723\\u804c\\u8005\\u662f\\u6211\"},{\"role_id\":\"30117056\",\"nick\":\"\\u5723\\u804c\\u8005\\u771f\\u662f\\u6211\"},{\"role_id\":\"30119531\",\"nick\":\"\\u6211\\u771f\\u662f\\u5723\\u804c\\u8005\"},{\"role_id\":\"30166714\",\"nick\":\"\\u7b2c\\u4e94\\u5251\\u5723\\u00b7\\u7edd\"},{\"role_id\":\"30173360\",\"nick\":\"\\u4f26\\u5bb6\\u4e5f\\u80fd\\u5c04\"},{\"role_id\":\"30305005\",\"nick\":\"\\u9020\\u7891\\u8005\\u00b7\\u5239\"},{\"role_id\":\"30307230\",\"nick\":\"\\u6211\\u662f\\u9ed1\\u6b66\"},{\"role_id\":\"30436728\",\"nick\":\"\\u9b54\\u4e44\\u9053\"},{\"role_id\":\"30843469\",\"nick\":\"\\u72d7\\u6b87\\u786a\\u4f60\\u4e0d\\u914d\"},{\"role_id\":\"31209434\",\"nick\":\"\\u821e\\u6c14\\u5927\\u6e7f\"},{\"role_id\":\"31283367\",\"nick\":\"\\u5361\\u5361\\u7f57\\u7279\\u00b7\\u56f8\"},{\"role_id\":\"31373443\",\"nick\":\"\\u4f60\\u6cd5\\u7237\"},{\"role_id\":\"31427057\",\"nick\":\"\\u65e0\\u6cd5\\u51b0\\u51bb\"},{\"role_id\":\"31506914\",\"nick\":\"\\u620f\\u8c82\\u8749\"},{\"role_id\":\"31627023\",\"nick\":\"\\u5730\\u72f1\\u00b7\\u53ec\\u5524\"},{\"role_id\":\"31935605\",\"nick\":\"[\\u5c0f\\u67d2]\"},{\"role_id\":\"32071049\",\"nick\":\"\\u9ea6\\u57ce\\u5173\\u7fbd\"},{\"role_id\":\"33685115\",\"nick\":\"\\u65e0\\u540d\\u7684\\u673a\\u68b0\\u5e08\"}],\"time\":\"1484805186\",\"msg\":\"success\"});";
            //int sta = aa.IndexOf("({") + 1;
            //string json = aa.Substring(sta, aa.IndexOf("});") - sta + 1);
            //userinfo = JsonConvert.DeserializeObject<DNFRoleInfo>(aa);
            userinfo = new DNFRoleInfo();
            _MJson m = new _MJson(aa);
            try
            {
                u_node = m.GetNode("data").toListnode();
            }
            catch
            {
                MessageBox.Show("该区找不到角色！！！");
                return;
            }
            StringBuilder tmp = new StringBuilder();
            tmp.Append("dnf%257Cyes%257C" + userid + "%257C" + area + "%257C");
            comboBox1.Items.Clear();
            foreach (var v in u_node.val)
            {
                comboBox1.Items.Add(v.GetNode("nick").toString());
                tmp.Append(v.GetNode("role_id").toString() + "*");
                //comboBox1.Items.Add(v.nick);
                //tmp.Append(v.role_id + "*");
            }
            if (comboBox1.Items.Count > 0)
            {
                if (File.Exists(userid + ".ini"))
                {
                    string[] tmpstr = File.ReadAllLines(userid + ".ini");
                    comboBox1.SelectedIndex = int.Parse(tmpstr[2]);

                }
                else comboBox1.SelectedIndex = 0;
                checkparam = tmp.ToString();
                //MessageBox.Show("获取角色成功");
            }
            else MessageBox.Show("该区找不到角色，或者出现错误");
            pictureBox1.Image = web.DowloadCheckImg("http://captcha.qq.com/getimage?aid=21000104", myCookieContainer);
            string query_role_result = web.SendDataByGET("http://apps.game.qq.com/comm-cgi-bin/content_admin/activity_center/query_role.cgi?game=dnf&area=" + area + "&sServiceDepartment=x6m5", "", ref myCookieContainer, "apps.game.qq.com", "http://dnf.qq.com/act/a20130805weixin/cdkey.htm?bg=per");
            if (query_role_result.IndexOf("msg:'ok'") > 0)
            {
                md5str = query_role_result.Substring(query_role_result.IndexOf("md5str:'") + 8, 32);

            }
            else
            {
                MessageBox.Show("获取角色加密信息失败，CDK可能无法正常兑换");
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex < 0)
                return;
            if (comboBox3.SelectedIndex < 0)
                return;
            if (comboBox1.SelectedIndex < 0)
                return;
            File.WriteAllText(userid + ".ini", comboBox2.SelectedIndex + Environment.NewLine + comboBox3.SelectedIndex + Environment.NewLine + comboBox1.SelectedIndex);
        }

        private void AD_webBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            string currentUri = ((WebBrowser)sender).Document.ActiveElement.GetAttribute("href");
            System.Diagnostics.Process.Start(currentUri);
        }



        ////string r = "{\"ret\":0,\"data\":{\"act\":{\"start_time\":1484582400,\"end_time\":1486310396,\"tlimit\":\"nolimit\",\"qqlimit_step\":\"d\",\"numlimit_step\":\"d\",\"op\":\"online_cdk\",\"qqlimit_num\":1,\"qqlimit_totalnum\":20,\"numlimit_num\":1,\"numlimit_per_num\":50000,\"numlimit_totalnum\":970000},\"join\":{\"time\":1484833645,\"level\":1,\"info\":\"ol|DTDLUAAAjGeGWxxw\",\"diamonds\":0},\"rule\":{\"rolexval\":\"5879\"},\"op\":{\"cdkey\":\"DTDLUAABHBDxRGYd\",\"cdkavailtime\":\"\",\"type\":\"1\",\"mid\":\"MA20170106164104022\"},\"hook\":{\"recordGift\":{\"actid\":160620,\"status\":0,\"type\":1,\"is_life_coupon\":\"0\",\"qqvipCardId\":0,\"time\":1484879153,\"name\":\"\\u5728\\u7ebf20\\u5206\\u949f\\u793c\\u5305\",\"level\":-1,\"info\":\"DTDLUAABHBDxRGYd\"}},\"actname\":\"\\u5728\\u7ebf20\\u5206\\u949f\\u793c\\u5305\",\"game\":\"Dnf\"},\"rettype\":0,\"time\":\"1484879153\",\"actid\":160620,\"msg\":\"success\"}";







    }
}
