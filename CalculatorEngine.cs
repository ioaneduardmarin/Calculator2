using System;
using System.Linq;

namespace GettingStartedWithCSharp
{
    public class CalculatorEngine : ICalculatorEngine
    {
        private decimal _value = 0m;
        private decimal _memory;
        public bool HasMemoryStored { get; private set; } = false;
        private readonly IMessageBoxDisplayService _messageBoxDisplayService;
        private readonly IUtils _utils;

        public CalculatorEngine(IMessageBoxDisplayService messageBoxDisplayService, IUtils utils)
        {
            _messageBoxDisplayService = messageBoxDisplayService;
            _utils = utils;
        }

        public decimal GetNumberToBeShown(string operation, decimal numarAfisat)
        {
            switch (operation)
            {
                case "+":
                    _value = _value + numarAfisat;
                    return _value;
                case "-":
                    _value =_value - numarAfisat;

                    return _value;
                case "*":
                    _value = _value * numarAfisat;
                    return _value;
                case "/":
                    try
                    {
                        _value = _value / numarAfisat;
                    }
                    catch (DivideByZeroException)
                    {
                        throw new ArgumentException(String.Format("Numarul {0} nu poate fi impartit la 0.", _value), "Value");
                    }
                    return _value;
                case "sqrt":
                    if (_value < 0)
                    {
                        throw new ArgumentException(String.Format("{0} este un numar negativ => Nu ii putem extrage radacina patratica!", _value), "Value");
                    }

                    _value = (decimal)(Math.Sqrt((double)_value));
                    
                    return _value;
                default:
                    return _value;
            }
        }

        public void MemoryAdd(decimal newValue)
        {
            _memory += newValue;
        }

        public void MemoryDiff(decimal newValue)
        {
            _memory -= newValue;
        }

        public void MemoryStore(string textAfisat)
        {
            _memory = decimal.Parse(textAfisat);
            HasMemoryStored = true;
        }

        public string MemoryRestore()
        {
            string text = _utils.FormatShownText(_memory);
            return text;
        }

        public void MemoryClear()
        {
            string mesaj = "Doresti sa golesti memoria?";
            string titlu = "Golire Memorie";
            bool clearMemoryStored = _messageBoxDisplayService.PromptUser(mesaj, titlu);
            if (clearMemoryStored == true)
            {
                _memory = 0;
                HasMemoryStored = false;
            }
        }

        public string MemoryShow()
        {
            string text = _utils.FormatShownText(_memory);
            return text;
        }

        public void SetValue(string textAfisat)
        {
            _value = decimal.Parse(textAfisat);
        }

        public decimal GetValue()
        {
            return _value;
        }

        public decimal ClearValue()
        {
            _value = 0;
            return _value;
        }
    }
}

public interface ICalculatorEngine
{
    decimal GetNumberToBeShown(string operation, decimal textAfisat);
    void MemoryAdd(decimal newValue);
    void MemoryDiff(decimal newValue);
    void MemoryStore(string textAfisat);
    string MemoryRestore();
    void MemoryClear();
    string MemoryShow();
    void SetValue(string textAfisat);
    decimal ClearValue();

    bool HasMemoryStored { get; }
}


