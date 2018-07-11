using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

using GmailParserViewProgram.Model;
using GmailParserViewProgram.Act;
using System.Runtime.InteropServices;

namespace GmailParserViewProgram
{
    public partial class FormMailTriggerLogic : Form
    {
        static Form parent = null;
        const string applicationName = "Settings";

        public void SetParent(Form form)
        {
            parent = form;
        }

        GRule gRule = new GRule();
        Calculate calculate;
        Thread thread;

        public FormMailTriggerLogic()
        {
            InitializeComponent();
            this.Text = applicationName;

        }

        GMessage gMessage = null;
        private async void FormMailTriggerLogic_Load(object sender, EventArgs e)
        {
            this.notifyIcon.Icon = new Icon(Application.StartupPath + "\\" + "MTP-beta-icon.ico");
            this.notifyIcon.Text = "Mail Trigger Parser";

            if (GLogin.Glogin == null)
            {
                GLogin.Init();
                await GLogin.Glogin.CreateCredential();
                GLogin.Glogin.CreateGmailService();
            }

            gMessage = new GMessage(GLogin.Glogin.GmailService);

            calculate = new Calculate(GLogin.Glogin.GmailService);
            calculate.callbackFileName += Calculate_callbackFileName;
            calculate.callbackProgressBar += Calculate_callbackProgressBar;
            calculate.callbackAlert += Calculate_callbackAlert;

            this.Deactivate += FormMailTriggerLogic_Deactivate;

            if (AutoRun.IsEnabled())
            {
                btn_start.Enabled = false;
                btn_stop.Enabled = true;
                thread = new Thread(new ThreadStart(calculate.Run));
                thread.Start();

                cb_autorun.Checked = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
            else
            {
                btn_stop.Enabled = false;
                cb_autorun.Checked = false;
            }


            dataGridView.Rows.Clear();

            l_version.Text =/* "Version : " + */"beta " + Application.ProductVersion.ToString();
            var data = await GLogin.Glogin.GmailService.Users.GetProfile("me").ExecuteAsync();
            l_mail.Text = data.EmailAddress;
            l_status.Text = "tap start";
            l_processedMessages.Text = "0";

            foreach (string str in GRule.GetFiles())
                RowAdd(FileParser.Reads<GRule>(str));
            GRule.SetChange(false);

            this.Select();
        }

        static bool showAlertInternet = false;
        private void Calculate_callbackAlert(object value)
        {
            showAlertInternet = (bool)value;
        }

        static int progress = 0;
        private void Calculate_callbackProgressBar(object value)
        {
            progress = (int)(double)value;
        }

        static string text = "none";

        private void Calculate_callbackFileName(object value)
        {
            text = (string)value;
        }

        private void FormMailTriggerLogic_Deactivate(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView_Enter(object sender, EventArgs e)
        {

        }

        private void RowRemove(GRule gRule)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
                if ((string)row.Cells["id"].Value == gRule.id)
                {
                    row.Cells["tag"].Value = gRule.tag;
                    row.Cells["path"].Value = gRule.path;
                    row.Cells["mode"].Value = gRule.mode;
                    row.Cells["type"].Value = gRule.type;
                    row.Cells["lastId"].Value = gRule.lastMesId;
                    return;
                }
            RowAdd(gRule);
        }

        private GRule GetRowGrule(int id)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
                if ((int)row.Cells["id"].Value == id)
                {
                    return new GRule((string)row.Cells["id"].Value, (string)row.Cells["tag"].Value, (string)row.Cells["path"].Value, (string)row.Cells["mode"].Value, (string)row.Cells["type"].Value);
                }
            return null;
        }

        private void RowAdd(GRule value)
        {
            int index = dataGridView.Rows.Add();
            dataGridView.Rows[index].Cells["id"].Value = value.id;
            dataGridView.Rows[index].Cells["tag"].Value = value.tag;
            dataGridView.Rows[index].Cells["path"].Value = value.path;
            dataGridView.Rows[index].Cells["mode"].Value = value.mode;
            dataGridView.Rows[index].Cells["type"].Value = value.type;
            dataGridView.Rows[index].Cells["lastId"].Value = value.lastMesId;
        }

        private void RefreshListData(GRule value, int index)
        {
            dataGridView.Rows[index].Cells["tag"].Value = value.tag;
            dataGridView.Rows[index].Cells["path"].Value = value.path;
            dataGridView.Rows[index].Cells["mode"].Value = value.mode;
            dataGridView.Rows[index].Cells["type"].Value = value.type;
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            string path = String.Empty;

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tb_local.Text = folderBrowserDialog.SelectedPath.Replace("\\", "/");
            }
        }

        private void tb_password_TextChanged(object sender, EventArgs e) { }
        private bool tb_local_text_find = false;
        private void tb_local_TextChanged(object sender, EventArgs e)
        {
            if (tb_local.Text != String.Empty) tb_local_text_find = true;
            else tb_local_text_find = false;
        }

        private bool tb_tag_text_find = false;
        private void tb_tag_TextChanged(object sender, EventArgs e)
        {
            if (tb_local.Text != String.Empty) tb_tag_text_find = true;
            else tb_tag_text_find = false;
        }

