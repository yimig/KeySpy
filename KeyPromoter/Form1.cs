using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyPromoter
{
    public partial class Form1 : Form
    {
        private Dictionary<bool, IKeyDown> method = new Dictionary<bool, IKeyDown>()
        {
            {true, new Win32ApiKeyDown()},
            {false, new DotNetApiKeyDown()},
        };

        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Keys key = (Keys) Enum.Parse(typeof(Keys), textBox1.Text.ToUpper());
            method[radioButton1.Checked].KeyDown(key);
        }
    }
}
