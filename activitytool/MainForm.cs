using MiniJson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace activitytool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        public DNFWebProxy Por = null;
        Listnode Lnode = null;
        xinyueForm xinyue = null;
        private void MainForm_Load(object sender, EventArgs e)
        {
            loadnewini();
            groupBox1.Visible = false;
            webBrowser_login.ScriptErrorsSuppressed = true;
        }
        private void loadnewini()
        {
            string str = DNFWebProxy.SendDataByGET("http://www.tx5d.com/api/v2/g.ashx", "");
            //string str = File.ReadAllText(Application.StartupPath + "\\atc.ini", System.Text.Encoding.GetEncoding("utf-8"));
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
            if (Por != null)
            {
                comboBox_region.Items.AddRange(Por.svlist.Select(x => x.t).ToArray());
                Por.SetActList(Lnode);
                label1.Text = Por.QQ;
                if (File.Exists(Por.QQ + ".ini"))
                {
                    string[] tmpstr = File.ReadAllLines(Por.QQ + ".ini");
                    comboBox_region.SelectedIndex = int.Parse(tmpstr[0]);
                    comboBox_area.SelectedIndex = int.Parse(tmpstr[1]);
                    comboBox_role.SelectedIndex = int.Parse(tmpstr[2]);
                    File.WriteAllText(Por.QQ + ".ini", comboBox_region.SelectedIndex + Environment.NewLine + comboBox_area.SelectedIndex + Environment.NewLine + comboBox_role.SelectedIndex);

                }
                pictureBox_Code.Image = Por.GetCodeBitmap();

            }
        }

        private void webBrowser_login_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser_login.Url.ToString().IndexOf("http://ui.ptlogin2.qq.com") == 0)
            {
                if (groupBox1.Visible == true)
                {
                    groupBox1.Visible = false;
                }

            }
            else
            {
                if (groupBox1.Visible == false)
                {
                    groupBox1.Visible = true;
                }
            }
            if (webBrowser_login.Url.ToString().IndexOf("http://game.qq.com/comm-htdocs/login/loginSuccess.html") == 0)
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
            if (obj.SelectedIndex < 0 || obj.Items.Count == 0)
                return;
            switch (obj.Name)
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
                        comboBox_role.Items.AddRange(Por.loadRole(Por.svlist[comboBox_region.SelectedIndex].opt_data_array[obj.SelectedIndex].v, Por.svlist[comboBox_region.SelectedIndex].opt_data_array[obj.SelectedIndex].t).ToArray());
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
            if (this.textBox_Code.InvokeRequired)
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
            if (obj.Name == "button_loginout")
            {
                Por = null;
                label1.Text = "未登录";
                HtmlDocument document = webBrowser_login.Document;
                document.ExecCommand("ClearAuthenticationCache", false, null);
                //SuppressWininetBehavior();
                webBrowser_login.Navigate("http://ui.ptlogin2.qq.com/cgi-bin/login?appid=21000115&f_url=loginerroralert&target=self&qtarget=self&s_url=http%3A//game.qq.com/comm-htdocs/login/loginSuccess.html&no_verifyimg=1&qlogin_jumpname=jump&daid=8");
                return;
            }
            if (Por == null)
            {
                MessageBox.Show("请先登录！！！");
                return;
            }
            if (!Por.ValueVerify())
            {
                MessageBox.Show("请选择角色！！！");
                return;
            }
            List<int> indexlist = new List<int>();
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                indexlist.Add(listView1.SelectedItems[i].Index);
            }
            switch (obj.Name)
            {
                case "button_onekeysubmit":
                    {
                        if (xinyue == null || xinyue.IsDisposed)
                        {
                            xinyue = new xinyueForm(true);
                            xinyue.Owner = this;
                            xinyue.Show();
                        }
                        else
                        {
                            xinyue.TopMost = true;
                            xinyue.TopMost = false;

                        }
                        Thread t = new Thread(OneSubmitAct);
                        t.Start();
                        //Por.OneSubmitAct(BoxAddText);
                    }
                    break;
                case "button_submitselect":
                    {
                        Por.SubmitAct(indexlist, BoxAddText);
                    }
                    break;
                case "button_gourl":
                    {
                        indexlist.ForEach(t =>
                        {
                            System.Diagnostics.Process.Start(Por.actnodeList[t].GetNode("actURL").toString());
                        });
                    }
                    break;
                case "button_submitCDK":
                    {
                        MessageBox.Show(Por.CDKexchange(textBox_CDK.Text, textBox_Code.Text));
                        pictureBox_Code.Image = Por.GetCodeBitmap();
                    }
                    break;
                case "button_reCode":
                    {
                        pictureBox_Code.Image = Por.GetCodeBitmap();
                    }
                    break;
                case "button_xinyue":
                    {
                        if (xinyue == null || xinyue.IsDisposed)
                        {
                            xinyue = new xinyueForm();
                            xinyue.Owner = this;
                            xinyue.Show();
                        }
                        else
                        {
                            xinyue.TopMost = true;
                            xinyue.TopMost = false;

                        }

                    }
                    break;
            }
        }
        public void OneSubmitAct()
        {
            Por.OneSubmitAct(BoxAddText);
 
        }
        private void AD_webBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            string currentUri = ((WebBrowser)sender).Document.ActiveElement.GetAttribute("href");
            System.Diagnostics.Process.Start(currentUri);
        }
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)] 
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData); 

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
