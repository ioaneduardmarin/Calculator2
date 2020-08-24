using System;
using System.IO;
using System.Windows.Forms;
using static GettingStartedWithCSharp.Calculator;

namespace GettingStartedWithCSharp
{
    class CalculatorPresenter
    {
        private readonly ICalculatorView calculatorView;
        private readonly IMessageBoxDisplayService messageBoxDisplayService;
        private CalculatorModel _calculatorModel = new CalculatorModel();

        public CalculatorPresenter(ICalculatorView calculatorView, IMessageBoxDisplayService messageBoxDisplayService)
        {
            this.calculatorView = calculatorView;
            this.messageBoxDisplayService = messageBoxDisplayService;
            calculatorView.DigitClicked += OnDigitClick;
            calculatorView.OperatorClicked += OnOperatorClick;
            calculatorView.ResultClicked += OnResultClick;
            calculatorView.SaveHistoryClicked += OnSaveHistoryClick;
            calculatorView.MemoryClicked += OnMemoryClick;
            calculatorView.ClearAllClicked += OnClearAllCLick;
            calculatorView.ClearEntryClicked += OnClearEntryClick;
            
        }

        private void OnDigitClick(object sender, EventArgs e)
        {
            if (_calculatorModel.Rezultat == "0" || (_calculatorModel.OperationPressed))
                _calculatorModel.Rezultat = "";

            if (_calculatorModel.ResultObtained)
            {
                _calculatorModel.Rezultat = "";
            }
            _calculatorModel.ResultObtained = false;
            _calculatorModel.OperationPressed = false;
            Button b = (Button)sender;
            _calculatorModel.Rezultat += b.Text;
            calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
        }

        private void OnClearEntryClick(object sender, EventArgs e)
        {
            _calculatorModel.Rezultat = "0";
            calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
        }

        private void OnOperatorClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            _calculatorModel.Operation = b.Text;
            try
            {
                _calculatorModel.Value = (decimal)(Double.Parse(_calculatorModel.Rezultat));
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Introduceti o valoare valida");
                _calculatorModel.Rezultat = "0";
                _calculatorModel.Equation = "0";
            }
            _calculatorModel.OperationPressed = true;
            _calculatorModel.Equation = _calculatorModel.Value + " " + _calculatorModel.Operation;
            calculatorView.EquationLabel(_calculatorModel.Equation);
        }

        private void OnResultClick(object sender, EventArgs e)
        {
            _calculatorModel.OperationPressed = false;
            _calculatorModel.Equation = "";
            switch (_calculatorModel.Operation)
            {
                case "+":
                    _calculatorModel.Rezultat = (_calculatorModel.Value + (decimal)Double.Parse(_calculatorModel.Rezultat)).ToString("0.000");
                    calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    break;
                case "-":
                    _calculatorModel.Rezultat = (_calculatorModel.Value - (decimal)Double.Parse(_calculatorModel.Rezultat)).ToString("0.000");
                    calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    break;
                case "*":
                    _calculatorModel.Rezultat = (_calculatorModel.Value * (decimal)Double.Parse(_calculatorModel.Rezultat)).ToString("0.000");
                    calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    break;
                case "/":
                    try
                    {
                        _calculatorModel.Rezultat = (_calculatorModel.Value / (decimal)Double.Parse(_calculatorModel.Rezultat)).ToString("0.000");
                        calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    }
                    catch (DivideByZeroException)
                    {
                        messageBoxDisplayService.Show("Impartirea la 0 nu este o operatie valida");
                        _calculatorModel.Rezultat = "operatie nevalida";
                        calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    }
                    break;
                case "sqrt":
                    if (_calculatorModel.Value < 0)
                    {

                        messageBoxDisplayService.Show("Help");
                        _calculatorModel.Rezultat = "operatie nevalida";
                        calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    }
                    else
                    {
                        _calculatorModel.Rezultat = (Math.Sqrt((double)_calculatorModel.Value)).ToString("0.000");
                        calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    }
                    break;
                default:
                    break;
            }
            _calculatorModel.ResultObtained = true;
            _calculatorModel.Istoric += (_calculatorModel.Rezultat + ", ");
            calculatorView.SetHistoryBoxText(_calculatorModel.Istoric);
        }

        private void OnClearAllCLick(object sender, EventArgs e)
        {
            _calculatorModel.Rezultat = "";
            calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
            _calculatorModel.Istoric = "";
            calculatorView.SetHistoryBoxText(_calculatorModel.Istoric);
            _calculatorModel.Value = 0;
        }

        private void OnSaveHistoryClick(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File|*";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = sfd.FileName;
                using (var sw = new StreamWriter(File.Create(path)))
                {
                    sw.Write(_calculatorModel.Istoric.Remove((_calculatorModel.Istoric.Length - 2), 1));
                    sw.Dispose();
                }
            }
        }

        private void OnMemoryClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            _calculatorModel.MemoryClick = b.Text;

            if (!_calculatorModel.IsMemoryStored)
            {
                calculatorView.DisableMemoryButtons();
            }

            switch (_calculatorModel.MemoryClick)
            {
                case "MC":
                    string mesaj = "Do you want to clear the memory?";
                    string titlu = "Memory Clear";
                    MessageBoxButtons butoane = MessageBoxButtons.YesNo;
                    DialogResult rezultat = MessageBox.Show(mesaj, titlu, butoane);
                    if (rezultat == DialogResult.Yes)
                    {
                        _calculatorModel.IsMemoryStored = false;
                        calculatorView.DisableMemoryButtons();
                    }
                    break;
                case "MR":
                    _calculatorModel.Rezultat = _calculatorModel.Memory.ToString();
                    calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    break;
                case "MS":
                    _calculatorModel.Memory = (decimal)Double.Parse(_calculatorModel.Rezultat);
                    _calculatorModel.IsMemoryStored = true;
                    calculatorView.EnableMemoryButtons();
                    break;
                case "M+":
                    _calculatorModel.Memory += (decimal)(Double.Parse(_calculatorModel.Rezultat));
                    break;
                case "M-":
                    _calculatorModel.Memory -= (decimal)Double.Parse(_calculatorModel.Rezultat);
                    break;
                case "M":
                    calculatorView.TxtMemoryShow = _calculatorModel.Memory.ToString();
                    calculatorView.MemoryButtonShow(calculatorView.TxtMemoryShow);
                    break;
                default:
                    break;
            }
        }
        
    }
  
}
