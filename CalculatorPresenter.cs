using System;
using System.IO;
using System.Windows.Forms;
using static GettingStartedWithCSharp.Calculator;

namespace GettingStartedWithCSharp
{
    class CalculatorPresenter
    {
        private readonly ICalculatorView calculatorView;
        private CalculatorModel _calculatorModel = new CalculatorModel();

        public CalculatorPresenter(ICalculatorView calculatorView)
        {
            this.calculatorView = calculatorView;
            calculatorView.DigitClicked += DigitClick;
            calculatorView.OperatorClicked += OperatorClick;
            calculatorView.ResultClicked += ResultClick;
            calculatorView.SaveHistoryClicked += SaveHistoryClick;
            calculatorView.MemoryClicked += MemoryClick;
            calculatorView.ClearAllClicked += ClearAllClick;
            calculatorView.ClearEntryClicked += ClearEntryClick;
        }

        private void DigitClick(object sender, EventArgs e)
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
            calculatorView.ResultBoxShow(_calculatorModel.Rezultat);
        }

        private void ClearEntryClick(object sender, EventArgs e)
        {
            _calculatorModel.Rezultat = "0";
            calculatorView.ResultBoxShow(_calculatorModel.Rezultat);
        }

        private void OperatorClick(object sender, EventArgs e)
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

        private void ResultClick(object sender, EventArgs e)
        {
            _calculatorModel.OperationPressed = false;
            _calculatorModel.Equation = "";
            switch (_calculatorModel.Operation)
            {
                case "+":
                    _calculatorModel.Rezultat = (_calculatorModel.Value + (decimal)Double.Parse(_calculatorModel.Rezultat)).ToString("0.000");
                    calculatorView.ResultBoxShow(_calculatorModel.Rezultat);
                    break;
                case "-":
                    _calculatorModel.Rezultat = (_calculatorModel.Value - (decimal)Double.Parse(_calculatorModel.Rezultat)).ToString("0.000");
                    calculatorView.ResultBoxShow(_calculatorModel.Rezultat);
                    break;
                case "*":
                    _calculatorModel.Rezultat = (_calculatorModel.Value * (decimal)Double.Parse(_calculatorModel.Rezultat)).ToString("0.000");
                    calculatorView.ResultBoxShow(_calculatorModel.Rezultat);
                    break;
                case "/":
                    try
                    {
                        _calculatorModel.Rezultat = (_calculatorModel.Value / (decimal)Double.Parse(_calculatorModel.Rezultat)).ToString("0.000");
                        calculatorView.ResultBoxShow(_calculatorModel.Rezultat);
                    }
                    catch (DivideByZeroException)
                    {
                        MessageBox.Show("Impartirea la 0 nu este o operatie valida");
                        _calculatorModel.Rezultat = "operatie nevalida";
                        calculatorView.ResultBoxShow(_calculatorModel.Rezultat);
                    }
                    break;
                case "sqrt":
                    if (_calculatorModel.Value < 0)
                    {
                        try { throw new Exception("Radacina patrata a numerelor negative nu este posibila"); }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Radacina patrata a numerelor negative nu este posibila");
                        }
                        _calculatorModel.Rezultat = "operatie nevalida";
                        calculatorView.ResultBoxShow(_calculatorModel.Rezultat);
                    }
                    else
                    {
                        _calculatorModel.Rezultat = (Math.Sqrt((double)_calculatorModel.Value)).ToString("0.000");
                        calculatorView.ResultBoxShow(_calculatorModel.Rezultat);
                    }
                    break;
                default:
                    break;
            }
            _calculatorModel.ResultObtained = true;
            _calculatorModel.Istoric += (_calculatorModel.Rezultat + ", ");
            calculatorView.HistoryBoxShow(_calculatorModel.Istoric);
        }

        private void ClearAllClick(object sender, EventArgs e)
        {
            _calculatorModel.Rezultat = "";
            calculatorView.ResultBoxShow(_calculatorModel.Rezultat);
            _calculatorModel.Istoric = "";
            calculatorView.HistoryBoxShow(_calculatorModel.Istoric);
            _calculatorModel.Value = 0;
        }

        private void SaveHistoryClick(object sender, EventArgs e)
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

        private void MemoryClick(object sender, EventArgs e)
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
                    calculatorView.ResultBoxShow(_calculatorModel.Rezultat);
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
