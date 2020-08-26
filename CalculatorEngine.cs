using System;
using System.Linq;

namespace GettingStartedWithCSharp
{
    public class CalculatorEngine : ICalculatorEngine
    {
        public decimal Value { get; set; } = 0m;
        public decimal Memory { get; set; }
        public bool HasMemoryStored { get; set; } = false;
        private readonly IMessageBoxDisplayService _messageBoxDisplayService;

        public CalculatorEngine(IMessageBoxDisplayService messageBoxDisplayService)
        {
            _messageBoxDisplayService = messageBoxDisplayService;
        }

        public string CalculusBehindResultClick(string operation, string numarAfisat)
        {
            switch (operation)
            {
                case "+":
                    numarAfisat = Convert.ToString(Value + decimal.Parse(numarAfisat));
                    return numarAfisat;
                case "-":
                    numarAfisat = Convert.ToString(Value - decimal.Parse(numarAfisat));

                    return numarAfisat;
                case "*":
                    numarAfisat = Convert.ToString(Value * decimal.Parse(numarAfisat));

                    return numarAfisat;
                case "/":
                    try
                    {
                        numarAfisat = Convert.ToString((Value / decimal.Parse(numarAfisat)));
                    }
                    catch (DivideByZeroException)
                    {
                        numarAfisat = "operatie nevalida";
                    }
                    return numarAfisat;
                case "sqrt":
                    if (Value < 0)
                    {
                        throw new ArgumentException(String.Format("{0} este un numar negativ => Nu ii putem extrage radacina patratica!", Value), "Value");
                    }
                    else
                    {
                        numarAfisat = Convert.ToString((decimal)(Math.Sqrt((double)Value)));
                    }
                    return numarAfisat;
                default:
                    return numarAfisat;
            }
        }

        public string GetFormattedShownText(string textAfisat)
        {
            if (textAfisat.ToCharArray().Count(c => c == '.') > 1)
            {
                throw new FormatException(String.Format("Aceasta nu este o valoare valida."));
            }
            else
            {
                textAfisat = FormatShownText(Convert.ToDecimal(textAfisat));
                return textAfisat;
            }
        }

        public void MemoryAdd(decimal newValue)
        {
            Memory += newValue;
        }

        public void MemoryDiff(decimal newValue)
        {
            Memory -= newValue;
        }

        public void MemoryStore(string textAfisat)
        {
            Memory = decimal.Parse(textAfisat);
            HasMemoryStored = true;
        }

        public string MemoryRestore()
        {
            string text = FormatShownText(Memory);
            return text;
        }

        public void MemoryClear()
        {
            string mesaj = "Doresti sa golesti memoria?";
            string titlu = "Golire Memorie";
            bool clearMemoryStored = _messageBoxDisplayService.PromptUser(mesaj, titlu);
            if (clearMemoryStored == true)
            {
                Memory = 0;
                HasMemoryStored = false;
            }
        }

        public string MemoryShow()
        {
            string text = FormatShownText(Memory);
            return text;
        }

        static string FormatShownText(decimal number)
        {
            return number.ToString("0.000");
        }

        public void SetValue(string textAfisat)
        {
            Value = decimal.Parse(textAfisat);
        }

        public decimal GetValue()
        {
            return Value;
        }

        public decimal ClearValue()
        {
            Value = 0;
            return Value;
        }
    }
}

public interface ICalculatorEngine
{
    string CalculusBehindResultClick(string operation, string textAfisat);
    string GetFormattedShownText(string textAfisat);
    void MemoryAdd(decimal newValue);
    void MemoryDiff(decimal newValue);
    void MemoryStore(string textAfisat);
    string MemoryRestore();
    void MemoryClear();
    string MemoryShow();
    void SetValue(string textAfisat);
    decimal GetValue();
    decimal ClearValue();

    bool HasMemoryStored { get; set; }
}


