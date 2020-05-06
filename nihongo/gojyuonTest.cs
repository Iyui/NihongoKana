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
    public partial class gojyuonTest : Form
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

        public gojyuonTest()
        {
            InitializeComponent();
        }
        Dictionary<string, string> tiku = new Dictionary<string, string>();
        //List<string> tiku = new List<string>();



        public gojyuonTest(int shuliang, bool[] mod)
        {

            InitializeComponent();
            cou = shuliang;
            count = shuliang;
            if (mod[0])
            {
                if (mod[2])
                {
                    for (int i = 0; i < hiragana.Length; i++)
                    {
                        tiku.Add(hiragana[i].ToString(), duyin1[i]);
                    }
                }
                if (mod[3])
                {
                    for (int i = 0; i < zhuoyin1.Length; i++)
                    {
                        tiku.Add(zhuoyin1[i].ToString(), duyin2[i]);
                    }
                }
                if (mod[4])
                {
                    for (int i = 0; i < aoyin1.Length; i++)
                    {
                        tiku.Add(aoyin1[i].ToString(), duyin3[i]);
                    }
                }
            }
            if (mod[1])
            {
                if (mod[2])
                {
                    for (int i = 0; i < katagana.Length; i++)
                    {
                        if (tiku.ContainsKey(katagana[i].ToString()))
                            continue;
                        tiku.Add(katagana[i].ToString(), duyin1[i]);
                    }
                }
                if (mod[3])
                {
                    for (int i = 0; i < zhuoyin2.Length; i++)
                    {
                        if (tiku.ContainsKey(zhuoyin2[i].ToString()))
                            continue;
                        tiku.Add(zhuoyin2[i].ToString(), duyin2[i]);
                    }
                }
                if (mod[4])
                {
                    for (int i = 0; i < aoyin2.Length; i++)
                    {
                        if (tiku.ContainsKey(aoyin2[i].ToString()))
                            continue;
                        tiku.Add(aoyin2[i].ToString(), duyin3[i]);
                    }
                }
            }
            Random rd = new Random();
            int r = rd.Next(0, tiku.Count - 1);
            var x = tiku.ElementAt(r);
            label1.Text = x.Key;
            textBox1.Focus();
            label2.Text = shuliang.ToString();
            timestart = Environment.TickCount;
            ti = Environment.TickCount;
            this.ActiveControl = textBox1;
        }

        int timestart = 0;
        private void gojyuonTest_Load(object sender, EventArgs e)
        {

        }

        private void Test(int error)
        {
            if (cou == 0)
            {
                var list = reco.OrderByDescending(s => s.Value);
                string rec = "";
                int c = 1;
                foreach (var k in list)
                {
                    if (c > 10)
                        break;
                    rec += k.Key + ":" + k.Value + "\r\n";
                    c++;
                }
                MessageBox.Show($"共{count}题，错误{error}题，正确率{(100 - ((float)error / (float)count) * 100).ToString("0.00")}%，用时{(Environment.TickCount - timestart) / 1000.0}秒" + "\r\n" + rec);
                this.Close();
            }
        }
        int err = 0;
        int cou = 0;
        int count;
        string d = "";
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (label1.Text.IndexOf(":") >= 0)
            {
                label1.Text = d;
            }
            var pd = label1.Text + ":" + tiku[label1.Text];
            d = label1.Text;
            if (tiku[label1.Text] == textBox1.Text.Trim()
                || ((label1.Text == "じ" || label1.Text == "ジ") && textBox1.Text.Trim() == "zi"
                || (label1.Text == "ぢ" || label1.Text == "ヂ") && textBox1.Text.Trim() == "ji"
                || (label1.Text == "じゃ" || label1.Text == "ジャ") && textBox1.Text.Trim() == "ja"
                || (label1.Text == "じゅ" || label1.Text == "ジュ") && textBox1.Text.Trim() == "ju"
                || (label1.Text == "じょ" || label1.Text == "ジョ") && textBox1.Text.Trim() == "jo"))
            {
                var t = Environment.TickCount;
                Record(label1.Text, (float)((t - ti) / 1000.0));
                ti = Environment.TickCount;
                Random rd = new Random();
                int r = rd.Next(0, tiku.Count - 1);
                var x = tiku.ElementAt(r);
                label1.Text = x.Key;
                cou--;
                textBox1.Text = "";
                label2.Text = cou.ToString();

            }
            else
            {
                textBox1.Text = "";
                label1.Text = pd;
                err++;
            }
            Test(err);
        }
        int ti = 0;
        Dictionary<string, float> reco = new Dictionary<string, float>();
        private void Record(string key, float val)
        {
            if (reco.ContainsKey(key))
            {
                if (reco[key] < val)
                    reco[key] = val;
            }
            else
            {
                reco.Add(key, val);
            }

        }


    }
}
