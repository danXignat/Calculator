using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Utils;
using System.Windows.Input;
using System.Windows;
using Calculator.Service;
using Calculator.Models;

namespace Calculator.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand NumberCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualSignCommand { get; }
        public ICommand ChangeSignCommand { get; }
        public ICommand PointCommand { get; }
        public ICommand BackSpaceCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand ResetMainDisplayCommand { get; }
        public ICommand ModeCommand { get; }

        private DisplayModel _displayModel; 
        private WaterfallCalcService _calcService;

        public string MainDisplayText {
            get => _displayModel.MainDisplayText;
            set => _displayModel.MainDisplayText = value;
        }
        public string TempDisplayText {
            get => _displayModel.TempDisplayText;
            set => _displayModel.TempDisplayText = value;
        }

        public CalculatorViewModel() {
            _displayModel = new DisplayModel();
            _displayModel.PropertyChanged += (sender, args) => {
                OnPropertyChanged(args.PropertyName);
            };

            _calcService = new WaterfallCalcService(_displayModel);

            NumberCommand           = new RelayCommand(ProcessDigit);
            OperatorCommand         = new RelayCommand(ProcessOperator, item => Char.IsDigit(MainDisplayText[^1]));
            EqualSignCommand        = new RelayCommand(ProcessEqualSign);
            ChangeSignCommand       = new RelayCommand(ProcessChangeSign, item => MainDisplayText != "0");
            PointCommand            = new RelayCommand(ProcessPoint, item => !MainDisplayText.Contains(".") && MainDisplayText != "0");
            BackSpaceCommand        = new RelayCommand(ProcessBackspace, item => MainDisplayText != "0");
            ResetCommand            = new RelayCommand(ProcessClear);
            ResetMainDisplayCommand = new RelayCommand(obj => { MainDisplayText = "0"; });
            ModeCommand             = new RelayCommand(ChangeMode);
        }

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            _displayModel.MainDisplayText += ".";
        }
        private void ChangeMode(object parameter) {
            ProcessClear(parameter);
        }
        private void ProcessClear(object parameter) {
            _displayModel.Reset();
            _calcService.Reset();
        }
        private void ProcessBackspace(object parameter) {
            if (MainDisplayText.Length == 1 || MainDisplayText.Length == 2 && MainDisplayText[0] == '-') {
                MainDisplayText = "0";
            }
            else {
                MainDisplayText = MainDisplayText.Substring(0, MainDisplayText.Length - 1);
            }
        }
        private void ProcessEqualSign(object parameter) {
            _calcService.ProcessEqualSign();
        }
    }
}
