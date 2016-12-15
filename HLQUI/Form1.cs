using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace HLQUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            dataGridViewLED.Columns.Clear();
            dataGridViewLED.RowHeadersVisible = false;
            dataGridViewLED.ColumnHeadersVisible = false;
            dataGridViewLED.AllowUserToResizeColumns = false; 
            dataGridViewLED.AllowUserToResizeRows = false; 
            dataGridViewLED.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; 
            dataGridViewLED.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            dataGridViewLED.MouseWheel += new System.Windows.Forms.MouseEventHandler(dataGridViewLED_MouseWheel);

            comboBoxCOM.Items.Clear();
            initSD();
        }

        void dataGridViewLED_MouseWheel(object sender, MouseEventArgs e)
        {
           
                foreach (var row in dataGridViewLED.Rows)
                {
                    ((DataGridViewRow)row).Height += e.Delta/10;
                }
                foreach (var col in dataGridViewLED.Columns)
                {
                    ((DataGridViewColumn)col).Width +=  e.Delta/10;
                }
                
                
            
        }
        private void initSD()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                //判断是不是U盘
                if (d.DriveType == DriveType.Removable)
                {
                    comboBoxCOM.Items.Add(d.RootDirectory);
                }
            }
            if (comboBoxCOM.Items.Count > 0)
            {
                comboBoxCOM.SelectedIndex = 0;
            }
        }
        private void buttonSend_Click(object sender, EventArgs e)
        {
            #region 存SD


            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(comboBoxCOM.Text+"cfg.ini");
            //string startCode = data["line"]["startCode"];

            data["BASIC"]["line"] = dataGridViewLED.Columns.Count.ToString();
            parser.WriteFile(comboBoxCOM.Text + "cfg.ini", data);
            

            for (int x=0;x<dataGridViewLED.Columns.Count;x++)
            {
                string lineContent = "";
                for (int y = 0; y < 60; y++)
                {
                    Color c = dataGridViewLED.Rows[y].Cells[x].Style.BackColor;
                    lineContent = lineContent + c.R.ToString("x").PadLeft(2, '0') + c.G.ToString("x").PadLeft(2, '0') + c.B.ToString("x").PadLeft(2, '0');
                }
                WriteToFile(comboBoxCOM.Text+"line" + x.ToString()+".txt", lineContent);
            }
            #endregion

        }
        public static void clearTxtFile(string path)
        {

        }

        public static void WriteToFile(string name, string content)
        {
            FileStream fs = null;
            try
            {
                if (File.Exists(name))
                {
                    fs = new FileStream(name, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(content);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    File.WriteAllText(name, content, Encoding.UTF8);
                }
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewLED_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridViewLED_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void dataGridViewLED_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void dataGridViewLED_MouseClick(object sender, MouseEventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Color c = cd.Color;

                foreach (DataGridViewTextBoxCell cell in dataGridViewLED.SelectedCells)
                {

                    cell.Style.BackColor = c;
                    cell.Style.ForeColor = c;
                }

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataList = new List<LedEntity>();
            initGrid(dataList);
        }
        private void initGrid(List<LedEntity> list)
        {
            dataGridViewLED.Rows.Clear();
            dataGridViewLED.Columns.Clear();
            for (int i = 1; i <= 600; i = i + 1)
            {
                DataGridViewColumn col = new DataGridViewColumn();
                col.Name = "T" + i.ToString();
                col.Width = 12;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                DataGridViewTextBoxCell dgvcell = new DataGridViewTextBoxCell();//这里根据自己需要来定义不同模板。当前模板为“文本单元格”
                //dgvcell.Value = new Bitmap(12, 12);
                col.CellTemplate = dgvcell;//设置模板

                dataGridViewLED.Columns.Add(col);
            }
            for (int j = 0; j <= 60; j++)
            {
                dataGridViewLED.Rows.Add();
                dataGridViewLED.Rows[j].Height = 12;

            }
            if (list != null)
            {
                foreach (var item in list)
                {
                    Color c = Color.FromArgb(item.Red, item.Green, item.Blue);
                    if(!(c.R == 0 && c.G ==0 && c.B ==0 )){
                        dataGridViewLED.Rows[item.x].Cells[item.y].Style.BackColor = c;
                        dataGridViewLED.Rows[item.x].Cells[item.y].Style.ForeColor = c;
                    }
                    
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //save
            for (int x = 0; x <= dataGridViewLED.Rows.Count-1; x++)
            {
                for (int y = 0; y <= dataGridViewLED.Columns.Count - 1; y++)
                {
                    var c = dataGridViewLED.Rows[x].Cells[y].Style.BackColor;
                    LedEntity ledItem = new LedEntity();
                    ledItem.x = x;
                    ledItem.y = y;
                    ledItem.Red = int.Parse(c.R.ToString());
                    ledItem.Green = int.Parse(c.G.ToString());
                    ledItem.Blue = int.Parse(c.B.ToString());

                    dataList.Add(ledItem);
                }
            }


            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "LED";
            if (save.ShowDialog() == DialogResult.OK)
            {
                
                FileStream fileStream = new FileStream(save.FileName + ".ldt", FileMode.Create);
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(fileStream, dataList);
                fileStream.Close();
            }
        }

        private List<LedEntity> dataList = new List<LedEntity>();

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                FileStream fileStream = new FileStream(open.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryFormatter b = new BinaryFormatter();
                dataList = (List<LedEntity>)b.Deserialize(fileStream);
                initGrid(dataList);
                fileStream.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                Bitmap oldbitmap = (Bitmap)Bitmap.FromFile(open.FileName);
                int Height = oldbitmap.Height;
                int Width = oldbitmap.Width;
                
                int start = dataGridViewLED.CurrentCell.ColumnIndex;

                Bitmap newbitmap = new Bitmap(oldbitmap,dataGridViewLED.Columns.Count-start ,60);

                

                for (int x = 0; x < newbitmap.Width - 1;x++ )
                {

                    for (int y = 0; y < newbitmap.Height-1; y++)
                    {
                        int r, g, b;
                        var pixel = newbitmap.GetPixel(x, y);
                        r = pixel.R;
                        g = pixel.G;
                        b = pixel.B;
                        Color c = Color.FromArgb(r, g, b);






                        dataGridViewLED.Rows[y].Cells[x].Style.BackColor = c;
                        dataGridViewLED.Rows[y].Cells[x].Style.ForeColor = c;



                    }
                }
                
                
            }
        }

       
           

    }
}
