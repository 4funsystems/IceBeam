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
        public List<PointRect> pointrects = new List<PointRect>();
        public List<KeyScript> keyscripts = new List<KeyScript>();
        public List<PersScript> persscripts = new List<PersScript>();
    }
    [Serializable]
    public class Pattern
    {
        public string name = "";
        public int category = 0;
        public Bitmap bmp = new Bitmap(1, 1);
        public Pattern()
        {
            this.name = "New Pattern";
        }
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
        public Script()
        {
            this.name = "New Function";
        }
        public void Execute(object lua_object)
        {
            Lua lua = lua_object as Lua;
            try
            {
                lua.DoString(this.code);
            }catch(Exception e)
            {
                Main.Log(e.Message);
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
        public int key = 27;
        public bool enabled = false;
        public KeyScript()
        {
            this.name = "New Key Script";
        }
        public override string ToString()
        {
            return "["+(key!=27?Main.GetKeyCode(key):"NotSet")+"] "+name;
        }
    }
    [Serializable]
    public class PersScript : Script
    {
        public bool enabled = false;
        public bool loop = false;
        public int min = 1000;
        public int max = 5000;
        public PersScript()
        {
            this.name = "New Persistent Script";
        }
        public override string ToString()
        {
            return "["+(enabled?"ON":"OFF")+"] "+ (loop ? "{" + min + " to " + max + "} " : "") + name;
        }
    }
    [Serializable]
    public class Variable
    {
        public string name = "";
        public object value = "";
        public Variable()
        {
            this.name = "New Variable";
        }
        public override string ToString()
        {
            return name + " - " + value.ToString();
        }

    }
    [Serializable]
    public class PointRect
    {
        public string name = "";
        public int type = 0;
        public Point point = Point.Empty;
        public Size size = Size.Empty;
        public PointRect()
        {
            this.name = "New Point-Area";
        }
        public Rectangle GetRectangle()
        {
            return new Rectangle(point, size);
        }
        public override string ToString()
        {
            return (type==0?"[A]":"[P]")+name+(type==0?" {x:"+point.X+",y:"+point.Y+",w:"+size.Width+",h:"+size.Height+"}": "{x:" + point.X + ",y:" + point.Y + "}");
        }
    }
}
