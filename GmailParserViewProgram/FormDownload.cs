using GmailParserViewProgram.Act;
using GmailParserViewProgram.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GmailParserViewProgram
{
    public partial class FormDownload : Form
    {
        GDownloads downloads = new GDownloads();
        static Form parent = null;
        const string applicationName = "Downloads";

        public void SetParent(Form form)
        {
            parent = form;
        }
        private void Exit()
        {
            parent.Show();
        }
        public FormDownload()
        {
            InitializeComponent();
        }

        private void FormDownload_FormClosed(object sender, FormClosedEventArgs e)
        {
            Exit();
        }

        private void FormDownload_Load(object sender, EventArgs e)
        {
            button1.Text = "Delete all";
            dataGridView1.Rows.Clear();
            /*GDownloads obj1 = new GDownloads("f", "C:/Users", DateTime.Now);
            GDownloads obj2 = new GDownloads("g", "C:/Users", DateTime.Now);
            GDownloads obj3 = new GDownloads("h", "C:/Users", DateTime.Now);
            FileParser.Add(GDownloads.pathfile, obj1);
            FileParser.Add(GDownloads.pathfile, obj2);
            FileParser.Add(GDownloads.pathfile, obj3);
            refreshListData(obj1);
            refreshListData(obj2);
            refreshListData(obj3);*/
            List<GDownloads> massive = FileParser.Read<GDownloads>(GDownloads.pathfile);
            foreach (GDownloads item in massive)
                refreshListData(item);
        }

        private void refreshListData(GDownloads value)
        {
            dataGridView1.Rows.Insert(0, 1);
            dataGridView1.Rows[0].Cells["filename"].Value = value.filename;
            dataGridView1.Rows[0].Cells["path"].Value = value.path;
            dataGridView1.Rows[0].Cells["upload_date"].Value = value.upload_date;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            FileParser.Delete(GDownloads.pathfile);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection breakfast = this.dataGridView1.SelectedRows;
            foreach (DataGridViewRow item in breakfast)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
                FileParser.RemoveItem(GDownloads.pathfile, item.Index);
            }
        }

        private void copyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection breakfast = this.dataGridView1.SelectedRows;
            foreach (DataGridViewRow item in breakfast)
            {
                string str = Regex.Replace((string)item.Cells["path"].Value, "/", "\\");
                Clipboard.SetText(str);
            }
        }

        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection breakfast = this.dataGridView1.SelectedRows;
            foreach (DataGridViewRow item in breakfast)
            {
                openFileDialog1 = new OpenFileDialog();
                string path = Regex.Replace((string)item.Cells["path"].Value, "/", "\\");
                string name = (string)item.Cells["filename"].Value;
                //openFileDialog1.InitialDirectory = path;
                //openFileDialog1.FileName = name;
                //openFileDialog1.ShowDialog();

                //System.Diagnostics.Process process = new System.Diagnostics.Process();
                //process.StartInfo.FileName = path;
                System.Diagnostics.Process.Start("Explorer.exe", @"/select," + path + "\\" + name);
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (!e.RowIndex.Equals(-1) && !e.ColumnIndex.Equals(-1) && e.Button.Equals(MouseButtons.Right))
            {
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
                dataGridView1.CurrentRow.Selected = true;
            }
        }
    }
}
