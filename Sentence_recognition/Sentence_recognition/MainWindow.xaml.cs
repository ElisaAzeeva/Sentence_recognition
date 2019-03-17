using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommonLib;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Properties

        #region IsTextModeProperty


        public bool IsTextMode
        {
            get { return (bool)GetValue(IsTextModeProperty); }
            set { SetValue(IsTextModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTextMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTextModeProperty =
            DependencyProperty.Register("IsTextMode", typeof(bool), 
                typeof(MainWindow), new PropertyMetadata(true));

        #endregion

        #region SentenceMembersProperty

        public SentenceMembers SentenceMembers
        {
            get { return (CommonLib.SentenceMembers)GetValue(SentenceMembersProperty); }
            set { SetValue(SentenceMembersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SentenceMembers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SentenceMembersProperty =
            DependencyProperty.Register("SentenceMembers", typeof(CommonLib.SentenceMembers), 
                typeof(MainWindow), new FrameworkPropertyMetadata((CommonLib.SentenceMembers)0b111111, 
                    new PropertyChangedCallback(SentenceMembersUpdated)) );

        private static void SentenceMembersUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindow mainWindow)
                mainWindow.UpdateText();
        }

        #endregion

        #region IsFileOpenProperty

        public bool IsFileOpen
        {
            get { return (bool)GetValue(IsFileOpenProperty); }
            set { SetValue(IsFileOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFileOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFileOpenProperty =
            DependencyProperty.Register("IsFileOpen", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        #endregion

        #endregion

        private RecognitionAPI recognazer;
        private Data data;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            recognazer = new RecognitionAPI();

        }

        private async void Open(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                IsFileOpen = false;

                data = null;
                block.Inlines.Clear();
                WordList.ItemsSource = null;
                WordList.Items.Clear();
                Window.DataContext = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.WaitForFullGCComplete();


                var progress = new Progress<double>(p => Window.Dispatcher.Invoke(() => Progress.Value = p));

                (_, data) = await Task.Run(() => recognazer.GetData(openFileDialog.FileName, progress));
                Analysis();
                // TODO Error check

                IsFileOpen = true;

                UpdateText();
            }
        }

        void UpdateText()
        {
            if (data == null) return;
            block.Inlines.Clear();
            block.Inlines.AddRange(RecognitionAPI.GetRuns(data, (CommonLib.SentenceMembers)SentenceMembers));
            Window.DataContext = data;
            WordList.ItemsSource = data.Statistics;
        }

        void Analysis()
        {
            Divide_Class ff = new Divide_Class();
            ff.Dictionary(); //Загружаем словарь
            List<List<chast_rechi>> w = ff.ParsingText("Из молодежи, не считая старшей дочери графини (которая была четырьмя годами старше сестры и держала себя уже как большая) и гостьи-барышни, в гостиной остались Николай и Соня-племянница. При свидании после долгой разлуки, как это всегда бывает, разговор долго не мог установиться; они спрашивали и отвечали коротко о таких вещах, о которых они сами знали, что надо было говорить долго. Наконец разговор стал понемногу останавливаться на прежде отрывочно сказанном, на вопросах о прошедшей жизни, о планах на будущее, о путешествии Пьера, о его занятиях, о войне и т. д. красивая чайка улетела за горизонт. Та сосредоточенность и убитость, которую заметил Пьер во взгляде князя Андрея, теперь выражалась еще сильнее в улыбке, с которою он слушал Пьера, в особенности тогда, когда Пьер говорил с одушевлением радости о прошедшем или будущем. Как будто князь Андрей и желал бы, но не мог принимать участия в том, что он говорил. Пьер начинал чувствовать, что перед князем Андреем восторженность, мечты, надежды на счастие и на добро неприличны. Ему совестно было высказывать все свои новые, масонские мысли, в особенности подновленные и возбужденные в нем его последним путешествием. Он сдерживал себя, боялся быть наивным; вместе с тем ему неудержимо хотелось поскорее показать своему другу, что он был теперь совсем другой, лучший Пьер, чем тот, который был в Петербурге.");
            List<chast_rechi> Result = ff.Parsing_FULL("\r\n Счастливая и озорная улыбка осветила его лицо", 1);
            data.Tokens = Result.Select(r => r.token).ToList();
            UpdateText();
        }
    }
}