﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;

namespace CommonLib
{
    public class RecognitionAPI
    {

        // Прогресс от 0 до 1
        public (ErrorCode e, Data d) GetData(string path, IProgress<double> progress)
        {
            // Путь должен проверяться на этой стороне.
            // Это может быть путь к текстовому, docx или иному 
            // другому файлу который используется программой.
            // (Или вообще какому нибудь левому файлу)

            for (double t = 0; t <= 1; t+= 0.01)
            {
                Thread.Sleep(10);
                progress?.Report(t);
            }

            Data data = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
            // Тестовая реализация
            data = new Data( /*File.ReadAllText("War_and_Peace.txt"),*/
                new List<string> {
                    "\r\ntest test\r\n testаа test",
                  //  "\r\ntest test test test\r\n"
                },
                new List<Token>{
                    new Token(0, 2, 4, SentenceMembers.Subject),
                    new Token(0, 7, 4, SentenceMembers.Predicate),
                    new Token(0, 14, 6, SentenceMembers.Definition),
                    //new Token(1, 3, 4, SentenceMembers.Circumstance),
                    //new Token(1, 9, 4, SentenceMembers.Addition),
                });

            return (0, data);
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
                    TextDecorations = MyTextDecorations.GetDecorationFromType(t.Type)
                };
                curent = t.Offset + t.Length;
            }
            yield return new Run(text.Substring(curent));
        }
    }
}