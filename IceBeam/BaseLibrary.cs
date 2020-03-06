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
            
        }

        public void RegisterVariables()
        {
            luahandler.lua["$mouse"] = GetCursor();
        }
        public Point GetCursor()
        {
            return Cursor.Position;
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
