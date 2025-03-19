using Calculator.Models;
using Calculator.Service;
using Calculator.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Calculator.ViewModels
{
    public class StandardViewModel : IBaseViewModel, INotifyPropertyChanged
    {
        private DisplayModel displayModel;
        private WaterfallCalcService _calcService;
        private MemoryCalcService _memoryService;

        private bool _isMemoryDropdownOpen;

        // Make PropertyChanged nullable to match INotifyPropertyChanged interface
        public event PropertyChangedEventHandler? PropertyChanged;

        //------------------------------------------------------------------------------Basic Calculator
        public ICommand NumberCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualSignCommand { get; }
        public ICommand ChangeSignCommand { get; }
        public ICommand PointCommand { get; }
        public ICommand BackSpaceCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand ResetMainDisplayCommand { get; }

        //------------------------------------------------------------------------------Memory
        public ICommand MemoryClearCommand { get; }
        public ICommand MemoryRecallCommand { get; }
        public ICommand MemoryAddCommand { get; }
        public ICommand MemorySubtractCommand { get; }
        public ICommand MemoryStoreCommand { get; }
        public ICommand MemoryDropdownCommand { get; }
        public ICommand MemoryItemAddCommand { get; }
        public ICommand MemoryItemSubtractCommand { get; }
        public ICommand MemoryItemRemoveCommand { get; }
        public ICommand MemoryItemRecallCommand { get; }

        public bool IsMemoryDropdownOpen
        {
            get => _isMemoryDropdownOpen;
            set
            {
                if (_isMemoryDropdownOpen != value)
                {
                    _isMemoryDropdownOpen = value;
                    OnPropertyChanged(nameof(IsMemoryDropdownOpen));
                }
            }
        }

        public ObservableCollection<MemoryItem> MemoryItems => _memoryService.MemoryItems;

        public bool HasMemoryValue => _memoryService.HasMemoryValue;

        public StandardViewModel(DisplayModel otherDisplayModel)
        {
            displayModel = otherDisplayModel ?? throw new ArgumentNullException(nameof(otherDisplayModel));
            _calcService = new WaterfallCalcService(displayModel);
            _memoryService = new MemoryCalcService(displayModel);
            _isMemoryDropdownOpen = false;

            NumberCommand = new RelayCommand(ProcessDigit);
            OperatorCommand = new RelayCommand(ProcessOperator, item => displayModel.MainDisplayText.Length > 0 && Char.IsDigit(displayModel.MainDisplayText[^1]));
            EqualSignCommand = new RelayCommand(ProcessEqualSign);
            ChangeSignCommand = new RelayCommand(ProcessChangeSign, item => displayModel.MainDisplayText != "0");
            PointCommand = new RelayCommand(ProcessPoint, item => !displayModel.MainDisplayText.Contains("."));
            BackSpaceCommand = new RelayCommand(ProcessBackspace, item => displayModel.MainDisplayText != "0");
            ResetCommand = new RelayCommand(ProcessClear);
            ResetMainDisplayCommand = new RelayCommand(obj => { displayModel.MainDisplayText = "0"; });

            MemoryClearCommand = new RelayCommand(obj => _memoryService.MemoryClear(), item => HasMemoryValue);
            MemoryRecallCommand = new RelayCommand(obj => _memoryService.MemoryRecall(), item => HasMemoryValue);
            MemoryAddCommand = new RelayCommand(obj => _memoryService.MemoryAdd());
            MemorySubtractCommand = new RelayCommand(obj => _memoryService.MemorySubtract());
            MemoryStoreCommand = new RelayCommand(obj => _memoryService.MemoryStore());
            MemoryDropdownCommand = new RelayCommand(obj => IsMemoryDropdownOpen = !IsMemoryDropdownOpen);
            MemoryItemAddCommand = new RelayCommand(obj => _memoryService.MemoryItemAdd(obj as MemoryItem ?? throw new ArgumentNullException(nameof(obj))));
            MemoryItemSubtractCommand = new RelayCommand(obj => _memoryService.MemoryItemSubtract(obj as MemoryItem ?? throw new ArgumentNullException(nameof(obj))));
            MemoryItemRemoveCommand = new RelayCommand(ProcessMemoryItemRemove);
            MemoryItemRecallCommand = new RelayCommand(ProcessMemoryItemRecall);
        }

        public void Initialize()
        {
            ProcessClear(null);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ProcessDigit(object? parameter)
        {
            string digit = parameter as string ?? throw new ArgumentNullException(nameof(parameter));
            _calcService.ProcessDigit(digit);
        }

        private void ProcessOperator(object? parameter)
        {
            string op = parameter as string ?? throw new ArgumentNullException(nameof(parameter));
            _calcService.ProcessOperator(op);
        }

        private void ProcessChangeSign(object? parameter)
        {
            _calcService.ProcessChangeSign();
        }

        private void ProcessPoint(object? parameter)
        {
            if (!displayModel.MainDisplayText.Contains("."))
            {
                displayModel.MainDisplayText += ".";
            }
        }

        private void ProcessClear(object? parameter)
        {
            displayModel.Reset();
            _calcService.Reset();
        }

        private void ProcessBackspace(object? parameter)
        {
            if (displayModel.MainDisplayText.Length == 1 || (displayModel.MainDisplayText.Length == 2 && displayModel.MainDisplayText[0] == '-'))
            {
                displayModel.MainDisplayText = "0";
            }
            else
            {
                displayModel.MainDisplayText = displayModel.MainDisplayText.Substring(0, displayModel.MainDisplayText.Length - 1);
            }
        }

        private void ProcessEqualSign(object? parameter)
        {
            _calcService.ProcessEqualSign();
        }

        private void ProcessMemoryItemRemove(object? parameter)
        {
            _memoryService.MemoryItemRemove(parameter as MemoryItem ?? throw new ArgumentNullException(nameof(parameter)));
        }

        private void ProcessMemoryItemRecall(object? parameter)
        {
            _memoryService.MemoryItemRecall(parameter as MemoryItem ?? throw new ArgumentNullException(nameof(parameter)));
            IsMemoryDropdownOpen = false;
        }
    }
}