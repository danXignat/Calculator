using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Utils {
    public enum TokenType {
        Operator,
        Operand,
        EqualSign,
        Unknown
    }

    public static class Classifier {
        public static bool IsOperator(string input) {
            return input == "+" || input == "-" || input == "*" || input == "/";
        }

        public static bool IsOperand(string input) {
            return double.TryParse(input, out _);
        }

        public static TokenType Classify(string input) {
            if (IsOperator(input)) {
                return TokenType.Operator;
            }
            else if (IsOperand(input)) {
                return TokenType.Operand;
            }
            else {
                return TokenType.Unknown;
            }
        }
    }
}
