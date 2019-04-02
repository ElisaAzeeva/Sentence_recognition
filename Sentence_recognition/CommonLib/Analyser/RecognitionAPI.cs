using Sentence_recognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;

using EPocalipse.IFilter;
using System.Reflection;

namespace CommonLib
{
    public class RecognitionAPI
    {
        private Analyser analyser;
        private dynamic dynamicDll;

        public ErrorCode Init() {

            var DLL = Assembly.LoadFile(new FileInfo(@".\DynamicLibrary.dll").FullName);
            var OpenFile = DLL.GetType("DynamicLibrary.OpenFile");
            dynamicDll = Activator.CreateInstance(OpenFile);

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

            if (Path.GetExtension(path) == ".analyser")
            {
                data = Data.Open(path);
                progress?.Report((1.0, "Файл загружен."));
                return (ErrorCode.Ok, data);
            }

            text = dynamicDll.Open(path);

            if (text==null)
                return (ErrorCode.Unknown, null);

            if (text.Trim()=="")
                return (ErrorCode.FileIsEmpty, null);
            ErrorCode e;
            (e, data) = analyser.ParsingText(text, progress);
            progress?.Report((1.0, "Файл загружен."));
            return (e, data);
        }

       
    }
}