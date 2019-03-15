using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using SolarixGrammarEngineNET;
using CommonLib;

namespace Sentence_recognition
{
    public struct chast_rechi
    {
        //ДЛЯ АЛЕКСЕЯ
        public Token token;
        //public int Sentence;//Номер предложение
        //public int Offset;//Номер первой буквы слова
        //public int Length;//Длина слова
        public SentenceMembers Type;//Член предложения
        public string text;//Слово
    }
    
    public enum TypeOfWord
    {
        Noun = 6,
        Verb = 12,
        Adverb = 20,
        Adjective = 9,
        Pronoun=8,
        Pronoun_such=7,
        Pritag = 27,
    }

    class Divide_Class
    {
        const String dictionaryPath = @"C:\Users\Eliza\Documents\GitHub\Sentence_recognition\Sentence_recognition\Sentence_recognition\bin\Debug\bin-windows\dictionary.xml";
        IntPtr hEngine;//указатель на движок разбора
        chast_rechi chast_v1 = new chast_rechi();
        string root;//Корень
        List<int> links = new List<int>();//Связи
        List<string> words = new List<string>();//Слова
        List<int> codeTypeOfWord = new List<int>();//Коды частей речи
        List<string> typeOfWord = new List<string>();//Части речи
        private List<string> divide_sent(string words,int number_senten,ref List<chast_rechi> chast)
        {
           
            int length = 0;// Считаем длину слова
            string str = default(string);
            int wordPos = 1;//Для номера слова в предложении
            words += '\0';//!!!!!!!!ALARM!!!!!!!! ОБНАРУЖЕН КОСТЫЛЬ
            int strl = words.Length;

            for(int i=0;i<strl;i++)
            {
                if (words[i] != '(')
                {
                    if((length != 0))//Чтобы не учитывать (,),( ), (;) и т.д.
                    if ((words[i] == ' ') || (words[i] == ',') || (words[i] == ';') || (words[i] == ':') || (words[i] == ')') || (words[i] == '\t') || (words[i] == '\0')||(words[i] == '-'))
                    {
                            //chast_v1.token(number_senten, i - length, length)
                            chast_v1.token = new Token();
                            chast_v1.token.Sentence=number_senten;
                            chast_v1.token.Offset = i - length;
                            chast_v1.token.Length = length;
                            //chast_v1.Sentence = number_senten;
                            //chast_v1.Offset = i - length;
                            //chast_v1.Length = length;
                            chast_v1.text = str;
                        chast.Add(chast_v1);
                        length = 0;
                        wordPos++;
                        str = default(string);
                    }
                    
                    if ((words[i] != ' ') && (words[i] != ',') && (words[i] != ';') && (words[i] != ':') && (words[i] != ')') && (words[i] != '\t') && (words[i] != '-'))
                    {
                        if (i != strl - 1)
                        {
                            str += words[i];
                            length++;
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
        public void Dictionary()
        {
            //TODO: path is const!!!
            hEngine = GrammarEngine.sol_CreateGrammarEngineW(dictionaryPath);
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
        public List<List<chast_rechi>> ParsingText(string text)
        {
            //Dictionary();
            List<List<chast_rechi>> ok = new List<List<chast_rechi>>();
            List<string> split = divide_text(text);
            int i = 0;

            foreach (string s in split)
            {
              ok.Add( Parsing_FULL(s, i++));
            }
            return ok;
        }
        public List<chast_rechi> Parsing_FULL(string Sent,int number_senten)
        {
              List<chast_rechi> chast = new List<chast_rechi>();
             divide_sent(Sent, number_senten, ref chast);
            IntPtr hPack11 = GrammarEngine.sol_SyntaxAnalysis(hEngine, Sent, GrammarEngine.MorphologyFlags.SOL_GREN_ALLOW_FUZZY, 0, (60000 | (20 << 22)), GrammarEngineAPI.RUSSIAN_LANGUAGE);
            int nroot = GrammarEngine.sol_CountRoots(hPack11, 0);
            links.Clear();
            words.Clear();
            codeTypeOfWord.Clear();
            typeOfWord.Clear();
           
            for (int iroot = 1; iroot < nroot - 1; ++iroot)
            {
                IntPtr hRoot = GrammarEngine.sol_GetRoot(hPack11, 0, iroot);
                root = GrammarEngine.sol_GetNodeContentsFX(hRoot);
                int broot = GrammarEngine.sol_CountLeafs(hRoot);
                Parsing(hRoot);
                Transform(codeTypeOfWord);
                Corelate(ref chast);
                GrammarEngine.sol_DeleteResPack(hPack11);
            }

            return chast;

        }
        private void Parsing(IntPtr rf)
        {
            int leafsCount = GrammarEngine.sol_CountLeafs(rf);// Количество листьев
            for (int Leaff = 0; Leaff < leafsCount; Leaff++)
            {
                //НЕЕЕЕЕ ЛЕЕЕЕЗЬЬЬЬ 
                IntPtr pLeaf = GrammarEngine.sol_GetLeaf(rf, Leaff);//Указатель на текущий лист
                int prKey = GrammarEngine.sol_GetNodeIEntry(hEngine, pLeaf);//Первичный ключ словарной статьи(для определения части речи)
                codeTypeOfWord.Add(GrammarEngine.sol_GetEntryClass(hEngine, prKey));// id части речи
                words.Add(GrammarEngine.sol_GetNodeContentsFX(pLeaf)); //Слова
                links.Add(GrammarEngine.sol_GetLeafLinkType(rf, Leaff));//Связи листа и его корня (предыдущего листа)
                int curLeafsCount = GrammarEngine.sol_CountLeafs(pLeaf);//Количество листьев у текущего листа
                if (curLeafsCount > 0)
                {
                    Parsing(pLeaf);//Рекурсия
                }
            }
        }

        private void Transform(List<int> b)
        {
            for (int i = 0; i < b.Count(); i++)
            {
                switch (b[i])
                {
                    case (int)TypeOfWord.Verb:
                    {
                        typeOfWord.Add("Глагол");
                        break;
                    }
                    case (int)TypeOfWord.Adverb:
                    {
                        typeOfWord.Add("Наречие");
                        break;
                    }
                    case (int)TypeOfWord.Adjective:
                    {
                        typeOfWord.Add("Прилагательное");
                        break;
                    }
                    case (int)TypeOfWord.Noun:
                    {
                        typeOfWord.Add("Существительное");
                        break;
                    }
                    case (int)TypeOfWord.Pronoun:
                    {
                        typeOfWord.Add("Местоимение");
                        break;
                    }
                    case (int)TypeOfWord.Pronoun_such:
                    {
                        typeOfWord.Add("Местоимение_обман");
                        break;
                    }
                    case (int)TypeOfWord.Pritag:
                    {
                        typeOfWord.Add("Притяжательная_частица");
                        break;
                    }
                    default:
                    {
                        typeOfWord.Add("Неопределено");
                        break;
                    }
                }
            }
        }

        private void Corelate(ref List<chast_rechi> chast)
        {
            chast_rechi example = new chast_rechi();
            int y = default(int); //TODO: explain
            foreach (string ag in words)
            {
                for (int i = 0; i < chast.Count(); i++)
                {
                    if (root == chast[i].text)
                    {
                        example = chast[i];
                        example.token.Type = SentenceMembers.Predicate;
                        chast[i] = example;
                    }
                    if (ag == chast[i].text) //Ищем нужное нам слово
                    {
                        switch (links[y])
                        {
                            case 4://Подлежащее
                            {
                                example = chast[i];
                                example.token.Type = SentenceMembers.Subject;
                                chast[i] = example;
                                break;
                            }
                            case 0://Дополнение
                            {
                                example = chast[i];
                                example.token.Type = SentenceMembers.Addition;
                                chast[i] = example;
                                break;
                            }
                            case 1:
                            {
                                if (typeOfWord[y] == "Прилагательное")
                                {
                                    example = chast[i];
                                    example.token.Type = SentenceMembers.Definition;
                                    chast[i] = example;
                                }
                                if (typeOfWord[y] == "Наречие")
                                {
                                    example = chast[i];
                                    example.token.Type = SentenceMembers.Circumstance;
                                    chast[i] = example;
                                }
                                if (typeOfWord[y] == "Притяжательная_частица")
                                {
                                    example = chast[i];
                                    example.token.Type = SentenceMembers.Addition;
                                    chast[i] = example;
                                }
                                break;
                            }
                            case 68:
                            {
                                if ((typeOfWord[y] == "Местоимение") || (typeOfWord[y] == "Местоимение_обман") || (typeOfWord[y] == "Существительное"))
                                {
                                    example = chast[i];
                                    example.token.Type = SentenceMembers.Subject;
                                    chast[i] = example;
                                }
                                if (typeOfWord[y] == "Глагол")
                                {
                                    example = chast[i];
                                    example.token.Type = SentenceMembers.Predicate;
                                    chast[i] = example;
                                }
                                if (typeOfWord[y] == "Прилагательное")
                                {
                                    example = chast[i];
                                    example.token.Type = SentenceMembers.Definition;
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
