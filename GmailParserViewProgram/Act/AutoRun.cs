using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GmailParserViewProgram.Act
{
    public class AutoRun
    {
        static private Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", true);
        private const string name = "MailTrigger";
        static public void Enabled ()
        {
            key.SetValue(name, Application.ExecutablePath);
        }

        static public void Disabled ()
        {
            key.DeleteValue(name);
        }

        static public bool IsEnabled ()
        {
            if (key.GetValue(name) != null) return true;
            return false;
        }
    }
}
