﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public MainWindow()
        {
            InitializeComponent();
        }
        public static bool IsLinux {
            get {
#if NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0
            // https://github.com/dotnet/corefx/blob/master/src/System.Runtime.InteropServices.RuntimeInformation/ref/System.Runtime.InteropServices.RuntimeInformation.cs
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
#else
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
#endif
            }

        }
        List<int> sv9Iz = new List<int>();
        List<string> slova = new List<string>();
        List<int> SOS = new List<int>();
        IntPtr hEngine;
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Divide_Class ll = new Divide_Class();
            int nomer = 0;
            List<string> split1 = new List<string>();
            List<string> split2 = new List<string>();
            split1 = ll.divide_text("Из молодежи, не считая старшей дочери графини (которая была четырьмя годами старше сестры и держала себя уже как большая) и гостьи-барышни, в гостиной остались Николай и Соня-племянница. Соня была тоненькая, миниатюрненькая брюнетка с мягким, отененным длинными ресницами взглядом, густою черною косою, два раза обвивавшею ее голову, и желтоватым оттенком кожи на лице и в особенности на обнаженных худощавых, но грациозных мускулистых руках и шее. Плавностью движений, мягкостью и гибкостью маленьких членов и несколько хитрою и сдержанною манерой она напоминала красивого, но еще не сформировавшегося котенка, который будет прелестною кошечкой. Она, видимо, считала приличным выказывать улыбкой участие к общему разговору; но против воли ее глаза из-под длинных густых ресниц смотрели на уезжающего в армию cousin с таким девическим страстным обожанием, что улыбка ее не могла ни на мгновение обмануть никого, и видно было, что кошечка присела только для того, чтоб еще энергичнее прыгнуть и заиграть с своим cousin, как скоро только они так же, как Борис с Наташей, выберутся из этой гостиной.");
            split2 = ll.divide_sent(split1[nomer], nomer);
             hEngine = GrammarEngine.sol_CreateGrammarEngineW(@"C:\bin-windows/dictionary.xml");
            if (hEngine == IntPtr.Zero)
            {
                Console.WriteLine("Could not load the dictionary");
                return;
            }
            Int32 r = GrammarEngine.sol_DictionaryVersion(hEngine);
            if (r != -1)
            {
                Console.WriteLine("Словарь загружен. Версия: " + r.ToString());
            }
            else
            {
                Console.WriteLine("Ошибка загрузки словаря.");
                return;
            }


            // Проверим, что там есть русский лексикон.
            int ie1 = GrammarEngine.sol_FindEntry(hEngine, "МАМА", SolarixGrammarEngineNET.GrammarEngineAPI.NOUN_ru, SolarixGrammarEngineNET.GrammarEngineAPI.RUSSIAN_LANGUAGE);
            if (ie1 == -1)
            {
                Console.WriteLine("Russian language is missing in lexicon.");
                return;
            }
            IntPtr hPack11 = GrammarEngine.sol_SyntaxAnalysis(hEngine, "Красивая чайка села на дерево и улетела ", GrammarEngine.MorphologyFlags.SOL_GREN_ALLOW_FUZZY, 0, (60000 | (20 << 22)), GrammarEngineAPI.RUSSIAN_LANGUAGE);
            //IntPtr hPack1 = GrammarEngine.sol_MorphologyAnalysis(hEngine, "Красивая чайка села на дерево и улетела", GrammarEngine.MorphologyFlags.SOL_GREN_ALLOW_FUZZY, 0, (60000 | (20 << 22)), GrammarEngineAPI.RUSSIAN_LANGUAGE);
            int fffff = GrammarEngine.sol_CountGrafs(hPack11);
            int nroot = GrammarEngine.sol_CountRoots(hPack11, 0);
            //  IntPtr hRoot = GrammarEngine.sol_GetRoot(hPack11, 0, iroot);
            // string fff = GrammarEngine.sol_GetNodeContentsFX(hRoot);


            //////ТУТ ДЕРЕВО И КАК ТО НАДО ВСЕ ЛИСТЬЯ ПОЛУЧИТЬ/////////////////////
            for (int iroot = 1; iroot < nroot - 1; ++iroot)
            {
                IntPtr hRoot = GrammarEngine.sol_GetRoot(hPack11, 0, iroot);
                string fff = GrammarEngine.sol_GetNodeContentsFX(hRoot);
                int broot = GrammarEngine.sol_CountLeafs(hRoot);
             
                        Razbor(hRoot);
                    
                
                GrammarEngine.sol_DeleteResPack(hPack11);

            }
        }
        private void Razbor(IntPtr rf)
        {
            int broofft = GrammarEngine.sol_CountLeafs(rf);// Количество листьев
            for (int Leaff = 0; Leaff < broofft; Leaff++)
            {
                //НЕЕЕЕЕ ЛЕЕЕЕЗЬЬЬЬ 
                IntPtr frf = GrammarEngine.sol_GetLeaf(rf, Leaff);//Указатель на текущий лист
                int afafaf = GrammarEngine.sol_GetNodeIEntry(hEngine, frf);//Первичный ключ словарной статьи(для определения части речи)
                SOS.Add(GrammarEngine.sol_GetEntryClass(hEngine, afafaf));// id части речи
                slova.Add(GrammarEngine.sol_GetNodeContentsFX(frf)); //Слова
                sv9Iz.Add(GrammarEngine.sol_GetLeafLinkType(rf, Leaff));//Связи листа и его корня (предыдущего листа)
                int broofftt = GrammarEngine.sol_CountLeafs(frf);//Количество листьев у текущего листа
                if (broofftt>0)
                {
                    Razbor(frf);//Рекурсия
                }
            }
        }
    }
}

