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

namespace Calculator.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand NumberCommand { get; }
        public ICommand ModeCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand BackSpaceCommand { get; }
        public ICommand ResetCommand { get; }

        public string MainDisplayText
        {
            get => _mainDisplayText;
            set
            {
                if (_mainDisplayText != value)
                {
                    _mainDisplayText = value;
                    OnPropertyChanged(nameof(MainDisplayText));
                }
            }
        }
        public string TempDisplayText {
            get => _tempDisplayText;
            set {
                if (_tempDisplayText != value) {
                    _tempDisplayText = value;
                    OnPropertyChanged(nameof(TempDisplayText));
                }
            }
        }
        private string _mainDisplayText = "0";
        private string _tempDisplayText = "";
        private WaterfalCalcService _calcService = new WaterfalCalcService();

        public CalculatorViewModel()
        {
            _calcService = new WaterfalCalcService();

            NumberCommand = new RelayCommand(ProcessDigit);
            ModeCommand = new RelayCommand(ChangeMode);
            OperatorCommand = new RelayCommand(ProcessOperator, item => Char.IsDigit(MainDisplayText[^1]));
            BackSpaceCommand = new RelayCommand(ProcessBackspace, item => MainDisplayText != "0");
            ResetCommand = new RelayCommand(ProcessClear);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ProcessDigit(object parameter)
        {
            string digit = parameter as string;

            MainDisplayText = _calcService.ProcessInput(digit, MainDisplayText);
            TempDisplayText = _calcService.TempDisplayText;
        }
        private void ProcessOperator(object parameter) {
            string op = parameter as string;

            MainDisplayText = _calcService.ProcessInput(op, MainDisplayText);
            TempDisplayText = _calcService.TempDisplayText;
        }
        private void ChangeMode(object parameter)
        {
            ProcessClear(parameter);
        }
        private void ProcessClear(object parameter) {
            MainDisplayText = "0";
            TempDisplayText = "";
            _calcService.Reset();
        }
        private void ProcessBackspace(object parameter) {
            if (MainDisplayText.Length == 1) {
                MainDisplayText = "0";
            }
            else {
                MainDisplayText = MainDisplayText.Substring(0, MainDisplayText.Length - 1);
            }
        }   
    }
}
