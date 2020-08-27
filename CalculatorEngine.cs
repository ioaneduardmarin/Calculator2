using System;

namespace GettingStartedWithCSharp
{
    public class CalculatorEngine : ICalculatorEngine
    {
        public decimal Value { get; private set; }
        private decimal _memory;
        public bool HasMemoryStored { get; private set; }

        public decimal SubmitOperation(string operation, decimal shownNumber)
        {

            switch (operation)
            {
                case "=":
                    Value = shownNumber;
                    return Value;
                case "+":
                    Value += shownNumber;
                    return Value;
                case "-":
                    Value -= shownNumber;

                    return Value;
                case "*":
                    Value *= shownNumber;
                    return Value;
                case "/":
                    try
                    {
                        Value /= shownNumber;
                    }
                    catch (DivideByZeroException)
                    {
                        var message = String.Format($"Numarul {Value} nu poate fi impartit la 0.");
                        throw new ArgumentException(message);
                    }
                    return Value;
                case "sqrt":
                    if (Value < 0)
                    {
                        var message = String.Format($"{Value} este un numar negativ => Nu ii putem extrage radacina patratica!");
                        throw new ArgumentException(message);
                    }
                    Value = (decimal)(Math.Sqrt((double)Value));
                    return Value;
                default:
                    return Value;
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

        public void MemoryStore(decimal shownNumber)
        {
            _memory = shownNumber;
            HasMemoryStored = true;
        }

        public decimal MemoryRestore()
        {
            return _memory;
        }

        public void MemoryClear()
        {
            _memory = 0m;
            HasMemoryStored = false;
        }

        public decimal MemoryShow()
        {
            return _memory;
        }

        public decimal ClearValue()
        {
            Value = 0m;
            return Value;
        }
    }
}

public interface ICalculatorEngine
{
    decimal SubmitOperation(string operation, decimal shownNumber);
    void MemoryClear();
    decimal MemoryRestore();
    void MemoryStore(decimal shownNumber);
    void MemoryAdd(decimal newValue);
    void MemoryDiff(decimal newValue);
    decimal MemoryShow();
    decimal ClearValue();

    bool HasMemoryStored { get; }
    decimal Value { get; }
}


