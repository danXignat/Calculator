using Calculator.Models;
using Calculator.Service;
using Calculator.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator.ViewModels {
    public class ProgrammerViewModel : IBaseViewModel {
        private DisplayModel displayModel;
        private WaterfallCalcService _calcService;

        public ICommand NumberCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualSignCommand { get; }
        public ICommand ChangeSignCommand { get; private set; } // Now has a private setter
        public ICommand BackSpaceCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand ResetMainDisplayCommand { get; }
        public ICommand BaseCommand { get; }

        private string _currentBase = "10";

        public string CurrentBase {
            get => _currentBase;
            private set {
                _currentBase = value;
                UpdateDisplayForCurrentBase();
            }
        }

        public ProgrammerViewModel(DisplayModel otherDisplayModel) {
            displayModel = otherDisplayModel ?? throw new ArgumentNullException(nameof(otherDisplayModel));
            _calcService = new WaterfallCalcService(displayModel);

            NumberCommand = new RelayCommand(ProcessDigit, CanExecuteNumberCommand);
            OperatorCommand = new RelayCommand(ProcessOperator);
            EqualSignCommand = new RelayCommand(ProcessEqualSign);
            ChangeSignCommand = new RelayCommand((_) => { }); // Initialize with empty action
            BackSpaceCommand = new RelayCommand(ProcessBackspace, item => displayModel.MainDisplayText != "0");
            ResetCommand = new RelayCommand(ProcessClear);
            ResetMainDisplayCommand = new RelayCommand(obj => { displayModel.MainDisplayText = "0"; });

            BaseCommand = new RelayCommand(ChangeBase);
        }

        public void Initialize() {
            ProcessClear(null);
            CurrentBase = "10";
        }

        private void ChangeBase(object? parameter) {
            string newBase = parameter as string ?? throw new ArgumentNullException(nameof(parameter));
            string pastBase = _currentBase;

            _currentBase = newBase;

            displayModel.MainDisplayText = BaseConverter.ConvertBase(displayModel.MainDisplayText, int.Parse(pastBase), int.Parse(newBase));
        }

        private bool CanExecuteNumberCommand(object? parameter) {
            string? digit = parameter as string;

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

        private void ProcessDigit(object? parameter) {
            string digit = parameter as string ?? throw new ArgumentNullException(nameof(parameter));
            _calcService.ProcessDigit(digit);
        }

        private void ProcessOperator(object? parameter) {
            string op = parameter as string ?? throw new ArgumentNullException(nameof(parameter));
            _calcService.ProcessOperator(op, _currentBase);
        }

        private void ProcessClear(object? parameter) {
            displayModel.Reset();
            _calcService.Reset();
        }

        private void ProcessBackspace(object? parameter) {
            if (displayModel.MainDisplayText.Length == 1 ||
                (displayModel.MainDisplayText.Length == 2 && displayModel.MainDisplayText[0] == '-')) {
                displayModel.MainDisplayText = "0";
            }
            else {
                displayModel.MainDisplayText = displayModel.MainDisplayText.Substring(0, displayModel.MainDisplayText.Length - 1);
            }
        }

        private void ProcessEqualSign(object? parameter) {
            _calcService.ProcessEqualSign(_currentBase);
        }

        private void UpdateDisplayForCurrentBase() {
            int currBase = int.Parse(_currentBase);
            displayModel.HexDisplayText = BaseConverter.ConvertBase(displayModel.MainDisplayText, currBase, 16);
            displayModel.DecDisplayText = BaseConverter.ConvertBase(displayModel.MainDisplayText, currBase, 10);
            displayModel.OctDisplayText = BaseConverter.ConvertBase(displayModel.MainDisplayText, currBase, 8);
            displayModel.BinDisplayText = BaseConverter.ConvertBase(displayModel.MainDisplayText, currBase, 2);
        }

        public void BaseDisplayModel_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(DisplayModel.MainDisplayText) && double.TryParse(displayModel.MainDisplayText, out _)) {
                UpdateDisplayForCurrentBase();
            }
        }
    }
}