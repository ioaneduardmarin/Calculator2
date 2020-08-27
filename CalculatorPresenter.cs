using System;
using System.Linq;
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
        //Preia valorile introduse odata cu apasarea butoanelor caracteristce cifrelor
        private string _textAfisat = "";
        //Preia valorile introduse odata cu apasarea butoanelor caracteristce operatorilor
        private string _operation = "";
        //Arata operatia aflata in desfasurare in prezent: este alcatuit din campurile _textAfisat si _operatie
        private string _equation = "";
        //Arata istoricul operatiilor realizate. El este realizat prin concatenarea la acesta a campului _textAfisat odata cu apasarea butonului "Rezulta" 
        private string _istoric = "";
        //Verifica daca a fost apasat vreun buton caracteristic operatorilor
        private bool _isOperatorPressed = false;
        //Verifica daca a fost apasat butonul "Rezulta"
        private bool _isResultObtained = false;
        //Verifica daca a fost apasat un buton caracteristic cifrelor
        private bool _isDigitPressed = false;
        //Preia valoarea primita de la calculator pentru a o afisa in cadrul View-ului
        private string _textDeAfisat;

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
            if (_textAfisat == "0" || _isOperatorPressed || _isResultObtained)
            { _textAfisat = ""; }
            _isDigitPressed = true;
            _isResultObtained = false;
            _isOperatorPressed = false;
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
                _calculatorEngine.SubmitOperation("=", Convert.ToDecimal(_textAfisat));
                _isOperatorPressed = true;
                _equation = _textAfisat + " " + _operation;
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
            _isDigitPressed = false;
            _isOperatorPressed = false;
            _equation = "";
            try
            {
                _textDeAfisat = Convert.ToString(_calculatorEngine.SubmitOperation(_operation, Convert.ToDecimal(_textAfisat)));
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is ArgumentException)
                {
                    _messageBoxDisplayService.Show(ex.Message);
                    _textDeAfisat = "operatie nevalida";
                }
            }
            if (Regex.Matches(_textDeAfisat, @"[a-zA-Z]").Count > 0)//
            { _messageBoxDisplayService.Show("Operatia nu este valida"); }
            else
            {
                try
                {
                    _textDeAfisat = GetFormattedNumberForDisplay(_textDeAfisat);
                }
                catch (FormatException formatEx)
                {
                    _messageBoxDisplayService.Show(formatEx.Message);
                    _textDeAfisat = "operatie nevalida";
                }
            }
            _isResultObtained = true;
            _calculatorView.SetResultBoxText(_textDeAfisat);
            _istoric += (_textDeAfisat + ", ");
            _calculatorView.SetHistoryBoxText(_istoric);
            if (_isDigitPressed)
            {
                _calculatorEngine.ClearValue();
            }
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
            if (String.IsNullOrEmpty(_textAfisat))
            { _messageBoxDisplayService.Show("Nu poti opera folosind o valoare nevalida."); }
            else
            {
                switch (memoryClick)
                {
                    case "MC":
                        string mesaj = "Doresti sa golesti memoria?";
                        string titlu = "Golire Memorie";
                        bool clearMemoryStored = _messageBoxDisplayService.PromptUser(mesaj, titlu);
                        if (clearMemoryStored == true)
                        {
                            _calculatorEngine.MemoryClear();
                        }
                        break;
                    case "MR":
                        _textAfisat = FormatShownText(_calculatorEngine.MemoryRestore());
                        _calculatorView.SetResultBoxText(_textAfisat);
                        break;
                    case "MS":
                        _calculatorEngine.MemoryStore(decimal.Parse(_textAfisat));
                        break;
                    case "M+":
                        try { _calculatorEngine.MemoryAdd(Convert.ToDecimal(_textAfisat)); }
                        catch (FormatException formatEx) { _messageBoxDisplayService.Show(formatEx.Message); }
                        break;
                    case "M-":
                        try { _calculatorEngine.MemoryDiff(Convert.ToDecimal(_textAfisat)); }
                        catch (FormatException formatEx) { _messageBoxDisplayService.Show(formatEx.Message); }
                        break;
                    case "M":
                        _calculatorView.MemoryButtonShow(FormatShownText(_calculatorEngine.MemoryShow()));
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

        private string GetFormattedNumberForDisplay(string textAfisat)
        {
            if (textAfisat.ToCharArray().Count(c => c == '.') > 1)
            {
                throw new FormatException(String.Format("Aceasta nu este o valoare valida."));
            }
            else
            {
                textAfisat = FormatShownText(Convert.ToDecimal(textAfisat));
                return textAfisat;
            }
        }

        private string FormatShownText(decimal number)
        {
            return number.ToString("0.000");
        }
    }
}
