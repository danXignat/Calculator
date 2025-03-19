using System.Windows;
using System.Windows.Controls;
using Calculator.ViewModels;

namespace Calculator.View
{
    public partial class StandardView : UserControl {
        public StandardView() {
            InitializeComponent();
        }

        private void MemoryButton_Click(object sender, RoutedEventArgs e) {
            var viewModel = DataContext as MainViewModel;

            if (viewModel != null) {
                viewModel.IsMemoryDropdownOpen = !viewModel.IsMemoryDropdownOpen;
            }
            else {
                var standardVM = DataContext as StandardViewModel;

                if (standardVM != null) {
                    standardVM.IsMemoryDropdownOpen = !standardVM.IsMemoryDropdownOpen;
                }
            }
        }
    }
}