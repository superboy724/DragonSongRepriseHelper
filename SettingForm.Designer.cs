namespace DragonSongRepriseHelper
{
    partial class SettingForm
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnRunRaid = new System.Windows.Forms.Button();
            this.lbRaidStatus = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbSettingPlayerStatus = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lbVersion = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPostNamazuUrl = new System.Windows.Forms.TextBox();
            this.tbPlayers = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbP2Step4ChangeTowerEnable = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbP2Step4Enable = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbP2Step2MarkDisabled = new System.Windows.Forms.CheckBox();
            this.cbP2Step3Enable = new System.Windows.Forms.CheckBox();
            this.cbP2Step2Enable = new System.Windows.Forms.CheckBox();
            this.cbP2Step1Enable = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lLtoNga = new System.Windows.Forms.LinkLabel();
            this.label11 = new System.Windows.Forms.Label();
            this.cbP3Step1Enable = new System.Windows.Forms.CheckBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.cbP4Step2Enable = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cbP4Step1Enable = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnLogClear = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(918, 357);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnRunRaid);
            this.tabPage1.Controls.Add(this.lbRaidStatus);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.lbSettingPlayerStatus);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.lbVersion);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tbPostNamazuUrl);
            this.tabPage1.Controls.Add(this.tbPlayers);
            this.tabPage1.Controls.Add(this.btnTest);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(910, 331);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基础设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnRunRaid
            // 
            this.btnRunRaid.Location = new System.Drawing.Point(223, 276);
            this.btnRunRaid.Name = "btnRunRaid";
            this.btnRunRaid.Size = new System.Drawing.Size(211, 37);
            this.btnRunRaid.TabIndex = 17;
            this.btnRunRaid.Text = "强制启动插件（副本状态不对的情况下）";
            this.btnRunRaid.UseVisualStyleBackColor = true;
            this.btnRunRaid.Click += new System.EventHandler(this.btnRunRaid_Click);
            // 
            // lbRaidStatus
            // 
            this.lbRaidStatus.AutoSize = true;
            this.lbRaidStatus.Location = new System.Drawing.Point(360, 7);
            this.lbRaidStatus.Name = "lbRaidStatus";
            this.lbRaidStatus.Size = new System.Drawing.Size(41, 12);
            this.lbRaidStatus.TabIndex = 16;
            this.lbRaidStatus.Text = "Status";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(303, 7);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 15;
            this.label12.Text = "副本状态：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(169, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 14;
            this.label10.Text = "设置状态：";
            // 
            // lbSettingPlayerStatus
            // 
            this.lbSettingPlayerStatus.AutoSize = true;
            this.lbSettingPlayerStatus.Location = new System.Drawing.Point(227, 7);
            this.lbSettingPlayerStatus.Name = "lbSettingPlayerStatus";
            this.lbSettingPlayerStatus.Size = new System.Drawing.Size(41, 12);
            this.lbSettingPlayerStatus.TabIndex = 13;
            this.lbSettingPlayerStatus.Text = "Status";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(391, 224);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(509, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "使用以下的宏可读取小队信息，读取完毕后删除服务器名并设置职责（兼容繁华绝神兵科技宏）";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(391, 242);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(500, 21);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "/e DSRH {<1>:<2>:<3>:<4>:<5>:<6>:<7>:<8>}";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(848, 316);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(29, 12);
            this.lbVersion.TabIndex = 10;
            this.lbVersion.Text = "ver:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(391, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(329, 156);
            this.label6.TabIndex = 9;
            this.label6.Text = "玩家列表填写规则如下，每一行一个玩家：\r\n[玩家ID],[职责：MT,ST,H1,H2,D1,D2,D3,D4]\r\n注意：文本框中的顺序必须与游戏里面的小队列表顺" +
    "序相同！\r\n\r\n示例如下：\r\n自由落体,H2\r\n白潘,MT\r\ndaisuke,ST\r\n少打音游,H1\r\n多读书,D2\r\n一碗屎,D1\r\n三倍冰淇淋,D3\r\n暴徒" +
    ",D4";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 246);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "鲶鱼精邮差投递地址：";
            // 
            // tbPostNamazuUrl
            // 
            this.tbPostNamazuUrl.Location = new System.Drawing.Point(137, 242);
            this.tbPostNamazuUrl.Name = "tbPostNamazuUrl";
            this.tbPostNamazuUrl.Size = new System.Drawing.Size(248, 21);
            this.tbPostNamazuUrl.TabIndex = 6;
            this.tbPostNamazuUrl.TextChanged += new System.EventHandler(this.tbPostNamazuUrl_TextChanged);
            // 
            // tbPlayers
            // 
            this.tbPlayers.Location = new System.Drawing.Point(8, 24);
            this.tbPlayers.Multiline = true;
            this.tbPlayers.Name = "tbPlayers";
            this.tbPlayers.Size = new System.Drawing.Size(377, 212);
            this.tbPlayers.TabIndex = 3;
            this.tbPlayers.TextChanged += new System.EventHandler(this.tbPlayers_TextChanged_1);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(6, 276);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(211, 37);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "测试标记";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "玩家列表";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbP2Step4ChangeTowerEnable);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.cbP2Step4Enable);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.cbP2Step2MarkDisabled);
            this.tabPage2.Controls.Add(this.cbP2Step3Enable);
            this.tabPage2.Controls.Add(this.cbP2Step2Enable);
            this.tabPage2.Controls.Add(this.cbP2Step1Enable);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(910, 331);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "P2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cbP2Step4ChangeTowerEnable
            // 
            this.cbP2Step4ChangeTowerEnable.AutoSize = true;
            this.cbP2Step4ChangeTowerEnable.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbP2Step4ChangeTowerEnable.Location = new System.Drawing.Point(153, 113);
            this.cbP2Step4ChangeTowerEnable.Name = "cbP2Step4ChangeTowerEnable";
            this.cbP2Step4ChangeTowerEnable.Size = new System.Drawing.Size(180, 16);
            this.cbP2Step4ChangeTowerEnable.TabIndex = 9;
            this.cbP2Step4ChangeTowerEnable.Text = "开启120换180提醒（未测试）";
            this.cbP2Step4ChangeTowerEnable.UseVisualStyleBackColor = true;
            this.cbP2Step4ChangeTowerEnable.CheckedChanged += new System.EventHandler(this.cbP2Step4ChangeTowerEnable_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 132);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(443, 24);
            this.label8.TabIndex = 8;
            this.label8.Text = "功能说明：如果出现120度塔（假设A点左塔C点右塔）的情况，聊天框中会进行提示\r\n需同时开启二运陨石换组提醒";
            // 
            // cbP2Step4Enable
            // 
            this.cbP2Step4Enable.AutoSize = true;
            this.cbP2Step4Enable.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbP2Step4Enable.Location = new System.Drawing.Point(11, 113);
            this.cbP2Step4Enable.Name = "cbP2Step4Enable";
            this.cbP2Step4Enable.Size = new System.Drawing.Size(136, 16);
            this.cbP2Step4Enable.TabIndex = 7;
            this.cbP2Step4Enable.Text = "二运陨石120度提醒";
            this.cbP2Step4Enable.UseVisualStyleBackColor = true;
            this.cbP2Step4Enable.CheckedChanged += new System.EventHandler(this.cbP2Step4Enable_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(365, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "功能说明：陨石点名前分组如果需要AC换位，文本框会提示换位信息";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(413, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "功能说明：二运骑士冲锋前如果出现同组点名，则给需要换位的D1D2标记大饼";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(275, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "功能说明：一运预站位可通过头顶123标记分配站位";
            // 
            // cbP2Step2MarkDisabled
            // 
            this.cbP2Step2MarkDisabled.AutoSize = true;
            this.cbP2Step2MarkDisabled.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbP2Step2MarkDisabled.Location = new System.Drawing.Point(145, 45);
            this.cbP2Step2MarkDisabled.Name = "cbP2Step2MarkDisabled";
            this.cbP2Step2MarkDisabled.Size = new System.Drawing.Size(192, 16);
            this.cbP2Step2MarkDisabled.TabIndex = 3;
            this.cbP2Step2MarkDisabled.Text = "仅文本框显示，开启后不标大饼";
            this.cbP2Step2MarkDisabled.UseVisualStyleBackColor = true;
            this.cbP2Step2MarkDisabled.CheckedChanged += new System.EventHandler(this.cbP2Step2MarkEnable_CheckedChanged);
            // 
            // cbP2Step3Enable
            // 
            this.cbP2Step3Enable.AutoSize = true;
            this.cbP2Step3Enable.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbP2Step3Enable.Location = new System.Drawing.Point(11, 79);
            this.cbP2Step3Enable.Name = "cbP2Step3Enable";
            this.cbP2Step3Enable.Size = new System.Drawing.Size(193, 16);
            this.cbP2Step3Enable.TabIndex = 2;
            this.cbP2Step3Enable.Text = "二运陨石换组提醒（整组换）";
            this.cbP2Step3Enable.UseVisualStyleBackColor = true;
            this.cbP2Step3Enable.CheckedChanged += new System.EventHandler(this.cbP2Step3Enable_CheckedChanged);
            // 
            // cbP2Step2Enable
            // 
            this.cbP2Step2Enable.AutoSize = true;
            this.cbP2Step2Enable.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbP2Step2Enable.Location = new System.Drawing.Point(11, 45);
            this.cbP2Step2Enable.Name = "cbP2Step2Enable";
            this.cbP2Step2Enable.Size = new System.Drawing.Size(128, 16);
            this.cbP2Step2Enable.TabIndex = 1;
            this.cbP2Step2Enable.Text = "二运分摊换位提醒";
            this.cbP2Step2Enable.UseVisualStyleBackColor = true;
            this.cbP2Step2Enable.CheckedChanged += new System.EventHandler(this.cbP2Step2Enable_CheckedChanged);
            // 
            // cbP2Step1Enable
            // 
            this.cbP2Step1Enable.AutoSize = true;
            this.cbP2Step1Enable.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbP2Step1Enable.Location = new System.Drawing.Point(11, 11);
            this.cbP2Step1Enable.Name = "cbP2Step1Enable";
            this.cbP2Step1Enable.Size = new System.Drawing.Size(141, 16);
            this.cbP2Step1Enable.TabIndex = 0;
            this.cbP2Step1Enable.Text = "一运无标预站位点名";
            this.cbP2Step1Enable.UseVisualStyleBackColor = true;
            this.cbP2Step1Enable.CheckedChanged += new System.EventHandler(this.cbP2Step1Enable_CheckedChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.lLtoNga);
            this.tabPage4.Controls.Add(this.label11);
            this.tabPage4.Controls.Add(this.cbP3Step1Enable);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(910, 331);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "P3";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // lLtoNga
            // 
            this.lLtoNga.AutoSize = true;
            this.lLtoNga.Location = new System.Drawing.Point(48, 42);
            this.lLtoNga.Name = "lLtoNga";
            this.lLtoNga.Size = new System.Drawing.Size(23, 12);
            this.lLtoNga.TabIndex = 7;
            this.lLtoNga.TabStop = true;
            this.lLtoNga.Text = "NGA";
            this.lLtoNga.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lLtoNga_LinkClicked);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 31);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(281, 24);
            this.label11.TabIndex = 6;
            this.label11.Text = "功能说明：麻将阶段通过头顶标记方便判断处理方式\r\n详见：";
            // 
            // cbP3Step1Enable
            // 
            this.cbP3Step1Enable.AutoSize = true;
            this.cbP3Step1Enable.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbP3Step1Enable.Location = new System.Drawing.Point(12, 12);
            this.cbP3Step1Enable.Name = "cbP3Step1Enable";
            this.cbP3Step1Enable.Size = new System.Drawing.Size(102, 16);
            this.cbP3Step1Enable.TabIndex = 5;
            this.cbP3Step1Enable.Text = "麻将分组标记";
            this.cbP3Step1Enable.UseVisualStyleBackColor = true;
            this.cbP3Step1Enable.CheckedChanged += new System.EventHandler(this.cbP3Step1Enable_CheckedChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.label14);
            this.tabPage5.Controls.Add(this.cbP4Step2Enable);
            this.tabPage5.Controls.Add(this.label13);
            this.tabPage5.Controls.Add(this.cbP4Step1Enable);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(910, 331);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "P4";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 92);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(353, 12);
            this.label14.TabIndex = 12;
            this.label14.Text = "功能说明：给第一次幻象冲点名的两人标记12（按职能顺序标记）";
            // 
            // cbP4Step2Enable
            // 
            this.cbP4Step2Enable.AutoSize = true;
            this.cbP4Step2Enable.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbP4Step2Enable.Location = new System.Drawing.Point(18, 73);
            this.cbP4Step2Enable.Name = "cbP4Step2Enable";
            this.cbP4Step2Enable.Size = new System.Drawing.Size(154, 16);
            this.cbP4Step2Enable.TabIndex = 11;
            this.cbP4Step2Enable.Text = "第一次幻象冲点名标记";
            this.cbP4Step2Enable.UseVisualStyleBackColor = true;
            this.cbP4Step2Enable.CheckedChanged += new System.EventHandler(this.cbP4Step2Enable_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 35);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(221, 12);
            this.label13.TabIndex = 9;
            this.label13.Text = "功能说明：给需要红蓝换色的人头上标记";
            // 
            // cbP4Step1Enable
            // 
            this.cbP4Step1Enable.AutoSize = true;
            this.cbP4Step1Enable.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbP4Step1Enable.Location = new System.Drawing.Point(18, 16);
            this.cbP4Step1Enable.Name = "cbP4Step1Enable";
            this.cbP4Step1Enable.Size = new System.Drawing.Size(102, 16);
            this.cbP4Step1Enable.TabIndex = 8;
            this.cbP4Step1Enable.Text = "红蓝换色提醒";
            this.cbP4Step1Enable.UseVisualStyleBackColor = true;
            this.cbP4Step1Enable.CheckedChanged += new System.EventHandler(this.cbP4Step1Enable_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnLogClear);
            this.tabPage3.Controls.Add(this.tbLog);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(910, 331);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Log";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnLogClear
            // 
            this.btnLogClear.Location = new System.Drawing.Point(3, 228);
            this.btnLogClear.Name = "btnLogClear";
            this.btnLogClear.Size = new System.Drawing.Size(730, 31);
            this.btnLogClear.TabIndex = 5;
            this.btnLogClear.Text = "清空";
            this.btnLogClear.UseVisualStyleBackColor = true;
            this.btnLogClear.Click += new System.EventHandler(this.btnLogClear_Click);
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(3, 3);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(730, 219);
            this.tbLog.TabIndex = 4;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "SettingForm";
            this.Size = new System.Drawing.Size(920, 363);
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPostNamazuUrl;
        private System.Windows.Forms.TextBox tbPlayers;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button btnLogClear;
        private System.Windows.Forms.CheckBox cbP2Step1Enable;
        private System.Windows.Forms.CheckBox cbP2Step2Enable;
        private System.Windows.Forms.CheckBox cbP2Step3Enable;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbP2Step2MarkDisabled;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbP2Step4ChangeTowerEnable;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbP2Step4Enable;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbSettingPlayerStatus;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.LinkLabel lLtoNga;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbP3Step1Enable;
        private System.Windows.Forms.Label lbRaidStatus;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnRunRaid;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox cbP4Step2Enable;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox cbP4Step1Enable;
    }
}
