﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static GettingStartedWithCSharp.CalculatorForm;

namespace GettingStartedWithCSharp
{
    class CalculatorPresenter
    {
        private readonly ICalculatorView _calculatorView;
        private readonly IMessageBoxDisplayService _messageBoxDisplayService;
        private readonly ISaveHistoryService _saveHistoryService;
        private CalculatorModel _calculatorModel = new CalculatorModel();
        private string _rezultat;

        public CalculatorPresenter(ICalculatorView calculatorView, IMessageBoxDisplayService messageBoxDisplayService, ISaveHistoryService saveHistoryService)
        {
            this._calculatorView = calculatorView;
            this._messageBoxDisplayService = messageBoxDisplayService;
            this._saveHistoryService = saveHistoryService;
            calculatorView.DigitClicked += OnDigitClick;
            calculatorView.OperatorClicked += OnOperatorClick;
            calculatorView.ResultClicked += OnResultClick;
            calculatorView.SaveHistoryClicked += OnSaveHistoryClick;
            calculatorView.MemoryClicked += OnMemoryClick;
            calculatorView.ClearAllClicked += OnClearAllCLick;
            calculatorView.ClearEntryClicked += OnClearEntryClick;
            calculatorView.EraseHistoryClicked += OnEraseHistory;
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
            _calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
        }

        private void OnClearEntryClick(object sender, EventArgs e)
        {
            _calculatorModel.Rezultat = "0";
            _calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
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
                _messageBoxDisplayService.Show("Introduceti o valoare valida");
                _calculatorModel.Rezultat = "0";
                _calculatorModel.Equation = "0";
            }
            _calculatorModel.OperationPressed = true;
            _calculatorModel.Equation = _calculatorModel.Value + " " + _calculatorModel.Operation;
            _calculatorView.EquationLabel(_calculatorModel.Equation);
        }

        private void OnResultClick(object sender, EventArgs e)
        {
            _calculatorModel.OperationPressed = false;
            _calculatorModel.Equation = "";
            switch (_calculatorModel.Operation)
            {
                case "+":
                    _calculatorModel.Rezultat = (_calculatorModel.Value + (decimal)Double.Parse(_calculatorModel.Rezultat)).ToString("0.000");
                    _calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    break;
                case "-":
                    _calculatorModel.Rezultat = (_calculatorModel.Value - (decimal)Double.Parse(_calculatorModel.Rezultat)).ToString("0.000");
                    _calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    break;
                case "*":
                    _calculatorModel.Rezultat = (_calculatorModel.Value * (decimal)Double.Parse(_calculatorModel.Rezultat)).ToString("0.000");
                    _calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    break;
                case "/":
                    try
                    {
                        _calculatorModel.Rezultat = (_calculatorModel.Value / (decimal)Double.Parse(_calculatorModel.Rezultat)).ToString("0.000");
                        _calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    }
                    catch (DivideByZeroException)
                    {
                        _messageBoxDisplayService.Show("Impartirea la 0 nu este o operatie valida.");
                        _calculatorModel.Rezultat = "operatie nevalida";
                        _calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    }
                    break;
                case "sqrt":
                    if (_calculatorModel.Value < 0)
                    {

                        _messageBoxDisplayService.Show("Extragerea radacinii patrate dintr-un numar negativ nu este o operatie valida.");
                        _calculatorModel.Rezultat = "operatie nevalida";
                        _calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    }
                    else
                    {
                        _calculatorModel.Rezultat = (Math.Sqrt((double)_calculatorModel.Value)).ToString("0.000");
                        _calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    }
                    break;
                default:
                    break;
            }
            _calculatorModel.ResultObtained = true;
            _calculatorModel.Istoric += (_calculatorModel.Rezultat + ", ");
            _calculatorView.SetHistoryBoxText(_calculatorModel.Istoric);
        }

        private void OnClearAllCLick(object sender, EventArgs e)
        {
            _calculatorModel.Rezultat = "";
            _calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
            _calculatorModel.Istoric = "";
            _calculatorView.SetHistoryBoxText(_calculatorModel.Istoric);
            _calculatorModel.Value = 0;
        }

        private void OnEraseHistory(object sender, EventArgs e)
        {
            string mesaj = "Doresti sa stergi istoricul?";
            string titlu = "Stergere Istoric";
            if (String.IsNullOrEmpty(_calculatorModel.Istoric))
            {
                _messageBoxDisplayService.Show("Istoricul este gol, nu avem ce sterge.");
            }
            else
            {
                bool clearHistory = _messageBoxDisplayService.PromptUser(mesaj,titlu);
                if (clearHistory == true)
                {
                    _calculatorModel.Istoric = "";
                    _calculatorView.SetHistoryBoxText(_calculatorModel.Istoric);
                }
            }
        }

        private void OnSaveHistoryClick(object sender, EventArgs e)
        {
            string mesaj = "Doresti sa stergi istoricul?";
            string titlu = "Stergere Istoric";
            if (String.IsNullOrEmpty(_calculatorModel.Istoric)||!(Regex.Matches(_calculatorModel.Istoric, @"[a-zA-Z0-9]").Count>0))
            {
                _messageBoxDisplayService.Show("Istoricul este gol, nu avem ce salva.");
                _calculatorModel.Istoric = "";
                _calculatorView.SetHistoryBoxText(_calculatorModel.Istoric);
            }
            else
            {
                bool isHistorySaved = _saveHistoryService.SaveHistory(_calculatorModel.Istoric);
                if (isHistorySaved == false)
                {
                    _messageBoxDisplayService.Show("Istoricul nu a fost salvat");
                }
                else
                {
                    _messageBoxDisplayService.Show("Istoricul a fost salvat.");
                    bool clearHistory = _messageBoxDisplayService.PromptUser(mesaj, titlu);
                    if (clearHistory == true)
                    {
                        _calculatorModel.Istoric = "";
                        _calculatorView.SetHistoryBoxText(_calculatorModel.Istoric);
                    }
                }
            }

        }

        private void OnMemoryClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string memoryClick = b.Text;
            string mesaj = "Doresti sa golesti memoria?";
            string titlu = "Golire Memorie";

            if (!_calculatorModel.IsMemoryStored)
            {
                _calculatorView.DisableMemoryButtons();
            }

            switch (memoryClick)
            {
                case "MC":
                    _calculatorModel.IsMemoryStored= _messageBoxDisplayService.PromptUser(mesaj,titlu);
                    _calculatorView.DisableMemoryButtons();
                    break;
                case "MR":
                    _calculatorModel.Rezultat = _calculatorModel.Memory.ToString();
                    _calculatorView.SetResultBoxText(_calculatorModel.Rezultat);
                    break;
                case "MS":
                    _calculatorModel.Memory = (decimal)Double.Parse(_calculatorModel.Rezultat);
                    _calculatorModel.IsMemoryStored = true;
                    _calculatorView.EnableMemoryButtons();
                    break;
                case "M+":
                    _calculatorModel.Memory += (decimal)(Double.Parse(_calculatorModel.Rezultat));
                    break;
                case "M-":
                    _calculatorModel.Memory -= (decimal)Double.Parse(_calculatorModel.Rezultat);
                    break;
                case "M":
                    _calculatorView.TxtMemoryShow = _calculatorModel.Memory.ToString();
                    _calculatorView.MemoryButtonShow(_calculatorView.TxtMemoryShow);
                    break;
                default:
                    break;
            }
        }
        
    }
  
}
