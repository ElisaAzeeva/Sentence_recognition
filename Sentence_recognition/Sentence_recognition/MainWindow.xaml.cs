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
            letter_v.AddRange(new List<word> {
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
            letter_d.AddRange(new List<word> {
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
            //Добавление слов на букву Е
            List<word> letter_e = new List<word>();
            letter_e.AddRange(new List<word> {
                                 new word("Е", "", true, false), //для лучшего поиска
                                 new word("его", "местоимение", false, true),
                                 new word("ее", "местоимение", false, true),
                                 new word("едва", "союз", false, false),
                                 new word("ежели", "союз", false, false),
                                 new word("если", "союз", false, false),
                                 new word("если бы", "союз", false, false),
                                 new word("единственно", "частица", false, false),
                                 new word("едва ли", "частица", false, false),
                               });
            //Добавление слов на букву Ж
            List<word> letter_gg = new List<word>();
            letter_gg.AddRange(new List<word> {
                                 new word("Ж", "", true, false), //для лучшего поиска
                                 new word("же", "частица", false, false),
                               });
            //Добавление слов на букву З
            List<word> letter_z = new List<word>();
            letter_z.AddRange(new List<word> {
                                 new word("З", "", true, false), //для лучшего поиска
                                 new word("здесь", "местоимение", false, false),
                                 new word("зачем", "местоимение", false, false),
                                 new word("зачем-то", "местоимение", false, false),
                                 new word("затем чтобы", "союз", false, false),
                                 new word("затем что", "союз", false, false),
                                 new word("зато", "союз", false, false),
                                 new word("зачем", "союз", false, false),
                               });
            //Добавление слов на букву И
            List<word> letter_i = new List<word>();
            letter_i.AddRange(new List<word> {
                                 new word("И", "", true, false), //для лучшего поиска
                                 new word("их", "местоимение", false, true),
                                 new word("и", "союз", false, false),
                                 new word("и все же", "союз", false, false),
                                 new word("и значит", "союз", false, false),
                                 new word("и поэтому", "союз", false, false),
                                 new word("и притом", "союз", false, false),
                                 new word("и все-таки", "союз", false, false),
                                 new word("и следовательно", "союз", false, false),
                                 new word("и то", "союз", false, false),
                                 new word("и тогда", "союз", false, false),
                                 new word("и еще", "союз", false, false),
                                 new word("ибо", "союз", false, false),
                                 new word("и вдобавок", "союз", false, false),
                                 new word("из-за того что", "союз", false, false),
                                 new word("или", "союз", false, false),
                                 new word("из", "предлог", false, false),
                                 new word("именно", "частица", false, false),
                                 new word("исключительно", "частица", false, false),
                               });
            //Добавление слов на букву К
            List<word> letter_k = new List<word>();
            letter_k.AddRange(new List<word> {
                                 new word("К", "", true, false), //для лучшего поиска
                                 new word("как", "местоимение", false, false),
                                 new word("который", "местоимение", false, false),
                                 new word("когда", "местоимение", false, false),
                                 new word("кто", "местоимение", false, false),
                                 new word("какой", "местоимение", false, false),
                                 new word("каждый", "местоимение", false, false),
                                 new word("какой-то", "местоимение", false, false),
                                 new word("куда", "местоимение", false, false),
                                 new word("кто-то", "местоимение", false, false),
                                 new word("как-то", "местоимение", false, false),
                                 new word("какой-нибудь", "местоимение", false, false),
                                 new word("когда-то", "местоимение", false, false),
                                 new word("кто-нибудь", "местоимение", false, false),
                                 new word("какой-либо", "местоимение", false, false),
                                 new word("куда-то", "местоимение", false, false),
                                 new word("каков", "местоимение", false, false),
                                 new word("кой", "местоимение", false, false),
                                 new word("кое-что", "местоимение", false, false),
                                 new word("когда-нибудь", "местоимение", false, false),
                                 new word("как-нибудь", "местоимение", false, false),
                                 new word("куда-нибудь", "местоимение", false, false),
                                 new word("кое-какой", "местоимение", false, false),
                                 new word("кое-как", "местоимение", false, false),
                                 new word("кое-кто", "местоимение", false, false),
                                 new word("кое-где", "местоимение", false, false),
                                 new word("кто-либо", "местоимение", false, false),
                                 new word("каковой", "местоимение", false, false),
                                 new word("когда-либо", "местоимение", false, false),
                                 new word("кабы", "союз", false, false),
                                 new word("как", "союз", false, false),
                                 new word("как скоро", "союз", false, false),
                                 new word("как будто", "союз", false, false),
                                 new word("как если бы", "союз", false, false),
                                 new word("как словно", "союз", false, false),
                                 new word("как только", "союз", false, false),
                                 new word("как-то", "союз", false, false),
                                 new word("когда", "союз", false, false),
                                 new word("коли", "союз", false, false),
                                 new word("к тому же", "союз", false, false),
                                 new word("кроме того", "союз", false, false),
                                 new word("к", "предлог", false, false),
                                 new word("кроме", "предлог", false, false),
                                 new word("как раз", "частица", false, false),
                                 new word("как", "частица", false, false),
                               });
            //Добавление слов на букву Л
            List<word> letter_l = new List<word>();
            letter_l.AddRange(new List<word> {
                                 new word("Л", "", true, false), //для лучшего поиска
                                 new word("либо", "союз", false, false),
                                 new word("лишь", "союз", false, false),
                                 new word("лишь бы", "союз", false, false),
                                 new word("лишь только", "союз", false, false),
                                 new word("ли", "союз", false, false), //отлавливаем повторяющийся союз ли..., ли
                               });
            //Добавление слов на букву М
            List<word> letter_m = new List<word>();
            letter_m.AddRange(new List<word> {
                                 new word("М", "", true, false), //для лучшего поиска
                                 new word("мы", "местоимение", false, false),
                                 new word("мой", "местоимение", false, true),
                                 new word("между тем как", "союз", false, false),
                               });
            //Добавление слов на букву Н
            List<word> letter_n = new List<word>();
            letter_n.AddRange(new List<word> {
                                 new word("Н", "", true, false), //для лучшего поиска
                                 new word("наш", "местоимение", false, false),
                                 new word("ничто", "местоимение", false, false),
                                 new word("никто", "местоимение", false, false),
                                 new word("никогда", "местоимение", false, false),
                                 new word("никакой", "местоимение", false, false),
                                 new word("некоторый", "местоимение", false, false),
                                 new word("некий", "местоимение", false, false),
                                 new word("нечто", "местоимение", false, false),
                                 new word("никуда", "местоимение", false, false),
                                 new word("некогда", "местоимение", false, false),
                                 new word("нигде", "местоимение", false, false),
                                 new word("ничуть", "местоимение", false, false),
                                 new word("некто", "местоимение", false, false),
                                 new word("нибудь", "местоимение", false, false),
                                 new word("ничей", "местоимение", false, false),
                                 new word("ниоткуда", "местоимение", false, false),
                                 new word("никой", "местоимение", false, false),
                                 new word("нежели", "союз", false, false),
                                 new word("невзирая на то что", "союз", false, false),
                                 new word("независимо от того что", "союз", false, false),
                                 new word("несмотря на то что", "союз", false, false),
                                 new word("но", "союз", false, false),
                                 new word("на", "предлог", false, false),
                                 new word("над", "предлог", false, false),
                                 new word("навстречу", "предлог", false, false),
                                 new word("не", "частица", false, false),
                                 new word("ни", "частица", false, false),
                                 new word("неужели", "частица", false, false),
                                 new word("ну", "частица", false, false),
                               });
            //Добавление слов на букву О
            List<word> letter_o = new List<word>();
            letter_o.AddRange(new List<word> {
                                 new word("О", "", true, false), //для лучшего поиска
                                 new word("он", "местоимение", false, false),
                                 new word("они", "местоимение", false, false),
                                 new word("она", "местоимение", false, false),
                                 new word("откуда", "местоимение", false, false),
                                 new word("отсюда", "местоимение", false, false),
                                 new word("оттуда", "местоимение", false, false),
                                 new word("оттого", "местоимение", false, false),
                                 new word("отчего", "местоимение", false, false),
                                 new word("откуда-то", "местоимение", false, false),
                                 new word("отчего-то", "местоимение", false, false),
                                 new word("отовсюду", "местоимение", false, false),
                                 new word("оный", "местоимение", false, false),
                                 new word("однако", "союз", false, false),
                                 new word("особенно", "союз", false, false),
                                 new word("оттого", "союз", false, false),
                                 new word("оттого что", "союз", false, false),
                                 new word("отчего", "союз", false, false),
                                 new word("о", "предлог", false, false),
                                 new word("от", "предлог", false, false),
                                 new word("около", "предлог", false, false),
                                 new word("отнюдь не", "частица", false, false),
                                 new word("оx", "частица", false, false),
                               });
            //Добавление слов на букву П
            List<word> letter_p = new List<word>();
            letter_p.AddRange(new List<word> {
                                 new word("П", "", true, false), //для лучшего поиска
                                 new word("потом", "местоимение", false, false),
                                 new word("потому", "местоимение", false, false),
                                 new word("почему", "местоимение", false, false),
                                 new word("поэтому", "местоимение", false, false),
                                 new word("почему-то", "местоимение", false, false),
                                 new word("почем", "местоимение", false, false),
                                 new word("перед тем как", "союз", false, false),
                                 new word("по мере того как", "союз", false, false),
                                 new word("по причине того что", "союз", false, false),
                                 new word("подобно тому как", "союз", false, false),
                                 new word("пока", "союз", false, false),
                                 new word("покамест", "союз", false, false),
                                 new word("покуда", "союз", false, false),
                                 new word("пока не", "союз", false, false),
                                 new word("после того как", "союз", false, false),
                                 new word("поскольку", "союз", false, false),
                                 new word("потому", "союз", false, false),
                                 new word("потому что", "союз", false, false),
                                 new word("почему", "союз", false, false),
                                 new word("прежде чем", "союз", false, false),
                                 new word("при всем том что", "союз", false, false),
                                 new word("при условии что", "союз", false, false),
                                 new word("притом", "союз", false, false),
                                 new word("причем", "союз", false, false),
                                 new word("пускай", "союз", false, false),
                                 new word("пусть", "союз", false, false),
                                 new word("по", "предлог", false, false),
                                 new word("под", "предлог", false, false),
                                 new word("при", "предлог", false, false),
                                 new word("перед", "предлог", false, false),
                                 new word("после", "предлог", false, false),
                                 new word("прямо", "частица", false, false),
                                 new word("почти", "частица", false, false),
                               });
            //Добавление слов на букву Р
            List<word> letter_r = new List<word>();
            letter_r.AddRange(new List<word> {
                                 new word("Р", "", true, false), //для лучшего поиска
                                 new word("ради того чтобы", "союз", false, false),
                                 new word("раз", "союз", false, false),
                                 new word("раньше чем", "союз", false, false),
                                 new word("разве", "частица", false, false),
                               });
            //Добавление слов на букву С
            List<word> letter_c = new List<word>();
            letter_c.AddRange(new List<word> {
                                 new word("С", "", true, false), //для лучшего поиска
                                 new word("свой", "местоимение", false, true),
                                 new word("себя", "местоимение", false, false),
                                 new word("самый", "местоимение", false, false),
                                 new word("сам", "местоимение", false, false),
                                 new word("свое", "местоимение", false, false),
                                 new word("сей", "местоимение", false, false),
                                 new word("сюда", "местоимение", false, false),
                                 new word("столь", "местоимение", false, false),
                                 new word("сколь", "местоимение", false, false),
                                 new word("сям", "местоимение", false, false),
                                 new word("с тем чтобы", "союз", false, false),
                                 new word("с тех пор как", "союз", false, false),
                                 new word("словно", "союз", false, false),
                                 new word("с", "предлог", false, false),
                                 new word("сквозь", "предлог", false, false),
                                 new word("согласно", "предлог", false, false),
                               });
            //Добавление слов на букву Т
            List<word> letter_t = new List<word>();
            letter_t.AddRange(new List<word> {
                                 new word("Т", "", true, false), //для лучшего поиска
                                 new word("так", "местоимение", false, false),
                                 new word("ты", "местоимение", false, false),
                                 new word("такой", "местоимение", false, true),
                                 new word("там", "местоимение", false, false),
                                 new word("тогда", "местоимение", false, false),
                                 new word("твой", "местоимение", false, true),
                                 new word("туда", "местоимение", false, false),
                                 new word("таков", "местоимение", false, true),
                                 new word("таковой", "местоимение", false, false),
                                 new word("такой-то", "местоимение", false, false),
                                 new word("тут-то", "местоимение", false, false),
                                 new word("тот-то", "местоимение", false, false),
                                 new word("тогда-то", "местоимение", false, false),
                                 new word("так же", "союз", false, false),
                                 new word("особенно", "союз", false, false),
                                 new word("оттого", "союз", false, false),
                                 new word("оттого что", "союз", false, false),
                                 new word("отчего", "союз", false, false),
                                 new word("так же", "союз", false, false),
                                 new word("так как", "союз", false, false),
                                 new word("так что", "союз", false, false),
                                 new word("также", "союз", false, false),
                                 new word("тем более что", "союз", false, false),
                                 new word("тогда как", "союз", false, false),
                                 new word("то есть", "союз", false, false),
                                 new word("тоже", "союз", false, false),
                                 new word("только", "союз", false, false),
                                 new word("только бы", "союз", false, false),
                                 new word("только что", "союз", false, false),
                                 new word("только лишь", "союз", false, false),
                                 new word("только чуть", "союз", false, false),
                                 new word("точно", "союз", false, false),
                                 new word("точь-в-точь", "частица", false, false),
                               });
            //Добавление слов на букву У
            List<word> letter_y = new List<word>();
            letter_y.AddRange(new List<word> {
                                 new word("У", "", true, false), //для лучшего поиска
                                 new word("у", "предлог", false, false),
                                 new word("уж", "частица", false, false),
                               });
            //Добавление слов на букву Х
            List<word> letter_x = new List<word>();
            letter_x.AddRange(new List<word> {
                                 new word("Х", "", true, false), //для лучшего поиска
                                 new word("хотя", "союз", false, false),
                                 new word("хотя и", "союз", false, false),
                               });
            //Добавление слов на букву Ч
            List<word> letter_ch = new List<word>();
            letter_ch.AddRange(new List<word> {
                                 new word("Ч", "", true, false), //для лучшего поиска
                                 new word("что", "местоимение", false, false),
                                 new word("что-то", "местоимение", false, false),
                                 new word("что-нибудь", "местоимение", false, false),
                                 new word("чего", "местоимение", false, false),
                                 new word("чей", "местоимение", false, false),
                                 new word("чей-то", "местоимение", false, false),
                                 new word("что-либо", "местоимение", false, false),
                                 new word("чего-то", "местоимение", false, false),
                                 new word("чей-нибудь", "местоимение", false, false),
                                 new word("чем", "союз", false, false),
                                 new word("чтоб", "союз", false, false),
                                 new word("чтобы", "союз", false, false),
                               });
            //Добавление слов на букву Э
            List<word> letter_ee = new List<word>();
            letter_ee.AddRange(new List<word> {
                                 new word("Э", "", true, false), //для лучшего поиска
                                 new word("этот", "местоимение", false, true),
                                 new word("эдакий", "местоимение", false, false),
                                 new word("экий", "местоимение", false, false),
                               });
            //Добавление слов на букву Я
            List<word> letter_ia = new List<word>();
            letter_ia.AddRange(new List<word> {
                                 new word("Я", "", true, false), //для лучшего поиска
                                 new word("я", "местоимение", false, true),
                               });
            first_letter.AddRange(new List<List<word>>{letter_a, letter_b, letter_v, letter_d, letter_e, letter_gg, letter_z, letter_i, letter_k, letter_l, letter_m, letter_n, letter_o, letter_p, letter_r, letter_c, letter_t, letter_y, letter_x, letter_ch, letter_ee, letter_ia });
        }
    }
}
