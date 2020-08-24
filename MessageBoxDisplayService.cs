using System.Windows.Forms;

namespace GettingStartedWithCSharp
{
    public class MessageBoxDisplayService : IMessageBoxDisplayService
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }

        public bool OnMemoryClear()
        {
            bool isMemoryStored = false;
            string mesaj = "Doresti sa golesti memoria?";
            string titlu = "Golire Memorie";
            MessageBoxButtons butoane = MessageBoxButtons.YesNo;
            DialogResult rezultat = MessageBox.Show(mesaj, titlu, butoane);
            if (rezultat == DialogResult.Yes)
            {
                isMemoryStored = false;
            }
            return isMemoryStored;
        }

        public bool HistoryClearing()
        {
            string mesaj = "Doresti sa stergi istoricul?";
            string titlu = "Stergere Istoric";
            MessageBoxButtons butoane = MessageBoxButtons.YesNo;
            DialogResult rezultat = MessageBox.Show(mesaj, titlu, butoane);
            if (rezultat == DialogResult.Yes)
            {
                return true;
            }
            else return false;
            
        }
    }

    public interface IMessageBoxDisplayService
    {
        void Show(string message);
        bool OnMemoryClear();
        bool HistoryClearing();
    }
}
