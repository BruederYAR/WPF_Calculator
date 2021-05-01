using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using calculator.Infrastructure.Commans;
using calculator.Views;
using calculator.Models;
using System.Windows;

namespace calculator.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private JournalPage journalPage = new JournalPage();
        private Page memoryPage = new MemoryPage();

        private ExpressionModel numberModel1 = new ExpressionModel();
        private ExpressionModel numberModel2 = new ExpressionModel();

        public Page CurrentPage { get; set; }
        public string Title { get; set; }

        private string number;
        public string Number {
            get => number;
            private set
            {
                number = value;
                NotifyPropertyChanged(nameof(Number));
            }
        }

        private string expression;
        public string ExpressionNumber 
        {
            get => expression;
            private set
            {
                expression = value;
                NotifyPropertyChanged(nameof(ExpressionNumber));
            } 
        }

        public MainWindowViewModel()
        {
            CurrentPage = memoryPage; //Текущая страница

            Title = "Калькулятор";

            OpenJournalPageCommand = new LambdaCommand(OnOpenJournalPageCommandExecuted, CanStandartCommandExecute); 
            OpenMemoryPageCommand = new LambdaCommand(OnOpenMemoryPageCommandExecuted, CanStandartCommandExecute);

            NumberButtonCommand = new LambdaCommand(OnNumberButtonCommandExecuted, CanStandartCommandExecute);
            OperatorButtonCommand = new LambdaCommand(OnOperatorButtonCommandExecuted, CanStandartCommandExecute);
            HardOperatorButtonCommand = new LambdaCommand(OnHardOperatorButtonCommandExecuted, CanStandartCommandExecute);
            ManagementButtonCommand = new LambdaCommand(OnManagementButtonCommandExecuted, CanStandartCommandExecute);
            NotifyPropertyChanged();
        }

        #region Команды
        #region Страницы
        private bool CanStandartCommandExecute(object p) => true;
        public ICommand OpenJournalPageCommand { get; }
        private void OnOpenJournalPageCommandExecuted(object p)
        {
            CurrentPage = journalPage;
            NotifyPropertyChanged(nameof(CurrentPage));
        }
        
        public ICommand OpenMemoryPageCommand { get; }
        private void OnOpenMemoryPageCommandExecuted(object p)
        {
            CurrentPage = memoryPage;
            NotifyPropertyChanged(nameof(CurrentPage));
        }
        #endregion

        #region Запись числа
        public ICommand NumberButtonCommand { get; }
        private bool NumberFlag = false; //Нужно для записи числа и ответа в 1 строку

        private void OnNumberButtonCommandExecuted(object p)
        {
            if(p != null)
            {
                if(NumberFlag) //Если ответ в строке, то обновляем строку
                {
                    Number = null;
                    NumberFlag = false;
                }

                Number += p.ToString();

                if (Number[0] == ',')
                    Number = Number.Substring(1);
                if (Number[0] == '0' && Number.Length == 1)
                    Number += ',';  
            }
        }
        #endregion

        #region Выражение
        public ICommand HardOperatorButtonCommand { get; }
        private void OnHardOperatorButtonCommandExecuted(object p)
        {
            if(ExpressionFlag && !NumberFlag)
            {
                SympleExpressionExecution(Symbol);
                HardExpressionExecution(p.ToString());
            }
            else
            {
                numberModel1.Number = Convert.ToDouble(Number);
                HardExpressionExecution(p.ToString());
            }

            

            NumberFlag = true; //Нужно, чтобы заменять ответ на 2 число
        }

        private bool ExpressionFlag = false;
        private string Symbol;
        public ICommand OperatorButtonCommand { get; }
        private void OnOperatorButtonCommandExecuted(object p)
        {
            if (p != null)
            {
                if (Symbol != p.ToString() & !NumberFlag & ExpressionFlag) //Нужно, чтобы при смене символа прорешалась прошлая задача
                {
                    SympleExpressionExecution(Symbol);
                    ExpressionFlag = false;
                }
                else if (Symbol != p.ToString()) //Если просто меняем символ
                {
                    Symbol = p.ToString();

                    if (ExpressionNumber != null)
                    {
                        ExpressionNumber = ExpressionNumber.Remove(ExpressionNumber.Length - 1);
                        ExpressionNumber += Symbol;
                    }
                }

                if (!ExpressionFlag) //Если задача не началась
                {
                    ExpressionNumber = null; //ОБнуляем прошлые задачи, чтобы не было нагромождения
                    ExpressionNumber += Number;
                    numberModel1.Number = Convert.ToDouble(Number);

                    ExpressionNumber += p.ToString();
                    ExpressionFlag = true;
                }
                else if(ExpressionFlag && !NumberFlag) //Если задача началась, и есть 2 число, выполняем задачу 2 значениями
                {
                    SympleExpressionExecution(Symbol);
                }

                NumberFlag = true; //Нужно, чтобы заменять ответ на 2 число

            }
            
        }

        private bool HardExpressionExecution(string Symbol = "")
        {
            bool result = false;
            string journalAnswer = null;


            switch (Symbol)
            {
                case "1/x":
                    journalAnswer += $"1 / {numberModel1.Number.ToString()} = ";
                    numberModel1.Number = 1 / numberModel1;
                    result = true;
                    break;

                case "x^2":
                    journalAnswer += $"{numberModel1.Number.ToString()} ^ 2 = ";
                    numberModel1.Number = numberModel1 * numberModel1;
                    result = true;
                    break;
                case "sqrt(x)":
                    journalAnswer += $"sqrt({numberModel1.Number.ToString()}) = ";
                    numberModel1.Sqrt();
                    result = true;
                    break;
            }


            if(result)
            {
                ExpressionNumber = journalAnswer.Remove(journalAnswer.Length -2);
                journalAnswer += numberModel1.Number.ToString();

                //Добавляем на страницу журнала новую запись
                journalPage.journalPageViewModel.Expressions.Add(journalAnswer);
                NotifyPropertyChanged(nameof(CurrentPage));

                numberModel2.Number = 0;

                Number = numberModel1.Number.ToString();

                Symbol = null;
                NumberFlag = false;
                ExpressionFlag = false;
            }
            

            return result;
        }

        private void SympleExpressionExecution(string Symbol = "") //Простые вычисления с 2 значениями
        {
            numberModel2.Number = Convert.ToDouble(Number);

            string journalAnswer = $"{numberModel1.Number.ToString()} {Symbol} {numberModel2.Number.ToString()} = "; //Запись в журнал
            switch (Symbol)
            {
                case "+":
                    numberModel1.Number = numberModel1 + numberModel2;
                    break;
                case "-":
                    numberModel1.Number = numberModel1 - numberModel2;
                    break;
                case "*":
                    numberModel1.Number = numberModel1 * numberModel2;
                    break;
                case "/":
                    numberModel1.Number = numberModel1 / numberModel2;
                    break;
            }
            journalAnswer += numberModel1.Number.ToString();

            //Добавляем на страницу журнала новую запись
            journalPage.journalPageViewModel.Expressions.Add(journalAnswer);
            NotifyPropertyChanged(nameof(CurrentPage));


            numberModel2.Number = 0; //Обнуляем до следующего раза

            //Обновление строк графики
            Number = numberModel1.Number.ToString();
            ExpressionNumber = Number + Symbol;
        }
        #endregion

        #region Управление
        public ICommand ManagementButtonCommand { get; }
        private void OnManagementButtonCommandExecuted(object p)
        {
            if (p != null)
            {
                switch (p.ToString())
                {
                    case "C":
                        Number = null;
                        ExpressionNumber = null;
                        Symbol = null;
                        NumberFlag = false;
                        ExpressionFlag = false;
                        break;
                    case "CE":
                        Number = null;
                        NumberFlag = false;
                        break;
                    case "Er":
                        if (Number != null && Number != "")
                            Number = Number.Remove(Number.Length - 1);
                        break;
                    case "=":
                        if (!HardExpressionExecution(Symbol))
                        {
                            SympleExpressionExecution(Symbol);
                            ExpressionFlag = false;
                            NumberFlag = false;
                        }
                        break;
                }
            }
        }
        #endregion

        #endregion

    }
}
