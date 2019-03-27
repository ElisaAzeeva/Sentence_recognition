using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace CommonLib
{
    [Serializable]
    public class Data
    {
        public List<string> Sentenses { get; }

        // READ ME READ ME READ ME READ ME
        // !!!
        // Должно быть отсортировано по полю Start.
        // Start + Length Должна быть меньше чем Start следующего токена.
        // Ни в одном из токенов не должно быть '\r\n' или любых других переносов строк.
        // !!!
        public List<Token> Tokens { get; set; } // set - Temperaly

        public List<Statistics> Statistics { get; }

        public Data(List<string> sentenses, List<Token> tokens)
        {
            Sentenses = sentenses;
            Tokens = tokens;

            Statistics = (from t in Tokens
                          where t.Type != 0
                          group new Case(t.Sentence, t.Offset) by new {
                              word = Sentenses[t.Sentence].Substring(t.Offset, t.Length).ToLowerInvariant(),
                              type = t.Type,
                              length = t.Length
                          } into g
                          select new Statistics(g.Key.type, g.ToList(), g.Key.length)).ToList();
        }

        static BinaryFormatter formatter = new BinaryFormatter();

        public static Data Open(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
                return (Data)formatter.Deserialize(fs);
        }

        public void Save(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.CreateNew))
                formatter.Serialize(fs, this);
        }

    }
}