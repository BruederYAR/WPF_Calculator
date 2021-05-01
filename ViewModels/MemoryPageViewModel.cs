using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using calculator.Infrastructure.Commans;

namespace calculator.ViewModels
{
    public class MemoryPageViewModel : BaseViewModel
    {
        MainWindowViewModel SenderViewModel;
        public ObservableCollection<string> Numbers { get; set; }

        private string selectedNumber;
        public string SelectedNumber
        {
            get => selectedNumber;
            set
            {
                selectedNumber = value;
                SenderViewModel.Number = SelectedNumber;
            }
        }
        
        public MemoryPageViewModel()
        {
            Numbers = new ObservableCollection<string>();
            DeleteCommand = new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
        }
        public MemoryPageViewModel(MainWindowViewModel sender) : this()
        {
            SenderViewModel = sender;
        }

        public ICommand DeleteCommand { get; }
        private bool CanDeleteCommandExecute(object p) => true;
        private void OnDeleteCommandExecuted(object p)
        {
            Numbers = new ObservableCollection<string>();
            NotifyPropertyChanged(nameof(Numbers));
        }

    }
}
