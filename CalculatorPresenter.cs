using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GettingStartedWithCSharp
{
    class CalculatorPresenter
    {
        private readonly ICalculatorView _calculatorView;
        private readonly IMessageBoxDisplayService _messageBoxDisplayService;
        private readonly ISaveHistoryService _saveHistoryService;
        private readonly IBusinessLogicClass _businessLogicObject;
        private string _textAfisat = "";
        private string _operation = "";
        private string _equation = "";
        private string _istoric = "";
        private bool _operationPressed = false;
        private bool _resultObtained = false;
        private bool _isMemoryStored = false;

        public CalculatorPresenter(ICalculatorView calculatorView, IMessageBoxDisplayService messageBoxDisplayService, ISaveHistoryService saveHistoryService, IBusinessLogicClass businessLogicObject)
        {
            this._calculatorView = calculatorView;
            this._messageBoxDisplayService = messageBoxDisplayService;
            this._saveHistoryService = saveHistoryService;
            this._businessLogicObject = businessLogicObject;
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
            if (_textAfisat == "0" || (_operationPressed))
            { _textAfisat = ""; }

            if (_resultObtained)
            {
                _textAfisat = "";
            }

            _resultObtained = false;
            _operationPressed = false;
            Button b = (Button)sender;
            _textAfisat += b.Text;
            _calculatorView.SetResultBoxText(_textAfisat);
        }

        private void OnClearEntryClick(object sender, EventArgs e)
        {
            _textAfisat = "0";
            _calculatorView.SetResultBoxText(_textAfisat);
        }

        private void OnOperatorClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            _operation = b.Text;
            try
            {
             _businessLogicObject.GetValue(_textAfisat);
            }
            catch (System.FormatException)
            {
                _messageBoxDisplayService.Show("Introduceti o valoare valida");
                _textAfisat = "0";
                _equation = "0";
            }
            _operationPressed = true;
            _equation = _businessLogicObject.ReturnValue() + " " + _operation;
            _calculatorView.EquationLabel(_equation);
        }

        private void OnResultClick(object sender, EventArgs e)
        {
            _operationPressed = false;
            _equation = "";
            _textAfisat = _businessLogicObject.WhenResultClick(_operation, _textAfisat);

            if (Regex.Matches(_textAfisat, @"[a-zA-Z]").Count > 0)
            { _messageBoxDisplayService.Show("Operatia nu este valida"); }
            else
            {
                _textAfisat = _businessLogicObject.GetFormattedShownText(_textAfisat);
            }
            _resultObtained = true;
            _calculatorView.SetResultBoxText(_textAfisat);
            _istoric += (_textAfisat + ", ");
            _calculatorView.SetHistoryBoxText(_istoric);
        }


        private void OnClearAllCLick(object sender, EventArgs e)
        {
            _businessLogicObject.ClearValue();
            _textAfisat = "";
            _calculatorView.SetResultBoxText(_textAfisat);
            _istoric = "";
            _calculatorView.SetHistoryBoxText(_istoric);
            _equation = "";
            _calculatorView.EquationLabel(_equation);
        }

        private void OnEraseHistory(object sender, EventArgs e)
        {
            string mesaj = "Doresti sa stergi istoricul?";
            string titlu = "Stergere Istoric";
            if (String.IsNullOrEmpty(_istoric))
            {
                _messageBoxDisplayService.Show("Istoricul este gol, nu avem ce sterge.");
            }
            else
            {
                bool clearHistory = _messageBoxDisplayService.PromptUser(mesaj, titlu);
                if (clearHistory == true)
                {
                    _istoric = "";
                    _calculatorView.SetHistoryBoxText(_istoric);
                }
            }
        }

        private void OnSaveHistoryClick(object sender, EventArgs e)
        {
            string mesaj = "Doresti sa stergi istoricul?";
            string titlu = "Stergere Istoric";
            if (String.IsNullOrEmpty(_istoric) || !(Regex.Matches(_istoric, @"[a-zA-Z0-9]").Count > 0))
            {
                _messageBoxDisplayService.Show("Istoricul este gol, nu avem ce salva.");
                _istoric = "";
                _calculatorView.SetHistoryBoxText(_istoric);
            }
            else
            {
                bool isHistorySaved = _saveHistoryService.SaveHistory(_istoric);
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
                        _istoric = "";
                        _calculatorView.SetHistoryBoxText(_istoric);
                    }
                }
            }

        }

        private void OnMemoryClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            WhenMemoryClick(b.Text);
        }


        public void WhenMemoryClick(string memoryClick)
        {
            string mesaj = "Doresti sa golesti memoria?";
            string titlu = "Golire Memorie";

            if (!_isMemoryStored)
            {
                _calculatorView.DisableMemoryButtons();
            }

            switch (memoryClick)
            {
                case "MC":
                    _businessLogicObject.MemoryClear();
                    _isMemoryStored = _messageBoxDisplayService.PromptUser(mesaj, titlu);
                    _calculatorView.DisableMemoryButtons();
                    break;
                case "MR":
                    _textAfisat = _businessLogicObject.MemoryRestore();
                    _calculatorView.SetResultBoxText(_textAfisat);
                    break;
                case "MS":
                    _businessLogicObject.MemoryStore(_textAfisat);
                    _isMemoryStored = true;
                    _calculatorView.EnableMemoryButtons();
                    break;
                case "M+":
                    _businessLogicObject.MemoryAdd(Convert.ToDecimal(_textAfisat));
                    break;
                case "M-":
                    _businessLogicObject.MemoryDiff(Convert.ToDecimal(_textAfisat));
                    break;
                case "M":
                    _calculatorView.MemoryButtonShow(_businessLogicObject.MemoryShow());
                    break;
                default:
                    break;
            }
        }
    }
}
