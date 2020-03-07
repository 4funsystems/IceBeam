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
    public partial class KeyForm : Form
    {
        Main main;
        public KeyForm(Main m)
        {
            InitializeComponent();
            main = m;
            this.KeyDown += KeyForm_KeyPress;
        }

        private void KeyForm_Load(object sender, EventArgs e)
        {
            this.Location = new Point(main.Location.X + (main.Size.Width / 2) - (this.Size.Width / 2), main.Location.Y + (main.Size.Height / 2) - (this.Size.Height / 2));
        }
        private void KeyForm_KeyPress(object sender, KeyEventArgs e)
        {
            main.SetKey(e.KeyValue);
            this.Close();
        }
    }
}
