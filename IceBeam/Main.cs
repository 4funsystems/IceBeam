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
        public LuaHandler lh;
        public static IceConsole console;
        public Main()
        {
            InitializeComponent();
            console = new IceConsole(this);
            lh = new LuaHandler(this);
            PersScriptsList.ItemCheck += PersScriptsList_ItemCheck;
            PatternImage.BackgroundImageChanged += PatternImage_ImageChanged;
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
        public string last_file = "";
        private void MenuFileNew_Click(object sender, EventArgs e)
        {
            last_file = "";
            lh.Reset();
        }
        private void MenuFileLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
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
                lh.SaveSettings(last_file);

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
        }

        private void KeyScriptCode_TextChanged(object sender, EventArgs e)
        {
            lh.settings.keyscripts[keyscript_temporary_index].code = KeyScriptCode.Text;

        }

        private void KeyScriptSetKey_Click(object sender, EventArgs e)
        {
            KeyForm kf = new KeyForm(keyscript_temporary_index,this);
            kf.Show();

        }
        public void SetKey(int id, int key)
        {
            lh.settings.keyscripts[id].key = key;
        }

        private void KeyScriptsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            keyscript_temporary_index = KeyScriptsList.SelectedIndex;
            KeyScriptName.Text = lh.settings.keyscripts[keyscript_temporary_index].name;
            KeyScriptKey.Text = "<"+GetKeyCode(lh.settings.keyscripts[keyscript_temporary_index].key)+">";
            KeyScriptCode.Text = lh.settings.keyscripts[keyscript_temporary_index].code;
            KeyScriptDetails.Show();
            KeyScriptRemove.Show();
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
        }

        private void PersScriptMin_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.persscripts[persscript_temporary_index].min = (int)PersScriptMin.Value;
        }

        private void PersScriptMax_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.persscripts[persscript_temporary_index].max = (int)PersScriptMax.Value;
        }

        private void PersScriptTurnOff_Click(object sender, EventArgs e)
        {
            lh.Pulse(lh.settings.persscripts[persscript_temporary_index]);
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
                }
                else
                {
                    PersScriptStatus.Text = "Not running";
                    PersScriptStatus.ForeColor = Color.Red;
                    PersScriptTurnOff.Text = "Run script";
                }
            }
        }

        private void PersScriptsList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            lh.Pulse(lh.settings.persscripts[e.Index]);
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
        }

        private void PatternRemove_Click(object sender, EventArgs e)
        {
            lh.settings.patterns.RemoveAt(pattern_temporary_index);
            UpdatePatternList(lh.settings.patterns);
            pattern_temporary_index = -1;
        }

        private void PatternName_TextChanged(object sender, EventArgs e)
        {
            lh.settings.patterns[pattern_temporary_index].name = PatternName.Text;
            int temp = pattern_temporary_index;
            UpdatePatternList(lh.settings.patterns);
            PatternsList.SelectedIndex = temp;
        }

        private void PatternCategory_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.patterns[pattern_temporary_index].category = (int)PatternCategory.Value;
            int temp = pattern_temporary_index;
            UpdatePatternList(lh.settings.patterns);
            PatternsList.SelectedIndex = temp;
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

        }

        private void PointAreaRemove_Click(object sender, EventArgs e)
        {
            lh.settings.pointareas.RemoveAt(pointarea_temporary_index);
            UpdatePointAreaList(lh.settings.pointareas);
            pointarea_temporary_index = -1;
        }

        private void PointAreaName_TextChanged(object sender, EventArgs e)
        {
            lh.settings.pointareas[pointarea_temporary_index].name = PointAreaName.Text;
            int temp = pointarea_temporary_index;
            UpdatePointAreaList(lh.settings.pointareas);
            PointsAreasList.SelectedIndex = temp;
        }

        private void PointAreaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lh.settings.pointareas[pointarea_temporary_index].type = PointAreaType.SelectedIndex;
            if (PointAreaType.SelectedIndex == 0)
                PointAreaSize.Show();
            else
                PointAreaSize.Hide();
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
        }

        private void PointAreaY_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.pointareas[pointarea_temporary_index].point.Y = (int)PointAreaY.Value;
        }

        private void PointAreaW_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.pointareas[pointarea_temporary_index].size.Width = (int)PointAreaW.Value;
        }

        private void PointAreaH_ValueChanged(object sender, EventArgs e)
        {
            lh.settings.pointareas[pointarea_temporary_index].size.Height = (int)PointAreaH.Value;
        }
        #endregion



    }
}
