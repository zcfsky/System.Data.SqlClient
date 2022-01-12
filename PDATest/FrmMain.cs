using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PDATest
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        string conStr = null;

        private void btnOpenCon_Click(object sender, EventArgs e)
        {
            string constr = txtSql.Text;
            if (string.IsNullOrEmpty(constr))
                return;
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();
                    conStr = constr;
                    txtSql.Text = "select @@version";
                    MessageBox.Show("连接成功");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnTestSql_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(conStr))
                return;
            string sql = txtSql.Text;
            if (string.IsNullOrEmpty(sql))
                MessageBox.Show("查询语句不能为空");
            using (SqlConnection con = new SqlConnection(conStr))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandText = sql;
                    MessageBox.Show("结果：" + cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}