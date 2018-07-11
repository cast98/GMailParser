using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AE.Net.Mail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;

using System.Text.RegularExpressions;


namespace GmailParserViewProgram.Model
{
    public class GMessage
    {
        public string messageId;
        public string raw;
        public MessagePartHeader headers;

        private GmailService service;
        private string email;

        public GMessage(GmailService service)
        {
            this.service = service;
            this.email = "me";
            ResetParametres();
            gDownloads = new GDownloads();
        }


        public void ResetParametres()
        {
            allMessages = 0;
            newMessages = 0;
            counterAllFiles = 0;
            downloadFiles = 0;
            progress = 0;
            processedMessages = 0;
        }

        static public double allMessages = 0; // все сообщения
        static public int newMessages = 0; // кол-во новых из них
        static public double processedMessages = 0; // обработанные сообщения
        static public int counterAllFiles = 0;
        static public int downloadFiles = 0; // кол-во скачанных файлов

        public float progress = 0;

        public void SetField(ref System.Windows.Forms.ProgressBar progressBar)
        {
            progressBar.Value = (int)Math.Round(progress);
        }

        GDownloads gDownloads = null;

        public void GetFile(string messageId, ref GRule gRule, Callback callback, Callback progressbar)
        {
            byte[] data = null;
            string filename = "";
            var emailRequest = service.Users.Messages.Get("me", messageId);
            emailRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;
            var parts = emailRequest.Execute().Payload.Parts;
            if (parts != null)
                foreach (var part in parts)
                {
                    if (!String.IsNullOrEmpty(part.Filename))
                    {
                        callback(part.Filename);
                        string[] strs = Regex.Split(part.Filename, "[.]+");

                        string type = String.Empty;
                        if (String.IsNullOrEmpty(gRule.type)) type = strs[strs.Length - 1];
                        else type = gRule.type;

                        if (type == strs[strs.Length - 1])
                        {
                            ++counterAllFiles;
                            String attId = part.Body.AttachmentId;
                            MessagePartBody attachPart = service.Users.Messages.Attachments.Get("me", messageId, attId).Execute();
                            String attachData = attachPart.Data.Replace('-', '+');
                            attachData = attachData.Replace('_', '/');

                            data = Convert.FromBase64String(attachData);

                            string name = Path.Combine(gRule.path, part.Filename);

                            if (File.Exists(name)) File.Delete(name);
                            using (FileStream fileStream = new FileStream(name, FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                fileStream.Seek(0, SeekOrigin.Begin);

                                int d = data.Length;
                                int offset = 0;
                                for (offset = 0; (data.Length - offset) > 500000; offset += 500000)
                                {
                                    int l = data.Length - offset;
                                    fileStream.Write(data, offset, 500000);
                                    fileStream.Flush();
                                }
                                fileStream.Write(data, offset , data.Length - offset);
                                fileStream.Close();
                            }

                            //File.WriteAllBytes(Path.Combine(gRule.path, part.Filename), data);
                            filename = part.Filename;

                            GDownloads gDownloads = new GDownloads(part.Filename, gRule.path, DateTime.Now);
                            Act.FileParser.Add(GDownloads.pathfile, gDownloads);
                            ++downloadFiles;
                        }
                    }
                }
            gRule.lastMesId = messageId;

            ++processedMessages;
            progressbar(((processedMessages / allMessages) * 100));
            //return filename;
        }

        public Stack<string> GetMessages(string query, ref string lastId)
        {
            UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List(email);
            if (!String.IsNullOrEmpty(query))
                request.Q = query; // "label:отчеты-aliter-axi";
            request.MaxResults = Int32.MaxValue;

            Stack<string> buffer = new Stack<string>();
            string first = String.Empty;
            do
            {

                ListMessagesResponse response = request.Execute();
                IList<Message> messages = response.Messages;
                if (messages == null) return buffer;
                request.PageToken = response.NextPageToken;
                if (!String.IsNullOrEmpty(lastId))
                {
                    foreach (Message mes in messages)
                    {
                        if (mes.Id == lastId) return buffer;
                        buffer.Push(mes.Id);
                        ++allMessages;
                    }
                }
                else
                {
                    foreach (Message mes in messages)
                    {
                        buffer.Push(mes.Id);
                        ++allMessages;
                    }
                }
            } while (!String.IsNullOrEmpty(request.PageToken));
            return buffer;
        }

        static public string Query(GRule gRule)
        {
            string result = String.Empty;
            string type = gRule.type;
            string mode = gRule.mode;
            string tags = gRule.tag;

            if (!String.IsNullOrEmpty(mode))
                switch (mode)
                {
                    case СONCEPT.MODE.all_messages:
                        return result;
                        break;
                    case СONCEPT.MODE.label:
                        result += Model.СONCEPT.GetLabel(tags);
                        result += " ";
                        break;
                    case СONCEPT.MODE.subject:
                        result += Model.СONCEPT.GetSubject(tags);
                        result += " ";
                        break;
                    default:
                        break;
                }

            if (!String.IsNullOrEmpty(type))
            {
                result += СONCEPT.GetFilename(type);
            }
            return result;

        }
    }
}
