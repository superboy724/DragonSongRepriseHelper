using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DragonSongRepriseHelper
{
    public partial class SettingForm : UserControl
    {
        SettingContainer settingContainer;
        Action testFunction;
        Action testFunction2;
        public SettingForm(SettingContainer settingContainer,Action testFunction,Action testFunction2)
        {
            this.settingContainer = settingContainer;
            this.testFunction = testFunction;
            this.testFunction2 = testFunction2;
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            testFunction();
        }

        private void tbPlayers_TextChanged_1(object sender, EventArgs e)
        {
            settingContainer.UpdateKey("players", tbPlayers.Text);
        }

        private void tbPostNamazuUrl_TextChanged(object sender, EventArgs e)
        {
            settingContainer.UpdateKey("post_namazu_url", tbPostNamazuUrl.Text);
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            tbPlayers.Text = settingContainer.Get("players");
            tbPostNamazuUrl.Text = settingContainer.Get("post_namazu_url");
            cbP2Step1Enable.Checked = settingContainer.Get("p2step1enable") == "true";
            cbP2Step2Enable.Checked = settingContainer.Get("p2step2enable") == "true";
            cbP2Step3Enable.Checked = settingContainer.Get("p2step3enable") == "true";
            cbP2Step4Enable.Checked = settingContainer.Get("p2step4enable") == "true";
            cbP2Step2MarkDisabled.Checked = settingContainer.Get("p2step2markdisabled") == "true";
            cbP2Step4ChangeTowerEnable.Checked = settingContainer.Get("p2step4ChangeTowerEnable") == "true";

            if (string.IsNullOrEmpty(tbPostNamazuUrl.Text))
            {
                tbPostNamazuUrl.Text = "http://127.0.0.1:请修改端口号/command";
            }

            if (!cbP2Step2Enable.Checked)
            {
                cbP2Step2MarkDisabled.Enabled = false;
            }
            if (!cbP2Step3Enable.Checked)
            {
                cbP2Step4Enable.Enabled = false;
                cbP2Step4ChangeTowerEnable.Enabled = false;
            }
            if (!cbP2Step4Enable.Checked)
            {
                cbP2Step4ChangeTowerEnable.Enabled = false;
            }

            Log.bindTb = this.tbLog;
        }

        private void btnLogClear_Click(object sender, EventArgs e)
        {
            this.tbLog.Text = "";
        }

        private void cbP2Step1Enable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.UpdateKey("p2step1enable", cbP2Step1Enable.Checked ? "true" : "false");
        }

        private void cbP2Step2Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (cbP2Step2Enable.Checked)
            {
                cbP2Step2MarkDisabled.Enabled = true;
            }
            else
            {
                cbP2Step2MarkDisabled.Enabled = false;
            }
            this.settingContainer.UpdateKey("p2step2enable", cbP2Step2Enable.Checked ? "true" : "false");
        }

        private void btnTestBus_Click(object sender, EventArgs e)
        {
            testFunction2();
        }

        private void cbP2Step3Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbP2Step3Enable.Checked)
            {
                cbP2Step4Enable.Enabled = false;
                cbP2Step4ChangeTowerEnable.Enabled = false;
            }
            else
            {
                cbP2Step4Enable.Enabled = true;
                if (!cbP2Step4Enable.Checked)
                {
                    cbP2Step4ChangeTowerEnable.Enabled = false;
                }
                else
                {
                    cbP2Step4ChangeTowerEnable.Enabled = true;
                }
            }
            this.settingContainer.UpdateKey("p2step3enable", cbP2Step3Enable.Checked ? "true" : "false");
        }

        private void cbP2Step2MarkEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.UpdateKey("p2step2markdisabled", cbP2Step2MarkDisabled.Checked ? "true" : "false");
        }

        private void cbP2Step4Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbP2Step4Enable.Checked)
            {
                cbP2Step4ChangeTowerEnable.Enabled = false;
            }
            else
            {
                cbP2Step4ChangeTowerEnable.Enabled = true;
            }
            this.settingContainer.UpdateKey("p2step4enable", cbP2Step4Enable.Checked ? "true" : "false");
        }

        private void cbP2Step4ChangeTowerEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.UpdateKey("p2step4ChangeTowerEnable", cbP2Step4ChangeTowerEnable.Checked ? "true" : "false");
        }

        public void SetPlayers(List<string> players)
        {
            tbPlayers.Text = "";
            foreach (var item in players)
            {
                tbPlayers.AppendText(item + "\r\n");
            }

            settingContainer.UpdateKey("players", tbPlayers.Text);
        }
    }
}
