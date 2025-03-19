using Calculator.Models;
using Calculator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Service {
    internal class WaterfallCalcService {
        private DisplayModel _displayModel;
        private string _firstOperand = "";
        private string _secondOperand = "";
        private string _op = "";

        private Action action = null;

        private bool _resetMainDisplay = false;
        private bool _newTerm = false;

        public WaterfallCalcService(DisplayModel displayModel) {
            _displayModel = displayModel;
        }

        public void ProcessDigit(string digit) {
            if (action != null) {
                action?.Invoke();
                action = null;
            }

            if (_displayModel.MainDisplayText == "0") {
                _displayModel.MainDisplayText = "";
            }
            
            _displayModel.MainDisplayText += digit;
        }

        public void ProcessOperator(string op, string numerationBase = "10") {
            if (CalculatorEngine.SupportedOp[op].Operator == Operations.Percent) {
                ProcessPercent(op);
            }
            else if (CalculatorEngine.isUnary(op)) {
                ProcessUnary(op, numerationBase);
            }
            else {
                ProcessBinary(op, numerationBase);
            }
        }
        public void ProcessChangeSign() {
            if (char.IsDigit(_displayModel.MainDisplayText[0])) {
                _displayModel.MainDisplayText = "-" + _displayModel.MainDisplayText;
            }
            else {
                _displayModel.MainDisplayText = _displayModel.MainDisplayText.Substring(1);
            }
        }
        private void ProcessUnary(string op, string numerationBase = "10") {
            string result = Utils.CalculatorEngine.Calculate(op, _displayModel.MainDisplayText, null, numerationBase);

            if (_secondOperand == "") {
                _secondOperand = Utils.CalculatorEngine.SupportedOp[op].DisplayFunction(_displayModel.MainDisplayText);
            }
            else {
                _secondOperand = Utils.CalculatorEngine.SupportedOp[op].DisplayFunction(_secondOperand);
            }

            _displayModel.TempDisplayText = $"{_firstOperand} {_op} {_secondOperand}";
            _displayModel.MainDisplayText = result;
        }
        private void ProcessBinary(string op, string numerationBase = "10") {
            if (_firstOperand == "") {
                _firstOperand = _displayModel.MainDisplayText;
            }

            if (_newTerm) {
                _firstOperand = Utils.CalculatorEngine.Calculate(_op, _firstOperand, _displayModel.MainDisplayText, numerationBase);
                _displayModel.MainDisplayText = _firstOperand;
                _newTerm = false;
            }

            _op = op;
            _displayModel.TempDisplayText = $"{_firstOperand} {op}";
            _resetMainDisplay = true;

            action = () => {
                _displayModel.MainDisplayText = "";
                _newTerm = true;
            };
        }
        private void ProcessPercent(string op) {
            string result = Utils.CalculatorEngine.Calculate(op, _firstOperand, _displayModel.MainDisplayText);

            _displayModel.TempDisplayText = $"{_firstOperand} {_op} {result}";
            _displayModel.MainDisplayText = result;
        }
        public void ProcessEqualSign(string numerationBase = "10") {
            _secondOperand = _displayModel.MainDisplayText;
            _displayModel.TempDisplayText = $"{_firstOperand} {_op} {_secondOperand} = ";
            _displayModel.MainDisplayText = Utils.CalculatorEngine.Calculate(_op, _firstOperand, _secondOperand, numerationBase);

            action = () => {
                _displayModel.TempDisplayText = "";
                _displayModel.MainDisplayText = "";
                Reset();
            };
        }

        public void Reset() {
            _firstOperand = "";
            _secondOperand = "";
            _op = "";
            _resetMainDisplay = false;
            _newTerm = false;
            action = null;
        }
    }
}
