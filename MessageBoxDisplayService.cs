using System.Windows.Forms;

namespace GettingStartedWithCSharp
{
    public class MessageBoxDisplayService : IMessageBoxDisplayService
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }

        public bool PromptUser(string message, string title)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                return true;
            }
            else return false;
        }
    }

    public interface IMessageBoxDisplayService
    {
        void Show(string message);
        bool PromptUser(string message, string title);
    }
}
