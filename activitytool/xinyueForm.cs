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
        MainForm Ow = null;
        private Thread maint = null;
        Dictionary<string, int> card = null;
        Listnode tasklist = null;
        bool auto = false;
        public xinyueForm(bool val=false)
        {
            
            InitializeComponent();
            auto = val;
        }
        private void xinyueForm_Load(object sender, EventArgs e)
        {
            
            // 增加判断，避免每次都开辟一个线程
            if (maint == null)
            {
                maint = new Thread(inti);
                maint.Start();
            }
            if (maint.ThreadState == ThreadState.Suspended) // 如果被挂起了，就唤醒
            {
                maint.Resume();
            }


        }
        private void inti()
        {
            Ow = (MainForm)this.Owner;
            Ow.Por.ryzcsSDID();
            relabel();
            relistView_task();
            if (auto)
            {
                autosubmit();
            }
        }
        public void autosubmit()
        {
            Ow.Por.XinyueRYBinding();
            onesubmit();
        }
        private void relabel()
        {
            SetlabelText(label_int, "心悦点:" + Ow.Por.XinyueGetint() + "  荣誉点：" + Ow.Por.XinyueGetRYint());
            Dictionary<string, string> binding = Ow.Por.XinyueGetRYBinding();
            if (binding != null)
            {
                SetlabelText(label_binding, "已绑定:" + binding["areaName"] + "-" + binding["roleName"]);
            }
            else
            {
                SetlabelText(label_binding, "未绑定角色");
            }
            
            card = Ow.Por.XinyueGetRYProp();
            SetlabelText(label_prop, "道具：双倍卡X" + card["two_score"] + "  免做卡X" + card["free_do"] + "  刷新卡X" + card["rd_do"]);
        }
        private void relistView_task()
        {
            string re = Ow.Por.XinyueGetRYtask();
            _MJson m = new _MJson(re);
            tasklist = m.toListnode();
            tasklist.val.ForEach(t =>
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = t.GetNode("id").toString();
                lvi.SubItems.Add(t.GetNode("task_name").toString());
                lvi.SubItems.Add(t.GetNode("score").toString());
                lvi.SubItems.Add(t.GetNode("status").toString() == "0" ? "未完成" : "已完成");
                SetlistView(listView_task, lvi);
            });
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
            List<int> indexlist = new List<int>();
            if (maint.ThreadState != ThreadState.Stopped)
                return;
            for (int i = 0; i < listView_task.SelectedItems.Count; i++)
            {
                indexlist.Add(listView_task.SelectedItems[i].Index);
            }
            switch (obj.Name)
            {
                case "button_binding": 
                    {
                        Ow.Por.XinyueRYBinding();
                    }
                    break;
                case "button_onesubmit":
                    {
                        Thread onet = new Thread(onesubmit);
                        onet.Start();
                    }
                    break;
                case "button_refresh":
                    {
                        if (Ow.Por.XinyueRYrefresh() == "0")
                        {
                            listView_task.Items.Clear();
                            Thread taskt = new Thread(relistView_task);
                            taskt.Start();
                        }
                        else
                        {
                            MessageBox.Show("刷新任务失败！！！");
                        }
                    }
                    break;
                case "button_submitS":
                    {
                        Ow.Por.XinyueRYtasksubmit(indexlist,"0",Ow.BoxAddText);
                    }
                    break;
                case "button_freesubmit":
                    {
                        Ow.Por.XinyueRYtasksubmit(indexlist, "1", Ow.BoxAddText);
                    }
                    break;
                case "button_towscore":
                    {
                        Ow.Por.XinyueRYtasksubmit(indexlist, "2", Ow.BoxAddText);
                    }
                    break;
            }
            Thread t = new Thread(relabel);
            t.Start();
        }
        private void onesubmit()
        {
            tasklist.val.ForEach(tmp =>
            {
                if (tmp.GetNode("status").toString() == "0")
                {
                    if (tmp.GetNode("score").toString() == "3" && checkBox_twoscore.Checked == true && card["two_score"] > 0)
                    {
                        Ow.Por.XinyueRYtasksubmit(new List<int> { tasklist.val.IndexOf(tmp) }, "2", Ow.BoxAddText);
                    }
                    else
                    {
                        Ow.Por.XinyueRYtasksubmit(new List<int> { tasklist.val.IndexOf(tmp) }, "0", Ow.BoxAddText);
                    }
                    Thread.Sleep(3000);

                }

            });
        }
    }
}
