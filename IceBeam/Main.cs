using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IceBeam
{
    public partial class Main : Form
    {
        public LuaHandler lh;
        public static IceConsole console;
        private GlobalKeyboardHook _globalKeyboardHook;
        public Main()
        {
            InitializeComponent();
            SetProcessDPIAware();
            CheckForIllegalCrossThreadCalls = false;
            console = new IceConsole(this);
            lh = new LuaHandler(this);
            PatternImage.BackgroundImageChanged += PatternImage_ImageChanged;
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        #region STATIC FUNCTIONS
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetProcessDPIAware();
        public static void Log(string s)
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
            {
                PersScriptsList.Items.Add(ps.ToString());
            }
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
        public string last_file = "";
        private void MenuFileNew_Click(object sender, EventArgs e)
        {
            last_file = "";
            lh.Reset();
        }
        private void MenuFileLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = last_file;
            ofd.DefaultExt = "ibs";
            ofd.Filter = "IceBeam Settings (.ibs) | *.ibs";
            ofd.ValidateNames = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(ofd.FileName))
                {
                    lh.LoadSettings(ofd.FileName);
                    last_file = ofd.FileName;
                }
            }
        }
        private void MenuFileSave_Click(object sender, EventArgs e)
        {
            if (last_file != "" && File.Exists(last_file))
            {
                lh.SaveSettings(last_file);
            }

        }
        private void MenuFileSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.FileName = last_file;
            ofd.DefaultExt = "ibs";
            ofd.Filter = "IceBeam Settings (.ibs) | *.ibs";
            ofd.ValidateNames = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                lh.SaveSettings(ofd.FileName);
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

        public void SetKey(int key)
        {            
            lh.settings.keyscripts[keyscript_temporary_index].key = key;
            int temp = keyscript_temporary_index;
            UpdateKeyScriptList(lh.settings.keyscripts);
            KeyScriptsList.SelectedIndex = temp;
        }

        private void KeyScriptsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (KeyScriptsList.SelectedIndex >= 0 && KeyScriptsList.SelectedIndex < KeyScriptsList.Items.Count)
            {
                keyscript_temporary_index = KeyScriptsList.SelectedIndex;
                KeyScriptName.Text = lh.settings.keyscripts[keyscript_temporary_index].name;
                KeyScriptKey.Text = (lh.settings.keyscripts[keyscript_temporary_index].key != 27?"<" + GetKeyCode(lh.settings.keyscripts[keyscript_temporary_index].key) + ">":"<NONE>");
                KeyScriptCode.Text = lh.settings.keyscripts[keyscript_temporary_index].code;
                KeyScriptEnabled.Checked = lh.settings.keyscripts[keyscript_temporary_index].enabled;
                KeyScriptDetails.Show();
                KeyScriptRemove.Show();
            }
        }

        private void KeyScriptNew_Click(object sender, EventArgs e)
        {
            lh.settings.keyscripts.Add(new KeyScript());
            UpdateKeyScriptList(lh.settings.keyscripts);
            KeyScriptsList.SelectedIndex = lh.settings.keyscripts.Count() - 1;
        }

        private void KeyScriptRemove_Click(object sender, EventArgs e)
        {
            lh.settings.keyscripts.RemoveAt(keyscript_temporary_index);
            UpdateKeyScriptList(lh.settings.keyscripts);
            keyscript_temporary_index = -1;
            KeyScriptRemove.Hide();
        }

        private void KeyScriptName_TextChanged(object sender, EventArgs e)
        {
            lh.settings.keyscripts[keyscript_temporary_index].name = KeyScriptName.Text;
            int temp = keyscript_temporary_index;
            UpdateKeyScriptList(lh.settings.keyscripts);
            KeyScriptsList.SelectedIndex = temp;
        }

        private void KeyScriptCode_TextChanged(object sender, EventArgs e)
        {
            lh.settings.keyscripts[keyscript_temporary_index].code = KeyScriptCode.Text;

        }

        private void KeyScriptSetKey_Click(object sender, EventArgs e)
        {
            KeyForm kf = new KeyForm(this);
            kf.Show();

        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            bool ret = true;
            foreach (KeyScript ks in lh.settings.keyscripts)
            {
                if (ks.enabled && ks.key == e.KeyboardData.VirtualCode && e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown && e.KeyboardData.VirtualCode != 27)
                {
                    lh.Run(ks);
                    ret = false;
                }
            }
            if (ret)
                return;


        }
        
        private void KeyScriptEnabled_CheckedChanged(object sender, EventArgs e)
        {
            lh.settings.keyscripts[keyscript_temporary_index].enabled = KeyScriptEnabled.Checked;
        }
        #endregion

        #region PERSISTENT SCRIPTS
        int persscript_temporary_index = -1;
        private void PersScriptNew_Click(object sender, EventArgs e)
        {
            lh.settings.persscripts.Add(new PersScript());
            UpdatePersScriptList(lh.settings.persscripts);
            PersScriptsList.SelectedIndex = lh.settings.persscripts.Count() - 1;
        }

        private void PersScriptRemove_Click(object sender, EventArgs e)
        {
            lh.settings.persscripts.RemoveAt(persscript_temporary_index);
            UpdatePersScriptList(lh.settings.persscripts);
            persscript_temporary_index = -1;
            PersScriptDetails.Hide();
            PersScriptRemove.Hide();
        }

        private void PersScriptName_TextChanged(object sender, EventArgs e)
        {
            lh.settings.persscripts[persscript_temporary_index].name = PersScriptName.Text;
            int temp = persscript_temporary_index;
            UpdatePersScriptList(lh.settings.persscripts);
            PersScriptsList.SelectedIndex = temp;
        }

        private void PersScriptLoop_CheckedChanged(object sender, EventArgs e)
        {
            lh.settings.persscripts[persscript_temporary_index].loop = PersScriptLoop.Checked;
            if (PersScriptLoop.Checked)
                PersScriptLoopPanel.Show();
            else
                PersScriptLoopPanel.Hide();
            int temp = persscript_temporary_index;
            UpdatePersScriptList(lh.settings.persscripts);
            PersScriptsList.SelectedIndex = temp;
        }

        private void PersScriptMin_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.persscripts[persscript_temporary_index].min = (int)PersScriptMin.Value;
            int temp = persscript_temporary_index;
            UpdatePersScriptList(lh.settings.persscripts);
            PersScriptsList.SelectedIndex = temp;
        }

        private void PersScriptMax_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.persscripts[persscript_temporary_index].max = (int)PersScriptMax.Value;
            int temp = persscript_temporary_index;
            UpdatePersScriptList(lh.settings.persscripts);
            PersScriptsList.SelectedIndex = temp;
        }

        private void PersScriptTurnOff_Click(object sender, EventArgs e)
        {
            lh.Pulse(lh.settings.persscripts[persscript_temporary_index]);
            UpdatePersScriptList(lh.settings.persscripts);
            if (lh.settings.persscripts[persscript_temporary_index].enabled)
            {
                PersScriptStatus.Text = "Running (Changes are disabled while script is being executed.)";
                PersScriptStatus.ForeColor = Color.Green;
                PersScriptTurnOff.Text = "Stop execution";
                PersScriptCode.Hide();
            }
            else
            {
                PersScriptStatus.Text = "Not running";
                PersScriptStatus.ForeColor = Color.Red;
                PersScriptTurnOff.Text = "Run script";
                PersScriptCode.Show();
            }
        }

        private void PersScriptCode_TextChanged(object sender, EventArgs e)
        {
            lh.settings.persscripts[persscript_temporary_index].code = PersScriptCode.Text;
        }

        private void PersScriptsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PersScriptsList.SelectedIndex >= 0 && PersScriptsList.SelectedIndex < PersScriptsList.Items.Count)
            {
                persscript_temporary_index = PersScriptsList.SelectedIndex;
                PersScriptDetails.Show();
                PersScriptRemove.Show();
                PersScriptName.Text = lh.settings.persscripts[persscript_temporary_index].name;
                PersScriptCode.Text = lh.settings.persscripts[persscript_temporary_index].code;
                PersScriptLoop.Checked = lh.settings.persscripts[persscript_temporary_index].loop;
                if (PersScriptLoop.Checked)
                {
                    PersScriptMin.Value = lh.settings.persscripts[persscript_temporary_index].min;
                    PersScriptMax.Value = lh.settings.persscripts[persscript_temporary_index].max;
                }
                if (lh.settings.persscripts[persscript_temporary_index].enabled)
                {
                    PersScriptStatus.Text = "Running (Changes are disabled while script is being executed.)";
                    PersScriptStatus.ForeColor = Color.Green;
                    PersScriptTurnOff.Text = "Stop execution";
                    PersScriptCode.Hide();
                }
                else
                {
                    PersScriptStatus.Text = "Not running";
                    PersScriptStatus.ForeColor = Color.Red;
                    PersScriptTurnOff.Text = "Run script";
                    PersScriptCode.Show();
                }
            }
        }


        #endregion

        #region PATTERNS
        int pattern_temporary_index = -1;
        public void SetImage(Bitmap bmp)
        {
            PatternImage.BackgroundImage = bmp;
        }

        private void PatternsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PatternsList.SelectedIndex >= 0 && PatternsList.SelectedIndex < PatternsList.Items.Count)
            {
                pattern_temporary_index = PatternsList.SelectedIndex;
                PatternDetails.Show();
                PatternRemove.Show();
                PatternCategory.Value = lh.settings.patterns[pattern_temporary_index].category;
                PatternName.Text = lh.settings.patterns[pattern_temporary_index].name;
                PatternImage.Image = lh.settings.patterns[pattern_temporary_index].bmp;
            }
        }

        private void PatternNew_Click(object sender, EventArgs e)
        {
            lh.settings.patterns.Add(new Pattern());
            UpdatePatternList(lh.settings.patterns);
            PatternsList.SelectedIndex = lh.settings.patterns.Count - 1;
            lh.RegisterUserVariables();
        }

        private void PatternRemove_Click(object sender, EventArgs e)
        {
            //lh.ClearVar("pat_" + lh.settings.patterns[pattern_temporary_index].category + "_" + lh.settings.patterns[pattern_temporary_index].name);
            lh.settings.patterns.RemoveAt(pattern_temporary_index);
            UpdatePatternList(lh.settings.patterns);
            pattern_temporary_index = -1;
            PatternDetails.Hide();
            PatternRemove.Hide();
        }

        private void PatternName_TextChanged(object sender, EventArgs e)
        {
            lh.settings.patterns[pattern_temporary_index].name = PatternName.Text;
            int temp = pattern_temporary_index;
            UpdatePatternList(lh.settings.patterns);
            lh.RegisterUserVariables();
            PatternsList.SelectedIndex = temp;
        }

        private void PatternCategory_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.patterns[pattern_temporary_index].category = (int)PatternCategory.Value;
            int temp = pattern_temporary_index;
            UpdatePatternList(lh.settings.patterns);
            PatternsList.SelectedIndex = temp;
            lh.RegisterUserVariables();
        }

        private void PatternLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
                PatternImage.Image = Image.FromFile(ofd.FileName);
        }

        private void PatternScreen_Click(object sender, EventArgs e)
        {
            Overlay ov = new Overlay(this, 0);
            ov.Show();
            this.Hide();
        }

        private void PatternSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
                lh.settings.patterns[pattern_temporary_index].Save(sfd.FileName);
        }

        private void PatternImage_ImageChanged(object sender, EventArgs e)
        {
            lh.settings.patterns[pattern_temporary_index].bmp = PatternImage.BackgroundImage as Bitmap;
            lh.RegisterUserVariables();
        }
        #endregion;

        #region POINTS AREAS
        int pointarea_temporary_index = -1;
        public void SetPointAreaProperties(Point pt)
        {
            PointAreaX.Value = pt.X;
            PointAreaY.Value = pt.Y;
        }

        public void SetPointAreaProperties(Rectangle rect)
        {

            PointAreaX.Value = rect.X;
            PointAreaY.Value = rect.Y;
            PointAreaW.Value = rect.Width;
            PointAreaH.Value = rect.Height;
        }

        private void PointsAreasList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PointsAreasList.SelectedIndex >= 0 && PointsAreasList.SelectedIndex < PointsAreasList.Items.Count)
            {
                pointarea_temporary_index = PointsAreasList.SelectedIndex;
                PointAreaDetails.Show();
                PointAreaRemove.Show();
                PointAreaName.Text = lh.settings.pointareas[pointarea_temporary_index].name;
                PointAreaType.SelectedIndex = lh.settings.pointareas[pointarea_temporary_index].type;
                PointAreaX.Value = lh.settings.pointareas[pointarea_temporary_index].point.X;
                PointAreaY.Value = lh.settings.pointareas[pointarea_temporary_index].point.Y;
                PointAreaW.Value = lh.settings.pointareas[pointarea_temporary_index].size.Width;
                PointAreaH.Value = lh.settings.pointareas[pointarea_temporary_index].size.Height;
            }
        }

        private void PointAreaNew_Click(object sender, EventArgs e)
        {
            lh.settings.pointareas.Add(new PointArea());
            UpdatePointAreaList(lh.settings.pointareas);
            PointsAreasList.SelectedIndex = lh.settings.pointareas.Count - 1;
            lh.RegisterUserVariables();

        }

        private void PointAreaRemove_Click(object sender, EventArgs e)
        {
            lh.settings.pointareas.RemoveAt(pointarea_temporary_index);
            UpdatePointAreaList(lh.settings.pointareas);
            pointarea_temporary_index = -1;
            PointAreaRemove.Hide();
            PointAreaDetails.Hide();
        }

        private void PointAreaName_TextChanged(object sender, EventArgs e)
        {
            lh.settings.pointareas[pointarea_temporary_index].name = PointAreaName.Text;
            int temp = pointarea_temporary_index;
            UpdatePointAreaList(lh.settings.pointareas);
            PointsAreasList.SelectedIndex = temp;
            lh.RegisterUserVariables();
        }

        private void PointAreaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lh.settings.pointareas[pointarea_temporary_index].type = PointAreaType.SelectedIndex;
            if (PointAreaType.SelectedIndex == 0)
                PointAreaSize.Show();
            else
                PointAreaSize.Hide();
            int temp = pointarea_temporary_index;
            UpdatePointAreaList(lh.settings.pointareas);
            PointsAreasList.SelectedIndex = temp;
            lh.RegisterUserVariables();
        }

        private void PointAreaGet_Click(object sender, EventArgs e)
        {
            Overlay ov = new Overlay(this, lh.settings.pointareas[pointarea_temporary_index].type + 1);
            ov.Show();
            this.Hide();
        }

        private void PointAreaX_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.pointareas[pointarea_temporary_index].point.X = (int)PointAreaX.Value;
            int temp = pointarea_temporary_index;
            UpdatePointAreaList(lh.settings.pointareas);
            PointsAreasList.SelectedIndex = temp;
            lh.RegisterUserVariables();
        }

        private void PointAreaY_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.pointareas[pointarea_temporary_index].point.Y = (int)PointAreaY.Value;
            int temp = pointarea_temporary_index;
            UpdatePointAreaList(lh.settings.pointareas);
            PointsAreasList.SelectedIndex = temp;
            lh.RegisterUserVariables();
        }

        private void PointAreaW_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.pointareas[pointarea_temporary_index].size.Width = (int)PointAreaW.Value;
            int temp = pointarea_temporary_index;
            UpdatePointAreaList(lh.settings.pointareas);
            PointsAreasList.SelectedIndex = temp;
            lh.RegisterUserVariables();
        }

        private void PointAreaH_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.pointareas[pointarea_temporary_index].size.Height = (int)PointAreaH.Value;
            int temp = pointarea_temporary_index;
            UpdatePointAreaList(lh.settings.pointareas);
            PointsAreasList.SelectedIndex = temp;
            lh.RegisterUserVariables();
        }


        #endregion

        #region VARIABLES
        int variable_temporary_index = -1;
        private void VariablesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VariablesList.SelectedIndex >= 0 && VariablesList.SelectedIndex < VariablesList.Items.Count)
            {
                variable_temporary_index = VariablesList.SelectedIndex;
                VariableDetails.Show();
                VariableRemove.Show();
                VariableName.Text = lh.settings.variables[variable_temporary_index].name;
                VariableValue.Text = lh.settings.variables[variable_temporary_index].value.ToString();
            }
        }

        private void VariableNew_Click(object sender, EventArgs e)
        {
            lh.settings.variables.Add(new Variable());
            UpdateVariableList(lh.settings.variables);
            VariablesList.SelectedIndex = lh.settings.variables.Count - 1;
            lh.RegisterUserVariables();
        }

        private void VariableRemove_Click(object sender, EventArgs e)
        {
            lh.settings.variables.RemoveAt(variable_temporary_index);
            UpdateVariableList(lh.settings.variables);
            variable_temporary_index = -1;
            VariableRemove.Hide();
            VariableDetails.Hide();
        }

        private void VariableName_TextChanged(object sender, EventArgs e)
        {
            lh.settings.variables[variable_temporary_index].name = VariableName.Text;
            int temp = variable_temporary_index;
            UpdateVariableList(lh.settings.variables);
            VariablesList.SelectedIndex = temp;
            lh.RegisterUserVariables();
        }

        private void VariableValue_TextChanged(object sender, EventArgs e)
        {
            lh.settings.variables[variable_temporary_index].value = VariableValue.Text;
            int temp = variable_temporary_index;
            UpdateVariableList(lh.settings.variables);
            VariablesList.SelectedIndex = temp;
            lh.RegisterUserVariables();
        }

        #endregion

        #region FUNCTIONS
        int function_temporary_index = -1;
        private void FunctionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FunctionsList.SelectedIndex >= 0 && FunctionsList.SelectedIndex < FunctionsList.Items.Count)
            {
                function_temporary_index = FunctionsList.SelectedIndex;
                FunctionDetails.Show();
                FunctionRemove.Show();
                FunctionName.Text = lh.settings.functions[function_temporary_index].name;
                FunctionCode.Text = lh.settings.functions[function_temporary_index].code;
            }
        }

        private void FunctionNew_Click(object sender, EventArgs e)
        {
            lh.settings.functions.Add(new Script());
            UpdateFunctionList(lh.settings.functions);
            FunctionsList.SelectedIndex = FunctionsList.Items.Count - 1;

        }

        private void FunctionName_TextChanged(object sender, EventArgs e)
        {
            lh.settings.functions[function_temporary_index].name = FunctionName.Text;
            int temp = function_temporary_index;
            UpdateFunctionList(lh.settings.functions);
            FunctionsList.SelectedIndex = temp;

        }

        private void FunctionCode_TextChanged(object sender, EventArgs e)
        {
            lh.settings.functions[function_temporary_index].code = FunctionCode.Text;
        }

        private void FunctionRemove_Click(object sender, EventArgs e)
        {
            lh.settings.functions.RemoveAt(function_temporary_index);
            UpdateFunctionList(lh.settings.functions);
            function_temporary_index = -1;
            FunctionDetails.Hide();
            FunctionRemove.Hide();
        }
        #endregion

    }
}
