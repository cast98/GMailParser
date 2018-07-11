using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmailParserViewProgram.Model
{
    static class СONCEPT
    {
        public struct MODE
        {
            public const string all_messages = "all messages";
            public const string subject = "subject";
            public const string label = "label";
            public const string filename = "filename";
        }

        static public string GetFilename(string str)
        {
            return MODE.filename + ":(" + str + ")";
        }

        static public string GetSubject(string str)
        {
            return MODE.subject + ":(" + str + ")";
        }

        static public string GetLabel(string str)
        {
            return MODE.label + ":(" + str + ")";
        }

        public struct DISCRIPTION
        {
            public const string all_messages = "(Rus)Поиск будет происходить , среди всех сообщений \n";

            public const string label = "(Rus)Поиск происходит на основе переданного в 'Tag' названия ярлыка \n" +
                       "которым помеченны сообщения.Введите ключевое слово(несколько слов) \n" +
                       "в поле 'Tag' и получите сообщения , которые помеченны \n" +
                       "заданым ярлыком. \n";

            public const string subject = "(Rus)Поиск происходит на основе слов в строке темы \n" +
                       "сообщения.Введите ключевое слово(несколько слов) в поле 'Tag' \n" +
                       "и получите сообщения , которое его содержит в \n" +
                       "своем заголовке. \n";
        }
    }
}
