using GettingStartedWithCSharp.Properties;
using System;

namespace GettingStartedWithCSharp
{
    public class CalculatorEngine : ICalculatorEngine
    {
        public decimal Value { get; private set; } = 0m;
        private decimal _memory;
        public bool HasMemoryStored { get; private set; } = false;

        public decimal SubmitOperation(string operation, decimal numarAfisat)
        {

            switch (operation)
            {
                case "=":
                    Value = numarAfisat;
                    return Value;
                case "+":
                    Value = Value + numarAfisat;
                    return Value;
                case "-":
                    Value = Value - numarAfisat;

                    return Value;
                case "*":
                    Value = Value * numarAfisat;
                    return Value;
                case "/":
                    try
                    {
                        Value = Value / numarAfisat;
                    }
                    catch (DivideByZeroException)
                    {
                        throw new ArgumentException(String.Format("Numarul {0} nu poate fi impartit la 0.", Value), "Value");
                    }
                    return Value;
                case "sqrt":
                    if (Value < 0)
                    {
                        throw new ArgumentException(String.Format("{0} este un numar negativ => Nu ii putem extrage radacina patratica!", Value), "Value");
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

        public void MemoryStore(decimal numarAfisat)
        {
            _memory = numarAfisat;
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

        public decimal GetValue()
        {
            return Value;
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
    decimal SubmitOperation(string operation, decimal numarAfisat);
    void MemoryClear();
    decimal MemoryRestore();
    void MemoryStore(decimal textAfisat);
    void MemoryAdd(decimal newValue);
    void MemoryDiff(decimal newValue);
    decimal MemoryShow();
    decimal ClearValue();

    bool HasMemoryStored { get; }
    decimal Value { get; }
}


