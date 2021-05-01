using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculator.Models
{
    public class ExpressionModel
    {

        public double Number { get; set; }
        //public string Expression { get; set; }

        public static double operator +(ExpressionModel model1, ExpressionModel model2) => model1.Number + model2.Number;
        public static double operator -(ExpressionModel model1, ExpressionModel model2) => model1.Number - model2.Number;
        public static double operator *(ExpressionModel model1, ExpressionModel model2) => model1.Number * model2.Number;
        public static double operator /(ExpressionModel model1, ExpressionModel model2) => model1.Number / model2.Number;
        public static double operator /(int model1, ExpressionModel model2) => model1/ model2.Number;

        public void Sqrt(ExpressionModel model = null)
        {
            if (model != null)
                Number = Math.Sqrt(model.Number);
            else
                Number = Math.Sqrt(Number);
        }
    }
}
