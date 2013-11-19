using System;
using System.Windows.Forms;
using log4net;

namespace Cubokta.Puyo
{
    public partial class ConfigurationForm : Form
    {
        private Button OkBtn;
        private CheckBox DebugRectEnabledChk;
        private static readonly ILog LOGGER =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private PuyofuConfiguration config;

        public ConfigurationForm(PuyofuConfiguration config)
        {
            this.config = config;

            InitializeComponent();
            Init();
        }

        private void Init()
        {
            DebugRectEnabledChk.Checked = config.DebugRectEnabled;
        }

        private void InitializeComponent()
        {
            this.OkBtn = new System.Windows.Forms.Button();
            this.DebugRectEnabledChk = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // OkBtn
            // 
            this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OkBtn.Location = new System.Drawing.Point(451, 369);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 0;
            this.OkBtn.Text = "確定";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // DebugRectEnabledChk
            // 
            this.DebugRectEnabledChk.AutoSize = true;
            this.DebugRectEnabledChk.Location = new System.Drawing.Point(12, 12);
            this.DebugRectEnabledChk.Name = "DebugRectEnabledChk";
            this.DebugRectEnabledChk.Size = new System.Drawing.Size(100, 16);
            this.DebugRectEnabledChk.TabIndex = 1;
            this.DebugRectEnabledChk.Text = "色枠を表示する";
            this.DebugRectEnabledChk.UseVisualStyleBackColor = true;
            // 
            // ConfigurationForm
            // 
            this.ClientSize = new System.Drawing.Size(538, 404);
            this.Controls.Add(this.DebugRectEnabledChk);
            this.Controls.Add(this.OkBtn);
            this.Name = "ConfigurationForm";
            this.Text = "設定";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            config.DebugRectEnabled = DebugRectEnabledChk.Checked;
            config.Save();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
