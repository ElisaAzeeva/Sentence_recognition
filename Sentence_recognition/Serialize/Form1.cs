using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serialize
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //ДЛЯ ПРИМЕРА 
            IntoFile into1 = new IntoFile("Пример 1");
            IntoFile into2 = new IntoFile("Пример 2");

            // массив для сериализации
            IntoFile[] into = new IntoFile[] { into1, into2 };

            BinaryFormatter formatter = new BinaryFormatter();

            //сериализация
            using (FileStream fs = new FileStream("file.dat", FileMode.OpenOrCreate))
            {
                // сериализуем весь массив into
                formatter.Serialize(fs, into);
            }

            // десериализация
            using (FileStream fs = new FileStream("file.dat", FileMode.OpenOrCreate))
            {
                IntoFile[] deserilizeInto = (IntoFile[])formatter.Deserialize(fs); //преобразуем возвращаемый из файла объект к типу OutFile

                foreach (IntoFile i in deserilizeInto)
                {
                    //пример вывода из файла
                    textBox1.Text += i.Variable+"; ";
                }
            }
        }

        //РАНДОМНАЯ СТРУКТУРА ДАННЫХ. ДЛЯ УСПЕШНОЙ СЕРИАЛИЗАЦИИ-ДЕСЕРИАЛИЗАЦИИ НАДО ИСПОЛЬЗОВАТЬ ОДНУ И ТУ ЖЕ СТРУКТУРУ ДАННЫХ
        [Serializable]
        public class IntoFile
        {
            public string Variable { get; set; }

            public IntoFile(string variable)
            {
                Variable = variable;
            }
        }


    }
}
