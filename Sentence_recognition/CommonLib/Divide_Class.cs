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
        public SentenceMembers Type;//Член предложения
        public string text;//Слово
    }

    class Divide_Class
    {
        const String dictionaryPath = @".\bin-windows\dictionary.xml";
        IntPtr hEngine;//указатель на движок разбора
        chast_rechi chast_v1 = new chast_rechi();
        string root;//Корень
        List<int> links = new List<int>();//Связи
        List<string> words = new List<string>();//Слова
        List<int> codeTypeOfWord = new List<int>();//Коды частей речи
        List<TypeOfWord> typeOfWord = new List<TypeOfWord>();//Части речи

        private List<string> divide_sent(string words, int number_senten, ref List<chast_rechi> chast)
        {

            int length = 0;// Считаем длину слова
            string str = default(string);
            int wordPos = 1;//Для номера слова в предложении
            words += '\0';//!!!!!!!!ALARM!!!!!!!! ОБНАРУЖЕН КОСТЫЛЬ
            int strl = words.Length;

            for (int i = 0; i < strl; i++)
            {
                if (words[i] != '(')
                {
                    if (length != 0) //Чтобы не учитывать (,),( ), (;) и т.д.
                        if (" ,;:)\t\0-".Contains(words[i]))
                        {
                            chast_v1.token = new Token
                            {
                                Sentence = number_senten,
                                Offset = i - length,
                                Length = length
                            };
                            chast_v1.text = str;

                            chast.Add(chast_v1);

                            length = 0;
                            wordPos++;
                            str = default(string);
                        }

                    if (!" ,;:)\t\0-".Contains(words[i]))
                    {
                        if (i != strl - 1)
                        {
                            str += words[i];
                            length++;
                        }
                    }
                }
            }

            return words.Split(" ,:\t();".ToCharArray()).Where(s => s.Trim() != "").ToList();
        }

        private List<string> divide_text(string words)
        {
            return words.Split("!?.".ToCharArray()).Where(s => s.Trim() != "").ToList();
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
            int ie1 = GrammarEngine.sol_FindEntry(hEngine, "МАМА", GrammarEngineAPI.NOUN_ru, GrammarEngineAPI.RUSSIAN_LANGUAGE);
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
                ok.Add(Parsing_FULL(s, i++));
            }
            return ok;
        }
        public List<chast_rechi> Parsing_FULL(string Sent, int number_senten)
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

                typeOfWord = codeTypeOfWord.Select(tof => (TypeOfWord)tof).ToList();

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
        /// <summary> Части речи </summary>
        public enum TypeOfWord
        {
            /// <summary> Существительное </summary>
            Noun = 6,
            /// <summary> Глагол </summary>
            Verb = 12,
            /// <summary> Наречие </summary>
            Adverb = 20,
            /// <summary> Прилагательное </summary>
            Adjective = 9,
            /// <summary> Местоимение </summary>
            Pronoun = 8,
            /// <summary> Местоимение_обман </summary>
            Pronoun_such = 7,
            /// <summary> Притяжательная_частица </summary>
            Pritag = 27,
        }

        private void Corelate(ref List<chast_rechi> chast)
        {
            chast_rechi example = new chast_rechi();
            int y = default(int); //TODO: explain
            foreach (string ag in words)
            {
                for (int i = 0; i < chast.Count(); i++)
                {
                    example = chast[i];

                    if (root == chast[i].text)
                        example.token.Type = SentenceMembers.Predicate;

                    if (ag == chast[i].text) //Ищем нужное нам слово
                    {
                        switch (links[y])
                        {
                            case 4: //Подлежащее
                                {
                                    example.token.Type = SentenceMembers.Subject;
                                    break;
                                }
                            case 0: //Дополнение
                                {
                                    example.token.Type = SentenceMembers.Addition;
                                    break;
                                }
                            case 1:
                                {
                                    if (typeOfWord[y] == TypeOfWord.Adjective)
                                        example.token.Type = SentenceMembers.Definition;
                                    if (typeOfWord[y] == TypeOfWord.Adverb)
                                        example.token.Type = SentenceMembers.Circumstance;
                                    if (typeOfWord[y] == TypeOfWord.Pritag)
                                        example.token.Type = SentenceMembers.Addition;
                                    break;
                                }
                            case 68:
                                {
                                    if ((typeOfWord[y] == TypeOfWord.Pronoun) || (typeOfWord[y] == TypeOfWord.Pronoun_such) || (typeOfWord[y] == TypeOfWord.Noun))
                                        example.token.Type = SentenceMembers.Subject;
                                    if (typeOfWord[y] == TypeOfWord.Verb)
                                        example.token.Type = SentenceMembers.Predicate;
                                    if (typeOfWord[y] == TypeOfWord.Adjective)
                                        example.token.Type = SentenceMembers.Definition;
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    chast[i] = example;
                }
                y++;
            }
        }
    }
}
