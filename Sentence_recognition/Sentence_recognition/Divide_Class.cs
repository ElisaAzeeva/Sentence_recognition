using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentence_recognition
{
    struct chast_rechi
    {
        //ДЛЯ АЛЕКСЕЯ
        public int nomer_sent;//Номер предложение
        public int Offset;//Номер первой буквы слова
        public int Lenght;//Длина слова
        public string ch_sent;//Член предложения
                              //ДЛЯ НАС (AKA КОСТЫЛЬЩИКИ)
        public int nomer;//Номер слова в предложении
        public string chast_rech;//Часть речи
    }
    class Divide_Class
    {
        chast_rechi chast_v1 = new chast_rechi();
        List <chast_rechi> chast = new List <chast_rechi>();
        public List<string> divide_sent(string words,int nomer_senten)
        {
            int dlina = 0;// Считаем длину слова
            int y = 1;//Для номера слова в предложении
            int strl = words.Length;
            for(int i=0;i<strl;i++)
            {
                if (words[i] != '(')
                {
                    if((dlina != 0))//Чтобы не учитывать (,),( ), (;) и т.д.
                    if ((words[i] == ' ') || (words[i] == ',') || (words[i] == ';') || (words[i] == ':') || (words[i] == ')') || (words[i] == '\t'))
                    {
                        chast_v1.nomer_sent = nomer_senten;
                        chast_v1.Offset = i - dlina;
                        chast_v1.Lenght = dlina;
                        chast_v1.nomer = y;
                        chast.Add(chast_v1);
                        dlina = 0;
                        y++;
                    }
                    char erqrqw = words[2];
                    if ((words[i] != ' ') && (words[i] != ',') && (words[i] != ';') && (words[i] != ':') && (words[i] != ')') && (words[i] != '\t'))
                        dlina++;
                }
            }
            List<string> split1 = new List<string>();
            string[] split = words.Split(new Char[] { ' ', ',', ':', '\t', '(', ')', ';' });

            foreach (string s in split)
            {

                if (s.Trim() != "")
                    split1.Add(s);

            }
            return split1;
        }
        public List<string> divide_text(string words)
        {

            List<string> split1 = new List<string>();
            string[] split = words.Split(new Char[] { '!', '?', '.' });

            foreach (string s in split)
            {

                if (s.Trim() != "")
                    split1.Add(s);

            }
            return split1;
        }
        public List <chast_rechi> Raspoznavanie(List<string> divide)
        {
            foreach(string divi in divide)
            {
                foreach(string divis in divide)
                for (int i=divi.Length;i>0;i++)
                    {

                    }
            }
            return chast;
        }
    }
}
