using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace nihongo
{
    public partial class Form2 : Form
    {
        public Form2(string nihongo,bool pianjia,string danyuan,bool ju)
        {
            InitializeComponent();
            Nihongo = nihongo;
            Pianjia = pianjia;
            Danyuan = danyuan;
            juzi = ju;
            ActiveControl = textBox1;
        }

        string Nihongo = "";
        bool Pianjia = false;
        string Danyuan = "";
        bool juzi = false;
        string ConnStr = "server=.;database=nihongo;Integrated Security = True";
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                SqlParameter[] parameter = new SqlParameter[] {
                    new SqlParameter("@中文",textBox1.Text),new SqlParameter("@translate",Nihongo),
                            new SqlParameter("@外来词",Pianjia),new SqlParameter("@单元",Danyuan ),new SqlParameter("@句子",juzi)};
                
                if(AddData(conn, goodstoreSql, parameter))
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("录入失败");
                }
            }
        }

        public bool AddData(SqlConnection conn, string sqlStr, params SqlParameter[] parameter)
        {
            try
            {
                //conn.Open();
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.Parameters.AddRange(parameter);
                //cmd.Transaction = tran;
                var row = cmd.ExecuteNonQuery();
                if (row > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("添加数据异常" + ex.Message);
            }
        }

        string goodstoreSql = "INSERT INTO note(" +
                        "中文,translate,外来词,单元,句子) " +
                        "VALUES(" +
                        "@中文,@translate,@外来词,@单元,@句子)";

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
