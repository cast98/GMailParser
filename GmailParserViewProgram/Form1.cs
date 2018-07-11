using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using GmailParserViewProgram.Model;

namespace GmailParserViewProgram
{
    public partial class FormMailTrigger : System.Windows.Forms.Form
    {
        const int FLAG_ICC_FORCE_CONNECTION = 1;

        public FormMailTrigger()
        {
            InitializeComponent();
        }

        private async void FormMailTrigger_Load(object sender, EventArgs e)
        {
            GLogin.Init();
            //while (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            if (Act.AutoRun.IsEnabled())
            {
                this.Opacity = 0.0f;
                this.ShowInTaskbar = false;
            }
            
        }

        static FormMailTriggerLogic formMailTriggerLogic = null;
        private async void btn_formLogicStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (await GLogin.Glogin.CreateCredential())
                {
                    GLogin.Glogin.CreateGmailService();
                    this.Opacity = 0.0f;
                    this.ShowInTaskbar = false;
                    formMailTriggerLogic = new FormMailTriggerLogic();
                    formMailTriggerLogic.SetParent(this);
                    formMailTriggerLogic.Show();
                }
            }
            catch (Exception) { }
        }

        private void Warning()
        {
            DialogResult result = MessageBox.Show("Check your Internet connection!", "Warning", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK && !System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) Warning();
            if (result == DialogResult.Cancel && !System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) Application.Exit();
        }

        static bool flag = true;
        private async void Tick_Tick(object sender, EventArgs e)
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                p_AlertNetwork.Visible = true;
            else {
                p_AlertNetwork.Visible = false;
                if (flag && Act.AutoRun.IsEnabled())
                {
                    flag = false;
                    try
                    {
                        if (await GLogin.Glogin.CreateCredential())
                        {
                            try
                            {
                                GLogin.Glogin.CreateGmailService();
                                this.Opacity = 0.0f;
                                this.ShowInTaskbar = false;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else this.Activate();
                        formMailTriggerLogic = new FormMailTriggerLogic();
                        formMailTriggerLogic.SetParent(this);
                        formMailTriggerLogic.Show();
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
