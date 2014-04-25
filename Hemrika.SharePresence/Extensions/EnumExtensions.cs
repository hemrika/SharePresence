// -----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Reflection;

    public static class EnumExtensions
    {
        public static bool Has<T>(this Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }


        public static bool Is<T>(this Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }


        public static T Add<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format(
                    "Could not append value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }


        public static T Remove<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format(
                    "Could not remove value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }

        /*
        /// <summary>
        /// TryParse the string to enum of type T.
        /// </summary>
        public static bool TryParse<T>(this string name, out T result, bool ignoreCase = false)
            where T : struct
        {
            return Enum.TryParse<T>(name, ignoreCase, out result);
        }
        */

        /// <summary>
        /// Parse the string to enum of type T.
        /// </summary>
        public static T Parse<T>(this string name, bool ignoreCase = false)
            where T : struct
        {
            return (T)Enum.Parse(typeof(T), name, ignoreCase);
        }


        /// <summary>
        /// Get values for type T enum.
        /// </summary>
        public static T[] GetEnumValues<T>()
            where T : struct
        {
            return EnumInternal<T>.ValueToNameMap.Select(x => x.Key).ToArray();
        }
        /// <summary>
        /// Get names (strings) for type T enum.
        /// </summary>
        public static string[] GetEnumNames<T>()
            where T : struct
        {
            return EnumInternal<T>.NameToValueMap.Select(x => x.Key).ToArray();
        }
        /// <summary>
        /// Get enum name (string) -> description (string) map for type T enum.
        /// </summary>
        public static IDictionary<string, string> GetEnumNameDescriptionMap<T>()
            where T : struct
        {
            return EnumInternal<T>.ValueToDescriptionMap
                .ToDictionary(x => GetEnumName(x.Key), x => x.Value);
        }
        /// <summary>
        /// Get the name (string) for the enum value or throw exception if not exists.
        /// </summary>
        public static string GetEnumName<T>(T enumValue)
            where T : struct
        {
            string enumName;
            if (EnumInternal<T>.ValueToNameMap.TryGetValue(enumValue, out enumName))
            {
                return enumName;
            }
            throw new ArgumentOutOfRangeException();
        }
        /// <summary>
        /// Get enum name (string) for description or throw exception if not exists.
        /// </summary>
        public static string GetEnumName<T>(this string description)
            where T : struct
        {
            T enumValue;
            if (EnumInternal<T>.DescriptionToValueMap.TryGetValue(description, out enumValue))
            {
                return GetEnumName<T>(enumValue);
            }
            throw new ArgumentOutOfRangeException();
        }
        /// <summary>
        /// Get the value for the enum name (string) or throw exception if not exists.
        /// </summary>
        public static T GetEnumValue<T>(this string name)
            where T : struct
        {
            T enumValue;
            if (EnumInternal<T>.NameToValueMap.TryGetValue(name, out enumValue))
            {
                return enumValue;
            }
            throw new ArgumentOutOfRangeException();
        }
        /// <summary>
        /// Get the value for the enum description (string) or throw exception if not exists.
        /// </summary>
        public static T GetEnumValueFromDescription<T>(this string description)
            where T : struct
        {
            T enumValue;
            if (EnumInternal<T>.DescriptionToValueMap.TryGetValue(description, out enumValue))
            {
                return enumValue;
            }
            throw new ArgumentOutOfRangeException();
        }
        /// <summary>
        /// Get the description (DescriptionAttribute) for the enum value or throw exception if not exists.
        /// </summary>
        public static string GetDescription<T>(this T enumValue)
            where T : struct
        {
            string description;
            if (EnumInternal<T>.ValueToDescriptionMap.TryGetValue(enumValue, out description))
            {
                return description;
            }
            throw new ArgumentOutOfRangeException();
        }
        /// <summary>
        /// Get enum json as name, description.
        /// </summary>
        public static string GetJson<T>()
            where T : struct
        {
            string json;
            if (EnumInternal<T>.TypeNameToJsonMap.TryGetValue(typeof(T).FullName, out json))
            {
                return json;
            }
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Get the description annotated value.
        /// </summary>
        /// <param name="value">The enum value.</param>
        /// <returns>The description annotated value.</returns>
        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /*
        /// <summary>
        /// Try to parse a string to an enum.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="e"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TEnum TryParse<TEnum>(this Enum e, String value, TEnum? defaultValue = null) where TEnum : struct
        {
            // As seen at: http://stackoverflow.com/a/12199994/439427
            TEnum tmp;
            if (!Enum.TryParse<TEnum>(value, true, out tmp))
            {
                tmp = defaultValue ?? new TEnum();
            }
            return tmp;
        }
        */
    }

    /// <summary>
    /// Enum helper class.
    /// </summary>
    /// <remarks>https://code.google.com/p/unconstrained-melody/source/browse/trunk/UnconstrainedMelody/EnumInternals.cs</remarks>
    public static class EnumInternal<T>
        where T : struct
    {
        /// <summary>
        /// enum name -> enum value
        /// </summary>
        internal static readonly IDictionary<string, T> NameToValueMap =
           new Dictionary<string, T>();
        /// <summary>
        /// enum value -> enum name
        /// </summary>
        internal static readonly IDictionary<T, string> ValueToNameMap =
           new Dictionary<T, string>();
        /// <summary>
        /// enum value -> description
        /// </summary>
        internal static readonly IDictionary<T, string> ValueToDescriptionMap =
            new Dictionary<T, string>();
        /// <summary>
        /// enum description -> value
        /// </summary>
        internal static readonly IDictionary<string, T> DescriptionToValueMap =
            new Dictionary<string, T>();
        /// <summary>
        /// enum type name -> json
        /// </summary>
        internal static readonly IDictionary<string, string> TypeNameToJsonMap =
            new Dictionary<string, string>();
        /// <summary>
        /// Build maps.
        /// </summary>
        static EnumInternal()
        {
            var buffer = new StringBuilder();
            var first = true;
            buffer.Append("{");
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                if (!first)
                {
                    buffer.Append(",");
                }
                first = false;
                var enumName = Enum.GetName(typeof(T), enumValue);
                NameToValueMap.Add(enumName, enumValue);
                ValueToNameMap.Add(enumValue, enumName);
                var description = GetDescription(enumValue);
                ValueToDescriptionMap.Add(enumValue, description);
                DescriptionToValueMap.Add(description, enumValue);
                buffer.AppendFormat("{0}{1}{2}: \"{3}\"",
                    Environment.NewLine, "\t", enumName, description);
            }
            buffer.AppendFormat("{0}}}", Environment.NewLine);
            TypeNameToJsonMap.Add(typeof(T).FullName, buffer.ToString());
        }
        /// <summary>
        /// Get description (DescriptionAttribute) for enum value or string representation if not exists.
        /// </summary>
        private static string GetDescription(T enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var description =
                field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .Select(x => x.Description)
                .SingleOrDefault();
            if (string.IsNullOrEmpty(description.ToString()))
            {
                description = enumValue.ToString();
            }
            return description.ToString();
        }
    }

}
