using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingStartedWithCSharp
{
    class Utils : IUtils
    {
        public string FormatShownText(decimal number)
        {
            return number.ToString("0.000");
        }
    }
    public interface IUtils
    {
        string FormatShownText(decimal number);
    }
}
