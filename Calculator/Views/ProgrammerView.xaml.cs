using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Calculator.View {
    public partial class ProgrammerView : UserControl {
        private string currentNumberSystem = "Dec";

        private readonly SolidColorBrush accentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14CA9C"));
        private readonly SolidColorBrush transparentBrush = new SolidColorBrush(Colors.Transparent);

        private readonly SolidColorBrush activeTextBrush = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush inactiveTextBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B0B0B0"));

        public ProgrammerView() {
            InitializeComponent();

            SetActiveNumberSystem("Dec");
        }

        private void SetActiveNumberSystem(string numberSystem) {
            currentNumberSystem = numberSystem;

            HexTab.BorderBrush = transparentBrush;
            DecTab.BorderBrush = transparentBrush;
            OctTab.BorderBrush = transparentBrush;
            BinTab.BorderBrush = transparentBrush;

            HexButton.Foreground = inactiveTextBrush;
            DecButton.Foreground = inactiveTextBrush;
            OctButton.Foreground = inactiveTextBrush;
            BinButton.Foreground = inactiveTextBrush;

            switch (numberSystem.ToLower()) {
                case "hex":
                    HexTab.BorderBrush = accentBrush;
                    HexButton.Foreground = activeTextBrush;
                    break;

                case "dec":
                    DecTab.BorderBrush = accentBrush;
                    DecButton.Foreground = activeTextBrush;
                    break;

                case "oct":
                    OctTab.BorderBrush = accentBrush;
                    OctButton.Foreground = activeTextBrush;
                    break;

                case "bin":
                    BinTab.BorderBrush = accentBrush;
                    BinButton.Foreground = activeTextBrush;
                    break;
            }
        }

        private void HexButton_Click(object sender, RoutedEventArgs e) {
            SetActiveNumberSystem("Hex");
        }

        private void DecButton_Click(object sender, RoutedEventArgs e) {
            SetActiveNumberSystem("Dec");
        }

        private void OctButton_Click(object sender, RoutedEventArgs e) {
            SetActiveNumberSystem("Oct");
        }

        private void BinButton_Click(object sender, RoutedEventArgs e) {
            SetActiveNumberSystem("Bin");
        }
    }
}