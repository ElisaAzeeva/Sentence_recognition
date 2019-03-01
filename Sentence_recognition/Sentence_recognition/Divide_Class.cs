using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentence_recognition
{
    class Divide_Class
    {
        public List<string> divide_sent(string words)
        {

            List<string> split1 = new List<string>();
            string[] split = words.Split(new Char[] { ' ', ',', '.', ':', '\t', '(', ')' });

            foreach (string s in split)
            {

                if (s.Trim() != "")
                    split1.Add(s);

            }
            return split1;
        }
        public List<string> divide_text(string words)
        {

            List<string> split1 = new List<string>();
            string[] split = words.Split(new Char[] { '!', '?', '.' });

            foreach (string s in split)
            {

                if (s.Trim() != "")
                    split1.Add(s);

            }
            return split1;
        }
    }
}
