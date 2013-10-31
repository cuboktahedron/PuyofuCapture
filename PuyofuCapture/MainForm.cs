using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using Cubokta;
using System.Collections.Generic;
using Cubokta.Common;
using Cubokta.Puyo.Common;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using System.Configuration;

namespace Cubokta.Puyo
{
    public partial class MainForm : Form
    {
        // エントリ・ポイント
        [STAThread]
        static void Main()
        {
            System.IO.File.Delete("debug.log");
            Application.Run(new MainForm());
        }

        private PictureBox sampleAkaImg;
        private PictureBox sampleMidoriImg;
        private PictureBox sampleAoImg;
        private PictureBox sampleKiImg;
        private PictureBox sampleMurasakiImg;
        private TrackBar similarityValueBar;
        private Label label1;
        private Label similarityValueLbl;
        private PictureBox sampleNoneImg;

        private PuyoTypeDetector detector = new PuyoTypeDetector();
        public MainForm()
        {
            InitializeComponent();
            statusLabel.Text = "";

            updateSamples();
            detector.SimilarityThreshold = 30000;
            similarityValueLbl.Text = similarityValueBar.Value.ToString();

            // 前回のキャプチャ範囲があればそれを使用しキャプチャを開始する
            String cRect = ConfigurationManager.AppSettings["captureRect"];
            String nRect = ConfigurationManager.AppSettings["nextRect"];
            if (cRect != "-1,-1,-1,-1" && nRect != "-1,-1,-1,-1")
            {
                string[] cRectValues = cRect.Split(',');
                string[] nRectValues = nRect.Split(',');
                beginCapturing(
                    new Rectangle(
                        int.Parse(cRectValues[0]), int.Parse(cRectValues[1]),
                        int.Parse(cRectValues[2]), int.Parse(cRectValues[3])),
                    new Rectangle(
                        int.Parse(nRectValues[0]), int.Parse(nRectValues[1]),
                        int.Parse(nRectValues[2]), int.Parse(nRectValues[3])));
            }
        }

        private IDictionary<PuyoType, PictureBox> sampleImgs;

        private void updateSamples()
        {
            sampleImgs = new Dictionary<PuyoType, PictureBox>();
            sampleImgs[PuyoType.NONE] = sampleNoneImg;
            sampleImgs[PuyoType.AKA] = sampleAkaImg;
            sampleImgs[PuyoType.MIDORI] = sampleMidoriImg;
            sampleImgs[PuyoType.AO] = sampleAoImg;
            sampleImgs[PuyoType.KI] = sampleKiImg;
            sampleImgs[PuyoType.MURASAKI] = sampleMurasakiImg;

            updateSample(PuyoType.NONE);
            updateSample(PuyoType.AKA);
            updateSample(PuyoType.MIDORI);
            updateSample(PuyoType.AO);
            updateSample(PuyoType.KI);
            updateSample(PuyoType.MURASAKI);
        }

        private void updateSample(PuyoType puyoType)
        {
            string filePath = Path.Combine("img", puyoType.ToString() + ".bmp");
            if (!File.Exists(filePath))
            {
                return;
            }

            Bitmap sampleBmp = (Bitmap)Bitmap.FromFile(filePath);
            RapidBitmapAccessor ba = new RapidBitmapAccessor(sampleBmp);
            ba.BeginAccess();
            detector.UpdateSample(puyoType, ba);
            ba.EndAccess();

            sampleImgs[puyoType].Image = sampleBmp;
        }

