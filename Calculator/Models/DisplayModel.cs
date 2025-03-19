using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Models {
    public class DisplayModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _mainDisplayText = "0";
        private string _tempDisplayText = "";

        private string _hexDisplay = "0";
        private string _decDisplay = "0";
        private string _octDisplay = "0";
        private string _binDisplay = "0";

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
        public string HexDisplayText {
            get => _hexDisplay;
            set {
                if (_hexDisplay != value) {
                    _hexDisplay = value;
                    OnPropertyChanged(nameof(HexDisplayText));
                }
            }
        }

        public string DecDisplayText {
            get => _decDisplay;
            set {
                if (_decDisplay != value) {
                    _decDisplay = value;
                    OnPropertyChanged(nameof(DecDisplayText));
                }
            }
        }

        public string OctDisplayText
        {
            get => _octDisplay;
            set {
                if (_octDisplay != value) {
                    _octDisplay = value;
                    OnPropertyChanged(nameof(OctDisplayText));
                }
            }
        }

        public string BinDisplayText
        {
            get => _binDisplay;
            set {
                if (_binDisplay != value) {
                    _binDisplay = value;
                    OnPropertyChanged(nameof(BinDisplayText));
                }
            }
        }
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Reset() {
            MainDisplayText = "0";
            TempDisplayText = "";

            HexDisplayText = "0";
            DecDisplayText = "0";
            OctDisplayText = "0";
            BinDisplayText = "0";
        }
    }
}
