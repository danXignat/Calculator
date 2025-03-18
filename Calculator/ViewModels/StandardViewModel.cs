using Calculator.Models;
using Calculator.Service;
using Calculator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator.ViewModels {
    public class StandardViewModel : IBaseViewModel {
        private DisplayModel displayModel;
        private WaterfallCalcService _calcService;

        public ICommand NumberCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualSignCommand { get; }
        public ICommand ChangeSignCommand { get; }
        public ICommand PointCommand { get; }
        public ICommand BackSpaceCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand ResetMainDisplayCommand { get; }

        public StandardViewModel(DisplayModel otherDisplayModel) {
            displayModel = otherDisplayModel;
            _calcService = new WaterfallCalcService(displayModel);

            NumberCommand           = new RelayCommand(ProcessDigit);
            OperatorCommand         = new RelayCommand(ProcessOperator, item => displayModel.MainDisplayText.Length > 0 && Char.IsDigit(displayModel.MainDisplayText[^1]));
            EqualSignCommand        = new RelayCommand(ProcessEqualSign);
            ChangeSignCommand       = new RelayCommand(ProcessChangeSign, item => displayModel.MainDisplayText != "0");
            PointCommand            = new RelayCommand(ProcessPoint, item => !displayModel.MainDisplayText.Contains(".") && displayModel.MainDisplayText != "0");
            BackSpaceCommand        = new RelayCommand(ProcessBackspace, item => displayModel.MainDisplayText != "0");
            ResetCommand            = new RelayCommand(ProcessClear);
            ResetMainDisplayCommand = new RelayCommand(obj => { displayModel.MainDisplayText = "0"; });
        }

        public void Initialize() {
            ProcessClear(null);
        }

        private void ProcessDigit(object parameter) {
            string digit = parameter as string;
            _calcService.ProcessDigit(digit);
        }

        private void ProcessOperator(object parameter) {
            string op = parameter as string;
            _calcService.ProcessOperator(op);
        }

        private void ProcessChangeSign(object parameter) {
            _calcService.ProcessChangeSign();
        }

        private void ProcessPoint(object parameter) {
            if (!displayModel.MainDisplayText.Contains(".")) {
                displayModel.MainDisplayText += ".";
            }
        }

        private void ProcessClear(object parameter) {
            displayModel.Reset();
            _calcService.Reset();
        }

        private void ProcessBackspace(object parameter) {
            if (displayModel.MainDisplayText.Length == 1 || (displayModel.MainDisplayText.Length == 2 && displayModel.MainDisplayText[0] == '-')) {
                displayModel.MainDisplayText = "0";
            }
            else {
                displayModel.MainDisplayText = displayModel.MainDisplayText.Substring(0, displayModel.MainDisplayText.Length - 1);
            }
        }

        private void ProcessEqualSign(object parameter) {
            _calcService.ProcessEqualSign();
        }
    }
}