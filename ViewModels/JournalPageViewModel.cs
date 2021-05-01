using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculator.ViewModels
{
    public class JournalPageViewModel : BaseViewModel
    {
        public ObservableCollection<string> Expressions { get; set; }
        public JournalPageViewModel()
        {
            Expressions = new ObservableCollection<string>();
        }
    }

}
