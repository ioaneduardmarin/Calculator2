using System;
using System.Windows.Forms;
using System.Windows.Input;

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
            var MessageService = new MessageBoxDisplayService();
            var CalcPresenter = new CalculatorPresenter(CalcView,MessageService);
            Application.Run(CalcView);
        }
    }
}

