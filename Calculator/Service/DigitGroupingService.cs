using System;
using System.Globalization;

namespace Calculator.Services {
    public class DigitGroupingService {
        private bool _isDigitGroupingEnabled;
        private readonly CultureInfo _currentCulture;

        public DigitGroupingService() {
            _currentCulture = CultureInfo.CurrentCulture;
            _isDigitGroupingEnabled = false;
        }

        public bool IsDigitGroupingEnabled {
            get => _isDigitGroupingEnabled;
            set => _isDigitGroupingEnabled = value;
        }

        public string GetFormattedNumber(string numberStr) {
            if (!_isDigitGroupingEnabled)
                return numberStr;

            if (double.TryParse(numberStr, NumberStyles.Any, _currentCulture, out double number)) {
                bool isInteger = number == Math.Floor(number);

                if (isInteger) {
                    return number.ToString("N0", _currentCulture);
                }
                else {
                    int decimalPlaces = GetDecimalPlaces(numberStr);
                    return number.ToString($"N{decimalPlaces}", _currentCulture);
                }
            }

            return numberStr;
        }

        public string RemoveDigitGrouping(string formattedNumber) {
            if (double.TryParse(formattedNumber, NumberStyles.Any, _currentCulture, out double number)) {
                return number.ToString(_currentCulture);
            }

            return formattedNumber;
        }

        private int GetDecimalPlaces(string numberStr) {
            int decimalSeparatorIndex = numberStr.IndexOf(_currentCulture.NumberFormat.NumberDecimalSeparator);

            if (decimalSeparatorIndex < 0)
                return 0;

            return numberStr.Length - decimalSeparatorIndex - 1;
        }

        public string GetNumberFormatInfo() {
            return $"Decimal Separator: '{_currentCulture.NumberFormat.NumberDecimalSeparator}', " +
                   $"Group Separator: '{_currentCulture.NumberFormat.NumberGroupSeparator}', " +
                   $"Example: {(12345.67).ToString("N2", _currentCulture)}";
        }
    }
}