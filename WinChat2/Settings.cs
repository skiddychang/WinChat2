using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinChat2
{
    public partial class Settings : Form
    {
        Dictionary<String, Object> SettingsDic;
        const String SettingsFilename = "Settings.txt";

        const String stNick = "Nick";
        const String stDefaultAddress = "DefaultAddress";
        const String stFontFamily = "FontFamily";
        const String stFontSize = "FontSize";
        const String stFontStyle = "FontStyle";

        Boolean FChanged = false;

        public Settings()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            SettingsDic = FileHelper.GetDictionary(SettingsFilename);

            try
            {
                tbNick.Text = SettingsDic["nick"].ToString();
                tbDefaultAddress.Text = SettingsDic["defaultaddress"].ToString();

                //try
                //{
                //fontDialog1.Font = new Font(SettingsDic["fontfamily"].ToString(), (float)SettingsDic["fontsize"], (FontStyle)Enum.Parse(typeof(FontStyle), SettingsDic["fontstyle"].ToString()));
                //}
                //catch {}
            


            }
            catch { }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Save()
        {
            FileHelper.WriteToFile(SettingsFilename, stNick, tbNick.Text);
            FileHelper.WriteToFile(SettingsFilename, stDefaultAddress, tbDefaultAddress.Text);


            if (FChanged)
            {
                FileHelper.WriteToFile(SettingsFilename, stFontFamily, fontDialog1.Font.FontFamily.Name.ToString());
                FileHelper.WriteToFile(SettingsFilename, stFontSize, fontDialog1.Font.Size.ToString());
                FileHelper.WriteToFile(SettingsFilename, stFontStyle, fontDialog1.Font.Style.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            this.Close();
        }

        private void btnChooseFont_Click(object sender, EventArgs e)
        {
            DialogResult Result = fontDialog1.ShowDialog();
            if (Result == System.Windows.Forms.DialogResult.OK)
                FChanged = true;
        }

        void fontDialog1_Disposed(object sender, EventArgs e)
        {
            FChanged = true;
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {
            FChanged = true;
        }
    }
}
