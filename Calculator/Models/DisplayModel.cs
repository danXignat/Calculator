using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Models {
    internal class DisplayModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _mainDisplayText = "0";
        private string _tempDisplayText = "";

        public string MainDisplayText {
            get => _mainDisplayText;
            set {
                if (_mainDisplayText != value) {
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

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Reset() {
            MainDisplayText = "0";
            TempDisplayText = "";
        }
    }
}
