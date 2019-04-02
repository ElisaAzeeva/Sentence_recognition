using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommonLib;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sentence_recognition
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

        public MainWindow()
        {
            InitializeComponent();
            recognazer = new RecognitionAPI();
            var error = recognazer.Init();

            if (error != ErrorCode.Ok)
            {
                MessageBox.Show($"Ошибка: {error.GetEnumDescription()}.");
                Application.Current.Shutdown();
            }
        }

        private async void Open(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                // Фильтр расширений открываемых файлов
                Filter = "Файлы Word (*.doc; *.docx)|*.doc; *.docx" +
                "|Сохранённый анализ (*.analyser)|*.analyser" +
                "|Текстовые файлы (*.txt)| *.txt" +
                "|Все файлы(*)|*",

                // Название OpenFileDialog'a
                Title = "Выберите файл для чтения"
            };

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

                OpenButton.IsEnabled = false;

                var progress = new Progress<(double p,string s)>(p =>
                                Window.Dispatcher.Invoke(() => {
                                    Progress.Value = p.p;
                                    ProgressText.Text = p.s;
                                }));

                ErrorCode error;

                (error, data) = await Task.Run(() => recognazer.GetData(openFileDialog.FileName, progress));

                OpenButton.IsEnabled = true;

                if (error != ErrorCode.Ok)
                {
                    MessageBox.Show($"Ошибка: {error.GetEnumDescription()}.");
                    return;
                }

                IsFileOpen = true;

                UpdateText();
            }
        }

        void UpdateText()
        {
            if (data == null) return;
            block.Inlines.Clear();
            block.Inlines.AddRange(data.GetRuns(SentenceMembers));
            Window.DataContext = data;
            WordList.ItemsSource = data.Statistics;
        }

        private void Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (data == null) return; // TODO

            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                // Фильтр расширений открываемых файлов
                Filter = "Результаты анализа (*.analyser)|*.analyser|Все файлы(*)|*",

                // Название OpenFileDialog'a
                Title = "Выберите файл для записи"
            };

            if (openFileDialog.ShowDialog() == true)
                data.Save(openFileDialog.FileName);
        }
    }
}