        private void Tick_Tick(object sender, EventArgs e)
        {
            l_status.Text = text;
            progressBarAll.Value = progress;
            if (gMessage != null)
            {
                l_allCounter.Text = "All messages : " + GMessage.allMessages.ToString();
                l_downloadfiles.Text = "Download files : " + GMessage.downloadFiles.ToString();
                l_processedMessages.Text = "Processed Messages : " + GMessage.processedMessages.ToString();
            }

            if (GRule.IsChange())
            {
                Save_ToolStripMenuItem.Text = "Save*";
                this.Text = applicationName + " (you have change)*";
            }
            else
            {
                Save_ToolStripMenuItem.Text = "Save";
                this.Text = applicationName;
            }

            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) p_AlertNetwork.Visible = true;
            else p_AlertNetwork.Visible = false;
        }

        private List<GRule> gRuleBuffer = new List<GRule>();
        private List<string> gRuleItemDeleteBuffer = new List<string>();
        private void btn_add_Click(object sender, EventArgs e)
        {
            if (
                tb_local.Text != String.Empty &&
                listBoxMode.Text != String.Empty)
            {
                GRule g = new GRule(tb_tag.Text, tb_local.Text, listBoxMode.Text, tb_type.Text);
                gRuleBuffer.Add(g);
                RowAdd(g);

                GRule.SetChange(true);
            }
            else
            {
                GRule.SetChange(false);
                Color col = tb_local.BackColor;
                if (tb_local.Text == String.Empty) tb_local.BackColor = Color.Red;
                if (listBoxMode.Text == String.Empty) listBoxMode.BackColor = Color.Red;
                if (MessageBox.Show("One of the fields does not have values!", "Warning") == DialogResult.OK)
                {
                    listBoxMode.BackColor = tb_type.BackColor = tb_tag.BackColor = tb_local.BackColor = col;
                }
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            tb_local.Text = String.Empty;
            tb_tag.Text = String.Empty;
            tb_type.Text = String.Empty;

            listBoxMode.SelectedIndex = -1;
        }

        private void FormMailTriggerLogic_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread != null)
                thread.Abort();
            notifyIcon = null;
            Exit();
        }

        private void cb_autorun_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_autorun.Checked)
            {
                if (!AutoRun.IsEnabled()) AutoRun.Enabled();
            }
            else if (AutoRun.IsEnabled()) AutoRun.Disabled();
        }

        private async void btn_start_Click(object sender, EventArgs e)
        {
            progressBarAll.Value = 0;
            gMessage.ResetParametres();
            if (gMessage != null)
            {
                btn_start.Enabled = false;
                btn_stop.Enabled = true;
                thread = new Thread(new ThreadStart(calculate.Run));
                thread.Start();
            }
        }

        private void Exit()
        {
            parent.Opacity = 100.0f;
            parent.ShowInTaskbar = true;
            parent.Focus();
        }

        static FormDownload formDownload = null;
        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                formDownload = new FormDownload();
                formDownload.SetParent(this);
                formDownload.Show();
            }
            catch (Exception) { }
        }

        private void Save_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GRule item in gRuleBuffer)
                FileParser.Write(item.GetFilePath(), item);
            foreach (string item in gRuleItemDeleteBuffer)
                FileParser.Delete(GRule.Delete(item));
            gRuleBuffer.Clear();
            GRule.SetChange(false);
        }

        private void btn_deleteAll_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            GRule.SetChange(true);
        }

        private void listBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (listBoxMode.Text)
            {

                case СONCEPT.MODE.all_messages:
                    l_discrition.Text = СONCEPT.DISCRIPTION.all_messages;
                    break;
                case СONCEPT.MODE.label:
                    l_discrition.Text = СONCEPT.DISCRIPTION.label;
                    break;
                case СONCEPT.MODE.subject:
                    l_discrition.Text = СONCEPT.DISCRIPTION.subject;
                    break;
                default:
                    l_discrition.Text = "Don't select";
                    break;
            }
        }

        BackgroundWorker bw = null;

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            // Do not access the form's BackgroundWorker reference directly.
            // Instead, use the reference provided by the sender parameter.
            bw = sender as BackgroundWorker;

            // Extract the argument.
            int arg = 0;
            //if (e != null)
            //arg = (int)e.Argument;

            // Start the time-consuming operation.


            if (bw.CancellationPending)
            {
                e.Cancel = true;
            }


            //e.Result =

            // If the operation was canceled by the user, 
            // set the DoWorkEventArgs.Cancel property to true.

        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageBox.Show("Operation was canceled");
            }
            else if (e.Error != null)
            {
                // There was an error during the operation.
                string msg = String.Format("An error occurred: {0}", e.Error.Message);
                MessageBox.Show(msg);
            }
            else
            {
                // The operation completed normally.
                string msg = String.Format("Result = {0}", e.Result);
                MessageBox.Show(msg);
            }
            btn_stop.Enabled = false;

        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                this.Activate();
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }

        private void deleteSelectItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection breakfast = this.dataGridView.SelectedRows;
            foreach (DataGridViewRow item in breakfast)
            {
                gRuleItemDeleteBuffer.Add((string)item.Cells["id"].Value);
                dataGridView.Rows.RemoveAt(item.Index);
            }
            GRule.SetChange(true);
        }

        private void deleteAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(null, "Are you sure?", "Attention", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (string str in GRule.GetFiles())
                    Act.FileParser.Delete(str);
                dataGridView.Rows.Clear();
            }
        }

        private async void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            await GLogin.Glogin.DeleteUserAuthorization();
            this.Close();
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            btn_stop.Enabled = false;
            btn_start.Enabled = true;

            showAlertInternet = false;

            thread.Abort();
        }

        private void dataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (!e.RowIndex.Equals(-1) && !e.ColumnIndex.Equals(-1) && e.Button.Equals(MouseButtons.Right))
            {
                dataGridView.CurrentCell = dataGridView[e.ColumnIndex, e.RowIndex];
                dataGridView.CurrentRow.Selected = true;
            }
        }
    }
}
