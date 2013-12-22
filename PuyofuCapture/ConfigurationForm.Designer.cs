using System;
using System.Windows.Forms;
using log4net;

namespace Cubokta.Puyo
{
    public partial class ConfigurationForm : Form
    {
        private Button OkBtn;
        private CheckBox DebugRectEnabledChk;

        private void InitializeComponent()
        {
            this.OkBtn = new System.Windows.Forms.Button();
            this.DebugRectEnabledChk = new System.Windows.Forms.CheckBox();
            this.captureStepNumTxt = new System.Windows.Forms.NumericUpDown();
            this.captureStepNumLbl = new System.Windows.Forms.Label();
            this.captureOnlyTsumoChk = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.captureStepNumTxt)).BeginInit();
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
            // captureStepNumTxt
            // 
            this.captureStepNumTxt.Location = new System.Drawing.Point(10, 86);
            this.captureStepNumTxt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.captureStepNumTxt.Name = "captureStepNumTxt";
            this.captureStepNumTxt.Size = new System.Drawing.Size(100, 19);
            this.captureStepNumTxt.TabIndex = 2;
            this.captureStepNumTxt.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // captureStepNumLbl
            // 
            this.captureStepNumLbl.AutoSize = true;
            this.captureStepNumLbl.Location = new System.Drawing.Point(10, 67);
            this.captureStepNumLbl.Name = "captureStepNumLbl";
            this.captureStepNumLbl.Size = new System.Drawing.Size(73, 12);
            this.captureStepNumLbl.TabIndex = 3;
            this.captureStepNumLbl.Text = "キャプチャ手数";
            // 
            // captureOnlyTsumoChk
            // 
            this.captureOnlyTsumoChk.AutoSize = true;
            this.captureOnlyTsumoChk.Location = new System.Drawing.Point(12, 34);
            this.captureOnlyTsumoChk.Name = "captureOnlyTsumoChk";
            this.captureOnlyTsumoChk.Size = new System.Drawing.Size(126, 16);
            this.captureOnlyTsumoChk.TabIndex = 4;
            this.captureOnlyTsumoChk.Text = "ツモのみキャプチャする";
            this.captureOnlyTsumoChk.UseVisualStyleBackColor = true;
            // 
            // ConfigurationForm
            // 
            this.ClientSize = new System.Drawing.Size(538, 404);
            this.Controls.Add(this.captureOnlyTsumoChk);
            this.Controls.Add(this.captureStepNumLbl);
            this.Controls.Add(this.captureStepNumTxt);
            this.Controls.Add(this.DebugRectEnabledChk);
            this.Controls.Add(this.OkBtn);
            this.KeyPreview = true;
            this.Name = "ConfigurationForm";
            this.Text = "設定";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ConfigurationForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.captureStepNumTxt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private NumericUpDown captureStepNumTxt;
        private Label captureStepNumLbl;
        private CheckBox captureOnlyTsumoChk;
    }
}
