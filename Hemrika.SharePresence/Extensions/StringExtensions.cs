// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Reflection;
    using System.Net;
    using System.Web;

    public static class StringExtensions
    {

        /// <summary>
        /// Returns true if the string is null or empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }


        /// <summary>
        /// Returns true if the string is NOT null or empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }


        /// <summary>
        /// Returns the same value back if string is not null or empty, otherwise return the default value provided.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string IfNullOrEmptyThenDefault(this string str, string defaultValue)
        {
            return str.IsNotNullOrEmpty() ? str : defaultValue;
        }


        /// <summary>
        /// Returns N/A if the string is null or empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string IfNullOrEmptyThenNa(this string str)
        {
            return str.IsNotNullOrEmpty() ? str : "N/A";
        }


        /// <summary>
        /// If the string provided exceeds the length provided then the string will be truncated.
        /// Optionally the truncated string can have an ellipsis appended to indicate that the string has
        /// been truncated.
        /// </summary>
        /// <param name="str">The string to truncate. If it's null or empty then the same string is returned.</param>
        /// <param name="maxLength">The maximum length allowed. May not be less than 1.</param>
        /// <returns></returns>
        public static string Truncate(this string str, int maxLength)
        {
            return str.Truncate(maxLength, true);
        }


        /// <summary>
        /// If the string provided exceeds the length provided then the string will be truncated.
        /// Optionally the truncated string can have an ellipsis appended to indicate that the string has
        /// been truncated.
        /// </summary>
        /// <param name="str">The string to truncate. If it's null or empty then the same string is returned.</param>
        /// <param name="maxLength">The maximum length allowed. May not be less than 1.</param>
        /// <param name="includeEllipsis">If true then if the string is truncated it will have an ellipsis appended</param>
        /// <returns></returns>
        public static string Truncate(this string str, int maxLength, bool includeEllipsis)
        {
            if (String.IsNullOrEmpty(str)) return str;
            if (maxLength < 1) throw new ArgumentException("maxLength may not be less than 1", "maxLength");


            var length = str.Length;
            var truncated = str.Substring(0, (length > maxLength ? maxLength : length));


            if (includeEllipsis && !(str.Length <= maxLength))
                truncated = String.Concat(truncated.Trim(), "...");


            return truncated;
        }


        /// <summary>
        /// Remove extra white spaces between the text.
        /// Example "This is  some    Text" will be "This is some Text".
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns></returns>
        public static string RemoveExtraSpaces(this string str)
        {
            var trimmer = new Regex(@"\s\s+");
            return trimmer.Replace(str.Replace("&nbsp;", " "), " ");
        }


        /// <summary>
        /// Used when we want to completely remove HTML code and not encode it with XML entities.
        /// </summary>
        /// <param name="input">Input string to remove HTML from</param>
        /// <returns></returns>
        public static string StripHtml(this string input)
        {
            // Will this simple expression replace all tags???
            var tagsExpression = new Regex(@"</?.+?>");//<.*?>
            return tagsExpression.Replace(input, "");
        }


        /// <summary>
        /// Get image src links from a string
        /// </summary>
        /// <param name="htmlSource">Input string</param>
        /// <returns></returns>
        public static string FetchFirstImgLinkFromHtmlSource(this string htmlSource)
        {
            //List<string> links = new List<string>();
            string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in matchesImgSrc)
            {
                string href = m.Groups[1].Value;
                return href;
                //links.Add(href);
            }
            return "";
        }

        /// <summary>
        /// Get the string slice between the two indexes.
        /// Inclusive for start index, exclusive for end index.
        /// </summary>
        public static string Slice(this string source, int start, int end)
        {
            // Handles negative ends
            if (end < 0)
            {
                end = source.Length + end;
            }


            int length = end - start;

            return source.Substring(start, length);
        }

        /// <summary>
        /// Gets all indexes of a given substring for a string
        /// </summary>
        /// <param name="value">The substring to find</param>
        /// <returns>A list of all indicies for <paramref name="value"/></returns>
        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("The string to find must not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }


        /// <summary>
        /// Gets all indexes of a given substring for a string, ignoring case if ignoreCase.
        /// </summary>
        /// <param name="value">The substring to find</param>
        /// <param name="ignoreCase">If true, ignore case during search</param>
        /// <returns>A list of all indicies for <paramref name="value"/></returns>
        public static List<int> AllIndexesOf(this string str, string value, bool ignoreCase)
        {
            if (!ignoreCase)
            {
                return AllIndexesOf(str, value);
            }
            else
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("The string to find must not be empty", "value");
                List<int> indexes = new List<int>();
                for (int index = 0; ; index += value.Length)
                {
                    index = str.IndexOf(value, index, StringComparison.CurrentCultureIgnoreCase);
                    if (index == -1)
                        return indexes;
                    indexes.Add(index);
                }
            }
        }

        public static string UrlEncode(this string data)
        {
            return HttpUtility.UrlEncode(data);
        }


        public static string UrlDecode(this string data)
        {
            return HttpUtility.UrlDecode(data);
        }


        public static string[] Split(this string data, string separator)
        {
            return data == null ? new string[] { } : data.Split(separator.ToCharArray());
        }

        public static string ToCsv(this IEnumerable<string> strings)
        {
            if (strings == null)
                return string.Empty;


            return string.Join(",", strings.ToArray());
        }


        public static string ToPipeDelimitedString(this IEnumerable<string> strings)
        {
            if (strings == null)
                return string.Empty;


            return string.Join("|", strings.ToArray());
        }

        /*
        /// <summary>
        /// Returns true if string is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Returns true if string is null, empty or only whitespaces.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string s)
        {
            return s.IsNullOrEmpty();
            //return string.IsNullOrWhiteSpace(s);
        }
        */
        
        /// <summary>
        /// Returns specified value if string is null/empty/whitespace else same string.
        /// </summary>
        public static string Or(this string s, string or)
        {
            if (!s.IsNullOrEmpty())
            {
                return s;
            }
            return or;
        }
         
        /// <summary>
        /// Returns empty if string is null/empty/whitespace else same string.
        /// </summary>
        public static string OrEmpty(this string s)
        {
            return s.Or("");
        }
        /// <summary>
        /// Returns html-encoded string.
        /// </summary>
        public static string HtmlEncode(this string s)
        {
            return HttpUtility.HtmlEncode(s);
        }
        /// <summary>
        /// Returns html-decoded string.
        /// </summary>
        public static string HtmlDecode(this string s)
        {
            return HttpUtility.HtmlDecode(s);
        }
        /*
        /// <summary>
        /// Returns url-encoded string.
        /// </summary>
        public static string UrlEncode(this string s)
        {
            return HttpUtility.UrlEncode(s);
        }
        /// <summary>
        /// Returns url-decoded string.
        /// </summary>
        public static string UrlDecode(this string s)
        {
            return HttpUtility.UrlDecode(s);
        }
        */

        /// <summary>
        /// Format string.
        /// </summary>
        public static string format(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static string RemoveTags(this string content)
        {
            /*
            if (!string.IsNullOrWhiteSpace(content))
                return Regex.Replace(content, "<[^>]*>", string.Empty);
            */
            return string.Empty;
        }

        public static string CutContent(this string content, int bound, string appendix = "")
        {
            if (bound < 0)
                throw new ArgumentException("Bound should be positive");

            if (string.IsNullOrEmpty(content))
                return string.Empty;

            if (content.Length <= bound)
            {
                return content;
            }

            var stringBuilder = new StringBuilder();

            string firstPart = content.Substring(0, bound);
            string[] firstPartWords = firstPart.Split(' ');

            if (firstPartWords.Length == 1)
            {
                stringBuilder.Append(content);
            }
            else if (firstPartWords.Length == 2)
            {
                stringBuilder.Append(firstPartWords[0]);
            }
            else if (firstPartWords.Length > 2)
            {
                stringBuilder.Append(firstPartWords[0] + " ");

                for (var i = 1; i < firstPartWords.Length - 2; i++)
                    stringBuilder.Append(firstPartWords[i] + " ");

                stringBuilder.Append(firstPartWords[firstPartWords.Length - 2]);
            }

            var result = stringBuilder.ToString();

            var badEndSimbols = new char[] { ' ', '.', ',', ':' };

            return string.Format("{0}{1}", result.TrimEnd(badEndSimbols), appendix);
        }

        public static string Reverse(this string input)
        {
            /*
            if (string.IsNullOrWhiteSpace(input))
                return input;
            */
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static bool EqualsInvariant(this string leftValue, string rightValue)
        {
            return leftValue.Equals(rightValue, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool TryAnyEquals(this string value, out int index,
        StringComparison comparison, params string[] against)
        {
            for (int i = 0; i < against.Length; i++)
            {
                if (value.Equals(against[i], comparison))
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }
        public static bool AnyEquals(this string value, StringComparison comparison, params string[] against)
        {
            int i;
            return TryAnyEquals(value, out i, comparison, against);
        }

        public static bool EqualsIgnoreCase(this string value, string other)
        {
            return value.Equals(other, StringComparison.OrdinalIgnoreCase);
        }
        public static bool AnyEqualsIgnoreCase(this string value, params string[] other)
        {
            return AnyEquals(value, StringComparison.OrdinalIgnoreCase, other);
        }
        public static bool TryAnyEqualsIgnoreCase(this string value, out int index, params string[] others)
        {
            return TryAnyEquals(value, out index, StringComparison.OrdinalIgnoreCase, others);
        }

        public static string ShortenString(this String text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (maxLength <= 3 | text.Length <= maxLength) return text;

            try
            {
                var mitte = Math.Floor((text.Length) * 0.75);

                var ueberfluessig = text.Length - maxLength + 3;

                var vorn = text.Substring(0, (int)(mitte - (ueberfluessig * 0.75)));
                var hinten = text.Substring((int)(mitte + (ueberfluessig * 0.25)));

                text = vorn + "..." + hinten;
            }
            catch (Exception)
            {
                return text.Substring(0, maxLength - 3) + "...";
            }
            return text;
        }

        public static string RemoveLastSeperator(this String text)
        {
            if (text == null) return null;
            if (string.IsNullOrEmpty(text)) return string.Empty;

            text = text.TrimEnd(' ');

            while (text.EndsWith(",") |
                   text.EndsWith(";") |
                   text.EndsWith("|") |
                   text.EndsWith("@") |
                   text.EndsWith("#") |
                   text.EndsWith("+") |
                   text.EndsWith("*") |
                   text.EndsWith("-") |
                   text.EndsWith("_"))
            {
                text = text.Substring(0, text.Length - 1);
            }
            return text;
        }

        /*
        /// <summary>
        /// Im CF gibt es kein tyrParse. Also hier eine eigene Variante.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value)
        {
            try
            {
                float.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        */

        /// <summary>
        /// Im CF gibt es kein tryParse. Macht ein Integer aus einem Sring, wenn möglich.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int TryToInt(this string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Gibt nach Übergabe eines Strings im Format "31.01.2004 12:00:00" das passende Date-Time Objekt zurück.
        /// </summary>
        /// <param name="date">String im Format 31.01.2004 12:00:00</param>
        /// <returns>DateTime Objekt</returns>
        public static DateTime GetDateTime(this string date)
        {
            try
            {
                IFormatProvider format = new CultureInfo("de-DE", true);
                return DateTime.Parse(date, format, DateTimeStyles.NoCurrentDateDefault);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static string GetLast(this string source, int tailLength)
        {
            if (tailLength >= source.Length)
                return source;
            return source.Substring(source.Length - tailLength);
        }

        public static string RemoveLast(this string source, int removeCharacters)
        {
            if (source.Length >= removeCharacters)
                return source.Substring(0, source.Length - removeCharacters);
            return source;
        }

        public static string RemoveFirst(this string source, int removeCharacters)
        {
            if (source.Length >= removeCharacters)
                return source.Substring(removeCharacters);
            return source;
        }

        public static string FillUpFirstCharacters(this string source, int totalLength, char fillUpWith)
        {
            if (source == null) source = string.Empty;
            while (source.Length < totalLength) source = fillUpWith + source;
            return source;
        }

        public static string FillUpLastCharacters(this string source, int totalLength, char fillUpWith)
        {
            if (source == null) source = string.Empty;
            while (source.Length < totalLength) source = source + fillUpWith;
            return source;
        }

        public static bool Matches(this string source, string compare)
        {
            return String.Equals(source, compare, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool MatchesTrimmed(this string source, string compare)
        {
            return String.Equals(source.Trim(), compare.Trim(), StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool MatchesRegex(this string inputString, string matchPattern)
        {
            return Regex.IsMatch(inputString, matchPattern,
                RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }

        /// <summary>
        /// Strips the last specified chars from a string.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <param name="removeFromEnd">The remove from end.</param>
        /// <returns></returns>
        public static string Chop(this string sourceString, int removeFromEnd)
        {
            string result = sourceString;
            if ((removeFromEnd > 0) && (sourceString.Length > removeFromEnd - 1))
                result = result.Remove(sourceString.Length - removeFromEnd, removeFromEnd);
            return result;
        }

        /// <summary>
        /// Strips the last specified chars from a string.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <param name="backDownTo">The back down to.</param>
        /// <returns></returns>
        public static string Chop(this string sourceString, string backDownTo)
        {
            int removeDownTo = sourceString.LastIndexOf(backDownTo);
            int removeFromEnd = 0;
            if (removeDownTo > 0)
                removeFromEnd = sourceString.Length - removeDownTo;

            string result = sourceString;

            if (sourceString.Length > removeFromEnd - 1)
                result = result.Remove(removeDownTo, removeFromEnd);

            return result;
        }

        /// <summary>
        /// Plurals to singular.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <returns></returns>
        public static string PluralToSingular(this string sourceString)
        {
            return sourceString.MakeSingular();
        }

        /// <summary>
        /// Singulars to plural.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <returns></returns>
        public static string SingularToPlural(this string sourceString)
        {
            return sourceString.MakePlural();
        }

        /// <summary>
        /// Make plural when count is not one
        /// </summary>
        /// <param name="number">The number of things</param>
        /// <param name="sourceString">The source string.</param>
        /// <returns></returns>
        public static string Pluralize(this int number, string sourceString)
        {
            if (number == 1)
                return String.Concat(number, " ", sourceString.MakeSingular());
            return String.Concat(number, " ", sourceString.MakePlural());
        }

        /// <summary>
        /// Removes the specified chars from the beginning of a string.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <param name="removeFromBeginning">The remove from beginning.</param>
        /// <returns></returns>
        public static string Clip(this string sourceString, int removeFromBeginning)
        {
            string result = sourceString;
            if (sourceString.Length > removeFromBeginning)
                result = result.Remove(0, removeFromBeginning);
            return result;
        }

        /// <summary>
        /// Removes chars from the beginning of a string, up to the specified string
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <param name="removeUpTo">The remove up to.</param>
        /// <returns></returns>
        public static string Clip(this string sourceString, string removeUpTo)
        {
            int removeFromBeginning = sourceString.IndexOf(removeUpTo);
            string result = sourceString;

            if (sourceString.Length > removeFromBeginning && removeFromBeginning > 0)
                result = result.Remove(0, removeFromBeginning);

            return result;
        }

        /// <summary>
        /// Strips the last char from a a string.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <returns></returns>
        public static string Chop(this string sourceString)
        {
            return Chop(sourceString, 1);
        }

        /// <summary>
        /// Strips the last char from a a string.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <returns></returns>
        public static string Clip(this string sourceString)
        {
            return Clip(sourceString, 1);
        }

        /// <summary>
        /// Fasts the replace.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static string FastReplace(this string original, string pattern, string replacement)
        {
            return FastReplace(original, pattern, replacement, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Fasts the replace.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="replacement">The replacement.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns></returns>
        public static string FastReplace(this string original, string pattern, string replacement,
                                            StringComparison comparisonType)
        {
            if (original == null)
                return null;

            if (String.IsNullOrEmpty(pattern))
                return original;

            int lenPattern = pattern.Length;
            int idxPattern = -1;
            int idxLast = 0;

            StringBuilder result = new StringBuilder();

            while (true)
            {
                idxPattern = original.IndexOf(pattern, idxPattern + 1, comparisonType);

                if (idxPattern < 0)
                {
                    result.Append(original, idxLast, original.Length - idxLast);
                    break;
                }

                result.Append(original, idxLast, idxPattern - idxLast);
                result.Append(replacement);

                idxLast = idxPattern + lenPattern;
            }

            return result.ToString();
        }

        /// <summary>
        /// Returns text that is located between the startText and endText tags.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <param name="startText">The text from which to start the crop</param>
        /// <param name="endText">The endpoint of the crop</param>
        /// <returns></returns>
        public static string Crop(this string sourceString, string startText, string endText)
        {
            int startIndex = sourceString.IndexOf(startText, StringComparison.CurrentCultureIgnoreCase);
            if (startIndex == -1)
                return String.Empty;

            startIndex += startText.Length;
            int endIndex = sourceString.IndexOf(endText, startIndex, StringComparison.CurrentCultureIgnoreCase);
            if (endIndex == -1)
                return String.Empty;

            return sourceString.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// Removes excess white space in a string.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <returns></returns>
        public static string Squeeze(this string sourceString)
        {
            char[] delim = { ' ' };
            string[] lines = sourceString.Split(delim, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            foreach (string s in lines)
            {
                if (!String.IsNullOrEmpty(s.Trim()))
                    sb.Append(s + " ");
            }
            //remove the last pipe
            string result = Chop(sb.ToString());
            return result.Trim();
        }

        /// <summary>
        /// Removes all non-alpha numeric characters in a string
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <returns></returns>
        public static string ToAlphaNumericOnly(this string sourceString)
        {
            return Regex.Replace(sourceString, @"\W*", "");
        }

        /// <summary>
        /// Creates a string array based on the words in a sentence
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <returns></returns>
        public static string[] ToWords(this string sourceString)
        {
            string result = sourceString.Trim();
            return result.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Strips all HTML tags from a string
        /// </summary>
        /// <param name="htmlString">The HTML string.</param>
        /// <returns></returns>
        public static string StripHTML(this string htmlString)
        {
            return StripHTML(htmlString, String.Empty);
        }

        /// <summary>
        /// Strips all HTML tags from a string and replaces the tags with the specified replacement
        /// </summary>
        /// <param name="htmlString">The HTML string.</param>
        /// <param name="htmlPlaceHolder">The HTML place holder.</param>
        /// <returns></returns>
        public static string StripHTML(this string htmlString, string htmlPlaceHolder)
        {
            const string pattern = @"<(.|\n)*?>";
            string sOut = Regex.Replace(htmlString, pattern, htmlPlaceHolder);
            sOut = sOut.Replace("&nbsp;", String.Empty);
            sOut = sOut.Replace("&amp;", "&");
            sOut = sOut.Replace("&gt;", ">");
            sOut = sOut.Replace("&lt;", "<");
            return sOut;
        }

        public static List<string> FindMatches(this string source, string find)
        {
            Regex reg = new Regex(find, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

            List<string> result = new List<string>();
            foreach (Match m in reg.Matches(source))
                result.Add(m.Value);
            return result;
        }

        /// <summary>
        /// Converts a generic List collection to a single comma-delimitted string.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static string ToDelimitedList(this IEnumerable<string> list)
        {
            return ToDelimitedList(list, ",");
        }

        /// <summary>
        /// Converts a generic List collection to a single string using the specified delimitter.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string ToDelimitedList(this IEnumerable<string> list, string delimiter)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in list)
                sb.Append(String.Concat(s, delimiter));
            string result = sb.ToString();
            result = Chop(result);
            return result;
        }

        /// <summary>
        /// Strips the specified input.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <param name="stripValue">The strip value.</param>
        /// <returns></returns>
        public static string Strip(this string sourceString, string stripValue)
        {
            if (!String.IsNullOrEmpty(stripValue))
            {
                string[] replace = stripValue.Split(new[] { ',' });
                for (int i = 0; i < replace.Length; i++)
                {
                    if (!String.IsNullOrEmpty(sourceString))
                        sourceString = Regex.Replace(sourceString, replace[i], String.Empty);
                }
            }
            return sourceString;
        }

        /// <summary>
        /// Converts ASCII encoding to Unicode
        /// </summary>
        /// <param name="asciiCode">The ASCII code.</param>
        /// <returns></returns>
        public static string AsciiToUnicode(this int asciiCode)
        {
            Encoding ascii = Encoding.UTF32;
            char c = (char)asciiCode;
            Byte[] b = ascii.GetBytes(c.ToString());
            return ascii.GetString((b));
        }



        /// <summary>
        /// Formats the args using String.Format with the target string as a format string.
        /// </summary>
        /// <param name="fmt">The format string passed to String.Format</param>
        /// <param name="args">The args passed to String.Format</param>
        /// <returns></returns>
        public static string ToFormattedString(this string fmt, params object[] args)
        {
            return String.Format(fmt, args);
        }

        /// <summary>
        /// Strings to enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value">The value.</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string Value)
        {
            T oOut = default(T);
            Type t = typeof(T);
            foreach (FieldInfo fi in t.GetFields())
            {
                if (fi.Name.Matches(Value))
                    oOut = (T)fi.GetValue(null);
            }

            return oOut;
        }

        /// <summary>
        /// Determines whether the specified eval string contains only alpha characters.
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified eval string is alpha; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlpha(this string evalString)
        {
            return !Regex.IsMatch(evalString, RegexPattern.ALPHA);
        }

        /// <summary>
        /// Determines whether the specified eval string contains only alphanumeric characters
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <returns>
        /// 	<c>true</c> if the string is alphanumeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlphaNumeric(this string evalString)
        {
            return !Regex.IsMatch(evalString, RegexPattern.ALPHA_NUMERIC);
        }

        /// <summary>
        /// Determines whether the specified eval string contains only alphanumeric characters
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <param name="allowSpaces">if set to <c>true</c> [allow spaces].</param>
        /// <returns>
        /// 	<c>true</c> if the string is alphanumeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlphaNumeric(this string evalString, bool allowSpaces)
        {
            if (allowSpaces)
                return !Regex.IsMatch(evalString, RegexPattern.ALPHA_NUMERIC_SPACE);
            return IsAlphaNumeric(evalString);
        }

        /// <summary>
        /// Determines whether the specified eval string contains only numeric characters
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <returns>
        /// 	<c>true</c> if the string is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string evalString)
        {
            return !Regex.IsMatch(evalString, RegexPattern.NUMERIC);
        }

        /// <summary>
        /// Determines whether the specified email address string is valid based on regular expression evaluation.
        /// </summary>
        /// <param name="emailAddressString">The email address string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified email address is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmail(this string emailAddressString)
        {
            return Regex.IsMatch(emailAddressString, RegexPattern.EMAIL);
        }

        /// <summary>
        /// Determines whether the specified string is lower case.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified string is lower case; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLowerCase(this string inputString)
        {
            return Regex.IsMatch(inputString, RegexPattern.LOWER_CASE);
        }

        /// <summary>
        /// Determines whether the specified string is upper case.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified string is upper case; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUpperCase(this string inputString)
        {
            return Regex.IsMatch(inputString, RegexPattern.UPPER_CASE);
        }

        /// <summary>
        /// Determines whether the specified string is a valid GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns>
        /// 	<c>true</c> if the specified string is a valid GUID; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGuid(this string guid)
        {
            return Regex.IsMatch(guid, RegexPattern.GUID);
        }

        /// <summary>
        /// Determines whether the specified string is a valid US Zip Code, using either 5 or 5+4 format.
        /// </summary>
        /// <param name="zipCode">The zip code.</param>
        /// <returns>
        /// 	<c>true</c> if it is a valid zip code; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsZIPCodeAny(this string zipCode)
        {
            return Regex.IsMatch(zipCode, RegexPattern.US_ZIPCODE_PLUS_FOUR_OPTIONAL);
        }

        /// <summary>
        /// Determines whether the specified string is a valid US Zip Code, using the 5 digit format.
        /// </summary>
        /// <param name="zipCode">The zip code.</param>
        /// <returns>
        /// 	<c>true</c> if it is a valid zip code; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsZIPCodeFive(this string zipCode)
        {
            return Regex.IsMatch(zipCode, RegexPattern.US_ZIPCODE);
        }

        /// <summary>
        /// Determines whether the specified string is a valid US Zip Code, using the 5+4 format.
        /// </summary>
        /// <param name="zipCode">The zip code.</param>
        /// <returns>
        /// 	<c>true</c> if it is a valid zip code; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsZIPCodeFivePlusFour(this string zipCode)
        {
            return Regex.IsMatch(zipCode, RegexPattern.US_ZIPCODE_PLUS_FOUR);
        }

        /// <summary>
        /// Determines whether the specified string is a valid Social Security number. Dashes are optional.
        /// </summary>
        /// <param name="socialSecurityNumber">The Social Security Number</param>
        /// <returns>
        /// 	<c>true</c> if it is a valid Social Security number; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSocialSecurityNumber(this string socialSecurityNumber)
        {
            return Regex.IsMatch(socialSecurityNumber, RegexPattern.SOCIAL_SECURITY);
        }

        /// <summary>
        /// Determines whether the specified string is a valid IP address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>
        /// 	<c>true</c> if valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIPAddress(this string ipAddress)
        {
            return Regex.IsMatch(ipAddress, RegexPattern.IP_ADDRESS);
        }

        /// <summary>
        /// Determines whether the specified string is a valid US phone number using the referenced regex string.
        /// </summary>
        /// <param name="telephoneNumber">The telephone number.</param>
        /// <returns>
        /// 	<c>true</c> if valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUSTelephoneNumber(this string telephoneNumber)
        {
            return Regex.IsMatch(telephoneNumber, RegexPattern.US_TELEPHONE);
        }

        /// <summary>
        /// Determines whether the specified string is a valid currency string using the referenced regex string.
        /// </summary>
        /// <param name="currency">The currency string.</param>
        /// <returns>
        /// 	<c>true</c> if valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUSCurrency(this string currency)
        {
            return Regex.IsMatch(currency, RegexPattern.US_CURRENCY);
        }

        /// <summary>
        /// Determines whether the specified string is a valid URL string using the referenced regex string.
        /// </summary>
        /// <param name="url">The URL string.</param>
        /// <returns>
        /// 	<c>true</c> if valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsURL(this string url)
        {
            return Regex.IsMatch(url, RegexPattern.URL);
        }

        /// <summary>
        /// Determines whether the specified string is consider a strong password based on the supplied string.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        /// 	<c>true</c> if strong; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStrongPassword(this string password)
        {
            return Regex.IsMatch(password, RegexPattern.STRONG_PASSWORD);
        }

    }
}
