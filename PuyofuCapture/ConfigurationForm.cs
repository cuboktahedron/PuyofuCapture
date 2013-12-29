/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/blob/master/license/LICENSE-MIT.txt
 */
using System;
using System.Windows.Forms;
using log4net;
using System.IO;

namespace Cubokta.Puyo
{
    /// <summary>
    /// 設定フォーム
    /// </summary>
    public partial class ConfigurationForm : Form
    {
        /// <summary>設定情報</summary>
        private PuyofuConfiguration config;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config">設定情報</param>
        public ConfigurationForm(PuyofuConfiguration config)
        {
            this.config = config;

            InitializeComponent();
            Init();
        }

        /// <summary>
        /// 初期化する
        /// </summary>
        private void Init()
        {
            DebugRectEnabledChk.Checked = config.DebugRectEnabled;
            captureStepNumTxt.Value = config.CaptureStepNum;
            captureOnlyTsumoChk.Checked = config.CaptureOnlyTsumo;
            saveFileTxt.Text = config.SaveFile;
            recordTemplateTxt.Text = config.RecordTemplate;
        }

        /// <summary>
        /// 確定ボタンを押下した
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void OkBtn_Click(object sender, EventArgs e)
        {
            config.DebugRectEnabled = DebugRectEnabledChk.Checked;
            config.CaptureStepNum = (int)captureStepNumTxt.Value;
            config.CaptureOnlyTsumo = captureOnlyTsumoChk.Checked;
            config.SaveFile = saveFileTxt.Text;
            config.RecordTemplate = recordTemplateTxt.Text;
            config.Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// キーを入力した
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void ConfigurationForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                Close();
            }
        }
    }
}
