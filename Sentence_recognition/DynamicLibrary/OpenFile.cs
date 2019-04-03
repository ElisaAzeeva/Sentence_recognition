using DocumentFormat.OpenXml.Packaging;
using EPocalipse.IFilter;
using System;
using System.IO;
using System.Linq;

namespace DynamicLibrary
{
    public class OpenFile
    {
        public string Open(string path)
        {
            try
            {
                switch (Path.GetExtension(path))
                {
                    case ".txt":
                        return File.ReadAllText(path);

                    case ".doc":
                        {
                            TextReader reader = new FilterReader(path);
                            using (reader)
                                return reader.ReadToEnd();
                        }
                    case ".docx":
                        {
                            var wordDoc = WordprocessingDocument.Open(path, true);
                            var body = wordDoc.MainDocumentPart.Document.Body;

                           return body.ChildElements
                                .Select(e => e.InnerText)
                                .Where(s => s != "")
                                .Aggregate("", (s1, s2) => s1 + s2);
                        }
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}