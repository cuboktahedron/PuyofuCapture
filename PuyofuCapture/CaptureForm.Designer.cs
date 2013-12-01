using System;
using System.Windows.Forms;
using System.Drawing;

namespace Cubokta.Puyo
{
    public partial class CaptureForm : Form
    {
        /// <summary>
        /// デザイナが生成したコード
        /// </summary>
        private void InitializeComponent()
        {
            this.screenImg = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.screenImg)).BeginInit();
            this.SuspendLayout();
            // 
            // screenImg
            // 
            this.screenImg.Location = new System.Drawing.Point(0, 0);
            this.screenImg.Name = "screenImg";
            this.screenImg.Size = new System.Drawing.Size(37, 37);
            this.screenImg.TabIndex = 0;
            this.screenImg.TabStop = false;
            this.screenImg.Paint += new System.Windows.Forms.PaintEventHandler(this.screenPict_Paint);
            this.screenImg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screenImg_MouseDown);
            this.screenImg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.screenImg_MouseMove);
            // 
            // CaptureForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 356);
            this.Controls.Add(this.screenImg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CaptureForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptureForm_FormClosed);
            this.Load += new System.EventHandler(this.CaptureForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CaptureForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.screenImg)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
