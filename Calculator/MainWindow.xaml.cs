using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Calculator.View;
using Calculator.ViewModels;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        private StandardView standardView;
        private ProgrammerView programmerView;

        public MainWindow()
        {
            InitializeComponent();

            standardView = new StandardView();
            programmerView = new ProgrammerView();

            CalculatorViewHost.Content = standardView;
            DataContext = new MainViewModel();
        }

        private void StandardButton_Click(object sender, RoutedEventArgs e)
        {
            StandardTab.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#14CA9C");
            ProgrammerTab.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("Transparent");
            StandardButton.Foreground = Brushes.White;
            ProgrammerButton.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B0B0B0");

            CalculatorViewHost.Content = standardView;
        }

        private void ProgrammerButton_Click(object sender, RoutedEventArgs e)
        {
            ProgrammerTab.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#14CA9C");
            StandardTab.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("Transparent");
            ProgrammerButton.Foreground = Brushes.White;
            StandardButton.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B0B0B0");

            CalculatorViewHost.Content = programmerView;
        }
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this; // Set the owner to center the about window relative to main window
            aboutWindow.ShowDialog(); // Show as dialog to block interaction with main window
        }

    }
}