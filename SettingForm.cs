using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

namespace DragonSongRepriseHelper
{
    public partial class SettingForm : UserControl
    {
        SettingContainer settingContainer;
        Action testFunction;
        Action testFunction2;
        bool isDestory = false;
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
            if (settingContainer.PlayerSetting.IsSettingOk())
            {
                this.lbSettingPlayerStatus.Text = "小队配置成功";
                this.lbSettingPlayerStatus.ForeColor = Color.Green;
            }
            else
            {
                this.lbSettingPlayerStatus.Text = "等待配置小队";
                this.lbSettingPlayerStatus.ForeColor = Color.Red;
            }
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

            cbP3Step1Enable.Checked = settingContainer.FunctionSetting.P3Step1Enable;
            cbP3Step2Enable.Checked = settingContainer.FunctionSetting.P3Step2Enable;
            cbP3Step2EndEnable.Checked = settingContainer.FunctionSetting.P3Step2EndEnable;

            cbP4Step1Enable.Checked = settingContainer.FunctionSetting.P4Step1Enable;
            cbP4Step2Enable.Checked = settingContainer.FunctionSetting.P4Step2Enable;

            cbP5Step1Enable.Checked = settingContainer.FunctionSetting.P5Step1Enable;

            cbP6Step2Enable.Checked = settingContainer.FunctionSetting.P6Step2Enable;

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

            if (settingContainer.PlayerSetting.IsSettingOk())
            {
                this.lbSettingPlayerStatus.Text = "小队配置成功";
                this.lbSettingPlayerStatus.ForeColor = Color.Green;
            }
            else
            {
                this.lbSettingPlayerStatus.Text = "等待配置小队";
                this.lbSettingPlayerStatus.ForeColor = Color.Red;
            }

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (settingContainer.IsRaidMode)
                    {
                        if (!this.IsHandleCreated)
                        {
                            return;
                        }
                        this.lbRaidStatus.BeginInvoke(new Action(() =>
                        {
                            this.lbRaidStatus.Text = "副本中";
                            this.lbRaidStatus.ForeColor = Color.Green;
                        }));

                    }
                    else
                    {
                        if (!this.IsHandleCreated)
                        {
                            return;
                        }
                        this.lbRaidStatus.BeginInvoke(new Action(() =>
                        {
                            this.lbRaidStatus.Text = "未在副本中";
                            this.lbRaidStatus.ForeColor = Color.Red;
                        }));
                    }
                }
            }).Start();

            this.lbRaidStatus.Text = "未在副本中";
            this.lbRaidStatus.ForeColor = Color.Red;

            Assembly a = Assembly.GetAssembly(typeof(SettingForm));
            AssemblyName name = a.GetName();
            lbVersion.Text = "ver:" + name.Version;
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
            this.settingContainer.FunctionSetting.P2Step3Enable = cbP2Step3Enable.Checked;
        }

        private void cbP2Step2MarkEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.FunctionSetting.P2Step2MarkDisabled = cbP2Step2MarkDisabled.Checked;
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
            this.settingContainer.FunctionSetting.P2Step4Enable = cbP2Step4Enable.Checked;
        }

        private void cbP2Step4ChangeTowerEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.FunctionSetting.P2Step4ChangeTowerEnable = cbP2Step4ChangeTowerEnable.Checked;
        }

        public void SetPlayers(List<string> players)
        {
            this.settingContainer.PlayerSetting.PlayerSettingClear();
            tbPlayers.Text = "";
            foreach (var item in players)
            {
                tbPlayers.AppendText(item + "\r\n");
            }
        }

        private void lLtoNga_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://bbs.nga.cn/read.php?tid=32379004&page=e");
        }

        private void cbP3Step1Enable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.FunctionSetting.P3Step1Enable = cbP3Step1Enable.Checked;
        }

        private void btnRunRaid_Click(object sender, EventArgs e)
        {
            this.settingContainer.IsRaidMode = true;
            this.settingContainer.ForceRun = true;
        }

        private void cbP4Step1Enable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.FunctionSetting.P4Step1Enable = cbP4Step1Enable.Checked;
        }

        private void cbP4Step2Enable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.FunctionSetting.P4Step2Enable = cbP4Step2Enable.Checked;
        }

        private void btnPlayDbgLog_Click(object sender, EventArgs e)
        {
            testFunction2();
        }

        private void cbP3Step2Enable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.FunctionSetting.P3Step2Enable = cbP3Step2Enable.Checked;
        }

        private void cbP3Step2EndEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.FunctionSetting.P3Step2EndEnable = cbP3Step2EndEnable.Checked;
        }

        private void cbP5Step1Enable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.FunctionSetting.P5Step1Enable = cbP5Step1Enable.Checked;
        }

        private void cbP6Step2Enable_CheckedChanged(object sender, EventArgs e)
        {
            this.settingContainer.FunctionSetting.P6Step2Enable = cbP6Step2Enable.Checked;
        }
    }
}
