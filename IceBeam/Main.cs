using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IceBeam
{
    public partial class Main : Form
    {
        public LuaHandler lua;
        public static IceConsole console;
        public string last_file = "";
        public Main()
        {
            InitializeComponent();
            console = new IceConsole(this);
            lua = new LuaHandler(this);

        }

        #region STATIC FUNCTIONS
        public static void Debug(string s)
        {
            console.Write(s);
        }
        public static void Clear()
        {
            console.Clear();
        }
        public static string GetKeyCode(int key)
        {
           return (new KeysConverter()).ConvertToString(key);
        }
        #endregion

        #region FORM UPDATE
        public void UpdateForm(Settings settings)
        {
            UpdateKeyScriptList(settings.keyscripts);
            UpdatePersScriptList(settings.persscripts);
            UpdatePatternList(settings.patterns);
            UpdatePointAreaList(settings.pointareas);
            UpdateVariableList(settings.variables);
            UpdateFunctionList(settings.functions);
        }
        void UpdateKeyScriptList(List<KeyScript> scripts)
        {
            KeyScriptsList.Items.Clear();
            foreach (KeyScript ks in scripts)
                KeyScriptsList.Items.Add(ks.ToString());
        }
        void UpdatePersScriptList(List<PersScript> scripts)
        {
            PersScriptsList.Items.Clear();
            foreach (PersScript ps in scripts)
                PersScriptsList.Items.Add(ps.ToString());
        }
        void UpdatePatternList(List<Pattern> patterns)
        {
            PatternsList.Items.Clear();
            foreach (Pattern p in patterns)
                PatternsList.Items.Add(p.ToString());
        }
        void UpdatePointAreaList(List<PointArea> pointareas)
        {
            PointsAreasList.Items.Clear();
            foreach (PointArea pa in pointareas)
                PointsAreasList.Items.Add(pa.ToString());
        }
        void UpdateVariableList(List<Variable> variables)
        {
            VariablesList.Items.Clear();
            foreach (Variable v in variables)
                VariablesList.Items.Add(v.ToString());
        }
        void UpdateFunctionList(List<Script> functions)
        {
            FunctionsList.Items.Clear();
            foreach (Script s in functions)
                FunctionsList.Items.Add(s.ToString());
        }
        #endregion

        #region TOP MENU
        private void MenuFileNew_Click(object sender, EventArgs e)
        {
            last_file = "";
            lua.Reset();
        }
        private void MenuFileLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(ofd.FileName))
                {
                    lua.LoadSettings(ofd.FileName);
                    last_file = ofd.FileName;
                }
            }
        }
        private void MenuFileSave_Click(object sender, EventArgs e)
        {
            if (last_file != "" && File.Exists(last_file))
                lua.SaveSettings(last_file);

        }
        private void MenuFileSaveAs_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void SaveFile()
        {
            SaveFileDialog ofd = new SaveFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                lua.SaveSettings(ofd.FileName);
                last_file = ofd.FileName;
            }
        }
        private void MenuFileExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void MenuConsole_Click(object sender, EventArgs e)
        {
            if (console.IsDisposed)
                console = new IceConsole(this);
            console.Show();
        }
        #endregion

        #region KEYSCRIPTS
        int keyscript_temporary_index = -1;
        private void KeyScriptNew_Click(object sender, EventArgs e)
        {
            lua.settings.keyscripts.Add(new KeyScript());
            UpdateKeyScriptList(lua.settings.keyscripts);
            KeyScriptsList.SelectedIndex = lua.settings.keyscripts.Count() - 1;
        }

        private void KeyScriptRemove_Click(object sender, EventArgs e)
        {
            lua.settings.keyscripts.RemoveAt(keyscript_temporary_index);
            UpdateKeyScriptList(lua.settings.keyscripts);
            keyscript_temporary_index = -1;
            KeyScriptRemove.Hide();
        }

        private void KeyScriptName_TextChanged(object sender, EventArgs e)
        {
            lua.settings.keyscripts[keyscript_temporary_index].name = KeyScriptName.Text;
        }

        private void KeyScriptCode_TextChanged(object sender, EventArgs e)
        {
            lua.settings.keyscripts[keyscript_temporary_index].code = KeyScriptCode.Text;

        }

        private void KeyScriptSetKey_Click(object sender, EventArgs e)
        {
            KeyForm kf = new KeyForm(keyscript_temporary_index,this);
            kf.Show();

        }
        public void SetKey(int id, int key)
        {
            lua.settings.keyscripts[id].key = key;
        }

        private void KeyScriptsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            keyscript_temporary_index = KeyScriptsList.SelectedIndex;
            KeyScriptName.Text = lua.settings.keyscripts[keyscript_temporary_index].name;
            KeyScriptKey.Text = "<"+GetKeyCode(lua.settings.keyscripts[keyscript_temporary_index].key)+">";
            KeyScriptCode.Text = lua.settings.keyscripts[keyscript_temporary_index].code;
            KeyScriptDetails.Show();
            KeyScriptRemove.Show();
        }

        #endregion
    }
}
