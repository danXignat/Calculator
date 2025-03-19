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
using System.Diagnostics;
using Calculator.Services;

namespace Calculator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DisplayModel _displayModel;
        private StandardViewModel _standardViewModel;
        private ProgrammerViewModel _programmerViewModel;
        private IBaseViewModel _currentViewModel;
        private DigitGroupingService _digitGroupingService;

        public IBaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));

                OnPropertyChanged(nameof(NumberCommand));
                OnPropertyChanged(nameof(OperatorCommand));
                OnPropertyChanged(nameof(EqualSignCommand));
                OnPropertyChanged(nameof(ChangeSignCommand));
                OnPropertyChanged(nameof(BackSpaceCommand));
                OnPropertyChanged(nameof(ResetCommand));
                OnPropertyChanged(nameof(ResetMainDisplayCommand));
                OnPropertyChanged(nameof(PointCommand));
                OnPropertyChanged(nameof(BaseCommand));

                OnPropertyChanged(nameof(MemoryClearCommand));
                OnPropertyChanged(nameof(MemoryRecallCommand));
                OnPropertyChanged(nameof(MemoryAddCommand));
                OnPropertyChanged(nameof(MemorySubtractCommand));
                OnPropertyChanged(nameof(MemoryStoreCommand));
                OnPropertyChanged(nameof(MemoryDropdownCommand));
                OnPropertyChanged(nameof(MemoryItemAddCommand));
                OnPropertyChanged(nameof(MemoryItemSubtractCommand));
                OnPropertyChanged(nameof(MemoryItemRemoveCommand));
                OnPropertyChanged(nameof(MemoryItemRecallCommand));
                OnPropertyChanged(nameof(IsMemoryDropdownOpen));
                OnPropertyChanged(nameof(MemoryItems));
                OnPropertyChanged(nameof(HasMemoryValue));
            }
        }

        //------------------------------------------------------------------------------------Basic calculator commands
        public ICommand NumberCommand => CurrentViewModel.NumberCommand;
        public ICommand OperatorCommand => CurrentViewModel.OperatorCommand;
        public ICommand EqualSignCommand => CurrentViewModel.EqualSignCommand;
        public ICommand ChangeSignCommand => CurrentViewModel.ChangeSignCommand;
        public ICommand BackSpaceCommand => CurrentViewModel.BackSpaceCommand;
        public ICommand ResetCommand => CurrentViewModel.ResetCommand;
        public ICommand ResetMainDisplayCommand => CurrentViewModel.ResetMainDisplayCommand;
        public ICommand PointCommand => (CurrentViewModel as StandardViewModel)?.PointCommand;

        //------------------------------------------------------------------------------------Programmer calculator specific commands
        public ICommand BaseCommand => (CurrentViewModel as ProgrammerViewModel)?.BaseCommand;
        public ICommand BitwiseCommand => (CurrentViewModel as ProgrammerViewModel)?.BitwiseCommand;

        //------------------------------------------------------------------------------------Memory commands
        public ICommand MemoryClearCommand => (CurrentViewModel as StandardViewModel)?.MemoryClearCommand;
        public ICommand MemoryRecallCommand => (CurrentViewModel as StandardViewModel)?.MemoryRecallCommand;
        public ICommand MemoryAddCommand => (CurrentViewModel as StandardViewModel)?.MemoryAddCommand;
        public ICommand MemorySubtractCommand => (CurrentViewModel as StandardViewModel)?.MemorySubtractCommand;
        public ICommand MemoryStoreCommand => (CurrentViewModel as StandardViewModel)?.MemoryStoreCommand;
        public ICommand MemoryDropdownCommand => (CurrentViewModel as StandardViewModel)?.MemoryDropdownCommand;
        public ICommand MemoryItemAddCommand => (CurrentViewModel as StandardViewModel)?.MemoryItemAddCommand;
        public ICommand MemoryItemSubtractCommand => (CurrentViewModel as StandardViewModel)?.MemoryItemSubtractCommand;
        public ICommand MemoryItemRemoveCommand => (CurrentViewModel as StandardViewModel)?.MemoryItemRemoveCommand;
        public ICommand MemoryItemRecallCommand => (CurrentViewModel as StandardViewModel)?.MemoryItemRecallCommand;

        //--------------------------------------------------------------------------------Memory properties
        public bool IsMemoryDropdownOpen
        {
            get => (CurrentViewModel as StandardViewModel)?.IsMemoryDropdownOpen ?? false;
            set
            {
                if (CurrentViewModel is StandardViewModel standardVM)
                {
                    standardVM.IsMemoryDropdownOpen = value;
                    OnPropertyChanged(nameof(IsMemoryDropdownOpen));
                }
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<MemoryItem> MemoryItems =>
            (CurrentViewModel as StandardViewModel)?.MemoryItems;

        public bool HasMemoryValue =>
            (CurrentViewModel as StandardViewModel)?.HasMemoryValue ?? false;

        //-----------------------------------------------------------------------Mode
        public ICommand ModeCommand { get; }

        //---------------------------------------------------------------------Copy paste

        public ICommand CutCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }

        //-----------------------------------------------------------------Display properties

        public ICommand ToggleDigitGroupingCommand { get; }

        //------------------------------------------------------------------------Digit grouping
        public string MainDisplayText
        {
            get => _digitGroupingService.GetFormattedNumber(_displayModel.MainDisplayText);
            set => _displayModel.MainDisplayText = value;
        }

        public string TempDisplayText
        {
            get => _displayModel.TempDisplayText;
            set => _displayModel.TempDisplayText = value;
        }
        public string HexDisplayText
        {
            get => _displayModel.HexDisplayText;
            set => _displayModel.HexDisplayText = value;
        }
        public string DecDisplayText
        {
            get => _displayModel.DecDisplayText;
            set => _displayModel.DecDisplayText = value;
        }
        public string OctDisplayText
        {
            get => _displayModel.OctDisplayText;
            set => _displayModel.OctDisplayText = value;
        }
        public string BinDisplayText
        {
            get => _displayModel.BinDisplayText;
            set => _displayModel.BinDisplayText = value;
        }

        public MainViewModel()
        {
            _displayModel = new DisplayModel();
            _displayModel.PropertyChanged += (sender, args) =>
            {
                OnPropertyChanged(args.PropertyName);
            };

            _standardViewModel = new StandardViewModel(_displayModel);
            _programmerViewModel = new ProgrammerViewModel(_displayModel);
            _digitGroupingService = new DigitGroupingService();

            CurrentViewModel = _standardViewModel;
            ModeCommand = new RelayCommand(ChangeMode);

            CutCommand = new RelayCommand(obj => ExecuteCut());
            CopyCommand = new RelayCommand(obj => ExecuteCopy());
            PasteCommand = new RelayCommand(obj => ExecutePaste());

            ToggleDigitGroupingCommand = new RelayCommand(obj => ExecuteToggleDigitGrouping());
        }

        private void ChangeMode(object parameter)
        {
            string mode = parameter as string;

            switch (mode)
            {
                case "Standard":
                    CurrentViewModel = _standardViewModel;
                    break;
                case "Programmer":
                    CurrentViewModel = _programmerViewModel;
                    break;
            }

            CurrentViewModel.Initialize();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ExecuteCut()
        {
            Clipboard.SetText(MainDisplayText);

            _displayModel.MainDisplayText = "0";
        }
        
        private void ExecuteCopy()
        {
            Clipboard.SetText(MainDisplayText);
        }

        private void ExecutePaste()
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    string clipboardText = Clipboard.GetText();

                    if (double.TryParse(clipboardText, out double result))
                    {
                        _displayModel.MainDisplayText = clipboardText;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Clipboard error: {ex.Message}");
            }
        }
        
        private void ExecuteToggleDigitGrouping()
        {
            _digitGroupingService.IsDigitGroupingEnabled = !_digitGroupingService.IsDigitGroupingEnabled;

            OnPropertyChanged(nameof(MainDisplayText));
        }
    }

}