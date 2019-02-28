using System;
using System.Globalization;
using System.Windows.Data;

namespace Sentence_recognition
{
    public class TEnumValueConverter<T> : IValueConverter
        where T : struct, IConvertible
    {
        private dynamic target;

        public TEnumValueConverter()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an Enum type");
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            T mask = (T)parameter;
            target = (T)value;
            return (mask & target) != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            target ^= (T)parameter;
            return target;
        }
    }
}
