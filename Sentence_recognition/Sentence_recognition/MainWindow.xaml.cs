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

                // Фильтр расширений открываемых файлов
                openFileDialog.Filter = "Файлы Word (*.doc; *.docx)| *.doc; *.docx|Текстовые файлы (*.txt)|*.txt|Все файлы(*.doc; *.docx; *.txt)|*.doc; *.docx; *.txt";

                // Начальная директория при выборе файла
                openFileDialog.InitialDirectory = @"C:\";

                // Название OpenFileDialog'a
                openFileDialog.Title = "Выберите файл для чтения";

                var progress = new Progress<double>(p => Window.Dispatcher.Invoke(() => Progress.Value = p));

                (_, data) = await Task.Run(() => recognazer.GetData(openFileDialog.FileName, progress));
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
    }
}