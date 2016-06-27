using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGG.Core.Entity;
using TGG.Share.Event;

namespace TGG
{
    public partial class FrmActivation : Form
    {
        public FrmActivation()
        {
            InitializeComponent();
            DBPreheat();
        }


        /// <summary>数据库预热</summary>
        private void DBPreheat()
        {
            DisplayGlobal.log.Write("激活数据库预热");
            code.Find("id=0"); ;
        }

        /// <summary>
        /// 向数据库生成激活码
        /// </summary>
        private void btn_activation_build_Click(object sender, EventArgs e)
        {
            InsertCode();
            BindData();
        }

        /// <summary>
        ///  向数据库插入生成激活码
        /// </summary>
        private void InsertCode()
        {
            try
            {
                var num = Convert.ToInt32(this.txt_activation_count.Text);
                for (int i = 0; i < num; i++)
                {
                    var entity = new code { state = 0, user_key = Guid.NewGuid().ToString() };
                    entity.Save();
                }
                DisplayGlobal.log.Write("激活码生成完毕");
                MessageBox.Show("激活码生成完毕");
            }
            catch (Exception ex)
            {
                DisplayGlobal.log.Write(ex.Message);
            }
        }

        private void btn_activation_Click(object sender, EventArgs e)
        {
            AvtivationCode();
            BindData();
        }

        private void AvtivationCode()
        {
            try
            {
                var key = this.txt_user_key.Text;
                var user = this.txt_user_code.Text;
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(user))
                {
                    DisplayGlobal.log.Write("激活码和账号不能为空");
                    MessageBox.Show("激活码和账号不能为空");
                    return;
                }
                var _where = string.Format("[user_key]='{0}'", key);
                var entity = code.Find(_where);
                if (entity == null)
                {
                    DisplayGlobal.log.Write("激活码不存在");
                    return;
                }
                if (entity.state != 0)
                {
                    DisplayGlobal.log.Write("激活码已被激活,请使用新激活码");
                    return;
                }

                _where = string.Format("[user_code]='{0}'", user);
                var _count = code.FindCount(_where,null,null,0,0);
                if (_count > 0)
                {
                    DisplayGlobal.log.Write("该账号已激活过,请使用新账号");
                    return;
                }
                entity.user_code = user;
                entity.state = 1;
                entity.Save();
                DisplayGlobal.log.Write("账号激活成功");
                MessageBox.Show("账号激活成功");
            }
            catch (Exception ex)
            {
                DisplayGlobal.log.Write(ex.Message);
            }
        }


        private void BindData()
        {
            dgv_code.Rows.Clear();
            var list = code.FindAll().ToList().OrderByDescending(m=>m.state);
            foreach (var item in list)
            {
                var dgvr = new DataGridViewRow();

                var t_2 = new DataGridViewTextBoxCell();
                t_2.Value = item.state == 0 ? "未激活" : "已激活";
                dgvr.Cells.Add(t_2);

                var t_1 = new DataGridViewTextBoxCell();
                t_1.Value = item.user_code;
                dgvr.Cells.Add(t_1);

                var t_0 = new DataGridViewTextBoxCell();
                t_0.Value = item.user_key;
                dgvr.Cells.Add(t_0);

                dgv_code.Rows.Add(dgvr);
            }
        }

        private void FrmActivation_Load(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
