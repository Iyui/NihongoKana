using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
namespace nihongo
{
    public partial class Form1 : Form
    {
        string connStr = "server=.;database=nihongo;Integrated Security = True";
        static readonly string hiragana = Gojyuon.hiragana;//平假名
        static readonly string katagana = Gojyuon.katagana;//片假名
        static readonly string[] duyin1 = Gojyuon.duyin1;

        static readonly string zhuoyin1 = Gojyuon.zhuoyin1;//浊音平假名
        static readonly string zhuoyin2 = Gojyuon.zhuoyin2;//浊音片假名
        static readonly string[] duyin2 = Gojyuon.duyin2;

        static readonly string[] aoyin1 = Gojyuon.aoyin1;//拗音平假名
        static readonly string[] aoyin2 = Gojyuon.aoyin2;//拗音片假名
        static readonly string[] duyin3 = Gojyuon.duyin3;


        Hashtable ht = new Hashtable();
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < hiragana.Length; i++)
            {
                ht.Add(hiragana[i].ToString(), katagana[i].ToString());
                if (!ht.ContainsKey(katagana[i]))
                ht.Add(katagana[i].ToString(), hiragana[i].ToString());
            }
            for (int i = 0; i < zhuoyin1.Length; i++)
            {
                try
                {
                    if (!ht.ContainsKey(zhuoyin1[i]))
                        ht.Add(zhuoyin1[i].ToString(), zhuoyin2[i].ToString());
                    if (!ht.ContainsKey(zhuoyin2[i]))
                        ht.Add(zhuoyin2[i].ToString(), zhuoyin1[i].ToString());
                }
                catch { }
            }
            for (int i = 0; i < aoyin1.Length; i++)
            {
                if (!ht.ContainsKey(aoyin1[i]))
                ht.Add(aoyin1[i].ToString(), aoyin2[i].ToString());
                if (!ht.ContainsKey(aoyin2[i]))
                ht.Add(aoyin2[i].ToString(), aoyin1[i].ToString());
            }
            ht.Add("ゃ", "ャ");
            ht.Add("ゅ", "ュ");
            ht.Add("ょ", "ョ");
            ht.Add("ャ", "ゃ");
            ht.Add("ュ", "ゅ");
            ht.Add("ョ", "ょ");
            //LoadSC();
      
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Showhk();
            
