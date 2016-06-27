using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGG.Core.Entity;
using TGG.Share.Event;
using XCode.DataAccessLayer;

namespace TGG
{
    public partial class FrmDBTools : Form
    {
        public FrmDBTools()
        {
            InitializeComponent();
        }

        /// <summary>初始化数据库视图</summary>
        private void InitDBView()
        {
            try
            {
                var connName = "TG";
                var commStr = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
                DAL.AddConnStr(connName, commStr, null, "MSSQL");
                var db = DAL.Create(connName);
                db.Session.Execute(TGG.Resources.Resource.tgg_view);
                DisplayGlobal.log.Write("初始化数据库视图成功");
            }
            catch (Exception)
            {
                DisplayGlobal.log.Write("初始化数据库视图失败");
            }
        }

        /// <summary>初始化数据库视图</summary>
        private void DeleteDBView()
        {
            try
            {
                var connName = "TG";
                var commStr = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
                DAL.AddConnStr(connName, commStr, null, "MSSQL");
                var db = DAL.Create(connName);
                db.Session.Execute(TGG.Resources.Resource.delete);
                DisplayGlobal.log.Write("删除数据库成功");
            }
            catch (Exception)
            {
                DisplayGlobal.log.Write("删除数据库失败");
            }
        }

        private void InitDBTable()
        {
            try
            {
                //var connName = "TG";
                //var commStr = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
                //DAL.AddConnStr(connName, commStr, null, "MSSQL");
                //var db = DAL.Create(connName);
                tg_user_login_log.GetEntityByUserId(0);
                DisplayGlobal.log.Write("删除数据库成功");
            }
            catch (Exception)
            {
                DisplayGlobal.log.Write("删除数据库失败");
            }
        }


        private void btn_Dalete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除数据?\r\n删除后游戏无法正常运行!请重启程序!", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                DeleteDBView();
        }

        private void btn_Init_Click(object sender, EventArgs e)
        {
            InitDBView();
        }

        private void btn_InitTable_Click(object sender, EventArgs e)
        {
            InitDBTable();
        }

        private void btn_upgrade_games_Click(object sender, EventArgs e)
        {
            UpgradeGames();
        }

        /// <summary>升级游戏集</summary>
        private void UpgradeGames()
        {
            try
            {
                var connName = "TG";
                var commStr = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
                DAL.AddConnStr(connName, commStr, null, "MSSQL");
                var db = DAL.Create(connName);
                var sql = "INSERT INTO tg_game(user_id) SELECT id FROM tg_user ";
                db.Session.Execute(sql);
                DisplayGlobal.log.Write("升级游戏集成功");
            }
            catch (Exception)
            {
                DisplayGlobal.log.Write("升级游戏集失败");
            }
        }

    }
}
