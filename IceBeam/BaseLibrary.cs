using NLua;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
            luahandler.lua.RegisterFunction("debug", this, this.GetType().GetMethod("Debug"));
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
        public void Debug(string s)
        {
            Main.Debug(s);
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
    }
}
