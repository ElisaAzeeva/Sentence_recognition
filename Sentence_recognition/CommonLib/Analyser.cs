using System;
using System.Collections.Generic;
using SolarixGrammarEngineNET;
using CommonLib;

namespace Sentence_recognition
{
    class Word
    {
        public SentenceMembers TypeInSentence { get; set; }
        public int Pos { get; set; }
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

    class Analyser
    {
        const string dictionaryPath = @".\bin-windows\dictionary.xml";

        IntPtr hEngine; // указатель на движок разбора
        List<Word> words = new List<Word>(); // Слова

        public ErrorCode Init()
        {
            hEngine = GrammarEngine.sol_CreateGrammarEngineW(dictionaryPath);

            if (hEngine == IntPtr.Zero)
                return ErrorCode.NoDictionary;

            int r = GrammarEngine.sol_DictionaryVersion(hEngine);

            if (r == -1)
                return ErrorCode.NoDictionary2;

            // Проверим, что там есть русский лексикон.
            int ie1 = GrammarEngine.sol_FindEntry(
                hEngine,
                "МАМА",
                GrammarEngineAPI.NOUN_ru,
                GrammarEngineAPI.RUSSIAN_LANGUAGE);

            if (ie1 == -1)
                return ErrorCode.NoRussion;

            return ErrorCode.Ok;
        }

        public List<List<Token>> ParsingText(List<string> sentenses, IProgress<(double, string)> progress)
        {
            List<List<Token>> ok = new List<List<Token>>();
            int i = 0;
            foreach (string s in sentenses)
            {
                progress?.Report(((double)i / (sentenses.Count - 1),
                    $"Обработка предложения: {i + 1}/{sentenses.Count}"));
                ok.Add(Parsing_FULL(s, i++));
            }

            return ok;
        }

        public List<Token> Parsing_FULL(string Sent, int number_senten)
        {
            List<Token> chast = Split.DivideSentance(Sent, number_senten);

            IntPtr hPack11 = GrammarEngine.sol_SyntaxAnalysis(hEngine, Sent,
                GrammarEngine.MorphologyFlags.SOL_GREN_ALLOW_FUZZY, 0, 60000 | (20 << 22),
                GrammarEngineAPI.RUSSIAN_LANGUAGE);

            int nroot = GrammarEngine.sol_CountRoots(hPack11, 0);

            words.Clear();

            for (int iroot = 1; iroot < nroot - 1; ++iroot)
            {
                IntPtr hRoot = GrammarEngine.sol_GetRoot(hPack11, 0, iroot);
                var pos = GrammarEngine.sol_GetNodePosition(hRoot);

                words.Add(new Word()
                {
                    TypeInSentence = SentenceMembers.Predicate,
                    Pos = pos
                });

                Parsing(hRoot);

                GrammarEngine.sol_DeleteResPack(hPack11);
            }
            words.Sort((w1, w2) => w1.Pos.CompareTo(w2.Pos));

            for (int i = 0; i < chast.Count; i++)
                chast[i].Type = words[i].TypeInSentence;

            return chast;
        }

        private void Parsing(IntPtr rf)
        {
            int leafsCount = GrammarEngine.sol_CountLeafs(rf); // Количество листьев

            for (int Leaff = 0; Leaff < leafsCount; Leaff++)
            {
                //НЕЕЕЕЕ ЛЕЕЕЕЗЬЬЬЬ 
                IntPtr pLeaf = GrammarEngine.sol_GetLeaf(rf, Leaff); //Указатель на текущий лист

                int prKey = GrammarEngine.sol_GetNodeIEntry(hEngine, pLeaf); //Первичный ключ словарной статьи(для определения части речи)

                var type = (TypeOfWord)GrammarEngine.sol_GetEntryClass(hEngine, prKey); // id части речи
                var link = GrammarEngine.sol_GetLeafLinkType(rf, Leaff); // Связь листа и его корня (предыдущего листа)
                var pos = GrammarEngine.sol_GetNodePosition(pLeaf);

                words.Add(new Word() {
                    TypeInSentence = GetSentanceTypeByWordTypeAndLinkType(type, link),
                    Pos = pos
                });

                int curLeafsCount = GrammarEngine.sol_CountLeafs(pLeaf); //Количество листьев у текущего листа

                if (curLeafsCount > 0)
                    Parsing(pLeaf); //Рекурсия
            }
        }

        private SentenceMembers GetSentanceTypeByWordTypeAndLinkType(TypeOfWord type, int link)
        {
            switch (link)
            {
                case 4: //Подлежащее
                    return SentenceMembers.Subject;
                case 0: //Дополнение
                    return SentenceMembers.Addition;
                case 1:
                    switch (type)
                    {
                        case TypeOfWord.Adverb:
                            return SentenceMembers.Circumstance;
                        case TypeOfWord.Adjective:
                            return SentenceMembers.Definition;
                        case TypeOfWord.Pritag:
                            return SentenceMembers.Addition;
                    }
                    break;
                case 68:
                    {
                        switch (type)
                        {
                            case TypeOfWord.Pronoun:
                            case TypeOfWord.Pronoun_such:
                            case TypeOfWord.Noun:
                                return SentenceMembers.Subject;
                            case TypeOfWord.Verb:
                                return SentenceMembers.Predicate;
                            case TypeOfWord.Adjective:
                                return SentenceMembers.Definition;
                        }
                        break;
                    }
            }
            return 0;
        }
    }
}
