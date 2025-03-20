using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator.ViewModels {
    public interface IBaseViewModel {
        void Initialize();

        ICommand NumberCommand { get; }
        ICommand OperatorCommand { get; }
        ICommand EqualSignCommand { get; }
        ICommand ChangeSignCommand { get; }
        ICommand BackSpaceCommand { get; }
        ICommand ResetCommand { get; }
        ICommand ResetMainDisplayCommand { get; }
    }
}
