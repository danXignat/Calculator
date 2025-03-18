using Calculator.Models;
using Calculator.Service;
using Calculator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator.ViewModels
{
    public class ProgrammerViewModel : IBaseViewModel {
        private DisplayModel displayModel;
        private WaterfallCalcService _calcService;

        public ICommand NumberCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualSignCommand { get; }
        public ICommand ChangeSignCommand { get; }
        public ICommand BackSpaceCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand ResetMainDisplayCommand { get; }

        
        public ICommand BaseCommand { get; }
        public ICommand BitwiseCommand { get; }

        private string _currentBase = "10";

        public string CurrentBase {
            get => _currentBase;
            private set {
                _currentBase = value;
                UpdateDisplayForCurrentBase();
            }
        }

        public ProgrammerViewModel(DisplayModel otherDisplayModel) {
            displayModel = otherDisplayModel;
            _calcService = new WaterfallCalcService(displayModel);

            NumberCommand           = new RelayCommand(ProcessDigit, CanExecuteNumberCommand);
            OperatorCommand         = new RelayCommand(ProcessOperator, item => displayModel.MainDisplayText.Length > 0 && Char.IsDigit(displayModel.MainDisplayText[^1]));
            EqualSignCommand        = new RelayCommand(ProcessEqualSign);
            BackSpaceCommand        = new RelayCommand(ProcessBackspace, item => displayModel.MainDisplayText != "0");
            ResetCommand            = new RelayCommand(ProcessClear);
            ResetMainDisplayCommand = new RelayCommand(obj => { displayModel.MainDisplayText = "0"; });
            
            BaseCommand = new RelayCommand(ChangeBase);
            //BitwiseCommand = new RelayCommand(ProcessBitwiseOperation);
        }

        public void Initialize() {
            ProcessClear(null);
            CurrentBase = "10";
        }
        private void ChangeBase(object parameter) {
            string baseString = parameter as string;

            _currentBase = baseString;

            UpdateDisplayForCurrentBase();
        }

        private bool CanExecuteNumberCommand(object parameter) {
            string digit = parameter as string;

            if (string.IsNullOrEmpty(digit)) {
                return false;
            }

            if (CurrentBase == "2" && (int.Parse(digit) > 1)) {
                return false;
            }

            if (CurrentBase == "8" && (int.Parse(digit) > 7)) {
                return false;
            }

            if (CurrentBase == "10" && (int.Parse(digit) > 9)) {
                return false;
            }

            return true;
        }

        private void ProcessDigit(object parameter) {
            string digit = parameter as string;

            if (displayModel.MainDisplayText == "0") {
                displayModel.MainDisplayText = digit;
            }
            else {
                displayModel.MainDisplayText += digit;
            }

            UpdateDisplayForCurrentBase();
        }

        private void ProcessOperator(object parameter) {
            string op = parameter as string;
            _calcService.ProcessOperator(op);
        }

        private void ProcessClear(object parameter) {
            displayModel.Reset();
            _calcService.Reset();
        }

        private void ProcessBackspace(object parameter) {
            if (displayModel.MainDisplayText.Length == 1 ||
                (displayModel.MainDisplayText.Length == 2 && displayModel.MainDisplayText[0] == '-')) {
                displayModel.MainDisplayText = "0";
            }
            else {
                displayModel.MainDisplayText = displayModel.MainDisplayText.Substring(0, displayModel.MainDisplayText.Length - 1);
            }
        }

        private void ProcessEqualSign(object parameter) {
            _calcService.ProcessEqualSign();
            UpdateDisplayForCurrentBase();
        }

        private void UpdateDisplayForCurrentBase() {
            if (double.TryParse(displayModel.MainDisplayText, out double value)) {
                int currBase = int.Parse(_currentBase);
                displayModel.HexDisplay = BaseConverter.ConvertBase(displayModel.MainDisplayText, currBase, 16);
                displayModel.DecDisplay = BaseConverter.ConvertBase(displayModel.MainDisplayText, currBase, 10);
                displayModel.OctDisplay = BaseConverter.ConvertBase(displayModel.MainDisplayText, currBase, 8);
                displayModel.BinDisplay = BaseConverter.ConvertBase(displayModel.MainDisplayText, currBase, 2);
            }
        }
    }
}
