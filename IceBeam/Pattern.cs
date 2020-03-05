using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceBeam
{
    [Serializable]
    public class Pattern
    {
        public string name = "";
        public int category = 0;
        public Bitmap bmp = new Bitmap(1, 1);

        public void Save(string dirpath)
        {
            bmp.Save(dirpath + category + "-" + name, System.Drawing.Imaging.ImageFormat.Png);
        }

    }
}
