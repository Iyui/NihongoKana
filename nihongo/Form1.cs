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
    public partial class Form1 : Form
    {
        static readonly string hiragana = Gojyuon.hiragana;//平假名
        static readonly string katagana = Gojyuon.katagana;//片假名
        static readonly string[] duyin1 = Gojyuon.duyin1;

        static readonly string zhuoyin1 = Gojyuon.zhuoyin1;//浊音平假名
        static readonly string zhuoyin2 = Gojyuon.zhuoyin2;//浊音片假名
        static readonly string[] duyin2 = Gojyuon.duyin2;

        static readonly string[] aoyin1 = Gojyuon.aoyin1;//拗音平假名
        static readonly string[] aoyin2 = Gojyuon.aoyin1;//拗音片假名
        static readonly string[] duyin3 = Gojyuon.duyin3;



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private string RomajiGojyuon(string code)
        {
            code = code.ToLower();
            if (code == "zi")
            {
                code = "ji";
            }
            else if (code == "ja")
                code = "zya";
            else if (code == "ju")
                code = "zyu";
            else if (code == "jo")
                code = "zyo";
            if (btKatagana.Checked)
            {
                
                int ind = Array.IndexOf(duyin1, code);
                if (ind != -1)
                {
                    return hiragana[ind].ToString();
                }
                else
                {
                    ind = Array.IndexOf(duyin2, code);
                    if (ind != -1)
                    {
                        return zhuoyin1[ind].ToString();
                    }
                    else
                    {
                        ind = Array.IndexOf(duyin3, code);
                        if (ind != -1)
                            return aoyin1[ind];
                        else
                            return " ";
                    }
                }
            }
            else
            {
                int ind = Array.IndexOf(duyin1, code);
                if (ind != -1)
                {
                    return katagana[ind].ToString();
                }
                else
                {
                    ind = Array.IndexOf(duyin2, code);
                    if (ind != -1)
                    {
                        return zhuoyin2[ind].ToString();
                    }
                    else
                    {
                        ind = Array.IndexOf(duyin3, code);
                        if (ind != -1)
                            return aoyin2[ind];
                        else
                            return " ";
                    }
                }
            }
        }

        string Code = "";
        string roma = "qwertyuiopasdfghjklzxcvbnm ";
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string r = "";
             r += e.KeyCode;
            if (roma.Contains(r.ToLower())||e.KeyCode == Keys.Space)
            {
                r = "";
            }
            else if(e.KeyCode == Keys.Back)
                r = "";
            else
                return;

            if (e.KeyCode == Keys.Space)
            {
                textBox2.Text += RomajiGojyuon(Code);
                Code = "";
            }
            else if (e.KeyCode == Keys.Back)
            {
                if (Code.Length > 0)
                    Code = Code.Substring(0, Code.Length - 1);
            }
            else
            {
                Code += e.KeyCode;
            }
            toolStripLabel1.Text = Code.ToLower();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsLetter(e.KeyChar) || !char.IsWhiteSpace(e.KeyChar))
            //{
            //    if (e.KeyChar != (char)Keys.Back)
            //    {
            //        e.Handled = true;
            //    }
            //}
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            Code = "";
            toolStripLabel1.Text = Code.ToLower();
        }
    }
}
