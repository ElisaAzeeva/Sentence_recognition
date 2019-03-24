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
            var @case = (Case)values[1];
            var sentence = @case.sentence;
            var offset = @case.offset;
            var data = values[2] as Data;

            if (parameter == null)
                return data.Sentenses[sentence].Substring(offset, length);

            var p = int.Parse((string)parameter);

            switch (p)
            {
                case 0:
                    return data.Sentenses[sentence].Substring(0, offset);
                default:
                case 1:
                    return data.Sentenses[sentence].Substring(offset, length);
                case 2:
                    return data.Sentenses[sentence].Substring(offset + length);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}