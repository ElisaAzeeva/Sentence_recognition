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
        class word
        {
            string name { get; set; } //собственно само слово
            string type { get; set; } //каким типом является слово
            bool capital { get; set; } //начинается ли слово с заглавной буквы
            bool change { get; set; } //может ли изменяться(по роду, числу...)
            public word(string Name, string Type, bool Capital, bool Change)
            {
                name = Name;
                type = Type;
                capital = Capital;
                change = Change;
            }
        };

        List<List<word>> first_letter = new List<List<word>>(); //создание списка: (начальная буква слова)->(слово, тип, начинается ли с заглавной буквы, может ли изменяться)
        public MainWindow()
        {
            InitializeComponent();
            //first_letter.AddRange(new List<List<word>> {
            //    new List<word> {
            //                     new word("А", "частица", true, false),
            //                     new word("а", "союз", false, false),

            //                   }
            //});

            //Добавление слов на букву А
            List<word> letter_a = new List<word>();
            letter_a.AddRange(new List<word> {
                                 new word("А", "", true, false), //для лучшего поиска
                                 new word("а", "союз", false, false),
                                 new word("а вдобавок", "союз", false, false),
                                 new word("а именно", "союз", false, false),
                                 new word("а также", "союз", false, false),
                                 new word("а то", "союз", false, false),
                               });
            //Добавление слов на букву Б
            List<word> letter_b = new List<word>();
            letter_b.AddRange(new List<word> {
                                 new word("Б", "", true, false), //для лучшего поиска
                                 new word("благодаря тому что", "союз", false, false),
                                 new word("благо", "союз", false, false),
                                 new word("буде", "союз", false, false),
                                 new word("будто", "союз", false, false),
                                 new word("без", "предлог", false, false),
                                 new word("близ", "предлог", false, false),
                                 new word("благодаря", "предлог", false, false),
                                 new word("бы", "частица", false, false),
                               });
            //Добавление слов на букву В
            List<word> letter_v = new List<word>();
            letter_b.AddRange(new List<word> {
                                 new word("В", "", true, false), //для лучшего поиска
                                 new word("в", "предлог", false, false),
                                 new word("весь", "местоимение", false, true),
                                 new word("вы", "местоимение", false, false),
                                 new word("вот", "местоимение", false, false),
                                 new word("всегда", "местоимение", false, false),
                                 new word("ваш", "местоимение", false, true),
                                 new word("всего", "местоимение", false, false),
                                 new word("вон", "местоимение", false, false),
                                 new word("всюду", "местоимение", false, false),
                                 new word("вдобавок", "союз", false, false),
                                 new word("в результате чего", "союз", false, false),
                                 new word("в результате того что", "союз", false, false),
                                 new word("в связи с тем что", "союз", false, false),
                                 new word("в силу того что", "союз", false, false),
                                 new word("в случае если", "союз", false, false),
                                 new word("в то время как", "союз", false, false),
                                 new word("в том случае если", "союз", false, false),
                                 new word("в силу чего", "союз", false, false),
                                 new word("ввиду того что", "союз", false, false),
                                 new word("вопреки тому что", "союз", false, false),
                                 new word("вроде того как", "союз", false, false),
                                 new word("вследствие чего", "союз", false, false),
                                 new word("вследствие того что", "союз", false, false),
                                 new word("вовсе не", "частица", false, false),
                                 new word("ведь", "частица", false, false),
                                 new word("всё-таки", "частица", false, false),
                                 new word("вряд ли", "частица", false, false),
                               });
            //Добавление слов на букву Д
            List<word> letter_d = new List<word>();
            letter_b.AddRange(new List<word> {
                                 new word("Д", "", true, false), //для лучшего поиска
                                 new word("другой", "местоимение", false, true),
                                 new word("доселе", "местоимение", false, false),
                                 new word("да вдобавок", "союз", false, false),
                                 new word("да еще", "союз", false, false),
                                 new word("да", "союз", false, false),
                                 new word("да и", "союз", false, false),
                                 new word("да и то", "союз", false, false),
                                 new word("дабы", "союз", false, false),
                                 new word("даже", "союз", false, false),
                                 new word("даром что", "союз", false, false),
                                 new word("для того чтобы", "союз", false, false),
                                 new word("до", "предлог", false, false),
                                 new word("далеко не", "частица", false, false),
                                 new word("даже", "частица", false, false),
                               });

            first_letter.AddRange(new List<List<word>>{letter_a, letter_b, letter_v, letter_d });
        }
    }
}
