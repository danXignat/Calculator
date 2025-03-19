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
            OperatorCommand         = new RelayCommand(ProcessOperator);
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

            displayModel.MainDisplayText = BaseConverter.ConvertBase(displayModel.MainDisplayText, int.Parse(_currentBase), int.Parse(baseString));

            _currentBase = baseString;
        }

        private bool CanExecuteNumberCommand(object parameter) {
            string digit = parameter as string;

            if (string.IsNullOrEmpty(digit)) {
                return false;
            }

            if (CurrentBase == "2" && !BaseConverter.BinaryDigits.Contains(digit)) {
                return false;
            }

            if (CurrentBase == "8" && !BaseConverter.OctalDigits.Contains(digit)) {
                return false;
            }

            if (CurrentBase == "10" && !BaseConverter.DecimalDigits.Contains(digit)) {
                return false;
            }

            return true;
        }

        private void ProcessDigit(object parameter) {
            string digit = parameter as string;

            _calcService.ProcessDigit(digit);

            UpdateDisplayForCurrentBase();
        }

        private void ProcessOperator(object parameter) {
            string op = parameter as string;
            _calcService.ProcessOperator(op, _currentBase);
            UpdateDisplayForCurrentBase();
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

            UpdateDisplayForCurrentBase();
        }

        private void ProcessEqualSign(object parameter) {
            _calcService.ProcessEqualSign(_currentBase);
            UpdateDisplayForCurrentBase();
        }

        private void UpdateDisplayForCurrentBase() {
            int currBase = int.Parse(_currentBase);
            displayModel.HexDisplayText = BaseConverter.ConvertBase(displayModel.MainDisplayText, currBase, 16);
            displayModel.DecDisplayText = BaseConverter.ConvertBase(displayModel.MainDisplayText, currBase, 10);
            displayModel.OctDisplayText = BaseConverter.ConvertBase(displayModel.MainDisplayText, currBase, 8);
            displayModel.BinDisplayText = BaseConverter.ConvertBase(displayModel.MainDisplayText, currBase, 2);
        }
    }
}
