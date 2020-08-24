namespace GettingStartedWithCSharp
{
    class CalculatorModel
    {
        public decimal Value { get; set; }
        public string Operation { get; set; }
        public string MemoryClick { get; set; }
        public bool IsMemoryStored { get; set; }
        public decimal Memory { get; set; }
        public bool OperationPressed { get; set; }
        public bool ResultObtained { get; set; }
        public string RezultBox { get; set; }
        public string HistoryBox { get; set; }
        public string Equation { get; set; }
    }
}