        private Button spoitBtn;
        private Button captureBtn;
        private PictureBox fieldImg;
        private System.Windows.Forms.Timer captureTimer;
        private System.ComponentModel.IContainer components;
        private Label statusLabel;
        private Label colorInfoLbl;
        private RadioButton FieldRadio1P;
        private RadioButton FieldRadio2P;
        private PictureBox nextImg;
        private Label playDateLbl;
        private Label playerNameLbl;
        private Label stepIdLbl;
        private DateTimePicker playDate;
        private TextBox playerNameTxt;
        private Button startBtn;
        private Label recordLbl;
        private TextBox recordTxt;
        private TextBox stepDataTxt;
        private NumericUpDown stepIdTxt;
        private Button stopBtn;
        private Splitter splitter1;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.spoitBtn = new System.Windows.Forms.Button();
            this.captureBtn = new System.Windows.Forms.Button();
            this.fieldImg = new System.Windows.Forms.PictureBox();
            this.captureTimer = new System.Windows.Forms.Timer(this.components);
            this.statusLabel = new System.Windows.Forms.Label();
            this.colorInfoLbl = new System.Windows.Forms.Label();
            this.FieldRadio1P = new System.Windows.Forms.RadioButton();
            this.FieldRadio2P = new System.Windows.Forms.RadioButton();
            this.nextImg = new System.Windows.Forms.PictureBox();
            this.playDateLbl = new System.Windows.Forms.Label();
            this.playerNameLbl = new System.Windows.Forms.Label();
            this.stepIdLbl = new System.Windows.Forms.Label();
            this.playDate = new System.Windows.Forms.DateTimePicker();
            this.playerNameTxt = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.startBtn = new System.Windows.Forms.Button();
            this.recordLbl = new System.Windows.Forms.Label();
            this.recordTxt = new System.Windows.Forms.TextBox();
            this.stepDataTxt = new System.Windows.Forms.TextBox();
            this.stepIdTxt = new System.Windows.Forms.NumericUpDown();
            this.stopBtn = new System.Windows.Forms.Button();
            this.sampleAkaImg = new System.Windows.Forms.PictureBox();
            this.sampleMidoriImg = new System.Windows.Forms.PictureBox();
            this.sampleAoImg = new System.Windows.Forms.PictureBox();
            this.sampleKiImg = new System.Windows.Forms.PictureBox();
            this.sampleMurasakiImg = new System.Windows.Forms.PictureBox();
            this.similarityValueBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.similarityValueLbl = new System.Windows.Forms.Label();
            this.sampleNoneImg = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.fieldImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepIdTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleAkaImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleMidoriImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleAoImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleKiImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleMurasakiImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.similarityValueBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleNoneImg)).BeginInit();
            this.SuspendLayout();
            // 
            // spoitBtn
            // 
            this.spoitBtn.Enabled = false;
            this.spoitBtn.Location = new System.Drawing.Point(24, 32);
            this.spoitBtn.Name = "spoitBtn";
            this.spoitBtn.Size = new System.Drawing.Size(175, 63);
            this.spoitBtn.TabIndex = 1;
            this.spoitBtn.Text = "スポイト";
            this.spoitBtn.UseVisualStyleBackColor = true;
            this.spoitBtn.Click += new System.EventHandler(this.spoitBtn_Click);
            // 
            // captureBtn
            // 
            this.captureBtn.Location = new System.Drawing.Point(24, 126);
            this.captureBtn.Name = "captureBtn";
            this.captureBtn.Size = new System.Drawing.Size(175, 70);
            this.captureBtn.TabIndex = 2;
            this.captureBtn.Text = "キャプチャスクリーン";
            this.captureBtn.UseVisualStyleBackColor = true;
            this.captureBtn.Click += new System.EventHandler(this.captureBtn_Click);
            // 
            // fieldImg
            // 
            this.fieldImg.Location = new System.Drawing.Point(224, 32);
            this.fieldImg.Name = "fieldImg";
            this.fieldImg.Size = new System.Drawing.Size(192, 384);
            this.fieldImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fieldImg.TabIndex = 3;
            this.fieldImg.TabStop = false;
            this.fieldImg.Paint += new System.Windows.Forms.PaintEventHandler(this.fieldImg_Paint);
            this.fieldImg.MouseClick += new System.Windows.Forms.MouseEventHandler(this.fieldImg_MouseClick);
            this.fieldImg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fieldImg_MouseMove);
            // 
            // captureTimer
            // 
            this.captureTimer.Interval = 50;
            this.captureTimer.Tick += new System.EventHandler(this.captureTimer_Tick);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(22, 445);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(41, 12);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "説明文";
            // 
            // colorInfoLbl
            // 
            this.colorInfoLbl.AutoSize = true;
            this.colorInfoLbl.Location = new System.Drawing.Point(523, 32);
            this.colorInfoLbl.Name = "colorInfoLbl";
            this.colorInfoLbl.Size = new System.Drawing.Size(67, 12);
            this.colorInfoLbl.TabIndex = 5;
            this.colorInfoLbl.Text = "サンプル画像";
            // 
            // FieldRadio1P
            // 
            this.FieldRadio1P.AutoSize = true;
            this.FieldRadio1P.Checked = true;
            this.FieldRadio1P.Location = new System.Drawing.Point(31, 208);
            this.FieldRadio1P.Name = "FieldRadio1P";
            this.FieldRadio1P.Size = new System.Drawing.Size(36, 16);
            this.FieldRadio1P.TabIndex = 6;
            this.FieldRadio1P.TabStop = true;
            this.FieldRadio1P.Text = "1P";
            this.FieldRadio1P.UseVisualStyleBackColor = true;
            // 
            // FieldRadio2P
            // 
            this.FieldRadio2P.AccessibleName = "";
            this.FieldRadio2P.AutoSize = true;
            this.FieldRadio2P.Location = new System.Drawing.Point(73, 208);
            this.FieldRadio2P.Name = "FieldRadio2P";
            this.FieldRadio2P.Size = new System.Drawing.Size(36, 16);
            this.FieldRadio2P.TabIndex = 6;
            this.FieldRadio2P.Text = "2P";
            this.FieldRadio2P.UseVisualStyleBackColor = true;
            // 
            // nextImg
            // 
            this.nextImg.Location = new System.Drawing.Point(438, 45);
            this.nextImg.Name = "nextImg";
            this.nextImg.Size = new System.Drawing.Size(32, 64);
            this.nextImg.TabIndex = 7;
            this.nextImg.TabStop = false;
            this.nextImg.Paint += new System.Windows.Forms.PaintEventHandler(this.nextImg_Paint);
            // 
            // playDateLbl
            // 
            this.playDateLbl.AutoSize = true;
            this.playDateLbl.Location = new System.Drawing.Point(451, 234);
            this.playDateLbl.Name = "playDateLbl";
            this.playDateLbl.Size = new System.Drawing.Size(29, 12);
            this.playDateLbl.TabIndex = 8;
            this.playDateLbl.Text = "日付";
            // 
            // playerNameLbl
            // 
            this.playerNameLbl.AutoSize = true;
            this.playerNameLbl.Location = new System.Drawing.Point(451, 261);
            this.playerNameLbl.Name = "playerNameLbl";
            this.playerNameLbl.Size = new System.Drawing.Size(54, 12);
            this.playerNameLbl.TabIndex = 9;
            this.playerNameLbl.Text = "プレイヤ名";
            // 
            // stepIdLbl
            // 
            this.stepIdLbl.AutoSize = true;
            this.stepIdLbl.Location = new System.Drawing.Point(451, 200);
            this.stepIdLbl.Name = "stepIdLbl";
            this.stepIdLbl.Size = new System.Drawing.Size(14, 12);
            this.stepIdLbl.TabIndex = 10;
            this.stepIdLbl.Text = "id";
            // 
            // playDate
            // 
            this.playDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.playDate.Location = new System.Drawing.Point(525, 229);
            this.playDate.Name = "playDate";
            this.playDate.Size = new System.Drawing.Size(200, 19);
            this.playDate.TabIndex = 11;
            // 
            // playerNameTxt
            // 
            this.playerNameTxt.Location = new System.Drawing.Point(525, 261);
            this.playerNameTxt.Name = "playerNameTxt";
            this.playerNameTxt.Size = new System.Drawing.Size(200, 19);
            this.playerNameTxt.TabIndex = 13;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 482);
            this.splitter1.TabIndex = 14;
            this.splitter1.TabStop = false;
            // 
            // startBtn
            // 
            this.startBtn.Enabled = false;
            this.startBtn.Location = new System.Drawing.Point(24, 268);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(175, 70);
            this.startBtn.TabIndex = 15;
            this.startBtn.Text = "開始";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // recordLbl
            // 
            this.recordLbl.AutoSize = true;
            this.recordLbl.Location = new System.Drawing.Point(451, 294);
            this.recordLbl.Name = "recordLbl";
            this.recordLbl.Size = new System.Drawing.Size(41, 12);
            this.recordLbl.TabIndex = 16;
            this.recordLbl.Text = "レコード";
            // 
            // recordTxt
            // 
            this.recordTxt.Location = new System.Drawing.Point(525, 294);
            this.recordTxt.Name = "recordTxt";
            this.recordTxt.ReadOnly = true;
            this.recordTxt.Size = new System.Drawing.Size(200, 19);
            this.recordTxt.TabIndex = 17;
            // 
            // stepDataTxt
            // 
            this.stepDataTxt.Location = new System.Drawing.Point(459, 324);
            this.stepDataTxt.Multiline = true;
            this.stepDataTxt.Name = "stepDataTxt";
            this.stepDataTxt.Size = new System.Drawing.Size(266, 92);
            this.stepDataTxt.TabIndex = 18;
            // 
            // stepIdTxt
            // 
            this.stepIdTxt.Location = new System.Drawing.Point(525, 198);
            this.stepIdTxt.Name = "stepIdTxt";
            this.stepIdTxt.Size = new System.Drawing.Size(120, 19);
            this.stepIdTxt.TabIndex = 19;
            // 
            // stopBtn
            // 
            this.stopBtn.Enabled = false;
            this.stopBtn.Location = new System.Drawing.Point(24, 346);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(175, 70);
            this.stopBtn.TabIndex = 20;
            this.stopBtn.Text = "停止";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // sampleAkaImg
            // 
            this.sampleAkaImg.Location = new System.Drawing.Point(549, 47);
            this.sampleAkaImg.Name = "sampleAkaImg";
            this.sampleAkaImg.Size = new System.Drawing.Size(32, 32);
            this.sampleAkaImg.TabIndex = 21;
            this.sampleAkaImg.TabStop = false;
            // 
            // sampleMidoriImg
            // 
            this.sampleMidoriImg.Location = new System.Drawing.Point(580, 47);
            this.sampleMidoriImg.Name = "sampleMidoriImg";
            this.sampleMidoriImg.Size = new System.Drawing.Size(32, 32);
            this.sampleMidoriImg.TabIndex = 22;
            this.sampleMidoriImg.TabStop = false;
            // 
            // sampleAoImg
            // 
            this.sampleAoImg.Location = new System.Drawing.Point(611, 47);
            this.sampleAoImg.Name = "sampleAoImg";
            this.sampleAoImg.Size = new System.Drawing.Size(32, 32);
            this.sampleAoImg.TabIndex = 23;
            this.sampleAoImg.TabStop = false;
            // 
            // sampleKiImg
            // 
            this.sampleKiImg.Location = new System.Drawing.Point(642, 47);
            this.sampleKiImg.Name = "sampleKiImg";
            this.sampleKiImg.Size = new System.Drawing.Size(32, 32);
            this.sampleKiImg.TabIndex = 24;
            this.sampleKiImg.TabStop = false;
            // 
            // sampleMurasakiImg
            // 
            this.sampleMurasakiImg.Location = new System.Drawing.Point(673, 47);
            this.sampleMurasakiImg.Name = "sampleMurasakiImg";
            this.sampleMurasakiImg.Size = new System.Drawing.Size(32, 32);
            this.sampleMurasakiImg.TabIndex = 25;
            this.sampleMurasakiImg.TabStop = false;
            // 
            // similarityValueBar
            // 
            this.similarityValueBar.LargeChange = 10000;
            this.similarityValueBar.Location = new System.Drawing.Point(550, 113);
            this.similarityValueBar.Maximum = 95000;
            this.similarityValueBar.Minimum = 5000;
            this.similarityValueBar.Name = "similarityValueBar";
            this.similarityValueBar.Size = new System.Drawing.Size(147, 45);
            this.similarityValueBar.SmallChange = 1000;
            this.similarityValueBar.TabIndex = 26;
            this.similarityValueBar.TickFrequency = 10000;
            this.similarityValueBar.Value = 30000;
            this.similarityValueBar.Scroll += new System.EventHandler(this.similarityValueBar_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(515, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 28;
            this.label1.Text = "閾値";
            // 
            // similarityValueLbl
            // 
            this.similarityValueLbl.AutoSize = true;
            this.similarityValueLbl.Location = new System.Drawing.Point(515, 126);
            this.similarityValueLbl.Name = "similarityValueLbl";
            this.similarityValueLbl.Size = new System.Drawing.Size(35, 12);
            this.similarityValueLbl.TabIndex = 29;
            this.similarityValueLbl.Text = "30000";
            // 
            // sampleNoneImg
            // 
            this.sampleNoneImg.Location = new System.Drawing.Point(518, 47);
            this.sampleNoneImg.Name = "sampleNoneImg";
            this.sampleNoneImg.Size = new System.Drawing.Size(32, 32);
            this.sampleNoneImg.TabIndex = 30;
            this.sampleNoneImg.TabStop = false;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(804, 482);
            this.Controls.Add(this.sampleNoneImg);
            this.Controls.Add(this.similarityValueLbl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.similarityValueBar);
            this.Controls.Add(this.sampleMurasakiImg);
            this.Controls.Add(this.sampleKiImg);
            this.Controls.Add(this.sampleAoImg);
            this.Controls.Add(this.sampleMidoriImg);
            this.Controls.Add(this.sampleAkaImg);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.stepIdTxt);
            this.Controls.Add(this.stepDataTxt);
            this.Controls.Add(this.recordTxt);
            this.Controls.Add(this.recordLbl);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.playerNameTxt);
            this.Controls.Add(this.playDate);
            this.Controls.Add(this.stepIdLbl);
            this.Controls.Add(this.playerNameLbl);
            this.Controls.Add(this.playDateLbl);
            this.Controls.Add(this.nextImg);
            this.Controls.Add(this.FieldRadio2P);
            this.Controls.Add(this.FieldRadio1P);
            this.Controls.Add(this.colorInfoLbl);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.fieldImg);
            this.Controls.Add(this.captureBtn);
            this.Controls.Add(this.spoitBtn);
            this.Name = "MainForm";
            this.Text = "PuyofuCapture";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.fieldImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepIdTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleAkaImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleMidoriImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleAoImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleKiImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleMurasakiImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.similarityValueBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleNoneImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        int pixelingTargetIndex;
        bool isPixeling = false;
        private void spoitBtn_Click(object sender, EventArgs e)
        {
            isPixeling = true;
            pixelingTargetIndex = (int)PuyoType.NONE;
            statusLabel.Text = (PuyoType)pixelingTargetIndex + "のサンプルピクセルをクリックしてください。右クリックでスキップします。";
        }

        private void captureBtn_Click(object sender, EventArgs e)
        {
            captureTimer.Stop();
            IsCapturing = false;

            int fieldNo = FieldRadio1P.Checked ? 0 : 1;
            Form captureForm = new CaptureForm(fieldNo);
            captureForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptureForm_FormClosed);
            captureForm.Show();
        }

        private Rectangle captureRect;
        private Bitmap captureBmp;
        private Rectangle nextRect;
        private Bitmap nextBmp;

        private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CaptureForm f = sender as CaptureForm;
            if (f == null)
            {
                return;
            }

            Rectangle cRect = f.GetCaptureRect();
            Rectangle nRect = f.GetNextRect();

            beginCapturing(cRect, nRect);
        }

        private void beginCapturing(Rectangle cRect, Rectangle nRect)
        {
            captureRect = cRect;
            nextRect = nRect;

            IsCapturing = true;
            spoitBtn.Enabled = true;
            startBtn.Enabled = true;
            captureTimer.Start();

            if (captureBmp != null)
            {
                captureBmp.Dispose();
            }
            captureBmp = new Bitmap(captureRect.Width, captureRect.Height);

            if (nextBmp != null)
            {
                nextBmp.Dispose();
            }
            captureBmp = new Bitmap(captureRect.Width, captureRect.Height);
            nextBmp = new Bitmap(nextRect.Width, nextRect.Height);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["captureRect"].Value =
                    captureRect.X + "," + captureRect.Y + "," + captureRect.Width + "," + captureRect.Height;
            config.AppSettings.Settings["nextRect"].Value =
                    nextRect.X + "," + nextRect.Y + "," + nextRect.Width + "," + nextRect.Height;
            config.Save();

        }

        private void captureTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private bool IsCapturing { get; set; }
        private CaptureField prevField = new CaptureField();
        private CaptureField curField = new CaptureField();

        private void fieldImg_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (!IsCapturing)
                {
                    return;
                }

                Graphics fieldImgG = e.Graphics;
                using (Graphics captureBmpG = Graphics.FromImage(captureBmp))
                using (Bitmap forAnalyzeBmp = new Bitmap(fieldImg.Width, fieldImg.Height))
                using (Graphics forAnalyzeG = Graphics.FromImage(forAnalyzeBmp))
                {
                    // フィールドのキャプチャ範囲を取り込む
                    captureBmpG.CopyFromScreen(new Point(captureRect.Left, captureRect.Top), new Point(0, 0), captureBmp.Size);

                    // 取り込んだ画像を画面に出力 // TODO: コメント反対？
                    Rectangle dest = new Rectangle(0, 0, 192, 384);
                    Rectangle src = new Rectangle(0, 0, captureRect.Width, captureRect.Height);
                    forAnalyzeG.DrawImage(captureBmp, dest, src, GraphicsUnit.Pixel);

                    // 解析用のBMPにも画面と同じ内容を出力
                    fieldImgG.DrawImage(forAnalyzeBmp, dest, dest, GraphicsUnit.Pixel);

                    if (!isPixeling)
                    {
                        // フィールドの状態を解析し、結果を描画
                        curField = a(forAnalyzeBmp);
                        DrawDebugRect(fieldImgG, curField);
                    }
                    else
                    {
                        // サンプル選択枠を描画
                        int x = pointOnFieldImg.X - (pointOnFieldImg.X % CaptureField.UNIT);
                        int y = pointOnFieldImg.Y - (pointOnFieldImg.Y % CaptureField.UNIT);

                        Rectangle pixelingCellRect = new Rectangle(x, y, CaptureField.UNIT, CaptureField.UNIT);
                        fieldImgG.DrawRectangle(new Pen(Color.Red, 2), pixelingCellRect);
                    }
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                Debug.Flush();
                throw exp;
            }
        }

        private void DrawDebugRect(Graphics g, CaptureField field)
        {
            using (Pen redPen = new Pen(Color.Red, 2))
            using (Pen greenPen = new Pen(Color.Green, 2))
            using (Pen bluePen = new Pen(Color.LightBlue, 2))
            using (Pen yellowPen = new Pen(Color.Yellow, 2))
            using (Pen purplePen = new Pen(Color.Purple, 2))
            {
                for (int y = 0; y < CaptureField.Y_MAX; y++)
                {
                    for (int x = 0; x < CaptureField.X_MAX; x++)
                    {
                        PuyoType type = field.GetPuyoType(x, y);
                        if (type == PuyoType.NONE)
                        {
                            continue;
                        }

                        Rectangle rect = field.GetRect(x, y);
                        Pen pen;
                        switch (type)
                        {
                            case PuyoType.AKA:
                                pen = redPen;
                                break;
                            case PuyoType.MIDORI:
                                pen = greenPen;
                                break;
                            case PuyoType.AO:
                                pen = bluePen;
                                break;
                            case PuyoType.KI:
                                pen = yellowPen;
                                break;
                            case PuyoType.MURASAKI:
                                pen = purplePen;
                                break;
                            default:
                                continue;
                        }

                        rect.X++;
                        rect.Width -= 2;
                        rect.Y++;
                        rect.Height -= 2;
                        g.DrawRectangle(pen, rect);
                    }
                }
            }
        }

        private CaptureField a(Bitmap bmp)
        {
            CaptureField field = new CaptureField();
            RapidBitmapAccessor ba = new RapidBitmapAccessor(bmp);
            ba.BeginAccess();
            for (int y = 0; y < CaptureField.Y_MAX; y++)
            {
                for (int x = 0; x < CaptureField.X_MAX; x++)
                {
                    field.SetPuyoType(x, y, detector.Detect(ba, field.GetRect(x, y)));
                }
            }

            ba.EndAccess();
            return field;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (captureBmp != null)
            {
                captureBmp.Dispose();
            }

            if (nextBmp != null)
            {
                nextBmp.Dispose();
            }

            Debug.Flush();
        }

        private void fieldImg_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isPixeling)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                // サンプル選択
                int x = pointOnFieldImg.X - (pointOnFieldImg.X % CaptureField.UNIT);
                int y = pointOnFieldImg.Y - (pointOnFieldImg.Y % CaptureField.UNIT);
                Rectangle pixelingCellRect = new Rectangle(x, y, CaptureField.UNIT, CaptureField.UNIT);

                using (Graphics captureBmpG = Graphics.FromImage(captureBmp))
                using (Bitmap forAnalyzeBmp = new Bitmap(fieldImg.Width, fieldImg.Height))
                using (Graphics forAnalyzeG = Graphics.FromImage(forAnalyzeBmp))
                {
                    // フィールドのキャプチャ範囲を取り込む
                    captureBmpG.CopyFromScreen(new Point(captureRect.Left, captureRect.Top), new Point(0, 0), captureBmp.Size);

                    // 取り込んだ画像を画面に出力
                    Rectangle dest = new Rectangle(0, 0, 192, 384);
                    Rectangle src = new Rectangle(0, 0, captureRect.Width, captureRect.Height);
                    forAnalyzeG.DrawImage(captureBmp, dest, src, GraphicsUnit.Pixel);

                    // 解析用のBMPにも画面と同じ内容を出力
                    Bitmap cellBmp = forAnalyzeBmp.Clone(pixelingCellRect, forAnalyzeBmp.PixelFormat);
                    PuyoType puyoType = (PuyoType)pixelingTargetIndex;

                    // 選択したサンプルを設定
                    RapidBitmapAccessor ba = new RapidBitmapAccessor(cellBmp);
                    ba.BeginAccess();
                    detector.UpdateSample(puyoType, ba);
                    ba.EndAccess();

                    // サンプルした画像を保存
                    if (sampleImgs[puyoType].Image != null)
                    {
                        sampleImgs[puyoType].Image.Dispose();
                    }
                    sampleImgs[puyoType].Image = cellBmp;
                    Directory.CreateDirectory("img");
                    cellBmp.Save("img/" + (PuyoType)pixelingTargetIndex + ".bmp", ImageFormat.Bmp);
                }
            }

            pixelingTargetIndex++;
            if (pixelingTargetIndex > (int)PuyoType.MURASAKI)
            {
                isPixeling = false;
                statusLabel.Text = "";
            }
            else
            {
                statusLabel.Text = (PuyoType)pixelingTargetIndex + "のサンプルピクセルをクリックしてください。右クリックでスキップします。";
            }
        }

        private ColorPairPuyo curNext;
        private ColorPairPuyo prevNext;
        bool readyForNextStepRecord = false;
        bool readyForNextStepRecord2 = false;
        bool isRecording = false;
        int captureFailCount = 0;
        private void nextImg_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (!IsCapturing)
                {
                    return;
                }

                Graphics nextG = e.Graphics;
                using (Graphics nextBmpG = Graphics.FromImage(nextBmp))
                using (Bitmap forAnalyzeBmp = new Bitmap(nextImg.Width, nextImg.Height))
                using (Graphics forAnalyzeG = Graphics.FromImage(forAnalyzeBmp))
                {
                    // ネクストのキャプチャ範囲を取り込む
                    nextBmpG.CopyFromScreen(new Point(nextRect.Left, nextRect.Top), new Point(0, 0), nextBmp.Size);
                    Rectangle dest = new Rectangle(0, 0, 32, 64);
                    Rectangle src = new Rectangle(0, 0, nextRect.Width, nextRect.Height);

                    // 取り込んだ画像を画面に出力
                    forAnalyzeG.DrawImage(nextBmp, dest, src, GraphicsUnit.Pixel);

                    // 解析用のBMPにも画面と同じ内容を出力
                    nextG.DrawImage(forAnalyzeBmp, dest, dest, GraphicsUnit.Pixel);

                    // ツモ情報を解析し、結果を描画
                    CaptureField field = b(forAnalyzeBmp);
                    DrawDebugNextRect(nextG, field);

                    ColorPairPuyo next = field.GetNext(0);

                    if (!isRecording || steps.Count() >= 16)
                    {
                        isRecording = false;
                        stopBtn.Enabled = false;
                        return;
                    }

                    if (next.Pivot != PuyoType.NONE && next.Satellite != PuyoType.NONE)
                    {
                        if (!readyForNextStepRecord)
                        {
                            Debug.WriteLine(next.Pivot + " " + next.Satellite);
                            curNext = next;
                        }

                        readyForNextStepRecord = true;
                    }
                    else if (next.Pivot == PuyoType.NONE && next.Satellite == PuyoType.NONE && !readyForNextStepRecord2)
                    {
                        if (readyForNextStepRecord)
                        {
                            if (prevNext != null)
                            {
                                readyForNextStepRecord2 = true;
                                ColorPairPuyo prevStep = prevField.GetStepFromDiff(curField, prevNext);
                                if (prevStep != null)
                                {
                                    //Debug.WriteLine("前回：\n" + prevField);
                                    //Debug.WriteLine("今回：\n" + curField);
                                    //Debug.WriteLine(prevStep.Pivot + " " + prevStep.Satellite + " " + prevStep.Dir + " " + prevStep.Pos);
                                    //Debug.Flush();
                                    steps.Add(prevStep);

                                    prevField.Drop(prevStep);
                                    prevNext = curNext;
                                    readyForNextStepRecord = false;
                                    readyForNextStepRecord2 = false;
                                    FCodeEncoder encoder = new FCodeEncoder();
                                    stepDataTxt.Text = encoder.Encode(steps);
                                    captureFailCount = 0;
                                }
                            }
                            else
                            {
                                prevNext = curNext;
                                readyForNextStepRecord = false;
                                FCodeEncoder encoder = new FCodeEncoder();
                                stepDataTxt.Text = encoder.Encode(steps);
                            }
                        }
                    }
                    else if (readyForNextStepRecord2)
                    {
                        Debug.Flush();
                        ColorPairPuyo prevStep = prevField.GetStepFromDiff(curField, prevNext);
                        if (prevStep != null)
                        {
                            Debug.WriteLine(prevStep.Pivot + " " + prevStep.Satellite + " " + prevStep.Dir + " " + prevStep.Pos);
                            steps.Add(prevStep);

                            prevField.Drop(prevStep);
                            prevNext = curNext;
                            readyForNextStepRecord = false;
                            readyForNextStepRecord2 = false;
                            FCodeEncoder encoder = new FCodeEncoder();
                            stepDataTxt.Text = encoder.Encode(steps);
                            captureFailCount = 0;
                        }
                        else
                        {
                            Debug.WriteLine("前回：\n" + prevField);
                            Debug.WriteLine("今回：\n" + curField);
                            Debug.WriteLine("★");
                            Debug.Flush();

                            captureFailCount++;

                            // 計1秒以上譜が特定できなければ失敗とする
                            if (captureFailCount > (1000 / captureTimer.Interval))
                            {
                                statusLabel.Text = "キャプチャ失敗！！";

                                isRecording = false;
                                stopBtn.Enabled = false;
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                Debug.Flush();
                throw exp;
            }
        }

        private CaptureField b(Bitmap bmp)
        {
            CaptureField field = new CaptureField();
            RapidBitmapAccessor ba = new RapidBitmapAccessor(bmp);
            ba.BeginAccess();
            ColorPairPuyo pp = new ColorPairPuyo();
            for (int y = 0; y < 2; y++)
            {
                pp[y] = detector.Detect(ba, field.GetNextRect(0, y));
            }
            field.SetNext(0, pp);

            ba.EndAccess();
            return field;
        }

        private void DrawDebugNextRect(Graphics g, CaptureField field)
        {
            using (Pen redPen = new Pen(Color.Red, 2))
            using (Pen greenPen = new Pen(Color.Green, 2))
            using (Pen bluePen = new Pen(Color.LightBlue, 2))
            using (Pen yellowPen = new Pen(Color.Yellow, 2))
            using (Pen purplePen = new Pen(Color.Purple, 2))
            {
                ColorPairPuyo pp = field.GetNext(0);
                for (int y = 0; y < 2; y++)
                {
                    if (pp[y] == PuyoType.NONE)
                    {
                        continue;
                    }

                    Rectangle rect = field.GetNextRect(0, y);
                    Pen pen;
                    switch (pp[y])
                    {
                        case PuyoType.AKA:
                            pen = redPen;
                            break;
                        case PuyoType.MIDORI:
                            pen = greenPen;
                            break;
                        case PuyoType.AO:
                            pen = bluePen;
                            break;
                        case PuyoType.KI:
                            pen = yellowPen;
                            break;
                        case PuyoType.MURASAKI:
                            pen = purplePen;
                            break;
                        default:
                            continue;
                    }

                    rect.X++;
                    rect.Width -= 2;
                    rect.Y++;
                    rect.Height -= 2;
                    g.DrawRectangle(pen, rect);

                }
            }
        }

        private List<PairPuyo> steps = new List<PairPuyo>();

        private void startBtn_Click(object sender, EventArgs e)
        {
            steps = new List<PairPuyo>();
            stepIdTxt.UpButton();
            stepDataTxt.Text = "";
            prevNext = null;
            curNext = null;
            isRecording = true;
            stopBtn.Enabled = true;
            prevField = new CaptureField();
            curField = new CaptureField();

            readyForNextStepRecord = false;
            readyForNextStepRecord2 = false;
            captureFailCount = 0;
            statusLabel.Text = "";
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            isRecording = false;
            stopBtn.Enabled = false;
        }

        private Point pointOnFieldImg;
        private void fieldImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isPixeling)
            {
                return;
            }

            pointOnFieldImg = e.Location;
        }

        private void similarityValueBar_Scroll(object sender, EventArgs e)
        {
            detector.SimilarityThreshold = similarityValueBar.Value;
            similarityValueLbl.Text = similarityValueBar.Value.ToString();
        }
    }
}