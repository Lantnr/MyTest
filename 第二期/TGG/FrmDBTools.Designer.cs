namespace TGG
{
    partial class FrmDBTools
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
            this.btn_InitView = new System.Windows.Forms.Button();
            this.btn_Dalete = new System.Windows.Forms.Button();
            this.btn_upgrade_games = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_InitView
            // 
            this.btn_InitView.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_InitView.Location = new System.Drawing.Point(41, 12);
            this.btn_InitView.Name = "btn_InitView";
            this.btn_InitView.Size = new System.Drawing.Size(111, 32);
            this.btn_InitView.TabIndex = 0;
            this.btn_InitView.Text = "初始化数据库";
            this.btn_InitView.UseVisualStyleBackColor = true;
            this.btn_InitView.Click += new System.EventHandler(this.btn_Init_Click);
            // 
            // btn_Dalete
            // 
            this.btn_Dalete.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Dalete.Location = new System.Drawing.Point(191, 12);
            this.btn_Dalete.Name = "btn_Dalete";
            this.btn_Dalete.Size = new System.Drawing.Size(111, 32);
            this.btn_Dalete.TabIndex = 1;
            this.btn_Dalete.Text = "删除表视图!";
            this.btn_Dalete.UseVisualStyleBackColor = true;
            this.btn_Dalete.Click += new System.EventHandler(this.btn_Dalete_Click);
            // 
            // btn_upgrade_games
            // 
            this.btn_upgrade_games.Location = new System.Drawing.Point(41, 62);
            this.btn_upgrade_games.Name = "btn_upgrade_games";
            this.btn_upgrade_games.Size = new System.Drawing.Size(111, 30);
            this.btn_upgrade_games.TabIndex = 2;
            this.btn_upgrade_games.Text = "升级游戏集";
            this.btn_upgrade_games.UseVisualStyleBackColor = true;
            this.btn_upgrade_games.Click += new System.EventHandler(this.btn_upgrade_games_Click);
            // 
            // FrmDBTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 211);
            this.Controls.Add(this.btn_upgrade_games);
            this.Controls.Add(this.btn_Dalete);
            this.Controls.Add(this.btn_InitView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmDBTools";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库工具";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_InitView;
        private System.Windows.Forms.Button btn_Dalete;
        private System.Windows.Forms.Button btn_upgrade_games;
    }
}