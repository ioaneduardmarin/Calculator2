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
            var utils = new Utils();
            var businessLogicObject = new BusinessLogicClass();
            var CalcPresenter = new CalculatorPresenter(calcView, messageService, saveHist, utils, businessLogicObject);
            Application.Run(calcView);
        }


        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var message = String.Format("A avut loc o eroare.\r\n" +
                "{0}\r\n" +
                "Please contact support.",
                ((Exception)e.ExceptionObject).Message);

            Console.WriteLine("ERROR {0}: {1}",
                DateTimeOffset.Now, e.ExceptionObject);

            MessageBox.Show(message, "Unexpected Error");
        }
    }
}

