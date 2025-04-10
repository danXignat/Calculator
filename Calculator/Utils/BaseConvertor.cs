﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Utils {
    public class BaseConverter {
        public static readonly HashSet<string> BinaryDigits = new HashSet<string> {
            "0", "1"
        };

        public static readonly HashSet<string> OctalDigits = new HashSet<string> {
            "0", "1", "2", "3", "4", "5", "6", "7"
        };

        public static readonly HashSet<string> DecimalDigits = new HashSet<string> {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
        };

        public static readonly HashSet<string> HexadecimalDigits = new HashSet<string> {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "A", "B", "C", "D", "E", "F"
        };

        public static bool IsValidBase(int baseValue) {
            return baseValue == 2 || baseValue == 8 || baseValue == 10 || baseValue == 16;
        }

        public static string ConvertBase(string number, int fromBase, int toBase) {
            if (!IsValidBase(fromBase) || !IsValidBase(toBase)) {
                throw new ArgumentException("Only bases 2, 8, 10, and 16 are supported.");
            }
            try {
                int decimalValue = Convert.ToInt32(number, fromBase);
                string result = "";
                switch (toBase) {
                    case 2:
                        result = Convert.ToString(decimalValue, 2);
                        break;
                    case 8:
                        result = Convert.ToString(decimalValue, 8);
                        break;
                    case 10:
                        result = decimalValue.ToString();
                        break;
                    case 16:
                        result = Convert.ToString(decimalValue, 16).ToUpper();
                        break;
                }
                return result;
            }
            catch (Exception ex) {
                throw new ArgumentException($"Conversion error: {ex.Message}");
            }
        }
    }
}