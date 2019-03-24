using System;
using System.Collections.Generic;

namespace CommonLib
{
    [Serializable]
    public struct Case
    {
        public int sentence;
        public int offset;

        public Case(int sentence, int offset)
        {
            this.sentence = sentence;
            this.offset = offset;
        }
    }

    // Не знаю насколько удачная структура. Время покажет. 
    [Serializable]
    public class Statistics
    {
        public SentenceMembers Type { get; }
        public List<Case> Cases { get; }
        public int Length { get; }

        public Statistics(SentenceMembers type, List<Case> cases, int length)
        {
            Type = type;
            Cases = cases;
            Length = length;
        }
    }
}