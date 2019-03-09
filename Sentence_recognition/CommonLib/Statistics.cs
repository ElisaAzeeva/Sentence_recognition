using System.Collections.Generic;

namespace CommonLib
{
    // Не знаю насколько удачная структура. Время покажет. 
    public class Statistics
    {
        public SentenceMembers Type { get; }
        public List<(int sentence, int offset)> Cases { get; }
        public int Length { get; }

        public Statistics(SentenceMembers type, List<(int sentence, int offset)> cases, int length)
        {
            Type = type;
            Cases = cases;
            Length = length;
        }
    }
}