using System.Windows;
using System.Windows.Controls;
using Calculator.View;

namespace Calculator {
    public partial class MainWindow : Window {
        private StandardView standardView;
        private ProgrammerView programmerView;

        public MainWindow() {
            InitializeComponent();

            standardView = new StandardView();
            programmerView = new ProgrammerView();

            CalculatorViewHost.Content = standardView;
        }

        private void CalculatorMode_Changed(object sender, RoutedEventArgs e) {
            if (CalculatorViewHost == null)
                return;

            if (sender == StandardRadio && StandardRadio.IsChecked == true) {
                CalculatorViewHost.Content = standardView;
            }
            else if (sender == ProgrammerRadio && ProgrammerRadio.IsChecked == true) {
                CalculatorViewHost.Content = programmerView;
            }
        }
    }
}