using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using GmailParserViewProgram.Model;
using System.Windows.Forms;

namespace GmailParserViewProgram.Act
{
    public class Calculate
    {
        public event Callback callbackFileName;
        public event Callback callbackProgressBar;
        public event Callback callbackAlert;

        public Calculate(Google.Apis.Gmail.v1.GmailService service)
        {
            gMessage = new GMessage(service);
        }

        GMessage gMessage;

        public void Run()
        {
            while (true)
            {
                try
                {
                    GRule gRule = null;
                    string[] files = GRule.GetFiles();
                    for (int i = 0; i < files.Length; i++)
                    {
                        gRule = FileParser.Reads<GRule>(files[i]);
                        callbackProgressBar(0.0);
                        Stack<string> messagesId = null;
                        messagesId = gMessage.GetMessages(GMessage.Query(gRule), ref gRule.lastMesId);
                        //callbackAlert(false);
                        if (messagesId.Count > 0)
                            foreach (string id in messagesId)
                            {
                                gMessage.GetFile(id, ref gRule, callbackFileName, callbackProgressBar);
                                FileParser.Write(gRule.GetFilePath(), gRule);
                            }
                        gMessage.ResetParametres();
                    }
                }
                catch (Exception)
                {
                    //if (MessageBox.Show("Message download error!!!! pidr sykkkkaaaa", "Exception") == DialogResult.OK) { };
                    /*
                    if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                        callbackAlert(true);
                        */
                }
            }

        }
    }
}
