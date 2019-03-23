using System;
using System.Collections.Generic;

namespace CommonLib
{
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
        public List<Token> Tokens { get; set; } // set - Temperaly

        public List<Statistics> Statistics { get; }

        public Data(List<string> sentenses, List<Token> tokens)
        {
            Sentenses = sentenses;
            Tokens = tokens;

            // TODO: DEBUG ONLY
            var sss = new List<Statistics>();

            for (int i = 0; i < 50; i++)
            {
                var cas = new List<(int,int)>();
                for (int j = 0; j < 50; j++)
                {
                    cas.Add((0, (i+j)%15));
                }                
                sss.Add(new Statistics(SentenceMembers.Addition, cas, i%10));
            }
            Statistics = sss;
        }
    }
}