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
        public List<IcePoint> points = new List<IcePoint>();
        public List<IceArea> areas = new List<IceArea>();
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

    }
    [Serializable]
    public class Script
    {
        public string name = "";
        public string code = "";

        public void Execute(object lua_object)
        {
            Lua lua = lua_object as Lua;
            lua.DoString(this.code);
        }
    }
    [Serializable]
    public class KeyScript : Script
    {
        public int key = 0;
    }
    [Serializable]
    public class PersScript : Script
    {
        public bool enabled = false;
        public bool loop = false;
        public int min = 1000;
        public int max = 5000;
    }
    [Serializable]
    public class Variable
    {
        public string name = "";
        public string type = "none";
        public object value = null;

    }
    [Serializable]
    public class IcePoint
    {
        public string name = "";
        public Point point = Point.Empty;
    }
    [Serializable]
    public class IceArea
    {
        public string name = "";
        public Rectangle area = Rectangle.Empty;
    }
}
