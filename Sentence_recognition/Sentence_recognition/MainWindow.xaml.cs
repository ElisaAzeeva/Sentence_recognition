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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> split1 = new List<string>();
            List<string> split2 = new List<string>();
            divide_sent("Съешь ещё этих мягких французских булок, да выпей чаю");
            split1= divide_text("Из молодежи, не считая старшей дочери графини (которая была четырьмя годами старше сестры и держала себя уже как большая) и гостьи-барышни, в гостиной остались Николай и Соня-племянница. Соня была тоненькая, миниатюрненькая брюнетка с мягким, отененным длинными ресницами взглядом, густою черною косою, два раза обвивавшею ее голову, и желтоватым оттенком кожи на лице и в особенности на обнаженных худощавых, но грациозных мускулистых руках и шее. Плавностью движений, мягкостью и гибкостью маленьких членов и несколько хитрою и сдержанною манерой она напоминала красивого, но еще не сформировавшегося котенка, который будет прелестною кошечкой. Она, видимо, считала приличным выказывать улыбкой участие к общему разговору; но против воли ее глаза из-под длинных густых ресниц смотрели на уезжающего в армию cousin с таким девическим страстным обожанием, что улыбка ее не могла ни на мгновение обмануть никого, и видно было, что кошечка присела только для того, чтоб еще энергичнее прыгнуть и заиграть с своим cousin, как скоро только они так же, как Борис с Наташей, выберутся из этой гостиной.");
            split2= divide_sent(split1[0]);
        }
        private List<string> divide_sent(string words)
        {

            List<string> split1=new List<string>();
            string[] split = words.Split(new Char[] { ' ', ',', '.', ':', '\t', '(',')' });

            foreach (string s in split)
            {

                if (s.Trim() != "")
                    split1.Add(s);
                
            }
            return split1;
        }
        private List<string> divide_text(string words)
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
