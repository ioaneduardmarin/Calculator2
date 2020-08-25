using System;

namespace GettingStartedWithCSharp

{
    public class BusinessLogicClass : IBusinessLogicClass
    {
        CalculatorModel _calculatorModel = new CalculatorModel();

        public string WhenResultClick(string operation, string numarAfisat, decimal value)
        {
            switch (operation)
            {
                case "+":
                    numarAfisat = Convert.ToString(value + (decimal)Double.Parse(numarAfisat));
                    return numarAfisat;
                case "-":
                    numarAfisat = Convert.ToString(value - (decimal)double.Parse(numarAfisat));

                    return numarAfisat; ;
                case "*":
                    numarAfisat = Convert.ToString(value * (decimal)double.Parse(numarAfisat));

                    return numarAfisat;
                case "/":
                    try
                    {
                        numarAfisat = Convert.ToString((value / (decimal)double.Parse(numarAfisat)));
                    }
                    catch (DivideByZeroException)
                    {
                        numarAfisat = "operatie nevalida";
                    }
                    return numarAfisat;
                case "sqrt":
                    if (_calculatorModel.Value < 0)
                    {
                        numarAfisat = "operatie nevalida";
                    }
                    else
                    {
                        numarAfisat = Convert.ToString((decimal)(Math.Sqrt((double)value)));
                    }
                    return numarAfisat;
                default:
                    return numarAfisat;
            }
        }


        public decimal MemAdd(decimal memoryValue, decimal newValue)
        {
            memoryValue += newValue;
            return memoryValue;
        }

        public decimal MemDiff(decimal memoryValue, decimal newValue)
        {
            memoryValue -= newValue;
            return memoryValue;
        }
    }
}

public interface IBusinessLogicClass
{
    string WhenResultClick(string operation, string textAfisat, decimal value);
    decimal MemAdd(decimal memoryValue, decimal newValue);
    decimal MemDiff(decimal memoryValue, decimal newValue);
}


