using System;
using System.Windows.Forms;
using static GettingStartedWithCSharp.CalculatorForm;

namespace GettingStartedWithCSharp
{
    public partial class CalculatorForm : Form, ICalculatorView
    {

        public CalculatorForm()
        {
            InitializeComponent();

        }

        public event EventHandler DigitClicked
        {
            add
            {
                Digit0.Click += value;
                Digit1.Click += value;
                Digit2.Click += value;
                Digit3.Click += value;
                Digit4.Click += value;
                Digit5.Click += value;
                Digit6.Click += value;
                Digit7.Click += value;
                Digit8.Click += value;
                Digit9.Click += value;
                Dot.Click += value;
            }
            remove
            {
                Digit0.Click -= value;
                Digit1.Click -= value;
                Digit2.Click -= value;
                Digit3.Click -= value;
                Digit4.Click -= value;
                Digit5.Click -= value;
                Digit6.Click -= value;
                Digit7.Click -= value;
                Digit8.Click -= value;
                Digit9.Click -= value;
                Dot.Click -= value;
            }
        }

        public event EventHandler OperatorClicked
        {
            add
            {
                Substraction.Click += value;
                Addition.Click += value;
                Multiplication.Click += value;
                Division.Click += value;
                SquareRoot.Click += value;
            }
            remove
            {
                Substraction.Click -= value;
                Addition.Click -= value;
                Multiplication.Click -= value;
                Division.Click -= value;
                SquareRoot.Click -= value;
            }
        }

        public event EventHandler MemoryClicked
        {
            add
            {
                MClear.Click += value;
                MRestore.Click += value;
                MPlus.Click += value;
                MMinus.Click += value; ;
                MStore.Click += value;
                MInfo.Click += value;
            }
            remove
            {
                MClear.Click -= value;
                MRestore.Click -= value;
                MPlus.Click -= value;
                MMinus.Click -= value; ;
                MStore.Click -= value;
                MInfo.Click -= value;
            }
        }

        public event EventHandler ClearEntryClicked
        {
            add
            {
                ClearEntry.Click += value;
            }
            remove
            {
                ClearEntry.Click -= value;
            }
        }

        public event EventHandler ClearAllClicked
        {
            add
            {
                ClearAll.Click += value;
            }
            remove
            {
                ClearAll.Click -= value;
            }
        }

        public event EventHandler SaveHistoryClicked
        {
            add
            {
                SaveHistory.Click += value;
            }
            remove
            {
                SaveHistory.Click -= value;
            }
        }

        public event EventHandler ResultClicked
        {
            add
            {
                Rezulta.Click += value;
            }
            remove
            {
                Rezulta.Click -= value;
            }
        }

        public event EventHandler EraseHistoryClicked
        {
            add
            {
                EraseHistory.Click += value;
            }
            remove
            {
                EraseHistory.Click -= value;
            }
        }

        public void SetResultBoxText(string rezultat)
        {
            ResultBox.Text = rezultat;
        }
        public void SetHistoryBoxText(string istoric)
        {
            HistoryBox.Text = istoric;
        }
        public void EquationLabel(string ecuatiePartiala)
        {
            Equation.Text = ecuatiePartiala;
        }
        public void MemoryButtonShow(string tooltipMemorie)
        {
            MemoryShow.SetToolTip(MInfo, tooltipMemorie);
        }

        public void EnableMemoryButtons()
        {
            MClear.Enabled = true;
            MInfo.Enabled = true;
            MRestore.Enabled = true;
        }

        public void DisableMemoryButtons()
        {
            MClear.Enabled = false;
            MInfo.Enabled = false;
            MRestore.Enabled = false;
        }
    }

    public interface ICalculatorView
    {
        event EventHandler DigitClicked;
        event EventHandler OperatorClicked;
        event EventHandler MemoryClicked;
        event EventHandler ClearEntryClicked;
        event EventHandler ClearAllClicked;
        event EventHandler SaveHistoryClicked;
        event EventHandler ResultClicked;
        event EventHandler EraseHistoryClicked;
        void MemoryButtonShow(string tooltipMemorie);
        void EquationLabel(string ecuatiePartiala);
        void SetHistoryBoxText(string istoric);
        void SetResultBoxText(string rezultat);
        void EnableMemoryButtons();
        void DisableMemoryButtons();
    }
}
