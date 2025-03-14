using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Utils {
    internal class Calculator {
        static public Dictionary<string, Func<double, double, double>> SuportedOp = new Dictionary<string, Func<double, double, double>> {
            { "+",   (lhs, rhs) => lhs + rhs },
            { "-",   (lhs, rhs) => lhs - rhs },
            { "×",   (lhs, rhs) => lhs * rhs },
            { "÷",   (lhs, rhs) => lhs / rhs },
            { "¹/ₓ", (lhs, _)   => 1 / lhs },
            { "x²",  (lhs, _)   => lhs * lhs },
            { "²√x", (lhs, _)   => double.Sqrt(lhs) }
        };

        static public string Calculate(string op, string lhs, string? rhs = null) {
            double first = double.Parse(lhs);
            double second = rhs != null ? double.Parse(rhs) : 0;

            return SuportedOp[op](first, second).ToString();
        }
    }
}
