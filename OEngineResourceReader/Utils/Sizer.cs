﻿namespace OEngineResourceReader.Utils
{
    public static class Sizer
    {
        private static readonly List<string> SizeSuffixes = new()
            { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        private static readonly List<string> SizeNames = new() {
            "bytes", "Kilobytes", "Megabytes", "Gigabytes", "Terabytes", "Petabytes", "Exabytes", "Zettabytes",
            "Yottabytes"
        };

        private static int _index;

        /// <summary>
        /// Gets the suffix abbreviation for a given value
        /// </summary>
        /// <param name="value">The number to get the size of</param>
        /// <param name="decimalPlaces">How many decimal places you want default is 0</param>
        /// <param name="includeNumber">Whether or not to include number default is false</param>
        /// <returns>a string</returns>
        public static string Suffix(long value, int decimalPlaces = 0, bool includeNumber = true)
        {
            decimal number = decimal.Round(Suffix((ulong)value), decimalPlaces);
            return includeNumber
                ? $"{number} {SizeSuffixes[_index]}"
                : $"{SizeSuffixes[_index]}";
        }

        /// <summary>
        /// Gets the suffix abbreviation for a given file
        /// </summary>
        /// <param name="file">The file to get the size of</param>
        /// <param name="decimalPlaces">How many decimal places you want default is 0</param>
        /// <param name="includeNumber">Whether or not to include number default is false</param>
        /// <returns>a string</returns>
        public static string Suffix(string file, int decimalPlaces = 0, bool includeNumber = true)
        {
            decimal number = decimal.Round(Suffix((ulong)new FileInfo(file).Length), decimalPlaces);
            return includeNumber
                ? $"{number} {SizeSuffixes[_index]}"
                : $"{SizeSuffixes[_index]}";
        }

        /// <summary>
        /// Gets the suffix abbreviation for given files added together
        /// </summary>
        /// <param name="files">A list of file to get the size of</param>
        /// <param name="decimalPlaces">How many decimal places you want default is 0</param>
        /// <param name="includeNumber">Whether or not to include number default is false</param>
        /// <returns>a string</returns>
        public static string AllSuffix(List<string> files, int decimalPlaces = 0, bool includeNumber = true)
        {
            decimal number =
                decimal.Round(
                    Suffix((ulong)files.Aggregate(0, (current, file) => (int)(current + new FileInfo(file).Length))),
                    decimalPlaces);
            return includeNumber
                ? $"{number} {SizeSuffixes[_index]}"
                : $"{SizeSuffixes[_index]}";
        }

        /// <summary>
        /// Gets the suffix name for a given value
        /// </summary>
        /// <param name="value">The number to get the size of</param>
        /// <param name="decimalPlaces">How many decimal places you want default is 0</param>
        /// <param name="includeNumber">Whether or not to include number default is false</param>
        /// <returns>a string</returns>
        public static string SuffixName(ulong value, int decimalPlaces = 0, bool includeNumber = true)
        {
            decimal number = decimal.Round(Suffix(value), decimalPlaces);
            return includeNumber
                ? $"{number} {SizeNames[_index]}"
                : $"{SizeNames[_index]}";
        }

        /// <summary>
        /// Gets the suffix name for a given file
        /// </summary>
        /// <param name="file">The file to get the size of</param>
        /// <param name="decimalPlaces">How many decimal places you want default is 0</param>
        /// <param name="includeNumber">Whether or not to include number default is false</param>
        /// <returns>a string</returns>
        public static string SuffixName(string file, int decimalPlaces = 0, bool includeNumber = true)
        {
            decimal number = decimal.Round(Suffix((ulong)new FileInfo(file).Length), decimalPlaces);
            return includeNumber
                ? $"{number} {SizeNames[_index]}"
                : $"{SizeNames[_index]}";
        }

        /// <summary>
        /// Gets the suffix name for given files added together
        /// </summary>
        /// <param name="files">A list of file to get the size of</param>
        /// <param name="decimalPlaces">How many decimal places you want default is 0</param>
        /// <param name="includeNumber">Whether or not to include number default is false</param>
        /// <returns>a string</returns>
        public static string AllSuffixName(List<string> files, int decimalPlaces = 0, bool includeNumber = true)
        {
            decimal number =
                decimal.Round(
                    Suffix((ulong)files.Aggregate(0, (current, file) => (int)(current + new FileInfo(file).Length))),
                    decimalPlaces);
            return includeNumber
                ? $"{number} {SizeNames[_index]}"
                : $"{SizeNames[_index]}";
        }

        private static decimal Suffix(ulong value)
        {
            _index = 0;
            decimal dValue = value;
            while (Math.Round(dValue, 1) >= 1000)
            {
                dValue /= 1024;
                _index++;
            }

            return dValue;
        }

        /// <summary>
        /// Gets the size of a file using FileInfo and returns an size abbreviation
        /// </summary>
        /// <param name="info">The FileInfo class</param>
        /// <param name="decimalPlaces">How many decimal places you want default is 0</param>
        /// <param name="includeNumber">Whether or not to include number default is false</param>
        /// <returns>a string</returns>
        public static string Suffix(this FileInfo info, int decimalPlaces = 0, bool includeNumber = false)
        {
            decimal number = Suffix((ulong)info.Length);
            return includeNumber
                ? $"{decimal.Round(number, decimalPlaces)} {SizeSuffixes[_index]}"
                : $"{SizeSuffixes[_index]}";
        }

        /// <summary>
        /// Gets the size of a file using FileInfo and returns an size name
        /// </summary>
        /// <param name="info">The FileInfo class</param>
        /// <param name="decimalPlaces">How many decimal places you want default is 0</param>
        /// <param name="includeNumber">Whether or not to include number default is false</param>
        /// <returns>a string</returns>
        public static string SuffixName(this FileInfo info, int decimalPlaces = 0, bool includeNumber = false)
        {
            decimal number = Suffix((ulong)info.Length);
            return includeNumber
                ? $"{decimal.Round(number, decimalPlaces)} {SizeNames[_index]}"
                : $"{SizeNames[_index]}";
        }
    }
}
