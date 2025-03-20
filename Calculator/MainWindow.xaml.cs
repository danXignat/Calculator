using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Calculator.View;
using Calculator.ViewModels;
using System.Configuration;

namespace Calculator {
    public partial class MainWindow : Window {
        private StandardView standardView;
        private ProgrammerView programmerView;
        private const string StandardMode = "Standard";
        private const string ProgrammerMode = "Programmer";

        public MainWindow() {
            InitializeComponent();
            standardView = new StandardView();
            programmerView = new ProgrammerView();
            DataContext = new MainViewModel();

            string lastMode = Properties.Settings.Default.LastUsedMode;
            if (lastMode == ProgrammerMode) {
                CalculatorViewHost.Content = programmerView;
                SetProgrammerActive();
            }
            else {
                CalculatorViewHost.Content = standardView;
                SetStandardActive();
            }
        }

        private void SetStandardActive() {
            StandardTab.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#14CA9C");
            ProgrammerTab.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("Transparent");
            StandardButton.Foreground = Brushes.White;
            ProgrammerButton.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B0B0B0");
        }

        private void SetProgrammerActive() {
            ProgrammerTab.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#14CA9C");
            StandardTab.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("Transparent");
            ProgrammerButton.Foreground = Brushes.White;
            StandardButton.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B0B0B0");
        }

        private void StandardButton_Click(object sender, RoutedEventArgs e) {
            SetStandardActive();
            CalculatorViewHost.Content = standardView;

            Properties.Settings.Default.LastUsedMode = StandardMode;
            Properties.Settings.Default.Save();
        }

        private void ProgrammerButton_Click(object sender, RoutedEventArgs e) {
            SetProgrammerActive();
            CalculatorViewHost.Content = programmerView;

            Properties.Settings.Default.LastUsedMode = ProgrammerMode;
            Properties.Settings.Default.Save();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e) {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }
    }
}