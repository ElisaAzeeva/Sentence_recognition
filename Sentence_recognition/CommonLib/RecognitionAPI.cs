﻿using Sentence_recognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;

using WPFRun = System.Windows.Documents.Run;

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
            Data data = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();

            string text = "";

            progress?.Report((.0, "Загрузка файла."));

            switch (Path.GetExtension(path))
            {
                case ".txt":
                    text = File.ReadAllText(path);
                    break;

                //case ".doc":
                case ".docx":
                    {
                        var wordDoc = WordprocessingDocument.Open(path, true);
                        var body = wordDoc.MainDocumentPart.Document.Body;
                        foreach (var child_element in body.ChildElements)
                        {
                            var temp = child_element.InnerText;
                            if (temp != "")
                                text += temp + "\r\n";
                        }
                    }
                    break;

                case ".analyser":
                    data = Data.Open(path);
                    return (0, data); // TODO errors

                default:
                    return (ErrorCode.UnknownFileType, null);
            }
            
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

        public static IEnumerable<WPFRun> GetRuns(Data data, SentenceMembers sm)
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

                yield return new WPFRun(text.Substring(curent, offset + t.Offset - curent));

                yield return new WPFRun(text.Substring(offset + t.Offset, t.Length))
                {
                    TextDecorations = MyTextDecorations.GetDecorationFromType(t.Type)
                };

                curent = offset + t.Offset + t.Length;
                //curent = t.Offset + t.Length;
            }
            yield return new WPFRun(text.Substring(curent));
        }
    }
}