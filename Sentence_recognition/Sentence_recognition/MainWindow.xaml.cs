using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;

namespace Sentence_recognition
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        public bool IsTextMode
        {
            get { return (bool)GetValue(IsTextModeProperty); }
            set { SetValue(IsTextModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTextMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTextModeProperty =
            DependencyProperty.Register("IsTextMode", typeof(bool), 
                typeof(MainWindow), new PropertyMetadata(true));

        public SentenceMembers SentenceMembers
        {
            get { return (SentenceMembers)GetValue(SentenceMembersProperty); }
            set { SetValue(SentenceMembersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SentenceMembers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SentenceMembersProperty =
            DependencyProperty.Register("SentenceMembers", typeof(SentenceMembers), 
                typeof(MainWindow), new FrameworkPropertyMetadata((SentenceMembers)0b111111, 
                    new PropertyChangedCallback(SentenceMembersUpdated)) );

        private static void SentenceMembersUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindow mainWindow)
                mainWindow.UpdateText();
        }

        #endregion

        private RecognitionAPI recognazer;
        private Data data;

        public MainWindow()
        {
            InitializeComponent();
            recognazer = new RecognitionAPI();
        }

        private void Open(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                (_, data) = recognazer.GetData(openFileDialog.FileName);
                UpdateText();
            }
        }

        void UpdateText()
        {
            if (data == null) return;
            block.Inlines.Clear();
            block.Inlines.AddRange(RecognitionAPI.GetRuns(data, SentenceMembers));
        }
    }
}