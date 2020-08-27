using System;
using System.Windows.Forms;

namespace GettingStartedWithCSharp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            var calcView = new CalculatorForm();
            var messageService = new MessageBoxDisplayService();
            var saveHist = new SaveHistoryService();
            var businessLogicObject = new CalculatorEngine();
            var calcPresenter = new CalculatorPresenter(calcView, messageService, saveHist, businessLogicObject);
            Application.Run(calcView);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var message = String.Format("A avut loc o eroare.", ((Exception)e.ExceptionObject).Message);
            MessageBox.Show(message, "Unexpected Error");
        }
    }
}