using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace THMang1
{
    public static class Utils
    {
        public static int CountTotalCharsExcludeWhitespace(this string source)
        {
            if (source == null)
            {
                return 0;
            }

            int count = 0;
            foreach (char c in source)
            {
                if (!char.IsWhiteSpace(c))
                {
                    count++;
                }
            }

            return count;
        }


        public static int CountWords(this string str)
        {
            // Handle null or empty strings
            if (string.IsNullOrWhiteSpace(str))
                return 0;

            // Normalize whitespace (convert tabs, newlines, etc. to single spaces)
            string normalized = Regex.Replace(str, @"\s+", " ").Trim();

            // Handle case with only whitespace characters
            if (string.IsNullOrEmpty(normalized))
                return 0;

            // Handle punctuation properly - consider words separated by punctuation
            // This treats hyphenated words as single words but splits on other punctuation
            string punctuationHandled = Regex.Replace(normalized, @"[^\w\s\-]", " ");

            // Remove standalone hyphens and normalize whitespace again
            punctuationHandled = Regex.Replace(punctuationHandled, @"\s-\s", " ");
            punctuationHandled = Regex.Replace(punctuationHandled, @"\s+", " ").Trim();

            // Handle edge case of a string with only punctuation
            if (string.IsNullOrWhiteSpace(punctuationHandled))
                return 0;

            // Split and count
            return punctuationHandled.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static bool IsPrintable(this string input)
        {
            if (input == null)
                return false;


            return input.All(c =>
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(c);
                return category != UnicodeCategory.OtherNotAssigned; // Exclude undefined characters
            });
        }

        public static int CountLines(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return 0; // Empty or null string has no lines.
            }

            int lineCount = 1; // Start with 1, as the last line might not end with a newline.
            int index = 0;

            while (index < source.Length)
            {
                if (source[index] == '\r')
                {
                    if (index + 1 < source.Length && source[index + 1] == '\n')
                    {
                        // Windows-style CRLF
                        index += 2;
                    }
                    else
                    {
                        // Mac OS 9 or older style CR
                        index++;
                    }
                    lineCount++;
                }
                else if (source[index] == '\n')
                {
                    // Unix-style LF
                    index++;
                    lineCount++;
                }
                else
                {
                    index++;
                }
            }

            return lineCount;
        }
    }
}
