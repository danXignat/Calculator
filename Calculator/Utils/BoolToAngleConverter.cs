﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Calculator.Utils {
    public class BoolToAngleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (bool)value ? 180 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return (double)value > 90;
        }
    }
}