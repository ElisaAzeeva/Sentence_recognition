using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Sentence_recognition
{
    // Любые ошибки которые должны обрабатываться интерфейсом.
    // Обязательно добавляйте описание ошибки в комментариях.
    public enum ErrorCode
    {
        Ok = 0
    }

    // Члены предложения.
    // Переименуйте по желанию 

    [Flags]
    public enum SentenceMembers
    {
        Subject     = 0b000001,
        Predicate   = 0b000010,
        Definition  = 0b000100,
        Addition    = 0b010000,
        Circumstance= 0b100000,
    }


    // Здесь все понятно.
    public class Token
    {
        public int Sentence { get; }
        public int Offset { get; }
        public int Length { get; }
        public SentenceMembers Type { get; set; }

        public Token(int sentence, int start, int length, SentenceMembers type)
        {
            Contract.Requires(length > 0);
            Contract.Requires(Sentence >= 0);
            Contract.Requires(Offset >= 0);

            Sentence = sentence;
            Offset = start;
            Length = length;
            Type = type;
        }
    }

    // Не знаю насколько удачная структура. Время покажет. 
    public class Statistics
    {
        public SentenceMembers Type { get; }
        public List<(string sentence, int offset)> Cases { get; }
        public int Length { get; }
    }

    // Данные которые я хочу
    public class Data
    {
        public List<string> Sentenses { get; }

        // READ ME READ ME READ ME READ ME
        // !!!
        // Должно быть отсортировано по полю Start.
        // Start + Length Должна быть меньше чем Start следующего токена.
        // Ни в одном из токенов не должно быть '\r\n' или любых других переносов строк.
        // !!!
        public List<Token> Tokens { get; }

        public List<Statistics> Statistics { get; }

        public Data(List<string> sentenses, List<Token> tokens)
        {
            Sentenses = sentenses;
            Tokens = tokens;
        }
    }

    public class RecognitionAPI
    {

        // Прогресс от 0 до 1
        public async Task<(ErrorCode e, Data d)> GetData(string path, IProgress<double> progress)
        {
            // Путь должен проверяться на этой стороне.
            // Это может быть путь к текстовому, docx или иному 
            // другому файлу который используется программой.
            // (Или вообще какому нибудь левому файлу)


            for (double t = 0; t < 1; t+= 0.01)
            {
                Thread.Sleep(100);
                progress?.Report(t);
            }

            // Тестовая реализация
            var data = new Data( /*File.ReadAllText("War_and_Peace.txt"),*/
                new List<string> {
                    "\r\ntest test testаа test\r\ntest test test test\r\n"
                },
                new List<Token>{
                    new Token(0, 2, 4, SentenceMembers.Subject),
                    new Token(0, 7, 4, SentenceMembers.Predicate),
                    new Token(0, 12, 6, SentenceMembers.Definition),
                    new Token(0, 19, 4, SentenceMembers.Circumstance),
                    new Token(0, 25, 4, SentenceMembers.Addition),
                });

            return (0, data);
        }

        // Тут соответствие обозначений и типов членов предложения.
        // Это можно протестировать unit тестами
        // Ещё возможно этот кусок кода лучше (а может и нет) перенести в MyTextDecorations.cs 
        public static TextDecorationCollection GetDecorationFromType(SentenceMembers type)
        {
            switch (type)
            {
                case SentenceMembers.Subject:
                    return MyTextDecorations.Underline;
                case SentenceMembers.Predicate:
                    return MyTextDecorations.DoubleUnderline;
                case SentenceMembers.Definition:
                    return MyTextDecorations.WavyUnderline;
                case SentenceMembers.Circumstance:
                    return MyTextDecorations.DashDotedUnderline;
                case SentenceMembers.Addition:
                    return MyTextDecorations.DashedUnderline;
                default:
                    return null;
            }
        }

        public static string GetText(Data data)
        {
            return data.Sentenses.Aggregate("", (s1,s2) => s1 + s2);
        }

        // Возможно этот кусок кода лучше (а может и нет) перенести куда-то.
        // Возможно в какой либо вспомогательный класс.
        // Это можно протестировать unit тестами.
        // TODO: Это фактически для одного предложения. (Поле Sentence не учитывается)
        // Нам нужна функция которая преобразует предложения в строки.
        // 
        public static IEnumerable<Run> GetRuns(Data data, SentenceMembers sm)
        {
            int curent = 0;
            string text = GetText(data);

            foreach (var t in data.Tokens)
            {
                if (!sm.HasFlag(t.Type))
                    continue;

                yield return new Run(text.Substring(curent, t.Offset - curent));
                yield return new Run(text.Substring(t.Offset, t.Length))
                {
                    TextDecorations = GetDecorationFromType(t.Type)
                };
                curent = t.Offset + t.Length;
            }
            yield return new Run(text.Substring(curent));
        }
    }
}