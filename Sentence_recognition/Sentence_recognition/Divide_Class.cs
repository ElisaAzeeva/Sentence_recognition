using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SolarixGrammarEngineNET;

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
        Pritag = 27,
    }

    class Divide_Class
    {
        IntPtr hEngine;//указатель на движок разбора
        chast_rechi chast_v1 = new chast_rechi();
        string fff;//Корень
        List<int> sv9Iz = new List<int>();//Связи
        List<string> slova = new List<string>();//Слова
        List<int> SOS = new List<int>();//Коды частей речи
        List<string> SOSI = new List<string>();//Части речи
        private List<string> divide_sent(string words,int nomer_senten,ref List<chast_rechi> chast)
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
                    if ((words[i] == ' ') || (words[i] == ',') || (words[i] == ';') || (words[i] == ':') || (words[i] == ')') || (words[i] == '\t') || (words[i] == '\0')||(words[i] == '-'))
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
                    
                    if ((words[i] != ' ') && (words[i] != ',') && (words[i] != ';') && (words[i] != ':') && (words[i] != ')') && (words[i] != '\t') && (words[i] != '-'))
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
        private List<string> divide_text(string words)
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
        public void Slovar()
        {
            hEngine = GrammarEngine.sol_CreateGrammarEngineW(@"C:\bin-windows/dictionary.xml");
            if (hEngine == IntPtr.Zero)
            {
                Console.WriteLine("Could not load the dictionary");
                return;
            }
            Int32 r = GrammarEngine.sol_DictionaryVersion(hEngine);
            if (r != -1)
            {
                Console.WriteLine("Словарь загружен. Версия: " + r.ToString());
            }
            else
            {
                Console.WriteLine("Ошибка загрузки словаря.");
                return;
            }


            // Проверим, что там есть русский лексикон.
            int ie1 = GrammarEngine.sol_FindEntry(hEngine, "МАМА", SolarixGrammarEngineNET.GrammarEngineAPI.NOUN_ru, SolarixGrammarEngineNET.GrammarEngineAPI.RUSSIAN_LANGUAGE);
            if (ie1 == -1)
            {
                Console.WriteLine("Russian language is missing in lexicon.");
                return;
            }
        }
        public List<List<chast_rechi>> RaZborText(string text)
        {
            //Slovar();
            List<List<chast_rechi>> ok = new List<List<chast_rechi>>();
            List<string> split = divide_text(text);
            int i = 0;

            foreach (string s in split)
            {
              ok.Add( Razbor_FULL(s, i++));
               
                Debug.WriteLine("FFFFFFFFFFF");
            }
            return ok;
        }
        public List<chast_rechi> Razbor_FULL(string Sent,int nomer_senten)
        {
              List<chast_rechi> chast = new List<chast_rechi>();
             divide_sent(Sent, nomer_senten, ref chast);
            IntPtr hPack11 = GrammarEngine.sol_SyntaxAnalysis(hEngine, Sent, GrammarEngine.MorphologyFlags.SOL_GREN_ALLOW_FUZZY, 0, (60000 | (20 << 22)), GrammarEngineAPI.RUSSIAN_LANGUAGE);
            int nroot = GrammarEngine.sol_CountRoots(hPack11, 0);
            sv9Iz.Clear();
            slova.Clear();
            SOS.Clear();
            SOSI.Clear();
           
            for (int iroot = 1; iroot < nroot - 1; ++iroot)
            {
                IntPtr hRoot = GrammarEngine.sol_GetRoot(hPack11, 0, iroot);
                fff = GrammarEngine.sol_GetNodeContentsFX(hRoot);
                int broot = GrammarEngine.sol_CountLeafs(hRoot);
                Razbor(hRoot);
                Preobraz(SOS);
                Sopostavlenie(ref chast);
                GrammarEngine.sol_DeleteResPack(hPack11);
            }

            return chast;

        }
        private void Razbor(IntPtr rf)
        {
            int broofft = GrammarEngine.sol_CountLeafs(rf);// Количество листьев
            for (int Leaff = 0; Leaff < broofft; Leaff++)
            {
                //НЕЕЕЕЕ ЛЕЕЕЕЗЬЬЬЬ 
                IntPtr frf = GrammarEngine.sol_GetLeaf(rf, Leaff);//Указатель на текущий лист
                int afafaf = GrammarEngine.sol_GetNodeIEntry(hEngine, frf);//Первичный ключ словарной статьи(для определения части речи)
                SOS.Add(GrammarEngine.sol_GetEntryClass(hEngine, afafaf));// id части речи
                slova.Add(GrammarEngine.sol_GetNodeContentsFX(frf)); //Слова
                sv9Iz.Add(GrammarEngine.sol_GetLeafLinkType(rf, Leaff));//Связи листа и его корня (предыдущего листа)
                int broofftt = GrammarEngine.sol_CountLeafs(frf);//Количество листьев у текущего листа
                if (broofftt > 0)
                {
                    Razbor(frf);//Рекурсия
                }
            }
        }

        private void Preobraz(List<int> b)
        {
            for (int i = 0; i < b.Count(); i++)
            {
                switch (b[i])
                {
                    case (int)ChastiRechi.Glagol:
                    {
                        SOSI.Add("Глагол");
                        break;
                    }
                    case (int)ChastiRechi.Narech:
                    {
                        SOSI.Add("Наречие");
                        break;
                    }
                    case (int)ChastiRechi.Prilag:
                    {
                        SOSI.Add("Прилагательное");
                        break;
                    }
                    case (int)ChastiRechi.Such:
                    {
                        SOSI.Add("Существительное");
                        break;
                    }
                    case (int)ChastiRechi.Mestoim:
                    {
                        SOSI.Add("Местоимение");
                        break;
                    }
                    case (int)ChastiRechi.Mestoim_such:
                    {
                        SOSI.Add("Местоимение_обман");
                        break;
                    }
                    case (int)ChastiRechi.Pritag:
                    {
                        SOSI.Add("Притяжательная_частица");
                        break;
                    }
                    default:
                    {
                        SOSI.Add("Хуита какая-то");
                        break;
                    }
                }
            }
        }

        private void Sopostavlenie(ref List<chast_rechi> chast)
        {
            chast_rechi example = new chast_rechi();
            int y = default(int);
            foreach (string ag in slova)
            {
                for (int i = 0; i < chast.Count(); i++)
                {
                    if (fff == chast[i].text)
                    {
                        example = chast[i];
                        example.ch_sent = SentenceMembers.Predicate;
                        chast[i] = example;
                    }
                    if (ag == chast[i].text) //Ищем нужное нам слово
                    {
                        switch (sv9Iz[y])
                        {
                            case 4://Подлежащее
                            {
                                example = chast[i];
                                example.ch_sent = SentenceMembers.Subject;
                                chast[i] = example;
                                break;
                            }
                            case 0://Дополнение
                            {
                                example = chast[i];
                                example.ch_sent = SentenceMembers.Addition;
                                chast[i] = example;
                                break;
                            }
                            case 1:
                            {
                                if (SOSI[y] == "Прилагательное")
                                {
                                    example = chast[i];
                                    example.ch_sent = SentenceMembers.Definition;
                                    chast[i] = example;
                                }
                                if (SOSI[y] == "Наречие")
                                {
                                    example = chast[i];
                                    example.ch_sent = SentenceMembers.Circumstance;
                                    chast[i] = example;
                                }
                                if (SOSI[y] == "Притяжательная_частица")
                                {
                                    example = chast[i];
                                    example.ch_sent = SentenceMembers.Addition;
                                    chast[i] = example;
                                }
                                break;
                            }
                            case 68:
                            {
                                if ((SOSI[y] == "Местоимение") || (SOSI[y] == "Местоимение_обман") || (SOSI[y] == "Существительное"))
                                {
                                    example = chast[i];
                                    example.ch_sent = SentenceMembers.Subject;
                                    chast[i] = example;
                                }
                                if (SOSI[y] == "Глагол")
                                {
                                    example = chast[i];
                                    example.ch_sent = SentenceMembers.Predicate;
                                    chast[i] = example;
                                }
                                if (SOSI[y] == "Прилагательное")
                                {
                                    example = chast[i];
                                    example.ch_sent = SentenceMembers.Definition;
                                    chast[i] = example;
                                }
                                break;
                            }
                            default:
                                break;
                        }

                    }
                }
                y++;
            }
        }
    }
}
