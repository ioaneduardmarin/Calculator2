using System;

namespace GettingStartedWithCSharp

{
    public class BusinessLogicClass : IBusinessLogicClass
    {
        CalculatorModel _calculatorModel = new CalculatorModel();
        
        public string WhenResultClick(string operation, string numarAfisat)
        {
            switch (operation)
            {
                case "+":
                    numarAfisat = Convert.ToString(_calculatorModel.Value + (decimal)Double.Parse(numarAfisat));
                    return numarAfisat;
                case "-":
                    numarAfisat = Convert.ToString(_calculatorModel.Value - (decimal)double.Parse(numarAfisat));

                    return numarAfisat; ;
                case "*":
                    numarAfisat = Convert.ToString(_calculatorModel.Value * (decimal)double.Parse(numarAfisat));

                    return numarAfisat;
                case "/":
                    try
                    {
                        numarAfisat = Convert.ToString((_calculatorModel.Value / (decimal)double.Parse(numarAfisat)));
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
                        numarAfisat = Convert.ToString((decimal)(Math.Sqrt((double)_calculatorModel.Value)));
                    }
                    return numarAfisat;
                default:
                    return numarAfisat;
            }
        }

        public string GetFormattedShownText(string textAfisat) 
        {
            textAfisat= FormatShownText(Convert.ToDecimal(textAfisat));
            return textAfisat;
        }

        public void MemoryAdd(decimal newValue)
        {
            _calculatorModel.Memory += newValue;
        }

        public void MemoryDiff(decimal newValue)
        {
            _calculatorModel.Memory -= newValue;
        }

        public void MemoryStore(string textAfisat)
        {
            _calculatorModel.Memory = (decimal)Double.Parse(textAfisat);
        }

        public string MemoryRestore()
        {
            string text = FormatShownText(_calculatorModel.Memory);
            return text;
        }

        public void MemoryClear()
        {
            _calculatorModel.Memory = 0;
        }

        public string MemoryShow()
        {
            string text = FormatShownText(_calculatorModel.Memory);
            return text;
        }

        public string FormatShownText(decimal number)
        {
            return number.ToString("0.000");
        }

        public void GetValue(string textAfisat)
        {
            _calculatorModel.Value=(decimal)(Double.Parse(textAfisat));
        }

        public decimal ReturnValue()
        {
            return _calculatorModel.Value;
        }

        public decimal ClearValue()
        {
            _calculatorModel.Value = 0;
            return _calculatorModel.Value;
        }
    }
}

public interface IBusinessLogicClass
{
    string WhenResultClick(string operation, string textAfisat);
    string GetFormattedShownText(string textAfisat);
    void MemoryAdd(decimal newValue);
    void MemoryDiff(decimal newValue);
    void MemoryStore(string textAfisat);
    string MemoryRestore();
    void MemoryClear();
    string MemoryShow();
    void GetValue(string textAfisat);
    decimal ReturnValue();
    decimal ClearValue();
}


