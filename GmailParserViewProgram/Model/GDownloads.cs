using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmailParserViewProgram.Model
{
    [Serializable]
    public class GDownloads 
    {
        public GDownloads() { }

        readonly public string filename;
        readonly public string path;
        readonly public DateTime upload_date;

        [NonSerialized]
        static public string pathfile = System.Windows.Forms.Application.StartupPath + "\\" + "downloads.in";

        public GDownloads(string filename, string path, DateTime date)
        {
            this.filename = filename;
            this.path = path;
            this.upload_date = date;
        }
    }
}
