using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace GettingStartedWithCSharp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var calcView = new CalculatorForm();
            var messageService = new MessageBoxDisplayService();
            var saveHist = new SaveHistoryService();
            var CalcPresenter = new CalculatorPresenter(calcView,messageService, saveHist);
            Application.Run(calcView);
        }
    }
}