            for (int i = 10;i<=100;i++)
                toolStripComboBox1.Items.Add(i);
            toolStripComboBox1.Text = "15";
           //ConditionQuery(dataGridView1);
        }

        private void Showhk()
        {
            if (btKatagana.Checked)
                toolStripLabel2.Text = "平假名";
            else
                toolStripLabel2.Text = "片假名";
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

        private void LoadhasError()
        {

        }

        private void LoadSC()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sqlStr = "SELECT [中文],[translate] FROM note where 错误次数>0";
                sqlStr = "SELECT [中文],[translate] FROM note";
                SqlCommand storers = conn.CreateCommand();
                SqlDataReader reader = null;
                storers.CommandText = sqlStr;
                conn.Open();
                using (reader = storers.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //if (!scDic.ContainsKey(reader["中文"].ToString().Trim()))
                           // scDic.Add(reader["中文"].ToString().Trim(), reader["translate"].ToString().Trim());
                           // scDic.Add( reader["translate"].ToString().Trim(), reader["中文"].ToString().Trim());
                        if (!scDic.ContainsKey(reader["translate"].ToString().Trim()))
                            scDic.Add(reader["translate"].ToString().Trim(), reader["中文"].ToString().Trim());
                    }
                }
            }
            toolStripLabel3.Text = scDic.ElementAt(indexx).Key;
            for (int i = 0; i < scDic.Count; i++)
                toolStripComboBox3.Items.Add(i);
            toolStripComboBox3.Text = indexx.ToString();
            toolStripLabel3.Text = scDic.ElementAt(indexx).Key;
            toolStripLabel4.Text = "";
        }
        Dictionary<string, string> scDic = new Dictionary<string, string>();
        string Code = "";
        string roma = "qwertyuiopasdfghjklzxcvbnm ";
        int indexx = 0;
        int error = 0;
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                btKatagana.Checked = !btKatagana.Checked;
                Showhk();
                return;
            }
            if (e.KeyCode == Keys.F2)
            {
                Switchhk();
                return;
            }
            if (e.KeyCode == Keys.F3)
            {
                Clear();
                return;
            }
            //if (e.KeyCode == Keys.ControlKey)
            //{
            //    luru();
            //    return;
            //}
            if (e.KeyCode == Keys.ControlKey)
            {
                if (tran(toolStripLabel3.Text,false))
                {
                    if (indexx == scDic.Count - 1)
                        indexx = -1;
                    toolStripLabel3.Text = scDic.ElementAt(++indexx).Key;
                    toolStripComboBox3.Text = indexx.ToString();
                    toolStripLabel4.Text = "";
                    SetErrorCount();
                    error = 0;
                    Clear();
                }
                else
                {
                    toolStripLabel4.Text = scDic.ElementAt(indexx).Value;
                    error++;
                }
                return;
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                return;
            }
            
            string r = "";
            string s = "";
            r += e.KeyCode;
            s = r;
            if (e.KeyCode == Keys.Enter)
            {
                if (Gojyuonnospace(s))
                {
                    textBox2.Text += RomajiGojyuon(Code.Trim());
                    Code = "";
                }
                textBox2.AppendText("\r\n");
                return;
            }
            if (roma.Contains(r.ToLower())||e.KeyCode == Keys.Space)
            {
                r = "";
            }
            else if(e.KeyCode == Keys.Back)
                r = "";
            else
                return;

            if (e.KeyCode == Keys.Back)
            {
                if (Code.Length > 0)
                    Code = Code.Substring(0, Code.Length - 1);
            }
            else if(e.KeyCode == Keys.Space)
            { }
            else
            {
                Code += e.KeyCode;
            }
            if (!tsbSpace.Checked)
            {
                if (e.KeyCode == Keys.Space)
                {
                    Code += " ";
                }
                if (Gojyuonnospace(s))
                {
                    textBox2.Text += RomajiGojyuon(Code.Trim());
                    Code = "";
                }
                
            }
            else
            {    
                
                if (e.KeyCode == Keys.Space)
                {
                    textBox2.Text += RomajiGojyuon(Code);
                    Code = "";
                }
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
        
        private bool tran(string key)
        {
            if(textBox2.Text.Trim()==scDic[key].Trim())
            {
                return true;
            }
            return false;
        }

        private bool tran(string key,bool b)
        {
            if (textBox1.Text.Trim() == "")
                return false;
            if ((scDic[key].Trim().IndexOf(textBox1.Text.Trim()))>=0)
            {
                return true;
            }
            return false;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            Code = "";
            toolStripLabel1.Text = Code.ToLower();
            toolStripLabel4.Text = "";
        }

        string yuanyin = "aeiou";

        private bool Gojyuonnospace(string code)
        {
            //.Substring(Code.Length - 1, 1).ToLower()
            if (Code.Length>1 && Code[0].ToString().ToLower()=="n" && !yuanyin.Contains(code.ToLower()))
            {
                Code = Code.Substring(1, Code.Length-1);
                textBox2.Text += RomajiGojyuon("n");
                //Code = "";
            }
            else if(Code.Length > 1 && Code.Trim().ToLower() == "n")
            {
              //  Code = Code.Substring(1, Code.Length - 1);
                textBox2.Text += RomajiGojyuon("n");
                Code = "";
            }
            else if(Code.Length >0 && code.ToLower()=="return"&&Code.Substring(Code.Length - 1, 1).ToLower()=="n")
            {
                textBox2.Text += RomajiGojyuon("n");
                Code = "";
            }
            else if(tsbSpace.Checked && Code.Length>0 && code.ToLower() == "return")
            {
                textBox2.Text += RomajiGojyuon(Code);
                Code = "";
            }
            return yuanyin.Contains(code.ToLower());
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            erande ed = new erande();
            ed.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Switchhk();
        }

        private void Switchhk()
        {
            var s = "";
            var rn = "";
            foreach (var c in textBox2.Text)
            {
                if (c.ToString() == "\r")
                    s += "\r\n";
                s += ht[c.ToString()];
            }
            textBox2.Text = s;
        }

        private void btKatagana_Click(object sender, EventArgs e)
        {
            Showhk();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("2.0 版本更新：\r\n1.っ （小写tsu）为促音，不发音，该软件中用“ci”代替。\r\n2.片假名中长音ー不发音，该软件中用“yi”代替。\r\n3.增加生词学习功能，暂无记忆功能，每次重启软件都会导致生词从头开始");
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_TextUpdate(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                textBox2.Font = new Font(textBox2.Font.Name, float.Parse(toolStripComboBox1.Text));
            }
            catch { };
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            luru();
        }

        private void luru()
        {
            Form2 fm2 = new Form2(textBox2.Text.Trim(), !btKatagana.Checked, toolStripComboBox2.Text, 句子ToolStripMenuItem.Checked);
            if (fm2.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                this.ActiveControl = textBox1;
            }
        }

        private void ConditionQuery(DataGridView dgv)
        {

            //DataGridView dgv = new DataGridView();
            dgv.Rows.Clear();
            SqlDataReader reader = null;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand storers = conn.CreateCommand();
                storers.CommandText = "SELECT [中文],[translate] FROM note";
                conn.Open();
                using (reader = storers.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return;
                    }
                }
                using (reader = storers.ExecuteReader())
                {
                    BindingSource Bs = new BindingSource();
                    Bs.DataSource = reader;
                    dgv.DataSource = Bs;
                }
            }
            FolderBrowserDialog sfd = new FolderBrowserDialog();
            if (DialogResult.OK == sfd.ShowDialog())
            {
                var filepath = sfd.SelectedPath;
                //foreach (var key in dicDgv.Keys)
                //{
                //    ExportExcel(filepath + "\\" + dicDgv[key], key, "宋体", 10, dicDgv[key]);
                //}
                ExportExcel(filepath + "\\生词", dgv, "宋体", 10, "生词");
            }
            //dgv.Clear();
           // ExportExcel("1", dgv, "宋体", 10, "生词");
        }

        /// <summary>
        /// NPOI DataGridView 导出 EXCEL
        /// </summary>
        /// <param name="fileName"> 默认保存文件名</param>
        /// <param name="dgv">DataGridView</param>
        /// <param name="fontname">字体名称</param>
        /// <param name="fontsize">字体大小</param>
        public void ExportExcel(string fileName, DataGridView dgv, string fontname, short fontsize, string filename)
        {
            //检测是否有数据
            //if (dgv.SelectedRows.Count == 0) return;
            //创建主要对象
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(filename);
            //设置字体，大小，对齐方式
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFFont font = (HSSFFont)workbook.CreateFont();
            font.FontName = fontname;
            font.FontHeightInPoints = fontsize;
            style.SetFont(font);
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center; //居中对齐
                                                                            //添加表头

            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(0);
            //dataRow = (HSSFRow)sheet.CreateRow(0);
            dataRow.CreateCell(0).SetCellValue(filename);
            dataRow = (HSSFRow)sheet.CreateRow(1);
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dataRow.CreateCell(i).SetCellValue(dgv.Columns[i].HeaderText);
                dataRow.GetCell(i).CellStyle = style;
            }
            //注释的这行是设置筛选的
            //sheet.SetAutoFilter(new CellRangeAddress(0, dgv.Columns.Count, 0, dgv.Columns.Count));
            //添加列及内容

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dataRow = (HSSFRow)sheet.CreateRow(i + 2);
                for (int j = 0; j < dgv.Columns.Count; j++)
                {
                    string ValueType = dgv.Rows[i].Cells[j].Value is null ? "System.String" : dgv.Rows[i].Cells[j].Value.GetType().ToString();
                    string Value = dgv.Rows[i].Cells[j].Value is null ? "" : dgv.Rows[i].Cells[j].Value.ToString();
                    switch (ValueType)
                    {
                        case "System.String"://字符串类型
                            dataRow.CreateCell(j).SetCellValue(Value);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(Value, out dateV);
                            dataRow.CreateCell(j).SetCellValue(dateV);
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(Value, out boolV);

                            dataRow.CreateCell(j).SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(Value, out intV);
                            dataRow.CreateCell(j).SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(Value, out doubV);
                            dataRow.CreateCell(j).SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            dataRow.CreateCell(j).SetCellValue("");
                            break;
                        default:
                            dataRow.CreateCell(j).SetCellValue("");
                            break;
                    }
                    dataRow.GetCell(j).CellStyle = style;
                    //设置宽度
                    var maxColumn = dgv.Columns.Count;
                    //列宽自适应，只对英文和数字有效
                    //for (int j = 0; j <= maxColumn; j++)
                    //{
                    //    sheet.AutoSizeColumn(j);
                    //}
                    //获取当前列的宽度，然后对比本列的长度，取最大值
                    for (int columnNum = 0; columnNum <= maxColumn; columnNum++)
                    {
                        int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                        for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                        {
                            IRow currentRow;
                            //当前行未被使用过
                            if (sheet.GetRow(rowNum) == null)
                            {
                                currentRow = sheet.CreateRow(rowNum);
                            }
                            else
                            {
                                currentRow = sheet.GetRow(rowNum);
                            }

                            if (currentRow.GetCell(columnNum) != null)
                            {
                                ICell currentCell = currentRow.GetCell(columnNum);
                                int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                                if (columnWidth < length)
                                {
                                    columnWidth = length;
                                }
                            }
                        }
                        sheet.SetColumnWidth(columnNum, columnWidth * 256);
                    }
                    // sheet.SetColumnWidth(j, (Value.Length) * 512);
                }
  
            }

            //保存文件
            string saveFileName = "";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel文件|*.xls";
            saveDialog.FileName = fileName;
            MemoryStream ms = new MemoryStream();
            if (true || saveDialog.ShowDialog() == DialogResult.OK)
            {
                saveFileName = saveDialog.FileName+"日语生词" + ".xls";
                if (saveFileName.IndexOf("/") > 0)
                {
                    // saveFileName=saveFileName.Insert(saveFileName.IndexOf("/"), @"\");
                    saveFileName = saveFileName.Replace("/", "-");
                }
                //if (!CheckFiles(saveFileName))
                //{
                //    MessageBox.Show("文件被站用，请关闭文件后重新进行导出操作 " + saveFileName);
                //    workbook = null;
                //    ms.Close();
                //    ms.Dispose();
                //    return;
                //}
                workbook.Write(ms);
                FileStream file = new FileStream(saveFileName, FileMode.Create);
                workbook.Write(file);
                file.Close();
                workbook = null;
                ms.Close();
                ms.Dispose();
                MessageBox.Show(fileName + " 保存成功", "提示", MessageBoxButtons.OK);
                //if (MessageBox.Show("导出成功,点击 [是] 后打开文件所在位置", "导出成功", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //    dateTabletoCSV.ClickOpenLocation(saveFileName);
            }
            else
            {
                workbook = null;
                ms.Close();
                ms.Dispose();
            }
        }



        /// <summary>  
        /// 将excel导入到datatable  
        /// </summary>  
        /// <param name="filePath">excel路径</param>  
        /// <param name="isColumnName">第一行是否是列名</param>  
        /// <returns>返回datatable</returns>  
        public static DataTable ExcelToDataTable(string filePath, bool isColumnName)
        {
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            int startRow = 0;
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    // 2007版本  
                    if (filePath.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本  
                    else if (filePath.IndexOf(".xls") > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(0);//读取第一个sheet，当然也可以循环读取每个sheet  
                        dataTable = new DataTable();
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum;//总行数  
                            if (rowCount > 0)
                            {
                                IRow firstRow = sheet.GetRow(0);//第一行  
                                int cellCount = firstRow.LastCellNum;//列数  

                                //构建datatable的列  
                                if (isColumnName)
                                {
                                    startRow = 1;//如果第一行是列名，则从第二行开始读取  
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        cell = firstRow.GetCell(i);
                                        if (cell != null)
                                        {
                                            if (cell.StringCellValue != null)
                                            {
                                                column = new DataColumn(cell.StringCellValue);
                                                dataTable.Columns.Add(column);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        column = new DataColumn("column" + (i + 1));
                                        dataTable.Columns.Add(column);
                                    }
                                }

                                //填充行  
                                for (int i = startRow; i <= rowCount; ++i)
                                {
                                    row = sheet.GetRow(i);
                                    if (row == null) continue;

                                    dataRow = dataTable.NewRow();
                                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                                    {
                                        cell = row.GetCell(j);
                                        if (cell == null)
                                        {
                                            dataRow[j] = "";
                                        }
                                        else
                                        {
                                            //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)  
                                            switch (cell.CellType)
                                            {
                                                case CellType.Blank:
                                                    dataRow[j] = "";
                                                    break;
                                                case CellType.Numeric:
                                                    short format = cell.CellStyle.DataFormat;
                                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理  
                                                    if (format == 14 || format == 31 || format == 57 || format == 58)
                                                        dataRow[j] = cell.DateCellValue;
                                                    else
                                                        dataRow[j] = cell.NumericCellValue;
                                                    break;
                                                case CellType.String:
                                                    dataRow[j] = cell.StringCellValue;
                                                    break;
                                            }
                                        }
                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }

        

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            BindingSource bs = new BindingSource();
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();       //获得路径
            if (!File.Exists(path))
                //path = Path.GetDirectoryName(path);
                return;
            bs.DataSource = ExcelToDataTable(path, true);
            dataGridView1.DataSource = bs;
            GetSC();
        }

        private void GetSC()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (!(dataGridView1[0, i].Value is null) && !scDic.ContainsKey(dataGridView1[0, i].Value.ToString().Trim()))
                    scDic.Add(dataGridView1[0, i].Value.ToString().Trim(), dataGridView1[1, i].Value?.ToString().Trim());
            }
            MessageBox.Show("加载成功");
            for (int i = 0; i < scDic.Count; i++)
                toolStripComboBox3.Items.Add(i);
            toolStripComboBox3.Text = indexx.ToString();
            toolStripLabel3.Text = scDic.ElementAt(indexx).Key;
            toolStripLabel4.Text = "";
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;                                                              //重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void toolStripComboBox3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = int.Parse(toolStripComboBox3.Text);
            indexx = ind;
            toolStripLabel3.Text = scDic.ElementAt(ind).Key;
        }

        private void SetErrorCount()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string errUpdate = "update note set 错误次数 = @错误次数 where translate = '" + textBox2.Text +"'";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@错误次数", error), };
                var isUpdateSucceed = UpdateData(conn, errUpdate, sp);
            }
        }

        private void SetErrorSign()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string errUpdate = "update note set 错误标记 = @错误标记 where translate = '" + toolStripLabel4.Text + "'";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@错误标记", true), };
                var isUpdateSucceed = UpdateData(conn, errUpdate, sp);
            }
        }

        private void GetCorrectTran()
        {
            if (MessageBox.Show("确定修改吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string errUpdate = "update note set translate = @translate where translate = '" + toolStripLabel4.Text + "'";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@translate", textBox2.Text), };
                var isUpdateSucceed = UpdateData(conn, errUpdate, sp);
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="sqlStr">更新语句</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        public bool UpdateData(SqlConnection conn,  string sqlStr, params SqlParameter[] parameter)
        {
            try
            {
                // conn.Open();
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.Parameters.AddRange(parameter);
                var row = cmd.ExecuteNonQuery();

                //conn.Close();
                if (row > 0)
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    return true;

                }
                else
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("更新数据异常" + ex.Message);
            }
        }

    }
}
