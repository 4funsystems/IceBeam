using NLua;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceBeam
{
    [Serializable]
    public class Settings
    {
        public List<Pattern> patterns = new List<Pattern>();
        public List<Script> functions = new List<Script>();
        public List<Variable> variables = new List<Variable>();
        public List<PointArea> pointareas = new List<PointArea>();
        public List<KeyScript> keyscripts = new List<KeyScript>();
        public List<PersScript> persscripts = new List<PersScript>();
    }
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
        public override string ToString()
        {
            return "[" + category + "] " + name;
        }

    }
    [Serializable]
    public class Script
    {
        public string name = "";
        public string code = "";

        public void Execute(object lua_object)
        {
            Lua lua = lua_object as Lua;
            try
            {
                lua.DoString(this.code);
            }catch(Exception e)
            {
                Main.Debug(e.Message);
            }
        }
        public override string ToString()
        {
            return name;
        }
    }
    [Serializable]
    public class KeyScript : Script
    {
        public int key = 0;
        public override string ToString()
        {
            return name;
        }
    }
    [Serializable]
    public class PersScript : Script
    {
        public bool enabled = false;
        public bool loop = false;
        public int min = 1000;
        public int max = 5000;
        public override string ToString()
        {
            return name + (loop ? "{" + min + " to " + max + "}" : "");
        }
    }
    [Serializable]
    public class Variable
    {
        public string name = "";
        public object value = null;
        public override string ToString()
        {
            return name + " - " + value.ToString();
        }

    }
    [Serializable]
    public class PointArea
    {
        public string name = "";
        public int type = 0;
        public Point point = Point.Empty;
        public Size size = Size.Empty;
        public Rectangle GetRectangle()
        {
            return new Rectangle(point, size);
        }
    }
}
