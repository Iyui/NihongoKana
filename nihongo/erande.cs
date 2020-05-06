using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nihongo
{
    public partial class erande : Form
    {
        public erande()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!checkBox5.Checked && !checkBox4.Checked)
            {
                MessageBox.Show("平/片假名必须选择至少一项");
                return;
            }
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked)
            {
                MessageBox.Show("清/浊/拗音必须选择至少一项");
                return;
            }
            bool[] t = new bool[] { checkBox5.Checked, checkBox4.Checked, checkBox1.Checked, checkBox2.Checked, checkBox3.Checked };
            gojyuonTest gyt = new gojyuonTest(int.Parse(comboBox1.Text), t);
            gyt.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void erande_Load(object sender, EventArgs e)
        {

        }
    }
}
