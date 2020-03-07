using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        public Settings settings;
        public Lua lua;
        public Thread executing_thread;
        public BaseLibrary baselib;
        public Main main;
        public LuaHandler(Main m)
        {
            this.main = m;
            Reset();
        }
        public void LoadSettings(string path)
        {
            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    settings = bf.Deserialize(fs) as Settings;
                }
                main.UpdateForm(settings);
            }
        }
        public void SaveSettings(string path)
        {
            if (!File.Exists(path)) File.Create(path).Close();
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                bf.Serialize(fs, settings);
            }
        }

        public void Reset()
        {
            lua = new Lua();
            settings = new Settings();
            main.UpdateForm(settings);
            Initialize();
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
        public void RegisterUserVariables()
        {

            //Insert the user variables/points/areas as variables inside the lua object.   
            foreach (Pattern p in settings.patterns)
            {
                lua["pat_"+p.category+"_" + p.name] = p.bmp;
            }
            foreach (Variable v in settings.variables)
            {
                lua["var_" + v.name] = v.value;
            }
            foreach (PointArea pa in settings.pointareas)
            {
                if (pa.type == 1)
                    lua["point_" + pa.name] = pa.point;
                else
                    lua["area_" + pa.name] = pa.GetRectangle();
            }
        }

        public void ExecuteText(string s)
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
                Main.Debug(e.Message);
            }
        }

        public async void Pulse(PersScript s)
        {
            if (s.enabled)
                s.enabled = false;
            else
            {
                s.enabled = true;
                await Run(s);
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
                if (settings.persscripts[settings.persscripts.IndexOf(s)].enabled)
                    await Task.Delay(core.GetRandom(100, 150));
                else
                    t.Abort();
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
