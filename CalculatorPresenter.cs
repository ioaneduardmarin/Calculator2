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
        private string _shownText = "";
        //Preia valorile introduse odata cu apasarea butoanelor caracteristce operatorilor
        private string _operation = "";
        //Arata operatia aflata in desfasurare in prezent: este alcatuit din campurile _shownText si showText
        private string _equation = "";
        //Arata istoricul operatiilor realizate. El este realizat prin concatenarea la acesta a campului _shownText odata cu apasarea butonului "Rezulta" 
        private string _history = "";
        //Verifica daca a fost apasat vreun buton caracteristic operatorilor
        private bool _isOperatorPressed;
        //Verifica daca a fost apasat butonul "Rezulta"
        private bool _isResultObtained;
        //Verifica daca a fost apasat un buton caracteristic cifrelor
        private bool _isDigitPressed;
        //Preia valoarea primita de la calculator pentru a o afisa in cadrul View-ului
        private string _textToBeShown;

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
            if (_shownText == "0" || _isOperatorPressed || _isResultObtained)
            { _shownText = ""; }
            _isDigitPressed = true;
            _isResultObtained = false;
            _isOperatorPressed = false;
            Button b = (Button)sender;
            _shownText += b.Text;
            _calculatorView.SetResultBoxText(_shownText);
        }

        private void OnClearEntryClick(object sender, EventArgs e)
        {
            _shownText = "0";
            _calculatorView.SetResultBoxText(_shownText);
        }

        private void OnOperatorClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            _operation = b.Text;
            try
            {
                _calculatorEngine.SubmitOperation("=", Convert.ToDecimal(_shownText));
                _isOperatorPressed = true;
                _equation = _shownText + " " + _operation;
                _calculatorView.EquationLabel(_equation);
            }
            catch (FormatException)
            {
                _messageBoxDisplayService.Show("Enter a valid value");
                _shownText = "0";
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
                Func<decimal, string> converter = Convert.ToString;
                _textToBeShown = converter(_calculatorEngine.SubmitOperation(_operation, Convert.ToDecimal(_shownText)));
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is ArgumentException)
                {
                    _messageBoxDisplayService.Show(ex.Message);
                    _textToBeShown = "Invalid operation";
                }
            }
            if (Regex.Matches(_textToBeShown, @"[a-zA-Z]").Count > 0)//
            { _messageBoxDisplayService.Show("Invalid operation"); }
            else
            {
                try
                {
                    _textToBeShown = GetFormattedNumberForDisplay(_textToBeShown);
                }
                catch (FormatException formatEx)
                {
                    _messageBoxDisplayService.Show(formatEx.Message);
                    _textToBeShown = "Invalid operation";
                }
            }
            _isResultObtained = true;
            _calculatorView.SetResultBoxText(_textToBeShown);
            _history += (_textToBeShown + ", ");
            _calculatorView.SetHistoryBoxText(_history);
            if (_isDigitPressed)
            {
                _calculatorEngine.ClearValue();
            }
        }

        private void OnClearAllClick(object sender, EventArgs e)
        {
            _calculatorEngine.ClearValue();
            _shownText = "";
            _calculatorView.SetResultBoxText(_shownText);
            _history = "";
            _calculatorView.SetHistoryBoxText(_history);
            _equation = "";
            _calculatorView.EquationLabel(_equation);
        }

        private void OnEraseHistory(object sender, EventArgs e)
        {
            string message = "Do you want to clear the history?";
            string title = "History Clear";
            if (String.IsNullOrEmpty(_history))
            {
                _messageBoxDisplayService.Show("The history is already empty!");
            }
            else
            {
                bool clearHistory = _messageBoxDisplayService.PromptUser(message, title);
                if (clearHistory)
                {
                    _history = "";
                    _calculatorView.SetHistoryBoxText(_history);
                }
            }
        }

        private void OnSaveHistoryClick(object sender, EventArgs e)
        {
            string message = "Do you want to empty the history?";
            string title = "History Clear";
            if (String.IsNullOrEmpty(_history) || !(Regex.Matches(_history, @"[a-zA-Z0-9]").Count > 0))
            {
                _messageBoxDisplayService.Show("The history is empty!");
                _history = "";
                _calculatorView.SetHistoryBoxText(_history);
            }
            else
            {
                bool isHistorySaved = _saveHistoryService.SaveHistory(_history);
                if (isHistorySaved == false)
                {
                    _messageBoxDisplayService.Show("History was not saved!");
                }
                else
                {
                    _messageBoxDisplayService.Show("History was saved!");
                    bool clearHistory = _messageBoxDisplayService.PromptUser(message, title);
                    if (clearHistory)
                    {
                        _history = "";
                        _calculatorView.SetHistoryBoxText(_history);
                    }
                }
            }
        }

        private void OnMemoryClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string memoryClick = b.Text;
            if (String.IsNullOrEmpty(_shownText))
            { _messageBoxDisplayService.Show("You can not use an invalid value!"); }
            else
            {
                switch (memoryClick)
                {
                    case "MC":
                        string message = "Do you want to empty the memory";
                        string title = "Memory Clear";
                        bool clearMemoryStored = _messageBoxDisplayService.PromptUser(message, title);
                        if (clearMemoryStored)
                        {
                            _calculatorEngine.MemoryClear();
                        }
                        break;
                    case "MR":
                        _shownText = FormatShownText(_calculatorEngine.MemoryRestore());
                        _calculatorView.SetResultBoxText(_shownText);
                        break;
                    case "MS":
                        _calculatorEngine.MemoryStore(decimal.Parse(_shownText));
                        break;
                    case "M+":
                        try { _calculatorEngine.MemoryAdd(Convert.ToDecimal(_shownText)); }
                        catch (FormatException formatEx) { _messageBoxDisplayService.Show(formatEx.Message); }
                        break;
                    case "M-":
                        try { _calculatorEngine.MemoryDiff(Convert.ToDecimal(_shownText)); }
                        catch (FormatException formatEx) { _messageBoxDisplayService.Show(formatEx.Message); }
                        break;
                    case "M":
                        _calculatorView.MemoryButtonShow(FormatShownText(_calculatorEngine.MemoryShow()));
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

        private string GetFormattedNumberForDisplay(string shownText)
        {
            if (shownText.ToCharArray().Count(c => c == '.') > 1)
            {
                throw new FormatException("Invalid Value.");
            }
            else
            {
                shownText = FormatShownText(Convert.ToDecimal(shownText));
                return shownText;
            }
        }

        private string FormatShownText(decimal number)
        {
            return number.ToString("0.000");
        }
    }
}
