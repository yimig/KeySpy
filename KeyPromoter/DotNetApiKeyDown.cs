using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyPromoter
{
    public class DotNetApiKeyDown : IKeyDown
    {
        public void KeyDown(Keys key)
        {
            var keyStr = key.ToString().Length > 1 ? "{" + key.ToString() + "}" : key.ToString();
            SendKeys.Send(keyStr);
        }
    }
}
