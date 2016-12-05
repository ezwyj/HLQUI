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

            initGrid();

                
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            //打开串口
            System.IO.Ports.SerialPort port = new System.IO.Ports.SerialPort("COM4");
            port.Write("WriteClear");
            port.Write("WriteLine"+ "");
            port.Write("WriteLine" + "");

            port.Close();
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
            initGrid();
        }
        private void initGrid()
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
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //save



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
                fileStream.Close();
            }
        }
    }
}
