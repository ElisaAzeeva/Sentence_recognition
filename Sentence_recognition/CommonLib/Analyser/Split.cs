using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CommonLib
{
    public static class Split
    {
        public static IEnumerable<string> DivideText(this string text)
        {
            // (?<=a)b (positive lookbehind) matches the b (and only the b) in cab,
            // but does not match bed or debt.
            string pattern = "(?<=[.!?])";
            return Regex.Split(text, pattern, RegexOptions.Multiline
                | RegexOptions.CultureInvariant).ToList();
        }

        public static List<Token> DivideSentance(
            string sentence, int number_senten) {
            List<Token> chast = new List<Token>();
            MatchCollection matches = Regex.Matches(sentence, @"\p{L}+|[,.?!()]",
                RegexOptions.Multiline | RegexOptions.CultureInvariant);

            return matches
                .Cast<Match>()
                .Select(m =>  new Token() {
                        Sentence = number_senten,
                        Length = m.Length,
                        Offset = m.Index
                }).ToList();
        }
    }
}
