using System;
using System.ComponentModel;
using System.Reflection;

namespace CommonLib
{
    // Любые ошибки которые должны обрабатываться интерфейсом.
    // Обязательно добавляйте описание ошибки в комментариях.
    public enum ErrorCode
    {
        Ok = 0,

        [Description("Could not load the dictionary")]
        NoDictionary = 1,

        [Description("Ошибка загрузки словаря")]
        NoDictionary2 = 2,

        [Description("Russian language is missing in lexicon")]
        NoRussion = 3,
        UnknownFileType = 4
    }

    public static class EnumHelper
    {
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attributes =  fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false)
                as DescriptionAttribute[];

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }


}