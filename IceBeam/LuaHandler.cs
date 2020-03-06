using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IceCore;
using NLua;

namespace IceBeam
{
    public class LuaHandler
    {
        public IceCore.IceCore core = new IceCore.IceCore();
        public Settings settings = new Settings();
        public Lua lua;
        public Thread executing_thread;
        public BaseLibrary baselib;
        public Main main;
        public LuaHandler(Main m)
        {
            this.main = m;
            lua = new Lua();
        }
        public void LoadSettings(string path)
        {

            lua = new Lua();
            Initialize();
        }
        public void SaveSettings(string path)
        {

        }



        public async void Initialize()
        {
            //Load the base classes as new lua datatypes.
            InsertTypes();
            //Load the base functions & variables into the lua object.
            baselib = new BaseLibrary(this);
            baselib.RegisterFunctions();
            baselib.RegisterVariables();
            //Insert the user global variable names (as they're asignations, they're done directly)
            RegisterUserVariables();      
            //Insert all the user function scripts into the lua object by running them once.
            //Even its done in another thread, it waits until its done so it doesn't overcharge the cpu too much.
            foreach (Script script in settings.functions)
            {
                await Run(script);
            }


        }

        public void InsertTypes()
        {
            lua.LoadCLRPackage();
            lua.DoString(@"import('System.Drawing')
                           import('IceCore')");

        }
        public void Debug(string s)
        {
            main.console.Write(s);
        }
        public void Clear()
        {
            main.console.Clear();
        }
        public void RegisterUserVariables()
        {

            //Insert the user variables/points/areas as variables inside the lua object.      
            foreach (Variable v in settings.variables)
            {
                lua["$"+v.name] = v.value;
            }
            foreach (IcePoint p in settings.points)
            {
                lua["point_" + p.name] = p.point;
            }
            foreach (IceArea a in settings.areas)
            {
                lua["area_" + a.name] = a.area;
            }
        }

        public void ExecuteString(string s)
        {
            ParameterizedThreadStart ts = new ParameterizedThreadStart(Execute);
            Thread t = new Thread(ts);
            t.Start(s);
        }
        public void Execute(object s)
        {
            try
            {
                lua.DoString(s.ToString());
            }
            catch (Exception e)
            {

            }
        }

        public async Task<bool> Run(Script s)
        {
            ParameterizedThreadStart ts = new ParameterizedThreadStart(s.Execute);
            executing_thread = new Thread(ts);
            executing_thread.Start(lua);
            while (executing_thread != null && executing_thread.IsAlive && executing_thread.ThreadState == ThreadState.Running)
            {
                await Task.Delay(core.GetRandom(100, 150));
            }
            return true;
        }

        public async Task<bool> Run(PersScript s)
        {
            ParameterizedThreadStart ts = new ParameterizedThreadStart(s.Execute);
            Thread t = new Thread(ts);
            t.Start(lua);
            while (t.IsAlive || t.ThreadState == ThreadState.Running)
            {
                await Task.Delay(core.GetRandom(100, 150));
            }
            if (settings.persscripts[settings.persscripts.IndexOf(s)].loop && settings.persscripts[settings.persscripts.IndexOf(s)].enabled)
            {
                await Task.Delay(core.GetRandom(s.min, s.max));
                await Run(s);
            }
            return true;
        }
        public void Run(KeyScript s)
        {
            ParameterizedThreadStart ts = new ParameterizedThreadStart(s.Execute);
            Thread t = new Thread(ts);
            t.Start(lua);
        }
    }
}
