namespace TGG
{
    partial class FrmActivation
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_activation_count = new System.Windows.Forms.TextBox();
            this.btn_activation_build = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_activation = new System.Windows.Forms.Button();
            this.txt_user_code = new System.Windows.Forms.TextBox();
            this.txt_user_key = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgv_code = new System.Windows.Forms.DataGridView();
            this.key_state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_code)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txt_activation_count);
            this.groupBox1.Controls.Add(this.btn_activation_build);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(468, 103);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "生成";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "激活码个数";
            // 
            // txt_activation_count
            // 
            this.txt_activation_count.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_activation_count.Location = new System.Drawing.Point(83, 25);
            this.txt_activation_count.Name = "txt_activation_count";
            this.txt_activation_count.Size = new System.Drawing.Size(379, 21);
            this.txt_activation_count.TabIndex = 1;
            this.txt_activation_count.Text = "1";
            // 
            // btn_activation_build
            // 
            this.btn_activation_build.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_activation_build.Location = new System.Drawing.Point(397, 65);
            this.btn_activation_build.Name = "btn_activation_build";
            this.btn_activation_build.Size = new System.Drawing.Size(65, 23);
            this.btn_activation_build.TabIndex = 0;
            this.btn_activation_build.Text = "生成激活码";
            this.btn_activation_build.UseVisualStyleBackColor = true;
            this.btn_activation_build.Click += new System.EventHandler(this.btn_activation_build_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_activation);
            this.groupBox2.Controls.Add(this.txt_user_code);
            this.groupBox2.Controls.Add(this.txt_user_key);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 106);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(468, 216);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "激活";
            // 
            // btn_activation
            // 
            this.btn_activation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_activation.Location = new System.Drawing.Point(397, 97);
            this.btn_activation.Name = "btn_activation";
            this.btn_activation.Size = new System.Drawing.Size(65, 23);
            this.btn_activation.TabIndex = 7;
            this.btn_activation.Text = "激活";
            this.btn_activation.UseVisualStyleBackColor = true;
            this.btn_activation.Click += new System.EventHandler(this.btn_activation_Click);
            // 
            // txt_user_code
            // 
            this.txt_user_code.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_user_code.Location = new System.Drawing.Point(83, 58);
            this.txt_user_code.Name = "txt_user_code";
            this.txt_user_code.Size = new System.Drawing.Size(379, 21);
            this.txt_user_code.TabIndex = 6;
            // 
            // txt_user_key
            // 
            this.txt_user_key.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_user_key.Location = new System.Drawing.Point(83, 24);
            this.txt_user_key.Name = "txt_user_key";
            this.txt_user_key.Size = new System.Drawing.Size(379, 21);
            this.txt_user_key.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "账号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "激活码";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(482, 351);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(474, 325);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgv_code);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(474, 325);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "查询";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgv_code
            // 
            this.dgv_code.AllowUserToAddRows = false;
            this.dgv_code.AllowUserToDeleteRows = false;
            this.dgv_code.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_code.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.key_state,
            this.user_code,
            this.user_key});
            this.dgv_code.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_code.Location = new System.Drawing.Point(3, 3);
            this.dgv_code.Name = "dgv_code";
            this.dgv_code.RowTemplate.Height = 23;
            this.dgv_code.Size = new System.Drawing.Size(468, 319);
            this.dgv_code.TabIndex = 0;
            // 
            // key_state
            // 
            this.key_state.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.key_state.HeaderText = "状态";
            this.key_state.Name = "key_state";
            this.key_state.Width = 54;
            // 
            // user_code
            // 
            this.user_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.user_code.FillWeight = 35F;
            this.user_code.HeaderText = "账号";
            this.user_code.MinimumWidth = 35;
            this.user_code.Name = "user_code";
            // 
            // user_key
            // 
            this.user_key.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.user_key.FillWeight = 65F;
            this.user_key.HeaderText = "激活码";
            this.user_key.Name = "user_key";
            // 
            // FrmActivation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 351);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmActivation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "激活工具";
            this.Load += new System.EventHandler(this.FrmActivation_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_code)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_activation_count;
        private System.Windows.Forms.Button btn_activation_build;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_activation;
        private System.Windows.Forms.TextBox txt_user_code;
        private System.Windows.Forms.TextBox txt_user_key;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgv_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn key_state;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_key;
    }
}