using System.Windows.Forms;

namespace GettingStartedWithCSharp
{
    public class MessageBoxDisplayService : IMessageBoxDisplayService
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }

        public bool PromptUser(string mesaj, string titlu)
        {
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
        bool PromptUser(string mesaj, string titlu);
    }
}
