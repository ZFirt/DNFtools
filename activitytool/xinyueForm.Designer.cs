namespace activitytool
{
    partial class xinyueForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(xinyueForm));
            this.label_int = new System.Windows.Forms.Label();
            this.listView_task = new System.Windows.Forms.ListView();
            this.task_id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.task_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.task_score = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.task_status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl_xinyue = new System.Windows.Forms.TabControl();
            this.tabPage_ryzc = new System.Windows.Forms.TabPage();
            this.checkBox_twoscore = new System.Windows.Forms.CheckBox();
            this.button_onesubmit = new System.Windows.Forms.Button();
            this.button_submitS = new System.Windows.Forms.Button();
            this.button_freesubmit = new System.Windows.Forms.Button();
            this.button_refresh = new System.Windows.Forms.Button();
            this.button_towscore = new System.Windows.Forms.Button();
            this.button_binding = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_binding = new System.Windows.Forms.Label();
            this.label_prop = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl_xinyue.SuspendLayout();
            this.tabPage_ryzc.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_int
            // 
            this.label_int.AutoSize = true;
            this.label_int.Location = new System.Drawing.Point(50, 23);
            this.label_int.Name = "label_int";
            this.label_int.Size = new System.Drawing.Size(59, 12);
            this.label_int.TabIndex = 0;
            this.label_int.Text = "加载中...";
            // 
            // listView_task
            // 
            this.listView_task.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.task_id,
            this.task_name,
            this.task_score,
            this.task_status});
            this.listView_task.FullRowSelect = true;
            this.listView_task.Location = new System.Drawing.Point(16, 21);
            this.listView_task.Name = "listView_task";
            this.listView_task.Size = new System.Drawing.Size(320, 229);
            this.listView_task.TabIndex = 0;
            this.listView_task.UseCompatibleStateImageBehavior = false;
            this.listView_task.View = System.Windows.Forms.View.Details;
            // 
            // task_id
            // 
            this.task_id.Text = "任务ID";
            // 
            // task_name
            // 
            this.task_name.Text = "任务名称";
            this.task_name.Width = 150;
            // 
            // task_score
            // 
            this.task_score.Text = "分数";
            this.task_score.Width = 40;
            // 
            // task_status
            // 
            this.task_status.Text = "状态";
            // 
            // tabControl_xinyue
            // 
            this.tabControl_xinyue.Controls.Add(this.tabPage_ryzc);
            this.tabControl_xinyue.Controls.Add(this.tabPage2);
            this.tabControl_xinyue.Location = new System.Drawing.Point(12, 47);
            this.tabControl_xinyue.Name = "tabControl_xinyue";
            this.tabControl_xinyue.SelectedIndex = 0;
            this.tabControl_xinyue.Size = new System.Drawing.Size(453, 396);
            this.tabControl_xinyue.TabIndex = 1;
            // 
            // tabPage_ryzc
            // 
            this.tabPage_ryzc.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_ryzc.Controls.Add(this.checkBox_twoscore);
            this.tabPage_ryzc.Controls.Add(this.button_onesubmit);
            this.tabPage_ryzc.Controls.Add(this.button_submitS);
            this.tabPage_ryzc.Controls.Add(this.button_freesubmit);
            this.tabPage_ryzc.Controls.Add(this.button_refresh);
            this.tabPage_ryzc.Controls.Add(this.button_towscore);
            this.tabPage_ryzc.Controls.Add(this.button_binding);
            this.tabPage_ryzc.Controls.Add(this.groupBox1);
            this.tabPage_ryzc.Controls.Add(this.listView_task);
            this.tabPage_ryzc.Location = new System.Drawing.Point(4, 22);
            this.tabPage_ryzc.Name = "tabPage_ryzc";
            this.tabPage_ryzc.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ryzc.Size = new System.Drawing.Size(445, 370);
            this.tabPage_ryzc.TabIndex = 0;
            this.tabPage_ryzc.Text = "-荣誉战场-";
            // 
            // checkBox_twoscore
            // 
            this.checkBox_twoscore.AutoSize = true;
            this.checkBox_twoscore.Checked = true;
            this.checkBox_twoscore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_twoscore.Location = new System.Drawing.Point(358, 62);
            this.checkBox_twoscore.Name = "checkBox_twoscore";
            this.checkBox_twoscore.Size = new System.Drawing.Size(66, 16);
            this.checkBox_twoscore.TabIndex = 5;
            this.checkBox_twoscore.Text = "3分双倍";
            this.checkBox_twoscore.UseVisualStyleBackColor = true;
            // 
            // button_onesubmit
            // 
            this.button_onesubmit.Location = new System.Drawing.Point(354, 20);
            this.button_onesubmit.Name = "button_onesubmit";
            this.button_onesubmit.Size = new System.Drawing.Size(75, 36);
            this.button_onesubmit.TabIndex = 3;
            this.button_onesubmit.Text = "一键完成";
            this.button_onesubmit.UseVisualStyleBackColor = true;
            this.button_onesubmit.Click += new System.EventHandler(this.button_Click);
            // 
            // button_submitS
            // 
            this.button_submitS.Location = new System.Drawing.Point(354, 113);
            this.button_submitS.Name = "button_submitS";
            this.button_submitS.Size = new System.Drawing.Size(75, 23);
            this.button_submitS.TabIndex = 3;
            this.button_submitS.Text = "完成当前";
            this.button_submitS.UseVisualStyleBackColor = true;
            this.button_submitS.Click += new System.EventHandler(this.button_Click);
            // 
            // button_freesubmit
            // 
            this.button_freesubmit.Location = new System.Drawing.Point(354, 142);
            this.button_freesubmit.Name = "button_freesubmit";
            this.button_freesubmit.Size = new System.Drawing.Size(75, 23);
            this.button_freesubmit.TabIndex = 3;
            this.button_freesubmit.Text = "免做完成";
            this.button_freesubmit.UseVisualStyleBackColor = true;
            this.button_freesubmit.Click += new System.EventHandler(this.button_Click);
            // 
            // button_refresh
            // 
            this.button_refresh.Location = new System.Drawing.Point(354, 84);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(75, 23);
            this.button_refresh.TabIndex = 3;
            this.button_refresh.Text = "刷新任务";
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.button_Click);
            // 
            // button_towscore
            // 
            this.button_towscore.Location = new System.Drawing.Point(354, 171);
            this.button_towscore.Name = "button_towscore";
            this.button_towscore.Size = new System.Drawing.Size(75, 23);
            this.button_towscore.TabIndex = 3;
            this.button_towscore.Text = "双倍完成";
            this.button_towscore.UseVisualStyleBackColor = true;
            this.button_towscore.Click += new System.EventHandler(this.button_Click);
            // 
            // button_binding
            // 
            this.button_binding.Location = new System.Drawing.Point(354, 227);
            this.button_binding.Name = "button_binding";
            this.button_binding.Size = new System.Drawing.Size(75, 23);
            this.button_binding.TabIndex = 3;
            this.button_binding.Text = "更改绑定";
            this.button_binding.UseVisualStyleBackColor = true;
            this.button_binding.Click += new System.EventHandler(this.button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_binding);
            this.groupBox1.Controls.Add(this.label_prop);
            this.groupBox1.Location = new System.Drawing.Point(16, 267);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(408, 85);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "状态";
            // 
            // label_binding
            // 
            this.label_binding.AutoSize = true;
            this.label_binding.Location = new System.Drawing.Point(20, 43);
            this.label_binding.Name = "label_binding";
            this.label_binding.Size = new System.Drawing.Size(41, 12);
            this.label_binding.TabIndex = 2;
            this.label_binding.Text = "加载中";
            // 
            // label_prop
            // 
            this.label_prop.AutoSize = true;
            this.label_prop.Location = new System.Drawing.Point(186, 43);
            this.label_prop.Name = "label_prop";
            this.label_prop.Size = new System.Drawing.Size(59, 12);
            this.label_prop.TabIndex = 1;
            this.label_prop.Text = "加载中...";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(445, 370);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "待拓展";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // xinyueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 463);
            this.Controls.Add(this.tabControl_xinyue);
            this.Controls.Add(this.label_int);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "xinyueForm";
            this.Text = "心悦专区";
            this.Load += new System.EventHandler(this.xinyueForm_Load);
            this.tabControl_xinyue.ResumeLayout(false);
            this.tabPage_ryzc.ResumeLayout(false);
            this.tabPage_ryzc.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_int;
        private System.Windows.Forms.ListView listView_task;
        private System.Windows.Forms.ColumnHeader task_id;
        private System.Windows.Forms.ColumnHeader task_name;
        private System.Windows.Forms.ColumnHeader task_score;
        private System.Windows.Forms.ColumnHeader task_status;
        private System.Windows.Forms.TabControl tabControl_xinyue;
        private System.Windows.Forms.TabPage tabPage_ryzc;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label_prop;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_binding;
        private System.Windows.Forms.Button button_binding;
        private System.Windows.Forms.Button button_onesubmit;
        private System.Windows.Forms.Button button_submitS;
        private System.Windows.Forms.Button button_freesubmit;
        private System.Windows.Forms.Button button_towscore;
        private System.Windows.Forms.CheckBox checkBox_twoscore;
        private System.Windows.Forms.Button button_refresh;
    }
}