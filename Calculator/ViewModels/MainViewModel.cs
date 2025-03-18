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


    public class MainViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private DisplayModel _displayModel;
        private StandardViewModel _standardViewModel;
        private ProgrammerViewModel _programmerViewModel;
        private IBaseViewModel _currentViewModel;

        public IBaseViewModel CurrentViewModel {
            get => _currentViewModel;
            set {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
                OnPropertyChanged(nameof(NumberCommand));
                OnPropertyChanged(nameof(OperatorCommand));
                OnPropertyChanged(nameof(EqualSignCommand));
                OnPropertyChanged(nameof(ChangeSignCommand));
                OnPropertyChanged(nameof(BackSpaceCommand));
                OnPropertyChanged(nameof(ResetCommand));
                OnPropertyChanged(nameof(ResetMainDisplayCommand));
                OnPropertyChanged(nameof(BaseCommand));
            }
        }

        public ICommand NumberCommand => CurrentViewModel.NumberCommand;
        public ICommand OperatorCommand => CurrentViewModel.OperatorCommand;
        public ICommand EqualSignCommand => CurrentViewModel.EqualSignCommand;
        public ICommand ChangeSignCommand => CurrentViewModel.ChangeSignCommand;
        public ICommand BackSpaceCommand => CurrentViewModel.BackSpaceCommand;
        public ICommand ResetCommand => CurrentViewModel.ResetCommand;
        public ICommand ResetMainDisplayCommand => CurrentViewModel.ResetMainDisplayCommand;

        
        public ICommand BaseCommand => (CurrentViewModel as ProgrammerViewModel)?.BaseCommand;
        public ICommand BitwiseCommand => (CurrentViewModel as ProgrammerViewModel)?.BitwiseCommand;

        
        public ICommand ModeCommand { get; }

        public string MainDisplayText {
            get => _displayModel.MainDisplayText;
            set => _displayModel.MainDisplayText = value;
        }

        public string TempDisplayText {
            get => _displayModel.TempDisplayText;
            set => _displayModel.TempDisplayText = value;
        }
        public string HexDisplayText {
            get => _displayModel.HexDisplay;
            set => _displayModel.HexDisplay = value;
        }
        public string DecDisplayText {
            get => _displayModel.DecDisplay;
            set => _displayModel.DecDisplay = value;
        }
        public string OctDisplayText {
            get => _displayModel.OctDisplay;
            set => _displayModel.OctDisplay = value;
        }
        public string BinDisplayText {
            get => _displayModel.BinDisplay;
            set => _displayModel.BinDisplay = value;
        }

        public MainViewModel() {
            _displayModel = new DisplayModel();
            _displayModel.PropertyChanged += (sender, args) => {
                OnPropertyChanged(args.PropertyName);
            };

            _standardViewModel = new StandardViewModel(_displayModel);
            _programmerViewModel = new ProgrammerViewModel(_displayModel);

            CurrentViewModel = _standardViewModel;
            ModeCommand = new RelayCommand(ChangeMode);
        }

        private void ChangeMode(object parameter) {
            string mode = parameter as string;

            switch (mode) {
                case "Standard":
                    CurrentViewModel = _standardViewModel;
                    break;
                case "Programmer":
                    CurrentViewModel = _programmerViewModel;
                    break;
            }

            CurrentViewModel.Initialize();
        }

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
