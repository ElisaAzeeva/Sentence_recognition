using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

using SolarixGrammarEngineNET;


namespace Sentence_recognition
{   
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] lines = File.ReadAllLines("War_and_Peace.txt");

        public MainWindow()
        {
            InitializeComponent();


            text.ItemsSource = lines;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var s = (ScrollViewer)sender;

            Debug.WriteLine($" Offset: {s.VerticalOffset}");

            Debug.WriteLine($" Height: {s.ViewportHeight}");

            Debug.WriteLine(lines[(int)s.VerticalOffset]);

        }
      
       
      
     
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            int number = 0;
            Divide_Class ff = new Divide_Class();
            ff.Dictionary(); //Загружаем словарь
            List<List<chast_rechi>> w = ff.ParsingText("Из молодежи, не считая старшей дочери графини (которая была четырьмя годами старше сестры и держала себя уже как большая) и гостьи-барышни, в гостиной остались Николай и Соня-племянница.");
            List < chast_rechi >  Result = ff.Parsing_FULL("Счастливая и озорная улыбка осветила его лицо",1);
            number = 2;
        }
  
    }
}

