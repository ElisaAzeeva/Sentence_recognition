using System;
using System.Globalization;
using System.Windows.Data;

namespace CommonLib.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class CasesToStringMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            var length = (int) values[0];
            var (sentence, offset) = ((int,int))values[1];
            var data = values[2] as Data;

            return data.Sentenses[sentence].Substring(offset, length);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}