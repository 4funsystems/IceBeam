using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IceBeam
{
    public partial class IceConsole : Form
    {
        Main m;
        public IceConsole(Main main)
        {
            InitializeComponent();
            m = main;
        }

        private void IceConsole_Load(object sender, EventArgs e)
        {

        }
        public void Write (string s)
        {
            if (s != null)
            {
                if (ConsoleList.Items.Count > 50)
                {
                    ConsoleList.Items.RemoveAt(0);
                }
                ConsoleList.Items.Add(s);
            }
        }
        public void Clear()
        {
            ConsoleList.Items.Clear();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            m.lh.ExecuteText(textBox1.Text);
        }
    }
}
