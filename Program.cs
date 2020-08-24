using System;
using System.Windows.Forms;

namespace GettingStartedWithCSharp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var CalcView = new Calculator();
            var CalcPresenter = new CalculatorPresenter(CalcView);
            Application.Run(CalcView);
        }
    }
}
