using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        Subject     = 0x000001,
        Predicate   = 0x000010,
        Definition  = 0x000100,
        Application = 0x001000,
        Addition    = 0x010000,
        Circumstance= 0x100000,
    }

    // Не уверен что здесь лучшее место чтобы написать это.
    public class SentenceMembersValueConverter : TEnumValueConverter<SentenceMembers> { }


    // Здесь все понятно.
    public class Token
    {
        public int Start { get; }
        public int Length { get; }
        public SentenceMembers Type { get; set; }

        public Token(int start, int length, SentenceMembers type)
        {
            Contract.Requires(length > 0);
            Contract.Requires(Start >= 0);

            Start = start;
            Length = length;
            Type = type;
        }
    }

    // Данные которые я хочу
    public class Data
    {
        public string Text { get; }

        // READ ME READ ME READ ME READ ME
        // !!!
        // Должно быть отсортировано по полю Start.
        // Start + Length Должна быть меньше чем Start следующего токена.
        // Ни в одном из токенов не должно быть '\r\n' или любых других переносов строк.
        // Эти требования (теоретически) отраженны в Contract.Assume 
        // !!!
        public List<Token> List { get; }

        // Здесь будет ещё одна структура отвечающая за статистику.

        public Data(string text, List<Token> list)
        {
            // Эти контракты слишком "тяжеловесные" для понимания.
            // Contract.ForAll(list.Take(list.Count() - 1).Select((t, i) => (t, i)), e => e.t.Start + e.t.Length );

            Text = text;
            List = list;
        }
    }

    public class RecognitionAPI
    {
        public (ErrorCode e, Data d) GetData(string path)
        {
            // Путь должен проверяться на этой стороне.
            // Это может быть путь к текстовому, docx или иному 
            // другому файлу который используется программой.
            // (Или вообще какому нибудь левому файлу)


            // Тестовая реализация
            var data = new Data( /*File.ReadAllText("War_and_Peace.txt"),*/ "\r\ntest test testаа test\r\ntest test test test\r\n",
                new List<Token>{
                    new Token(2,4,SentenceMembers.Subject),
                    new Token(7,4,SentenceMembers.Predicate),
                    new Token(12,6,SentenceMembers.Definition),
                    new Token(19,4,SentenceMembers.Circumstance),
                    new Token(25,4,SentenceMembers.Addition),
                });

            return (0, data);
        }

        // Тут соответствие обозначений и типов членов предложения.
        // И оно некорректно. 
        // TODO: Поправить.
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
                case SentenceMembers.Application:
                case SentenceMembers.Addition:
                    return MyTextDecorations.DashedUnderline;
                default:
                    return null;
            }
        }

        // Возможно этот кусок кода лучше (а может и нет) перенести куда-то.
        // Возможно в какой либо вспомогательный класс.
        // Это можно протестировать unit тестами.
        public static IEnumerable<Run> GetRuns(Data data, SentenceMembers sm)
        {
            int curent = 0;
            foreach (var t in data.List)
            {
                if (!sm.HasFlag(t.Type))
                    continue;

                yield return new Run(data.Text.Substring(curent, t.Start - curent));
                yield return new Run(data.Text.Substring(t.Start, t.Length))
                {
                    TextDecorations = GetDecorationFromType(t.Type)
                };
                curent = t.Start + t.Length;
            }
            yield return new Run(data.Text.Substring(curent));
        }
    }
}