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
        private readonly ICalculatorEngine _calculatorEngine;
        private string _textAfisat = ""; //Acest camp preia valorile introduse odata cu apasarea butoanelor caracteristce cifrelor
        private string _operation = ""; //Acest camp preia valorile introduse odata cu apasarea butoanelor caracteristce operatorilor
        private string _equation = ""; //Acest camp arata operatia aflata in desfasurare in prezent: este alcatuit din campurile _textAfisat si _operatie
        private string _istoric = ""; //Acest camp arata istoricul operatiilor realizare. El este realizat prin concatenarea la acesta a campului _textAfisat odata cu apasarea butonului "Rezulta" 
        private bool _isoperationPressed = false; //Acest camp verifica daca a fost apasat vreun buton caracteristic operatorilor
        private bool _isresultObtained = false; //Acest buton verifica daca a fost apasat butonul "Rezuta"

        public CalculatorPresenter(ICalculatorView calculatorView, IMessageBoxDisplayService messageBoxDisplayService, ISaveHistoryService saveHistoryService, ICalculatorEngine businessLogicObject)
        {
            _calculatorView = calculatorView;
            _messageBoxDisplayService = messageBoxDisplayService;
            _saveHistoryService = saveHistoryService;
            _calculatorEngine = businessLogicObject;
            calculatorView.DigitClicked += OnDigitClick;
            calculatorView.OperatorClicked += OnOperatorClick;
            calculatorView.ResultClicked += OnResultClick;
            calculatorView.SaveHistoryClicked += OnSaveHistoryClick;
            calculatorView.MemoryClicked += OnMemoryClick;
            calculatorView.ClearAllClicked += OnClearAllClick;
            calculatorView.ClearEntryClicked += OnClearEntryClick;
            calculatorView.EraseHistoryClicked += OnEraseHistory;
        }

        private void OnDigitClick(object sender, EventArgs e)
        {
            if (_textAfisat == "0" || _isoperationPressed || _isresultObtained)
            { _textAfisat = ""; }

            _isresultObtained = false;
            _isoperationPressed = false;
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
                _calculatorEngine.SetValue(_textAfisat);
                _isoperationPressed = true;
                _equation = _calculatorEngine.GetValue() + " " + _operation;
                _calculatorView.EquationLabel(_equation);
            }
            catch (System.FormatException)
            {
                _messageBoxDisplayService.Show("Introduceti o valoare valida");
                _textAfisat = "0";
                _equation = "0";
                //vreau ca operatia sa ramana salvata in eventualitatea in care userul a realizat din greseala o operatie "nevalida" si doreste sa continue efectuarea acesteia, cu un operand valid, dupa ce curata ResultBox-ul.
            }
        }

        private void OnResultClick(object sender, EventArgs e)
        {
            _isoperationPressed = false;
            _equation = "";
            try
            {
                _textAfisat = _calculatorEngine.CalculusBehindResultClick(_operation, _textAfisat);
            }
            catch (ArgumentException argEx)
            {
                _messageBoxDisplayService.Show(argEx.Message);
                _textAfisat = "operatie nevalida";
            }

            if (Regex.Matches(_textAfisat, @"[a-zA-Z]").Count > 0)//
            { _messageBoxDisplayService.Show("Operatia nu este valida"); }
            else
            {
                try
                {
                    _textAfisat = _calculatorEngine.GetFormattedShownText(_textAfisat);
                }
                catch (FormatException formatEx)
                {
                    _messageBoxDisplayService.Show(formatEx.Message);
                    _textAfisat = "operatie nevalida";
                }
            }
            _isresultObtained = true;
            _calculatorView.SetResultBoxText(_textAfisat);
            _istoric += (_textAfisat + ", ");
            _calculatorView.SetHistoryBoxText(_istoric);
        }

        private void OnClearAllClick(object sender, EventArgs e)
        {
            _calculatorEngine.ClearValue();
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
            string memoryClick = b.Text;

            switch (memoryClick)
            {
                case "MC":
                    _calculatorEngine.MemoryClear();
                    break;
                case "MR":
                    _textAfisat = _calculatorEngine.MemoryRestore();
                    _calculatorView.SetResultBoxText(_textAfisat);
                    break;
                case "MS":
                    _calculatorEngine.MemoryStore(_textAfisat);
                    break;
                case "M+":
                    _calculatorEngine.MemoryAdd(Convert.ToDecimal(_textAfisat));
                    break;
                case "M-":
                    _calculatorEngine.MemoryDiff(Convert.ToDecimal(_textAfisat));
                    break;
                case "M":
                    _calculatorView.MemoryButtonShow(_calculatorEngine.MemoryShow());
                    break;
                default:
                    break;
            }

            if (!_calculatorEngine.HasMemoryStored)
            {
                _calculatorView.DisableMemoryButtons();
            }
            else
            {
                _calculatorView.EnableMemoryButtons();
            }
        }
    }
}
