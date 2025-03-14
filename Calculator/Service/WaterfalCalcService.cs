using Calculator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Service {
    internal class WaterfallCalcService {
        private string _firstOperand = "";
        private string _secondOperator = "";
        private string _operator = "";
        private bool _resetMainDisplay = false;
        private bool _settingSecondOp = false;
        public string TempDisplayText => _firstOperand + _operator;

        public WaterfallCalcService() {

        }

        public string ProcessInput(string input, string currDisplay) {
            switch(Classifier.Classify(input)) {
                case Classifier.TokenType.Operand:
                    return ProcessOperand(input, currDisplay);
                    break;

                case Classifier.TokenType.Operator:
                    return ProcessOperator(input, currDisplay);
                    break;

                case Classifier.TokenType.EqualSign:
                    return ProcessEqualSign(currDisplay);
                    break;

                default:
                    return "Error";
                    break;
            }
        }
        public void Reset() {
            _firstOperand = "";
            _secondOperator = "";
            _operator = "";
            _resetMainDisplay = false;
            _settingSecondOp = false;
        }
        private string ProcessOperand(string input, string currDisplay) {
            if (_resetMainDisplay) {
                currDisplay = "";
                _resetMainDisplay = false;
                _settingSecondOp = true;
            }

            if (currDisplay == "0") {
                currDisplay = "";
            }

            return currDisplay + input;
        }
        private string ProcessOperator(string input, string currDisplay) {
            if (_settingSecondOp) {
                _firstOperand = Utils.Calculator.Calculate(_operator, _firstOperand, currDisplay);
                currDisplay = _firstOperand;
                _settingSecondOp = false;
            }
            else {
                _firstOperand = currDisplay;
            }

            _operator = input;
            _resetMainDisplay = true;

            return currDisplay;
        }
        private string ProcessEqualSign(string currDisplay) {
            return Utils.Calculator.Calculate(_operator, _firstOperand, currDisplay);
        }
    }
}
