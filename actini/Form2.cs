using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace actini
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        int tmpindex = -1;
        actinfo tmpactinfo;
        int tmptype;
        public Form2(actinfo selected, int type = 0, int index = -1)
        {
            
            InitializeComponent();
            if (type == 2)
                button3.Visible = button4.Visible = true;
            RE(selected,type,index);
        }

        private void RE(actinfo selected, int type = 0, int index = -1)
        {
            tmpindex = index;
            tmptype = type;
            actname_textBox.Text = selected.actname;
            actid_textBox.Text = selected.actid.ToString();
            flowid_textBox.Text = selected.flowid.ToString();
            start_time_textBox.Text = selected.start_time.ToString();
            end_time_textBox.Text = selected.end_time.ToString();
            actURL_textBox.Text = selected.actURL;
            Host_textBox.Text = selected.Host;
            Referer_textBox.Text = selected.Referer;
            giftname_textBox.Text = selected.giftname;
            model_textBox.Text = selected.model.ToString();
            subURL_textBox.Text = selected.subURL;
            subMethod_textBox.Text = selected.subMethod;
            autoSub_textBox.Text = selected.autoSub;
            subDate_textBox.Text = selected.subDate;
            Ext1_textBox.Text = selected.Ext1;
            Ext2_textBox.Text = selected.Ext2;
            Ext3_textBox.Text =  System.Web.HttpUtility.UrlDecode(selected.Ext3);
            tmpactinfo = selected;
            switch (type)
            {
                case 0: button1.Text = "添加项"; this.Text = "添加一个新的项"; break;
                case 1: button1.Text = "添加子项"; this.Text = "为添加[" + selected.actname + "]一个子项"; break;
                case 2: button1.Text = "修改"; this.Text = "正在修改[" + selected.actname + "]"; break;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            iniForm f1 = (iniForm)this.Owner;
            if (tmptype == 2)
            {
                tmpactinfo.actname = actname_textBox.Text;
                tmpactinfo.actid = int.Parse(actid_textBox.Text);
                tmpactinfo.flowid = int.Parse(flowid_textBox.Text);
                tmpactinfo.start_time = int.Parse(start_time_textBox.Text);
                tmpactinfo.end_time = int.Parse(end_time_textBox.Text);
                tmpactinfo.Host = Host_textBox.Text;
                tmpactinfo.Referer = Referer_textBox.Text;
                tmpactinfo.giftname = giftname_textBox.Text;
                tmpactinfo.model = int.Parse(model_textBox.Text);
                tmpactinfo.actURL = actURL_textBox.Text;
                tmpactinfo.subURL = subURL_textBox.Text;
                tmpactinfo.subMethod = subMethod_textBox.Text;
                tmpactinfo.autoSub = autoSub_textBox.Text;
                tmpactinfo.subDate = subDate_textBox.Text;
                tmpactinfo.Ext1 = Ext1_textBox.Text;
                tmpactinfo.Ext2 = Ext2_textBox.Text;
                tmpactinfo.Ext3 = System.Web.HttpUtility.UrlEncode(Ext3_textBox.Text);
                f1.refresh();
                this.Text = "修改[" + tmpactinfo.actname + "]成功！！！";
                //this.Close();


            }
            else
            {
                actinfo tmp = new actinfo();
                tmp.actname = actname_textBox.Text;
                tmp.actid = int.Parse(actid_textBox.Text);
                tmp.flowid = int.Parse(flowid_textBox.Text);
                tmp.start_time = int.Parse(start_time_textBox.Text);
                tmp.end_time = int.Parse(end_time_textBox.Text);
                tmp.Host = Host_textBox.Text;
                tmp.Referer = Referer_textBox.Text;
                tmp.giftname = giftname_textBox.Text;
                tmp.model = int.Parse(model_textBox.Text);
                tmp.actURL = actURL_textBox.Text;
                tmp.subURL = subURL_textBox.Text;
                tmp.subMethod = subMethod_textBox.Text;
                tmp.autoSub = autoSub_textBox.Text;
                tmp.subDate = subDate_textBox.Text;
                tmp.Ext1 = Ext1_textBox.Text;
                tmp.Ext2 = Ext2_textBox.Text;
                tmp.Ext3 = System.Web.HttpUtility.UrlEncode(Ext3_textBox.Text);
                f1.AddAtc(tmp, tmpindex);

                //f1.AddAtc(actname_textBox.Text, int.Parse(actid_textBox.Text), int.Parse(start_time_textBox.Text), int.Parse(end_time_textBox.Text), Host_textBox.Text, Referer_textBox.Text, giftname_textBox.Text, int.Parse(model_textBox.Text), tmpindex);
                //this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            int index=(this.Owner as iniForm).tmpactinfoList.IndexOf(tmpactinfo);
            if (index > 0)
                RE((this.Owner as iniForm).tmpactinfoList[index-1],2);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int index = (this.Owner as iniForm).tmpactinfoList.IndexOf(tmpactinfo);
            if (index < (this.Owner as iniForm).tmpactinfoList.Count-1)
                RE((this.Owner as iniForm).tmpactinfoList[index +1 ], 2);
        }
    }
}
