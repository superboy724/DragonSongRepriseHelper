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
            settingContainer.PlayerSetting.SetPlayerFromPlayerText(tbPlayers.Text);
        }

        private void tbPostNamazuUrl_TextChanged(object sender, EventArgs e)
        {
            settingContainer.FunctionSetting.PostNamazuSetting = tbPostNamazuUrl.Text;
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            tbPlayers.Text = settingContainer.PlayerSetting.BuildPlayerTextBoxStr();
            tbPostNamazuUrl.Text = settingContainer.FunctionSetting.PostNamazuSetting;
            cbP2Step1Enable.Checked = settingContainer.FunctionSetting.P2Step1Enable;
            cbP2Step2Enable.Checked = settingContainer.FunctionSetting.P2Step2Enable;
            cbP2Step3Enable.Checked = settingContainer.FunctionSetting.P2Step3Enable;
            cbP2Step4Enable.Checked = settingContainer.FunctionSetting.P2Step4Enable;
            cbP2Step2MarkDisabled.Checked = settingContainer.FunctionSetting.P2Step2MarkDisabled;
            cbP2Step4ChangeTowerEnable.Checked = settingContainer.FunctionSetting.P2Step4ChangeTowerEnable;

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
            this.settingContainer.FunctionSetting.P2Step1Enable = cbP2Step1Enable.Checked;
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
            this.settingContainer.FunctionSetting.P2Step2Enable = cbP2Step2Enable.Checked;
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
            cbP2Step2Enable.Checked = cbP2Step3Enable.Checked;
        }

        private void cbP2Step2MarkEnable_CheckedChanged(object sender, EventArgs e)
        {
            cbP2Step2Enable.Checked = cbP2Step2MarkDisabled.Checked;
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
            cbP2Step2Enable.Checked = cbP2Step4Enable.Checked;
        }

        private void cbP2Step4ChangeTowerEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.FunctionSetting.P2Step4ChangeTowerEnable = cbP2Step4ChangeTowerEnable.Checked;
        }

        public void SetPlayers(List<string> players)
        {
            tbPlayers.Text = "";
            foreach (var item in players)
            {
                tbPlayers.AppendText(item + "\r\n");
            }
        }
    }
}
