using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

using GmailParserViewProgram.Act;

namespace GmailParserViewProgram.Model
{
    [Serializable]
    public class GRule
    {
        public string id;
        public string tag;
        public string path;
        public string mode;
        public string type;
        public string lastMesId;

        public GRule()
        {

        }

        [NonSerialized]
        static List<string> keys = null;
        
        static private string GenerationKeys ()
        {
            if (keys == null)
            {
                keys = new List<string>();
                foreach (FileInfo f in (new DirectoryInfo(directory)).GetFiles())
                    keys.Add(f.Name);
            }
            string str = string.Empty;
            byte[] bytes = new byte[16];
            Random random = new Random(GetFiles().Length);
            while (true)
            {
                str = string.Empty;
                random.NextBytes(bytes);
                for (byte i = 0; i < bytes.Length; i++)
                    str += Convert.ToInt16(bytes[i]).ToString();
                if (!keys.Exists(item => item == str)) { keys.Add(str); return str; }
            }
        } 
        
        static public string Delete (string id)
        {
            return directory + "\\" + id + ".rule";
        }

        [NonSerialized]
        static string directory = System.Windows.Forms.Application.StartupPath + "\\" + "data\\rules";
        [NonSerialized]
        static public int size = Directory.GetFiles(directory).Length;

        [NonSerialized]
        static public bool change = false;
        static public bool IsChange()
        {
            return change;
        }

        static public void SetChange(bool value)
        {
            change = value;
        }

        static public string[] GetFiles ()
        {
            return Directory.GetFiles(directory);
        }
     
        public string GetFilePath ()
        {
            return directory + "\\" + id.ToString() + ".rule";
        }

        public GRule(string tag, string path , string mode , string type)
        {
            this.id = GenerationKeys();
            this.tag = tag;
            this.path = path;
            this.mode = mode;
            this.type = type;
        }
        public GRule(string id , string tag, string path , string mode , string type)
        {
            this.id = id;
            this.tag = tag;
            this.path = path;
            this.mode = mode;
            this.type = type;
        }
        public GRule(string id, string tag, string path, string mode, string type , string lastId)
        {
            this.id = id;
            this.tag = tag;
            this.path = path;
            this.mode = mode;
            this.type = type;
            this.lastMesId = lastId;
        }
    }
}