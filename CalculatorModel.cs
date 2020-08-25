namespace GettingStartedWithCSharp
{
    class CalculatorModel
    {
        public decimal Value { get; set; }
        public string Operation { get; set; }
        public bool IsMemoryStored { get; set; }
        public decimal Memory { get; set; }
        public bool OperationPressed { get; set; }
        public bool ResultObtained { get; set; }
        public string Istoric { get; set; }
        public string Equation { get; set; }
    }
}
