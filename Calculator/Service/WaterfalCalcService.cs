using Calculator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Service {
    internal class WaterfalCalcService {
        private string _firstOperand = "";
        private bool _secondOperandSetting = false;
        private string _operator = "";
        public string TempDisplayText => _firstOperand + _operator;

        public WaterfalCalcService() {

        }

        public string ProcessInput(string input, string currDisplay)
        {
            switch(Classifier.Classify(input)) {
                case TokenType.Operand:
                    return ProcessOperand(input, currDisplay);
                    break;

                case TokenType.Operator:
                    return ProcessOperator(input, currDisplay);
                    break;

                case TokenType.EqualSign:
                    return ProcessEqualSign(currDisplay);
                    break;

                default:
                    return "Error";
                    break;
            }
        }
        public void Reset() {
            _firstOperand = "";
            _secondOperandSetting = false;
            _operator = "";
        }

        private string ProcessOperand(string input, string currDisplay) {
            if (currDisplay == "0") {
                currDisplay = "";
            }

            if (_operator != "" && _firstOperand != "" && !_secondOperandSetting) {
                _secondOperandSetting = true;
                currDisplay = "";
            }

            return currDisplay + input;
        }
        private string ProcessOperator(string input, string currDisplay) {
            if (_firstOperand != "" && _operator != "") {
                 currDisplay = ProcessEqualSign(currDisplay);
            }

            _firstOperand = currDisplay;
            _operator = input;

            return currDisplay;
        }
        private string ProcessEqualSign(string currDisplay) {
            _secondOperandSetting = false;

            return Utils.Calculator.Calculate(_firstOperand, currDisplay, _operator).ToString();
        }

    }
}
