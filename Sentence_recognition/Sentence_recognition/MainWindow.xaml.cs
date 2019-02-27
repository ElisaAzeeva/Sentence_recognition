using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        #region Properties

        public bool IsTextMode
        {
            get { return (bool)GetValue(IsTextModeProperty); }
            set { SetValue(IsTextModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTextMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTextModeProperty =
            DependencyProperty.Register("IsTextMode", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));

        #endregion

        private RecognitionAPI recognazer;

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
                block.Inlines.Clear();
                var (errorCode, d) = recognazer.GetData(openFileDialog.FileName);
                block.Inlines.AddRange(RecognitionAPI.GetRuns(d));
            }
        }
    }
}