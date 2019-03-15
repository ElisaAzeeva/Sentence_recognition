using System.Diagnostics.Contracts;

namespace CommonLib
{
    // Здесь все понятно.
    public class Token
    {
        public int Sentence { get; set; }
        public int Offset { get; set; }
        public int Length { get; set;}
        public SentenceMembers Type { get; set; }

        public Token(int sentence, int start, int length, SentenceMembers type)
        {
            Contract.Requires(length > 0);
            Contract.Requires(Sentence >= 0);
            Contract.Requires(Offset >= 0);

            Sentence = sentence;
            Offset = start;
            Length = length;
            Type = type;
        }

        public Token()
        {
        }
    }
}