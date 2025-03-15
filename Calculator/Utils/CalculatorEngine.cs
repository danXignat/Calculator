using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Utils {
    public enum Arity {
        Unary, Binary
    }

    public enum Operations {
        Add, Subtract, Multiply, Divide, Inverse, Square, SquareRoot, Percent
    }
    internal class CalculatorEngine {
        public record OpData {
            public Func<double, double, double> Action { get; set; }
            public Arity Arity { get; set; }
            public Func<string, string> DisplayFunction { get; set; }
            public Operations Operator { get; set; }
            public OpData(Func<double, double, double> action, Arity arity, Operations op, Func<string, string> displayFunction = null) =>
                (Action, Arity, Operator, DisplayFunction) = (action, arity, op, displayFunction);
        }

        static public Dictionary<string, OpData> SupportedOp = new Dictionary<string, OpData> {
            { "+",   new OpData((lhs, rhs) => lhs + rhs,         Arity.Binary, Operations.Add) },
            { "-",   new OpData((lhs, rhs) => lhs - rhs,         Arity.Binary, Operations.Subtract) },
            { "×",   new OpData((lhs, rhs) => lhs * rhs,         Arity.Binary, Operations.Multiply) },
            { "÷",   new OpData((lhs, rhs) => lhs / rhs,         Arity.Binary, Operations.Divide) },
            { "¹/ₓ", new OpData((lhs, _)   => 1 / lhs,           Arity.Unary,  Operations.Inverse,    (val) => $"1/({val})") },
            { "x²",  new OpData((lhs, _)   => lhs * lhs,         Arity.Unary,  Operations.Square,     (val) => $"sqr({val})") },
            { "²√x", new OpData((lhs, _)   => Math.Sqrt(lhs),    Arity.Unary,  Operations.SquareRoot, (val) => $"√({val})") },
            { "%",   new OpData((lhs, rhs) => lhs * (rhs / 100), Arity.Binary, Operations.Percent) }
        };

        static public string Calculate(string op, string lhs, string? rhs = null) {
            double first = double.Parse(lhs);
            double second = rhs != null ? double.Parse(rhs) : 0;

            if (SupportedOp[op].Arity == Arity.Binary && rhs == null) {
                throw new ArgumentException($"Operation {op} requires two operands");
            }

            return SupportedOp[op].Action(first, second).ToString();
        }

        static public bool isUnary(string op) {
            return SupportedOp[op].Arity == Arity.Unary;
        }
        static public bool isBinary(string op) {
            return SupportedOp[op].Arity == Arity.Binary;
        }
    }
}
