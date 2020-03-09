using NLua;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IceBeam
{
    public class BaseLibrary
    {
        LuaHandler luahandler;
        public BaseLibrary(LuaHandler h)
        {
            this.luahandler = h;
        }
        public void RegisterFunctions()
        {
            luahandler.lua.RegisterFunction("log", this, this.GetType().GetMethod("Log"));
            luahandler.lua.RegisterFunction("clear", this, this.GetType().GetMethod("Clear"));
            luahandler.lua.RegisterFunction("lclick", this, this.GetType().GetMethod("LClick"));
            luahandler.lua.RegisterFunction("rclick", this, this.GetType().GetMethod("RClick"));
            luahandler.lua.RegisterFunction("ldrag", this, this.GetType().GetMethod("LDrag"));
            luahandler.lua.RegisterFunction("rdrag", this, this.GetType().GetMethod("RDrag"));
            luahandler.lua.RegisterFunction("setcursor", this, this.GetType().GetMethod("SetCursor"));
            luahandler.lua.RegisterFunction("getcursor", this, this.GetType().GetMethod("GetCursor"));
            luahandler.lua.RegisterFunction("point", this, this.GetType().GetMethod("point"));
            luahandler.lua.RegisterFunction("rectangle", this, this.GetType().GetMethod("rectangle"));
            luahandler.lua.RegisterFunction("screenshot", this, this.GetType().GetMethod("Screenshot"));
            luahandler.lua.RegisterFunction("findimages", this, this.GetType().GetMethod("Find"));
            luahandler.lua.RegisterFunction("findimage", this, this.GetType().GetMethod("FindOne"));
            luahandler.lua.RegisterFunction("findpatterns", this, this.GetType().GetMethod("FindPat"));
            luahandler.lua.RegisterFunction("findpattern", this, this.GetType().GetMethod("FindPatOne"));
            luahandler.lua.RegisterFunction("findpatternsonarea", this, this.GetType().GetMethod("FindPat"));
            luahandler.lua.RegisterFunction("findpatternonarea", this, this.GetType().GetMethod("FindPatOne"));
            luahandler.lua.RegisterFunction("findimageonarea", this, this.GetType().GetMethod("FindImageOnArea"));
            luahandler.lua.RegisterFunction("findimagesonarea", this, this.GetType().GetMethod("FindImagesOnArea"));
            luahandler.lua.RegisterFunction("write", this, this.GetType().GetMethod("Write"));
            luahandler.lua.RegisterFunction("sendkeyboard", this, this.GetType().GetMethod("SendKBD"));
            luahandler.lua.RegisterFunction("saveimage", this, this.GetType().GetMethod("SaveImage"));
            luahandler.lua.RegisterFunction("random", this, this.GetType().GetMethod("Random"));
            luahandler.lua.RegisterFunction("wait", this, this.GetType().GetMethod("Wait"));
            luahandler.lua.RegisterFunction("pattern", this, this.GetType().GetMethod("pattern"));
            luahandler.lua.RegisterFunction("savepattern", this, this.GetType().GetMethod("SavePattern"));


        }

        public void RegisterVariables()
        {
        }



        public Point GetCursor()
        {
            return Cursor.Position;
        }
        public Point point(int x, int y)
        {
            return new Point(x, y);
        }
        public Rectangle rectangle(int x, int y, int w, int h)
        {
            return new Rectangle(x, y, w, h);
        }
        public Bitmap Screenshot(Rectangle rect)
        {
            return luahandler.core.Screenshot(rect);
        }


        public Rectangle FindOne(Bitmap small, Bitmap big)
        {
            return luahandler.core.FindOne(small, big, luahandler.chromaColor);
        }
        public List<Rectangle> Find(Bitmap small, Bitmap big)
        {
            return luahandler.core.Find(small, big, luahandler.chromaColor);
        }


        public Rectangle FindPatternOnArea(Pattern pat, Rectangle rect)
        {
            return luahandler.core.FindOne(pat.bmp, luahandler.core.Screenshot(rect), luahandler.chromaColor);
        }
        public List<Rectangle> FindPatternsOnArea(Pattern pat, Rectangle rect)
        {
            return luahandler.core.Find(pat.bmp, luahandler.core.Screenshot(rect), luahandler.chromaColor);
        }


        public Rectangle FindImageOnArea(Bitmap bmp, Rectangle rect)
        {
            return luahandler.core.FindOne(bmp, luahandler.core.Screenshot(rect), luahandler.chromaColor);
        }
        public List<Rectangle> FindImagesOnArea(Bitmap bmp, Rectangle rect)
        {
            return luahandler.core.Find(bmp, luahandler.core.Screenshot(rect), luahandler.chromaColor);
        }

        public Rectangle FindPatOne(Pattern pat, Bitmap big)
        {
            return luahandler.core.FindOne(pat.bmp, big, luahandler.chromaColor);
        }
        public List<Rectangle> FindPat(Pattern pat, Bitmap big)
        {
            return luahandler.core.Find(pat.bmp, big, luahandler.chromaColor);
        }
        public void Log(string s)
        {
            Main.Log(s);
        }
        public void Clear()
        {
            Main.Clear();
        }

        public void LClick(Point location)
        {
            luahandler.core.MouseEvent_SlowLClick(location);
        }
        public void RClick(Point location)
        {
            luahandler.core.MouseEvent_SlowRClick(location);
        }
        public void LDrag(Point start,Point end)
        {
            luahandler.core.MouseEvent_L_SlowDrag(start, end);
        }
        public void RDrag(Point start, Point end)
        {
            luahandler.core.MouseEvent_R_SlowDrag(start, end);
        }
        public void SetCursor(Point location)
        {
            luahandler.core.SetCursor(location);
        }
        public void Write(string text)
        {
            luahandler.core.KeySend_SendWait(text);
            luahandler.core.KeySend_SendWait("{ENTER}");
        }
        public void SendKBD(string text)
        {
            luahandler.core.KeySend_SendWait(text);
        }
        public void SaveImage(Bitmap bmp, string path)
        {
            bmp.Save(path);
        }
        public int Random(int a, int b)
        {
            return luahandler.core.GetRandom(a, b);
        }
        public void Wait(int a, int b)
        {
            int x = luahandler.core.GetRandom(a, b);
            Thread.Sleep(x);
        }
        public Pattern pattern(Bitmap _bmp, string _name, int _category)
        {
            return new Pattern() { bmp = _bmp, name = _name, category = _category };
        }
        public void SavePattern(Pattern pattern)
        {
            luahandler.settings.patterns.Add(pattern);
            luahandler.main.UpdateForm(luahandler.settings);
        }

    }
}
