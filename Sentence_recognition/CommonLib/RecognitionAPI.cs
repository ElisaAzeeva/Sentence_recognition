using Sentence_recognition;
using System;
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

            Divide_Class ff = new Divide_Class();
            ff.Dictionary(); //Загружаем словарь
            //List<List<chast_rechi>> w = ff.ParsingText("Из молодежи, не считая старшей дочери графини (которая была четырьмя годами старше сестры и держала себя уже как большая) и гостьи-барышни, в гостиной остались Николай и Соня-племянница.");
            List<chast_rechi> Result = ff.Parsing_FULL("\r\n Счастливая и озорная улыбка осветила его лицо", 1);

            // Тестовая реализация
            data = new Data( /*File.ReadAllText("War_and_Peace.txt"),*/
                new List<string> {
                    "\r\n Счастливая и озорная улыбка осветила его лицо",
                    //"\r\n2 block. 2 block. 2 block. 2 block. 2 block. 2 block. 2 block.\r\n"
                },
                Result.Select(r => r.token).ToList()
                );

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

            int currentSentence = 1;
            int offset = 0;

            foreach (var t in data.Tokens)
            {
                if (!sm.HasFlag(t.Type))
                    continue;

                if (t.Sentence!=currentSentence)
                {
                    var sum = 0;
                    for (int i = currentSentence; i < t.Sentence; i++)
                        sum += data.Sentenses[i].Length;
                    currentSentence = t.Sentence;
                    offset += sum;
                    //curent = 0;
                }

                yield return new Run(text.Substring(curent, offset + t.Offset - curent));
                //yield return new Run(text.Substring(curent, offset + curent - t.Offset ));

                yield return new Run(text.Substring(offset + t.Offset, t.Length))
                {
                    TextDecorations = MyTextDecorations.GetDecorationFromType(t.Type)
                };

                //curent = offset + t.Offset + t.Length;
                curent = t.Offset + t.Length;
            }
            yield return new Run(text.Substring(curent));
        }
    }
}