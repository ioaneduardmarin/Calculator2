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
            string mesaj = "Do you want to clear the memory?";
            string titlu = "Memory Clear";
            MessageBoxButtons butoane = MessageBoxButtons.YesNo;
            DialogResult rezultat = MessageBox.Show(mesaj, titlu, butoane);
            if (rezultat == DialogResult.Yes)
            {
                isMemoryStored = false;
            }
            return isMemoryStored;
        }
    }

    public interface IMessageBoxDisplayService
    {
        void Show(string message);
        bool OnMemoryClear();
    }
}
