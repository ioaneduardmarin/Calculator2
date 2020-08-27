using System.IO;
using System.Windows.Forms;

namespace GettingStartedWithCSharp
{
    class SaveHistoryService : ISaveHistoryService
    {
        public bool SaveHistory(string istoric)
        {
            bool isHistorySaved = false;
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Text File|*"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                using (var sw = new StreamWriter(File.Create(path)))
                {
                    sw.Write(istoric);
                    sw.Dispose();
                    isHistorySaved = true;
                }
            }

            if (!isHistorySaved)
                return false;
            else
                return true;
        }
    }

    public interface ISaveHistoryService
    {
        bool SaveHistory(string istoric);
    }
}
