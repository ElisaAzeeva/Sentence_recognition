using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sentence_recognition
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum SentenceMembers
        {
            Subject,
            Predicate,
            Definition,
            Application,
            Addition,
            Circumstance,
        }

        class Token
        {
            public int Start { get; }
            public int Length { get; }
            public SentenceMembers Type { get; set; }

            public Token(int start, int length, SentenceMembers type)
            {
                Start = start;
                Length = length;
                Type = type;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            string text = @"
test test testаа test
test test test test
";
                
            var list = new List<Token>{
                new Token(2,4,SentenceMembers.Subject),
                new Token(7,4,SentenceMembers.Predicate),
                new Token(12,6,SentenceMembers.Definition),
                new Token(19,4,SentenceMembers.Addition),
            };

            block.Inlines.Clear();

            TextDecorationCollection GetDecorationFromType(SentenceMembers type)
            {
                // TODO Add other 
                switch (type)
                {
                    case SentenceMembers.Subject:
                        return MyTextDecorations.DashDotedUnderline;
                    case SentenceMembers.Predicate:
                        return MyTextDecorations.Underline;
                    case SentenceMembers.Definition:
                        return MyTextDecorations.WavyUnderline;
                    case SentenceMembers.Circumstance:
                        //return MyTextDecorations.OverLine;
                    case SentenceMembers.Application:
                    case SentenceMembers.Addition:
                        return MyTextDecorations.DashedUnderline;
                    default:
                        return null;
                }
            }

            IEnumerable<Run> GetRuns()
            {
                int curent = 0;
                foreach (var t in list)
                {
                    yield return new Run(text.Substring(curent, t.Start - curent));
                    yield return new Run(text.Substring(t.Start, t.Length)) {
                        TextDecorations = GetDecorationFromType(t.Type)
                    };
                    curent = t.Start + t.Length;
                }
                yield return new Run(text.Substring(curent));
            }

            block.Inlines.AddRange(GetRuns());
        }
    }
}