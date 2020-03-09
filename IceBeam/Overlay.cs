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
    public partial class Overlay : Form
    {
        Main main;
        UserRect rect;
        int type;
        int index = -1;
        public Overlay(Main m, int type)
        {
            InitializeComponent();
            main = m;
            this.type = type;

            this.Location = Screen.PrimaryScreen.Bounds.Location;
            this.Size = Screen.PrimaryScreen.Bounds.Size;
            pictureBox1.Location = this.Location;
            pictureBox1.Size = this.Size;
            this.WindowState = FormWindowState.Minimized;
            Bitmap bmp = main.lh.core.Screenshot(Screen.PrimaryScreen.Bounds);
            pictureBox1.Image = bmp;
            this.WindowState = FormWindowState.Maximized;
            this.KeyDown += Overlay_KeyDown;

            if (type == 2)
                SetupPoint();
            else
                SetupRect();
        }
        private void Overlay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                HandleEnter();
            }
        }
        void SetupPoint()
        {
            this.Cursor = Cursors.Hand;
        }
        void SetupRect()
        {
            rect = new UserRect(new Rectangle(100,100,100,100));
            rect.SetPictureBox(pictureBox1);
            rect.allowDeformingDuringMovement = true;
        }
        void HandleEnter()
        {
            if (type == 0)
            {
                Bitmap bmp = pictureBox1.Image as Bitmap;
                main.SetImage(bmp.Clone(rect.rect,System.Drawing.Imaging.PixelFormat.DontCare));
                main.Show();
                this.Close();
            }
            else if (type == 1)
            {
                main.SetPointRectProperties(rect.rect);
                main.Show();
                this.Close();
            }
        }
        void HandleClick()
        {
            if (type == 2)
            {
                this.Cursor = Cursors.Arrow;
                main.SetPointRectProperties(Cursor.Position);
                main.Show();
                this.Close();
            }
        }
        private void Overlay_Load(object sender, EventArgs e)
        {
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            HandleClick();
        }
    }
}
