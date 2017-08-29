using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniJson;
using System.Threading;

namespace activitytool
{
    public partial class xinyueForm : Form
    {
        public xinyueForm()
        {
            InitializeComponent();
        }
        MainForm Ow = null;
        private Thread t = null;
        private void xinyueForm_Load(object sender, EventArgs e)
        {
            
            // 增加判断，避免每次都开辟一个线程
            if (t == null)
            {
                t = new Thread(inti);
                t.Start();
            }
            if (t.ThreadState == ThreadState.Suspended) // 如果被挂起了，就唤醒
            {
                t.Resume();
            }


        }
        private void inti()
        {
            Ow = (MainForm)this.Owner;
            Ow.Por.ryzcsSDID();
            SetlabelText(label_int, "心悦点:" + Ow.Por.XinyueGetint() + "  荣誉点：" + Ow.Por.XinyueGetRYint());
            Dictionary<string, string> binding = Ow.Por.XinyueGetRYBinding();
            SetlabelText(label_binding,"已绑定:" + binding["areaName"] + "-" + binding["roleName"]);
            string re = Ow.Por.XinyueGetRYtask();
            _MJson m = new _MJson(re);
            Listnode tasklist = m.toListnode();
            tasklist.val.ForEach(t =>
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = t.GetNode("id").toString();
                lvi.SubItems.Add(t.GetNode("task_name").toString());
                lvi.SubItems.Add(t.GetNode("score").toString());
                lvi.SubItems.Add(t.GetNode("status").toString());
                SetlistView(listView_task,lvi);
            });
            Dictionary<string, int> card = Ow.Por.XinyueGetRYProp();
            SetlabelText(label_prop, "道具：双倍卡X" + card["two_score"] + "  免做卡X" + card["free_do"] + "  刷新卡X" + card["rd_do"]);
 
        }
        public delegate void SetTextCallback(object sender,string text);
        public void SetlabelText(object sender, string text)
        {
            Label obj = (Label)sender;
            if (obj.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetlabelText);
                this.Invoke(d, new object[] { sender, text });
            }
            else
            {
                obj.Text = text;
            }
        }
        public delegate void SetListViewItemCallback(object sender, ListViewItem text);
        public void SetlistView(object sender, ListViewItem lvi)
        {
            ListView obj = (ListView)sender;
            if (obj.InvokeRequired)
            {
                SetListViewItemCallback d = new SetListViewItemCallback(SetlistView);
                this.Invoke(d, new object[] { sender,lvi });
            }
            else
            {
                obj.Items.Add(lvi);
            }
        }
        private void button_Click(object sender, EventArgs e)
        {
            Button obj = (Button)sender;
            switch (obj.Name)
            {
                case "button_binding": 
                    {
                        Ow.Por.XinyueRYBinding();
                    }
                    break;
            }
        }
    }
}
