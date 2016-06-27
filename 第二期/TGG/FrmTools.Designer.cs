namespace TGG
{
    partial class FrmTools
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
            this.txt_result = new System.Windows.Forms.TextBox();
            this.btn_ToTime = new System.Windows.Forms.Button();
            this.txt_write = new System.Windows.Forms.TextBox();
            this.btn_60 = new System.Windows.Forms.Button();
            this.txt_60 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_task = new System.Windows.Forms.Button();
            this.btn_update = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_time_yyyy = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.txt_Ticks = new System.Windows.Forms.TextBox();
            this.txt_time_MM = new System.Windows.Forms.TextBox();
            this.txt_time_dd = new System.Windows.Forms.TextBox();
            this.txt_time_HH = new System.Windows.Forms.TextBox();
            this.txt_time_hhmm = new System.Windows.Forms.TextBox();
            this.txt_time_ss = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_Math = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_result
            // 
            this.txt_result.Location = new System.Drawing.Point(215, 15);
            this.txt_result.Name = "txt_result";
            this.txt_result.Size = new System.Drawing.Size(154, 21);
            this.txt_result.TabIndex = 1;
            // 
            // btn_ToTime
            // 
            this.btn_ToTime.Location = new System.Drawing.Point(134, 15);
            this.btn_ToTime.Name = "btn_ToTime";
            this.btn_ToTime.Size = new System.Drawing.Size(75, 23);
            this.btn_ToTime.TabIndex = 2;
            this.btn_ToTime.Text = "to time ->";
            this.btn_ToTime.UseVisualStyleBackColor = true;
            this.btn_ToTime.Click += new System.EventHandler(this.btn_ToTime_Click);
            // 
            // txt_write
            // 
            this.txt_write.Location = new System.Drawing.Point(18, 15);
            this.txt_write.Name = "txt_write";
            this.txt_write.Size = new System.Drawing.Size(110, 21);
            this.txt_write.TabIndex = 3;
            // 
            // btn_60
            // 
            this.btn_60.Location = new System.Drawing.Point(134, 42);
            this.btn_60.Name = "btn_60";
            this.btn_60.Size = new System.Drawing.Size(75, 23);
            this.btn_60.TabIndex = 5;
            this.btn_60.Text = "to 60. ->";
            this.btn_60.UseVisualStyleBackColor = true;
            this.btn_60.Click += new System.EventHandler(this.btn_60_Click);
            // 
            // txt_60
            // 
            this.txt_60.Location = new System.Drawing.Point(215, 42);
            this.txt_60.Name = "txt_60";
            this.txt_60.Size = new System.Drawing.Size(154, 21);
            this.txt_60.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_task
            // 
            this.btn_task.Location = new System.Drawing.Point(175, 20);
            this.btn_task.Name = "btn_task";
            this.btn_task.Size = new System.Drawing.Size(75, 23);
            this.btn_task.TabIndex = 7;
            this.btn_task.Text = "task";
            this.btn_task.UseVisualStyleBackColor = true;
            this.btn_task.Click += new System.EventHandler(this.btn_task_Click);
            // 
            // btn_update
            // 
            this.btn_update.Location = new System.Drawing.Point(94, 20);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(75, 23);
            this.btn_update.TabIndex = 8;
            this.btn_update.Text = "Update";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_task);
            this.groupBox1.Controls.Add(this.btn_update);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 184);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 62);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Test";
            // 
            // txt_time_yyyy
            // 
            this.txt_time_yyyy.Location = new System.Drawing.Point(42, 18);
            this.txt_time_yyyy.Name = "txt_time_yyyy";
            this.txt_time_yyyy.Size = new System.Drawing.Size(58, 21);
            this.txt_time_yyyy.TabIndex = 10;
            this.txt_time_yyyy.Text = "2014";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(41, 71);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(58, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "转换";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txt_Ticks
            // 
            this.txt_Ticks.Location = new System.Drawing.Point(134, 73);
            this.txt_Ticks.Name = "txt_Ticks";
            this.txt_Ticks.Size = new System.Drawing.Size(152, 21);
            this.txt_Ticks.TabIndex = 12;
            // 
            // txt_time_MM
            // 
            this.txt_time_MM.Location = new System.Drawing.Point(135, 18);
            this.txt_time_MM.Name = "txt_time_MM";
            this.txt_time_MM.Size = new System.Drawing.Size(58, 21);
            this.txt_time_MM.TabIndex = 13;
            this.txt_time_MM.Text = "10";
            // 
            // txt_time_dd
            // 
            this.txt_time_dd.Location = new System.Drawing.Point(228, 18);
            this.txt_time_dd.Name = "txt_time_dd";
            this.txt_time_dd.Size = new System.Drawing.Size(58, 21);
            this.txt_time_dd.TabIndex = 14;
            this.txt_time_dd.Text = "22";
            // 
            // txt_time_HH
            // 
            this.txt_time_HH.Location = new System.Drawing.Point(41, 43);
            this.txt_time_HH.Name = "txt_time_HH";
            this.txt_time_HH.Size = new System.Drawing.Size(58, 21);
            this.txt_time_HH.TabIndex = 15;
            this.txt_time_HH.Text = "0";
            // 
            // txt_time_hhmm
            // 
            this.txt_time_hhmm.Location = new System.Drawing.Point(135, 43);
            this.txt_time_hhmm.Name = "txt_time_hhmm";
            this.txt_time_hhmm.Size = new System.Drawing.Size(58, 21);
            this.txt_time_hhmm.TabIndex = 16;
            this.txt_time_hhmm.Text = "0";
            // 
            // txt_time_ss
            // 
            this.txt_time_ss.Location = new System.Drawing.Point(228, 43);
            this.txt_time_ss.Name = "txt_time_ss";
            this.txt_time_ss.Size = new System.Drawing.Size(58, 21);
            this.txt_time_ss.TabIndex = 17;
            this.txt_time_ss.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "年";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "月";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "日";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "小时";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(105, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 22;
            this.label5.Text = "分钟";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(206, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 23;
            this.label6.Text = "秒";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(470, 286);
            this.tabControl1.TabIndex = 24;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(462, 260);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "测试";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_Math);
            this.groupBox3.Controls.Add(this.txt_time_MM);
            this.groupBox3.Controls.Add(this.txt_time_hhmm);
            this.groupBox3.Controls.Add(this.txt_time_dd);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txt_time_HH);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txt_Ticks);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txt_time_ss);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txt_time_yyyy);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(3, 78);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(456, 106);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "时间转Ticks";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_ToTime);
            this.groupBox2.Controls.Add(this.btn_60);
            this.groupBox2.Controls.Add(this.txt_60);
            this.groupBox2.Controls.Add(this.txt_result);
            this.groupBox2.Controls.Add(this.txt_write);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(456, 75);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ticks转时间";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(462, 260);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据库工具";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_Math
            // 
            this.btn_Math.Location = new System.Drawing.Point(326, 17);
            this.btn_Math.Name = "btn_Math";
            this.btn_Math.Size = new System.Drawing.Size(75, 23);
            this.btn_Math.TabIndex = 24;
            this.btn_Math.Text = "Math";
            this.btn_Math.UseVisualStyleBackColor = true;
            this.btn_Math.Click += new System.EventHandler(this.btn_Math_Click);
            // 
            // FrmTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 286);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTools";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工具";
            this.Load += new System.EventHandler(this.FrmTools_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_result;
        private System.Windows.Forms.Button btn_ToTime;
        private System.Windows.Forms.TextBox txt_write;
        private System.Windows.Forms.Button btn_60;
        private System.Windows.Forms.TextBox txt_60;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_task;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_time_yyyy;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txt_Ticks;
        private System.Windows.Forms.TextBox txt_time_MM;
        private System.Windows.Forms.TextBox txt_time_dd;
        private System.Windows.Forms.TextBox txt_time_HH;
        private System.Windows.Forms.TextBox txt_time_hhmm;
        private System.Windows.Forms.TextBox txt_time_ss;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btn_Math;
    }
}