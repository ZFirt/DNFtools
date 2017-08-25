using MiniJson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace activitytool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        DNFWebProxy Por = null;
        Listnode Lnode = null;
        private void MainForm_Load(object sender, EventArgs e)
        {
            loadnewini();
            groupBox1.Visible = false;
            webBrowser_login.ScriptErrorsSuppressed = true;
        }
        private void loadnewini()
        {
            //string str = DNFWebProxy.SendDataByGET("http://www.tx5d.com/api/g.ashx","");
            string str = File.ReadAllText(Application.StartupPath + "\\atc.ini", System.Text.Encoding.GetEncoding("utf-8"));
            _MJson m = new _MJson(str);
            Lnode = m.GetNode("Date").toListnode();
            label2.Text = m.GetNode("ver").toString();
            AD_webBrowser.Navigate(m.GetNode("adlink").toString());
            loadListnode(Lnode);
        }
        private void loadListnode(Listnode tmp, string q = "")
        {
            if (q != "")
                q = "  " + q;
            if (tmp == null)
                return;
            foreach (var v in tmp.val)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = q + v.GetNode("actname").toString();
                lvi.SubItems.Add(v.GetNode("giftname").toString());
                switch (v.GetNode("autoSub").toString())
                {
                    case "0": lvi.SubItems.Add("-"); break;
                    case "1": lvi.SubItems.Add("√"); break;
                    case "2": lvi.SubItems.Add("×"); break;
                }
                listView1.Items.Add(lvi);
                if (v.GetNode("atcExt").toString() != "null")
                {
                    if (q == "")
                        loadListnode(v.GetNode("atcExt").toListnode(), "  └");
                    else
                        loadListnode(v.GetNode("atcExt").toListnode(), q);
                }
            }
        }
        private void init()
        {
            string cookieStr = webBrowser_login.Document.Cookie;
            Por = new DNFWebProxy(cookieStr);
            if(Por!=null)
            { 
                comboBox_region.Items.AddRange(Por.svlist.Select(x => x.t).ToArray());
                Por.SetActList(Lnode);
                label1.Text = Por.QQ;

            }
        }

        private void webBrowser_login_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser_login.Url.ToString().IndexOf("https://xui.ptlogin2.qq.com/") == 0)
            {
                if (groupBox1.Visible == true)
                {
                    groupBox1.Visible = false;
                }

            }
            else 
            {
                if (groupBox1.Visible ==false)
                {
                    groupBox1.Visible = true;
                }
            }
            if (webBrowser_login.Url.ToString().IndexOf("http://dnf.qq.com/gift.shtml") == 0)
            {

                if (webBrowser_login.Document.Url == e.Url)
                { 
                    //webBrowser_login.Navigate("http://dnf.qq.com/act/a20130805weixin/cdkey.htm?bg=pe");
                    init();
                }
            }
        }
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox obj = (ComboBox)sender;
            if (obj.SelectedIndex < 0||obj.Items.Count==0)
                return;
            switch(obj.Name)
            {
                case "comboBox_region":
                    {
                        comboBox_area.Items.Clear();
                        comboBox_area.Items.AddRange(Por.svlist[obj.SelectedIndex].opt_data_array.Select(x => x.t).ToArray());
                    }
                    break;
                case "comboBox_area":
                    {
                        comboBox_role.Items.Clear();
                        comboBox_role.Items.AddRange(Por.loadRole(Por.svlist[comboBox_region.SelectedIndex].opt_data_array[obj.SelectedIndex].v).ToArray());
                    }
                    break;
                case "comboBox_role":
                    {
                        Por.SetRoleId(obj.SelectedIndex);
                    }
                    break;

            }

        }
        public delegate void SetTextCallback(string text);
        public void BoxAddText(string text)
        {
            if (this.textBox3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(BoxAddText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.AppendText(text);
            }
        }
        private void button_Click(object sender, EventArgs e)
        {
            AD_webBrowser.Visible = false;
            Button obj = (Button)sender;
            List<int> indexlist = new List<int>();
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                indexlist.Add(listView1.SelectedItems[i].Index);
            }
            switch (obj.Name)
            {
                case "button_onekeysubmit":
                    {
                        Por.OneSubmitAct(BoxAddText);
                    }
                    break;
                case "button_submitselect":
                    {
                        Por.SubmitAct(indexlist,BoxAddText);
                    }
                    break;
                case "button_gourl":
                    {
                        indexlist.ForEach(t => {
                            System.Diagnostics.Process.Start(Por.actnodeList[t].GetNode("actURL").toString());
                        });
                    }
                    break;
                case "button_loginout":
                    {
                        Por = null;
                        label1.Text = "未登录";
                        HtmlDocument document = webBrowser_login.Document;
                        document.ExecCommand("ClearAuthenticationCache", false, null);
                        //SuppressWininetBehavior();
                        webBrowser_login.Navigate("https://xui.ptlogin2.qq.com/cgi-bin/xlogin?proxy_url=http://game.qq.com/comm-htdocs/milo/proxy.html&appid=21000127&target=top&s_url=http%3A%2F%2Fdnf.qq.com%2Fgift.shtml&style=20&daid=8");
                    }
                    break;
            }
        }
        private void AD_webBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            string currentUri = ((WebBrowser)sender).Document.ActiveElement.GetAttribute("href");
            System.Diagnostics.Process.Start(currentUri);
        }

        [System.Runtime.InteropServices.DllImport("wininet.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetOption(int hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        /// <summary>
        /// 使用InternetSetOption操作wininet.dll清除webbrowser里的cookie
        /// </summary>
        private static unsafe void SuppressWininetBehavior()
        {
            /* SOURCE: http://msdn.microsoft.com/en-us/library/windows/desktop/aa385328%28v=vs.85%29.aspx
                * INTERNET_OPTION_SUPPRESS_BEHAVIOR (81):
                *      A general purpose option that is used to suppress behaviors on a process-wide basis. 
                *      The lpBuffer parameter of the function must be a pointer to a DWORD containing the specific behavior to suppress. 
                *      This option cannot be queried with InternetQueryOption. 
                *      
                * INTERNET_SUPPRESS_COOKIE_PERSIST (3):
                *      Suppresses the persistence of cookies, even if the server has specified them as persistent.
                *      Version:  Requires Internet Explorer 8.0 or later.
                */
            int option = (int)3/* INTERNET_SUPPRESS_COOKIE_PERSIST*/;
            int* optionPtr = &option;

            bool success = InternetSetOption(0, 81/*INTERNET_OPTION_SUPPRESS_BEHAVIOR*/, new IntPtr(optionPtr), sizeof(int));
            if (!success)
            {
                MessageBox.Show("Something went wrong ! Clear Cookie Failed!");
            }

        }

    }
}
