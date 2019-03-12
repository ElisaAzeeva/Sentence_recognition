using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentence_recognition
{
    public struct chast_rechi
    {
        //ДЛЯ АЛЕКСЕЯ
        public int nomer_sent;//Номер предложение
        public int Offset;//Номер первой буквы слова
        public int Lenght;//Длина слова
        public SentenceMembers ch_sent;//Член предложения
        public string text;//Слово
    }
    //Взято у Алексея нужно будет в последствии убарть
    public enum SentenceMembers
    {
        Subject = 0b000001,
        Predicate = 0b000010,
        Definition = 0b000100,
        Addition = 0b010000,
        Circumstance = 0b100000,
    }
    
    public enum ChastiRechi
    {
        Such = 6,
        Glagol = 12,
        Narech = 20,
        Prilag = 9,
        Mestoim=8,
        Mestoim_such=7,
    }
    class Divide_Class
    {
        chast_rechi chast_v1 = new chast_rechi();
        public List <chast_rechi> chast = new List <chast_rechi>();
        public List<string> divide_sent(string words,int nomer_senten)
        {
           
            int dlina = 0;// Считаем длину слова
            string str = default(string);
            int y = 1;//Для номера слова в предложении
            words += '\0';//!!!!!!!!ALARM!!!!!!!! ОБНАРУЖЕН КОСТЫЛЬ
            int strl = words.Length;

            for(int i=0;i<strl;i++)
            {
                if (words[i] != '(')
                {
                    if((dlina != 0))//Чтобы не учитывать (,),( ), (;) и т.д.
                    if ((words[i] == ' ') || (words[i] == ',') || (words[i] == ';') || (words[i] == ':') || (words[i] == ')') || (words[i] == '\t') || (words[i] == '\0'))
                    {
                        chast_v1.nomer_sent = nomer_senten;
                        chast_v1.Offset = i - dlina;
                        chast_v1.Lenght = dlina;
                        chast_v1.text = str;
                        chast.Add(chast_v1);
                        dlina = 0;
                        y++;
                        str = default(string);
                    }
                    
                    if ((words[i] != ' ') && (words[i] != ',') && (words[i] != ';') && (words[i] != ':') && (words[i] != ')') && (words[i] != '\t'))
                    {
                        if (i != strl - 1)
                        {
                            str += words[i];
                            dlina++;
                        }
                    }
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
    }
}
