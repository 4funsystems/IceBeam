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
        int id;
        public KeyForm(int id, Main m)
        {
            InitializeComponent();
            main = m;
            this.id = id;
            this.KeyDown += KeyForm_KeyPress;
        }

        private void KeyForm_Load(object sender, EventArgs e)
        {

        }
        private void KeyForm_KeyPress(object sender, KeyEventArgs e)
        {
            main.SetKey(id,e.KeyValue);
            this.Close();
        }
    }
}
