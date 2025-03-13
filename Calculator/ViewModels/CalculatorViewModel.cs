using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Utils;
using System.Windows.Input;
using System.Windows;

namespace Calculator.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // This method notifies the View that a property has changed
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _displayText = "0";

        // Property for binding to TextBox
        public string DisplayText
        {
            get => _displayText;
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;
                    OnPropertyChanged(nameof(DisplayText));
                }
            }
        }

        // Command for handling number button clicks
        public ICommand NumberCommand { get; }
        public ICommand ModeCommand { get; }
        public CalculatorViewModel()
        {
            NumberCommand = new RelayCommand(AppendNumber);
            ModeCommand = new RelayCommand(ChangeMode);
        }

        // Method to handle number button clicks
        private void AppendNumber(object parameter)
        {
            string digit = parameter as string;

            if (DisplayText == "0")
            {
                DisplayText = digit; // Replace initial zero
            }
            else
            {
                DisplayText += digit; // Append digit
            }
        }

        private void ChangeMode(object parameter)
        {
            DisplayText = "0";
        }
    }
}
