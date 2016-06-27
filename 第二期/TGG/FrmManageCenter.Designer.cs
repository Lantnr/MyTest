namespace TGG
{
    partial class FrmManageCenter
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmManageCenter));
            this.tspb_Load = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tss_Msg = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_UpdateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_Version = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.rtb_Loginfo = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_Start = new System.Windows.Forms.ToolStripButton();
            this.tsb_Stop = new System.Windows.Forms.ToolStripButton();
            this.tsb_Reset = new System.Windows.Forms.ToolStripButton();
            this.tsb_Initial = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btn_UnloadNext = new System.Windows.Forms.Button();
            this.btn_LoadNext = new System.Windows.Forms.Button();
            this.chk_SelectAll = new System.Windows.Forms.CheckBox();
            this.dgv_tgg = new System.Windows.Forms.DataGridView();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ModuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleDLL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsLoad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer_remove = new System.Windows.Forms.Timer(this.components);
            this.timer_Unload = new System.Windows.Forms.Timer(this.components);
            this.timer_Load = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_tgg)).BeginInit();
            this.SuspendLayout();
            // 
            // tspb_Load
            // 
            this.tspb_Load.Name = "tspb_Load";
            this.tspb_Load.Size = new System.Drawing.Size(100, 16);
            this.tspb_Load.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(52, 17);
            this.toolStripStatusLabel1.Text = "           ";
            // 
            // tss_Msg
            // 
            this.tss_Msg.Enabled = false;
            this.tss_Msg.Name = "tss_Msg";
            this.tss_Msg.Size = new System.Drawing.Size(44, 17);
            this.tss_Msg.Text = "Ready";
            // 
            // tssl_UpdateTime
            // 
            this.tssl_UpdateTime.Enabled = false;
            this.tssl_UpdateTime.Name = "tssl_UpdateTime";
            this.tssl_UpdateTime.Size = new System.Drawing.Size(126, 17);
            this.tssl_UpdateTime.Text = "2014-05-29 15:30:30";
            // 
            // tssl_Version
            // 
            this.tssl_Version.Enabled = false;
            this.tssl_Version.Name = "tssl_Version";
            this.tssl_Version.Size = new System.Drawing.Size(92, 17);
            this.tssl_Version.Text = "Version:1.0.0.0";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssl_Version,
            this.tssl_UpdateTime,
            this.tss_Msg,
            this.toolStripStatusLabel1,
            this.tspb_Load});
            this.statusStrip1.Location = new System.Drawing.Point(0, 374);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(614, 22);
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // rtb_Loginfo
            // 
            this.rtb_Loginfo.BackColor = System.Drawing.SystemColors.Control;
            this.rtb_Loginfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Loginfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rtb_Loginfo.Location = new System.Drawing.Point(0, 0);
            this.rtb_Loginfo.Name = "rtb_Loginfo";
            this.rtb_Loginfo.Size = new System.Drawing.Size(614, 238);
            this.rtb_Loginfo.TabIndex = 13;
            this.rtb_Loginfo.Text = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_Start,
            this.tsb_Stop,
            this.tsb_Reset,
            this.tsb_Initial});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(614, 25);
            this.toolStrip1.TabIndex = 20;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_Start
            // 
            this.tsb_Start.Image = global::TGG.Properties.Resources.start;
            this.tsb_Start.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Start.Name = "tsb_Start";
            this.tsb_Start.Size = new System.Drawing.Size(52, 22);
            this.tsb_Start.Text = "启动";
            this.tsb_Start.Click += new System.EventHandler(this.tsb_Start_Click);
            // 
            // tsb_Stop
            // 
            this.tsb_Stop.Image = global::TGG.Properties.Resources.stop;
            this.tsb_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Stop.Name = "tsb_Stop";
            this.tsb_Stop.Size = new System.Drawing.Size(52, 22);
            this.tsb_Stop.Text = "停止";
            this.tsb_Stop.Click += new System.EventHandler(this.tsb_Stop_Click);
            // 
            // tsb_Reset
            // 
            this.tsb_Reset.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsb_Reset.Image = global::TGG.Properties.Resources.refresh;
            this.tsb_Reset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Reset.Name = "tsb_Reset";
            this.tsb_Reset.Size = new System.Drawing.Size(76, 22);
            this.tsb_Reset.Text = "重置数据";
            this.tsb_Reset.Visible = false;
            this.tsb_Reset.Click += new System.EventHandler(this.tsb_Reset_Click);
            // 
            // tsb_Initial
            // 
            this.tsb_Initial.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsb_Initial.Image = global::TGG.Properties.Resources.initial;
            this.tsb_Initial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Initial.Name = "tsb_Initial";
            this.tsb_Initial.Size = new System.Drawing.Size(64, 22);
            this.tsb_Initial.Text = "初始化";
            this.tsb_Initial.Visible = false;
            this.tsb_Initial.Click += new System.EventHandler(this.tsb_Initial_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rtb_Loginfo);
            this.splitContainer1.Size = new System.Drawing.Size(614, 349);
            this.splitContainer1.SplitterDistance = 107;
            this.splitContainer1.TabIndex = 22;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btn_UnloadNext);
            this.splitContainer2.Panel1.Controls.Add(this.btn_LoadNext);
            this.splitContainer2.Panel1.Controls.Add(this.chk_SelectAll);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgv_tgg);
            this.splitContainer2.Size = new System.Drawing.Size(614, 107);
            this.splitContainer2.SplitterDistance = 118;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 22;
            // 
            // btn_UnloadNext
            // 
            this.btn_UnloadNext.Location = new System.Drawing.Point(12, 76);
            this.btn_UnloadNext.Name = "btn_UnloadNext";
            this.btn_UnloadNext.Size = new System.Drawing.Size(75, 23);
            this.btn_UnloadNext.TabIndex = 2;
            this.btn_UnloadNext.Text = "卸载";
            this.btn_UnloadNext.UseVisualStyleBackColor = true;
            this.btn_UnloadNext.Click += new System.EventHandler(this.btn_UnloadNext_Click);
            // 
            // btn_LoadNext
            // 
            this.btn_LoadNext.Location = new System.Drawing.Point(12, 47);
            this.btn_LoadNext.Name = "btn_LoadNext";
            this.btn_LoadNext.Size = new System.Drawing.Size(75, 23);
            this.btn_LoadNext.TabIndex = 1;
            this.btn_LoadNext.Text = "加载";
            this.btn_LoadNext.UseVisualStyleBackColor = true;
            this.btn_LoadNext.Click += new System.EventHandler(this.btn_LoadNext_Click);
            // 
            // chk_SelectAll
            // 
            this.chk_SelectAll.AutoSize = true;
            this.chk_SelectAll.Location = new System.Drawing.Point(12, 13);
            this.chk_SelectAll.Name = "chk_SelectAll";
            this.chk_SelectAll.Size = new System.Drawing.Size(96, 16);
            this.chk_SelectAll.TabIndex = 0;
            this.chk_SelectAll.Text = "扩展模块全选";
            this.chk_SelectAll.UseVisualStyleBackColor = true;
            this.chk_SelectAll.CheckedChanged += new System.EventHandler(this.chk_SelectAll_CheckedChanged);
            // 
            // dgv_tgg
            // 
            this.dgv_tgg.AllowUserToAddRows = false;
            this.dgv_tgg.AllowUserToDeleteRows = false;
            this.dgv_tgg.AllowUserToOrderColumns = true;
            this.dgv_tgg.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgv_tgg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_tgg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.ModuleName,
            this.ModuleDLL,
            this.Type,
            this.ModuleCount,
            this.IsLoad});
            this.dgv_tgg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_tgg.Location = new System.Drawing.Point(0, 0);
            this.dgv_tgg.Name = "dgv_tgg";
            this.dgv_tgg.ReadOnly = true;
            this.dgv_tgg.RowHeadersVisible = false;
            this.dgv_tgg.RowTemplate.Height = 23;
            this.dgv_tgg.Size = new System.Drawing.Size(491, 107);
            this.dgv_tgg.TabIndex = 6;
            this.dgv_tgg.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_tgg_CellClick);
            // 
            // chk
            // 
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.ReadOnly = true;
            this.chk.ThreeState = true;
            this.chk.Width = 20;
            // 
            // ModuleName
            // 
            this.ModuleName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ModuleName.HeaderText = "Name";
            this.ModuleName.Name = "ModuleName";
            this.ModuleName.ReadOnly = true;
            this.ModuleName.Width = 54;
            // 
            // ModuleDLL
            // 
            this.ModuleDLL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ModuleDLL.HeaderText = "DLL";
            this.ModuleDLL.Name = "ModuleDLL";
            this.ModuleDLL.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // ModuleCount
            // 
            this.ModuleCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ModuleCount.HeaderText = "Count";
            this.ModuleCount.Name = "ModuleCount";
            this.ModuleCount.ReadOnly = true;
            this.ModuleCount.Width = 60;
            // 
            // IsLoad
            // 
            this.IsLoad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.IsLoad.HeaderText = "IsLoad";
            this.IsLoad.Name = "IsLoad";
            this.IsLoad.ReadOnly = true;
            this.IsLoad.Width = 66;
            // 
            // timer_remove
            // 
            this.timer_remove.Interval = 1000;
            this.timer_remove.Tick += new System.EventHandler(this.timer_remove_Tick);
            // 
            // timer_Unload
            // 
            this.timer_Unload.Interval = 1000;
            this.timer_Unload.Tick += new System.EventHandler(this.timer_Unload_Tick);
            // 
            // timer_Load
            // 
            this.timer_Load.Interval = 1000;
            this.timer_Load.Tick += new System.EventHandler(this.timer_Load_Tick);
            // 
            // FrmManageCenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 396);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FrmManageCenter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TG模块管理中心";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmManageCenter_FormClosed);
            this.Load += new System.EventHandler(this.FrmManageCenter_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmManageCenter_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_tgg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripProgressBar tspb_Load;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tss_Msg;
        private System.Windows.Forms.ToolStripStatusLabel tssl_UpdateTime;
        private System.Windows.Forms.ToolStripStatusLabel tssl_Version;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.RichTextBox rtb_Loginfo;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_Start;
        private System.Windows.Forms.ToolStripButton tsb_Stop;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgv_tgg;
        private System.Windows.Forms.CheckBox chk_SelectAll;
        private System.Windows.Forms.Button btn_UnloadNext;
        private System.Windows.Forms.Button btn_LoadNext;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleDLL;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsLoad;
        private System.Windows.Forms.Timer timer_remove;
        private System.Windows.Forms.Timer timer_Unload;
        private System.Windows.Forms.Timer timer_Load;
        private System.Windows.Forms.ToolStripButton tsb_Initial;
        private System.Windows.Forms.ToolStripButton tsb_Reset;
    }
}