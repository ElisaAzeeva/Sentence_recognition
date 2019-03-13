using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CommonLib.Converters
{
    [ValueConversion(typeof(SentenceMembers), typeof(TextDecorationCollection))]
    public class SentenceMembersToTextDecorationCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(TextDecorationCollection))
                throw new InvalidOperationException("The target must be a TextDecorationCollection");

            return MyTextDecorations.GetDecorationFromType((SentenceMembers)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
