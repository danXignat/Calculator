using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Utils;
using static Calculator.Utils.CalculatorEngine;

namespace Calculator.Utils {
    public enum Arity {
        Unary, Binary
    }
    public enum Operations {
        Add, Subtract, Multiply, Divide, Inverse, Square, SquareRoot, Percent, XOR, OR, AND, NOT, LeftShift, RightShift
    }

    internal class CalculatorEngine {
        public record OpData {
            public Func<double, double, double> Action { get; set; }
            public Arity Arity { get; set; }
            public Func<string, string>? DisplayFunction { get; set; }
            public Operations Operator { get; set; }
            public OpData(Func<double, double, double> action, Arity arity, Operations op, Func<string, string>? displayFunction = null) =>
                (Action, Arity, Operator, DisplayFunction) = (action, arity, op, displayFunction);
        }

        static public Dictionary<string, OpData> SupportedOp = new Dictionary<string, OpData> {
            { "+",      new OpData((lhs, rhs) => lhs + rhs,                       Arity.Binary, Operations.Add) },
            { "-",      new OpData((lhs, rhs) => lhs - rhs,                       Arity.Binary, Operations.Subtract) },
            { "×",      new OpData((lhs, rhs) => lhs * rhs,                       Arity.Binary, Operations.Multiply) },
            { "÷",      new OpData((lhs, rhs) => lhs / rhs,                       Arity.Binary, Operations.Divide) },
            { "¹/ₓ",    new OpData((lhs, _)   => 1 / lhs,                         Arity.Unary,  Operations.Inverse,    (val) => $"1/({val})") },
            { "x²",     new OpData((lhs, _)   => lhs * lhs,                       Arity.Unary,  Operations.Square,     (val) => $"sqr({val})") },
            { "²√x",    new OpData((lhs, _)   => Math.Sqrt(lhs),                  Arity.Unary,  Operations.SquareRoot, (val) => $"√({val})") },
            { "%",      new OpData((lhs, rhs) => lhs * (rhs / 100),               Arity.Binary, Operations.Percent) },
            { "AND",    new OpData((lhs, rhs) => (double)((long)lhs & (long)rhs), Arity.Binary, Operations.AND) },
            { "OR",     new OpData((lhs, rhs) => (double)((long)lhs | (long)rhs), Arity.Binary, Operations.OR) },
            { "XOR",    new OpData((lhs, rhs) => (double)((long)lhs ^ (long)rhs), Arity.Binary, Operations.XOR) },
            { "NOT",    new OpData((lhs, _)   => (double)(~(long)lhs),            Arity.Unary, Operations.NOT, (val) => $"NOT({val})") },
            { "LSHIFT", new OpData((lhs, rhs) => (double)((long)lhs << (int)rhs), Arity.Binary, Operations.LeftShift) },
            { "RSHIFT", new OpData((lhs, rhs) => (double)((long)lhs >> (int)rhs), Arity.Binary, Operations.RightShift) }
        };

        static public string Calculate(string op, string lhs, string? rhs = null, string numerationBase = "10") {
            int baseValue = int.Parse(numerationBase);

            string convertedLhs = baseValue == 10 ? lhs : BaseConverter.ConvertBase(lhs, baseValue, 10);
            string convertedRhs = rhs != null ? (baseValue == 10 ? rhs : BaseConverter.ConvertBase(rhs, baseValue, 10)) : "0";

            double first = Convert.ToDouble(convertedLhs);
            double second = Convert.ToDouble(convertedRhs);

            if (SupportedOp[op].Arity == Arity.Binary && rhs == null)
            {
                throw new ArgumentException($"Operation {op} requires two operands");
            }

            string result = SupportedOp[op].Action(first, second).ToString();

            return baseValue == 10 ? result : BaseConverter.ConvertBase(result, 10, baseValue);
        }

        static public bool isUnary(string op) {
            return SupportedOp[op].Arity == Arity.Unary;
        }
        static public bool isBinary(string op) {
            return SupportedOp[op].Arity == Arity.Binary;
        }
    }
}
