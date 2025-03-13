using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Utils {
    internal class Calculator {
        public static double Calculate(string firstOperand, string secondOperand, string op) {
            double first = double.Parse(firstOperand);
            double second = double.Parse(secondOperand);

            switch (op) {
                case "+":
                    return first + second;
                    break;

                case "-":
                    return first - second;
                    break;

                case "*":
                    return first * second;
                    break;

                case "/":
                    return first / second;
                    break;

                default:
                    return 0;
                    break;
            }
        }
    }
}
