using Sentence_recognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
//using System.Windows;
using System.Windows.Documents;
//using Microsoft.Win32;
using System.Text;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Wordprocessing;
namespace CommonLib
{
    public class RecognitionAPI
    {
        private Analyser analyser;

        public ErrorCode Init() {

            analyser = new Analyser();
            ErrorCode error;

            if ((error = analyser.Init()) != ErrorCode.Ok)
                return error;
            return ErrorCode.Ok;
        }

        // Прогресс от 0 до 1
        public (ErrorCode e, Data d) GetData(string path, IProgress<(double, string)> progress)
        {
            string text = "";

            // Путь должен проверяться на этой стороне.
            // Это может быть путь к текстовому, docx или иному 
            // другому файлу который используется программой.
            // (Или вообще какому нибудь левому файлу)

            progress?.Report((.0, "Загрузка файла."));

            if (Path.GetExtension(path) == ".txt")
                text = File.ReadAllText(path); //encoding?
            else // Если расширение файла .doc или .docx
            {
                //SaveFileDialog SaveFile = new SaveFileDialog();

                // Фильтр расширений сохраняемого файла
                //SaveFile.Filter = "Текстовый файл (*.txt)|*.txt";

                // Название SaveFileDialog'a
                //SaveFile.Title = "Выберите файл, куда хотите сохранить конвертированный текст";

                // Проверка на существующий файл с выбранным названием
                //SaveFile.OverwritePrompt = true;

                //if (SaveFile.ShowDialog() == true)
                //{
                    // Очищаем выбранный файл для сохранения
                    //File.WriteAllText(SaveFile.FileName, string.Empty);

                    // Магия
                    //WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true);
                    //Body body = wordDoc.MainDocumentPart.Document.Body;
                    //var WordText = body.ChildElements;
                    //foreach (var child_element in WordText)
                    //{
                    //    var FinalText = child_element.InnerText;
                    //    if (FinalText != "")
                    //        File.AppendAllText(SaveFile.FileName, FinalText + "\r\n");
                    //}
                //}
            }

            Data data = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();

            var sentenses = text.DivideText().ToList();

            List<Token> w = analyser.ParsingText(sentenses, progress)
                .SelectMany(x => x)
                .ToList();

            data = new Data(sentenses, w);

            return (0, data);
        }

        public static string GetText(Data data)
        {
            return data.Sentenses.Aggregate("", (s1,s2) => s1 + s2);
        }

        public static IEnumerable<Run> GetRuns(Data data, SentenceMembers sm)
        {
            int curent = 0;
            string text = GetText(data);

            int currentSentence = 0;
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

                yield return new Run(text.Substring(offset + t.Offset, t.Length))
                {
                    TextDecorations = MyTextDecorations.GetDecorationFromType(t.Type)
                };

                curent = offset + t.Offset + t.Length;
                //curent = t.Offset + t.Length;
            }
            yield return new Run(text.Substring(curent));
        }
    }
}