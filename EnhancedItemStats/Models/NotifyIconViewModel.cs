namespace EnhancedItemStats.Models {
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class NotifyIconViewModel {
        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand {
            get {
                return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
            }
        }
    }

    public class DelegateCommand : ICommand {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter) {
            this.CommandAction();
        }

        public bool CanExecute(object parameter) {
            return this.CanExecuteFunc == null || this.CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
