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
            this.recordTemplateTxt = new System.Windows.Forms.TextBox();
            this.templateLbl = new System.Windows.Forms.Label();
            this.saveFileLbl = new System.Windows.Forms.Label();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.saveFileTxt = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.captureStepNumTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // OkBtn
            // 
            this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OkBtn.Location = new System.Drawing.Point(451, 369);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 5;
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
            this.DebugRectEnabledChk.TabIndex = 0;
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
            this.captureStepNumLbl.TabIndex = 6;
            this.captureStepNumLbl.Text = "キャプチャ手数";
            // 
            // captureOnlyTsumoChk
            // 
            this.captureOnlyTsumoChk.AutoSize = true;
            this.captureOnlyTsumoChk.Location = new System.Drawing.Point(12, 34);
            this.captureOnlyTsumoChk.Name = "captureOnlyTsumoChk";
            this.captureOnlyTsumoChk.Size = new System.Drawing.Size(126, 16);
            this.captureOnlyTsumoChk.TabIndex = 1;
            this.captureOnlyTsumoChk.Text = "ツモのみキャプチャする";
            this.captureOnlyTsumoChk.UseVisualStyleBackColor = true;
            // 
            // recordTemplateTxt
            // 
            this.recordTemplateTxt.Location = new System.Drawing.Point(12, 197);
            this.recordTemplateTxt.Multiline = true;
            this.recordTemplateTxt.Name = "recordTemplateTxt";
            this.recordTemplateTxt.Size = new System.Drawing.Size(318, 165);
            this.recordTemplateTxt.TabIndex = 4;
            this.recordTemplateTxt.Text = "  {\r\n    date: \'#date\',\r\n    id: \'#id\',\r\n    record: \'#record\',\r\n    tags: [#tags" +
    "],\r\n  },\r\n";
            // 
            // templateLbl
            // 
            this.templateLbl.AutoSize = true;
            this.templateLbl.Location = new System.Drawing.Point(12, 173);
            this.templateLbl.Name = "templateLbl";
            this.templateLbl.Size = new System.Drawing.Size(59, 12);
            this.templateLbl.TabIndex = 8;
            this.templateLbl.Text = "テンプレート";
            // 
            // saveFileLbl
            // 
            this.saveFileLbl.AutoSize = true;
            this.saveFileLbl.Location = new System.Drawing.Point(10, 117);
            this.saveFileLbl.Name = "saveFileLbl";
            this.saveFileLbl.Size = new System.Drawing.Size(63, 12);
            this.saveFileLbl.TabIndex = 7;
            this.saveFileLbl.Text = "保存ファイル";
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // saveFileTxt
            // 
            this.saveFileTxt.Location = new System.Drawing.Point(10, 132);
            this.saveFileTxt.Name = "saveFileTxt";
            this.saveFileTxt.Size = new System.Drawing.Size(320, 19);
            this.saveFileTxt.TabIndex = 3;
            // 
            // ConfigurationForm
            // 
            this.ClientSize = new System.Drawing.Size(538, 404);
            this.Controls.Add(this.saveFileTxt);
            this.Controls.Add(this.saveFileLbl);
            this.Controls.Add(this.templateLbl);
            this.Controls.Add(this.recordTemplateTxt);
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
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private NumericUpDown captureStepNumTxt;
        private Label captureStepNumLbl;
        private CheckBox captureOnlyTsumoChk;
        private TextBox recordTemplateTxt;
        private Label templateLbl;
        private Label saveFileLbl;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private TextBox saveFileTxt;
    }
}
