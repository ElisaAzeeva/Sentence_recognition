using System.Diagnostics.Contracts;

namespace CommonLib
{
    // Здесь все понятно.
    public class Token
    {
        public int Sentence { get; }
        public int Offset { get; }
        public int Length { get; }
        public SentenceMembers Type { get; }

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
    }
}