using System;
using System.Globalization;

namespace Calculator.Services
{
    public class DigitGroupingService
    {
        private bool _isDigitGroupingEnabled;
        private readonly CultureInfo _currentCulture;

        public DigitGroupingService()
        {
            _currentCulture = CultureInfo.CurrentCulture;
            _isDigitGroupingEnabled = false;
        }

        public bool IsDigitGroupingEnabled
        {
            get => _isDigitGroupingEnabled;
            set => _isDigitGroupingEnabled = value;
        }

        public string GetFormattedNumber(string numberStr)
        {
            // If digit grouping is disabled, return the original number
            if (!_isDigitGroupingEnabled)
                return numberStr;

            // Try to parse the number
            if (double.TryParse(numberStr, NumberStyles.Any, _currentCulture, out double number))
            {
                // Check if it's an integer or has decimal places
                bool isInteger = number == Math.Floor(number);

                if (isInteger)
                {
                    // Format as integer with thousand separators
                    return number.ToString("N0", _currentCulture);
                }
                else
                {
                    // Format as decimal with thousand separators
                    // Determine the number of decimal places in the original string
                    int decimalPlaces = GetDecimalPlaces(numberStr);
                    return number.ToString($"N{decimalPlaces}", _currentCulture);
                }
            }

            // If parsing fails, return the original string
            return numberStr;
        }

        public string RemoveDigitGrouping(string formattedNumber)
        {
            // Try to parse the formatted number
            if (double.TryParse(formattedNumber, NumberStyles.Any, _currentCulture, out double number))
            {
                // Return the plain number format
                return number.ToString(_currentCulture);
            }

            // If parsing fails, return the original string
            return formattedNumber;
        }

        private int GetDecimalPlaces(string numberStr)
        {
            int decimalSeparatorIndex = numberStr.IndexOf(_currentCulture.NumberFormat.NumberDecimalSeparator);

            if (decimalSeparatorIndex < 0)
                return 0;

            return numberStr.Length - decimalSeparatorIndex - 1;
        }

        // Get information about the current number format
        public string GetNumberFormatInfo()
        {
            return $"Decimal Separator: '{_currentCulture.NumberFormat.NumberDecimalSeparator}', " +
                   $"Group Separator: '{_currentCulture.NumberFormat.NumberGroupSeparator}', " +
                   $"Example: {(12345.67).ToString("N2", _currentCulture)}";
        }
    }
}