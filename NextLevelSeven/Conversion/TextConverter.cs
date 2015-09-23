﻿using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Conversion
{
    public static class TextConverter
    {
        /// <summary>
        ///     Convert from standard string to HL7 formatted text.
        /// </summary>
        /// <param name="input">String value to convert.</param>
        /// <param name="delimiter">Delimiter to use (typically component)</param>
        /// <returns>Converted string.</returns>
        public static string ConvertFromFormattedText(IEnumerable<string> input, char delimiter)
        {
            return string.Join(new string(delimiter, 1), input);
        }

        /// <summary>
        ///     Convert from standard string to HL7 formatted text, using an encoding configuration.
        /// </summary>
        /// <param name="input">String value to convert.</param>
        /// <param name="delimiter">Delimiter to use (typically component)</param>
        /// <param name="encoding">Encoding to use for escaping.</param>
        /// <returns>Converted string.</returns>
        internal static string ConvertFromFormattedText(IEnumerable<string> input, char delimiter,
            EncodingConfigurationBase encoding)
        {
            return ConvertFromFormattedText(input.Select(encoding.Escape), delimiter);
        }

        /// <summary>
        ///     Convert from standard string to HL7 string.
        /// </summary>
        /// <param name="input">String value to convert.</param>
        /// <returns>Converted string.</returns>
        public static string ConvertFromString(string input)
        {
            return input ?? "\"\"";
        }

        /// <summary>
        ///     Convert from standard string to HL7 formatted text.
        /// </summary>
        /// <param name="input">String value to convert.</param>
        /// <param name="delimiter">Delimiter to use (typically component)</param>
        /// <returns>Converted string.</returns>
        public static IEnumerable<string> ConvertToFormattedText(string input, char delimiter)
        {
            return input.Split(delimiter);
        }

        /// <summary>
        ///     Convert from standard string to HL7 formatted text.
        /// </summary>
        /// <param name="input">String value to convert.</param>
        /// <param name="delimiter">Delimiter to use (typically component)</param>
        /// <param name="encoding">Encoding to use for escaping.</param>
        /// <returns>Converted string.</returns>
        internal static IEnumerable<string> ConvertToFormattedText(string input, char delimiter,
            EncodingConfigurationBase encoding)
        {
            return ConvertToFormattedText(encoding.UnEscape(input), delimiter);
        }

        /// <summary>
        ///     Convert HL7 string to standard string.
        /// </summary>
        /// <param name="input">String to convert.</param>
        /// <returns>Converted string.</returns>
        public static string ConvertToString(string input)
        {
            return input == "\"\""
                ? null
                : input;
        }
    }
}