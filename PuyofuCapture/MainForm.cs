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
using log4net;

namespace Cubokta.Puyo
{
    public partial class MainForm : Form
    {
        private static readonly ILog LOGGER =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Label fpsLbl;

        private PuyofuConfiguration config;

        // エントリ・ポイント
        [STAThread]
        static void Main()
        {
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
        private TextBox tagsTxt;
        private Label tagsLbl;
        private MenuStrip mainMenu;
        private ToolStripMenuItem MMConfigurationMenuItem;
        private ToolStripMenuItem MMFileMenuItem;
        private ToolStripMenuItem MMFileEndMenuItem;

        private PuyoTypeDetector detector = new PuyoTypeDetector();
        public MainForm()
        {
            InitializeComponent();
            statusLabel.Text = "";

            config = new PuyofuConfiguration();
            config.Init();

            updateSamples();
            detector.SimilarityThreshold = config.SimilarityThreshold;
            similarityValueBar.Value = config.SimilarityThreshold;
            similarityValueLbl.Text = similarityValueBar.Value.ToString();
            stepIdTxt.Text = config.RecordId.ToString();
            playDate.Text = config.RecordDate;
            playerNameTxt.Text = config.PlayerName;

            // 前回のキャプチャ範囲があればそれを使用しキャプチャを開始する
            if (config.CaptureRect.Top > 0
                && config.CaptureRect.Left > 0
                && config.CaptureRect.Width > 0
                && config.CaptureRect.Height > 0
                && config.NextRect.Top > 0
                && config.NextRect.Left > 0
                && config.NextRect.Width > 0
                && config.NextRect.Height > 0)
            {
                beginCapturing();
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
            this.tagsTxt = new System.Windows.Forms.TextBox();
            this.tagsLbl = new System.Windows.Forms.Label();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.MMFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileEndMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MMConfigurationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fpsLbl = new System.Windows.Forms.Label();
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
            this.mainMenu.SuspendLayout();
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
            this.statusLabel.Location = new System.Drawing.Point(22, 429);
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
            this.FieldRadio1P.TabIndex = 3;
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
            this.FieldRadio2P.TabIndex = 4;
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
            this.playDateLbl.Location = new System.Drawing.Point(451, 188);
            this.playDateLbl.Name = "playDateLbl";
            this.playDateLbl.Size = new System.Drawing.Size(29, 12);
            this.playDateLbl.TabIndex = 8;
            this.playDateLbl.Text = "日付";
            // 
            // playerNameLbl
            // 
            this.playerNameLbl.AutoSize = true;
            this.playerNameLbl.Location = new System.Drawing.Point(451, 215);
            this.playerNameLbl.Name = "playerNameLbl";
            this.playerNameLbl.Size = new System.Drawing.Size(54, 12);
            this.playerNameLbl.TabIndex = 9;
            this.playerNameLbl.Text = "プレイヤ名";
            // 
            // stepIdLbl
            // 
            this.stepIdLbl.AutoSize = true;
            this.stepIdLbl.Location = new System.Drawing.Point(451, 154);
            this.stepIdLbl.Name = "stepIdLbl";
            this.stepIdLbl.Size = new System.Drawing.Size(14, 12);
            this.stepIdLbl.TabIndex = 10;
            this.stepIdLbl.Text = "id";
            // 
            // playDate
            // 
            this.playDate.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.playDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.playDate.Location = new System.Drawing.Point(525, 183);
            this.playDate.Name = "playDate";
            this.playDate.Size = new System.Drawing.Size(200, 19);
            this.playDate.TabIndex = 9;
            // 
            // playerNameTxt
            // 
            this.playerNameTxt.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.playerNameTxt.Location = new System.Drawing.Point(525, 212);
            this.playerNameTxt.Name = "playerNameTxt";
            this.playerNameTxt.Size = new System.Drawing.Size(200, 19);
            this.playerNameTxt.TabIndex = 10;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 26);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 456);
            this.splitter1.TabIndex = 14;
            this.splitter1.TabStop = false;
            // 
            // startBtn
            // 
            this.startBtn.Enabled = false;
            this.startBtn.Location = new System.Drawing.Point(24, 268);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(175, 70);
            this.startBtn.TabIndex = 5;
            this.startBtn.Text = "開始";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // recordLbl
            // 
            this.recordLbl.AutoSize = true;
            this.recordLbl.Location = new System.Drawing.Point(451, 273);
            this.recordLbl.Name = "recordLbl";
            this.recordLbl.Size = new System.Drawing.Size(41, 12);
            this.recordLbl.TabIndex = 16;
            this.recordLbl.Text = "レコード";
            // 
            // recordTxt
            // 
            this.recordTxt.Location = new System.Drawing.Point(525, 270);
            this.recordTxt.Name = "recordTxt";
            this.recordTxt.ReadOnly = true;
            this.recordTxt.Size = new System.Drawing.Size(200, 19);
            this.recordTxt.TabIndex = 12;
            // 
            // stepDataTxt
            // 
            this.stepDataTxt.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stepDataTxt.Location = new System.Drawing.Point(459, 295);
            this.stepDataTxt.Multiline = true;
            this.stepDataTxt.Name = "stepDataTxt";
            this.stepDataTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.stepDataTxt.Size = new System.Drawing.Size(266, 175);
            this.stepDataTxt.TabIndex = 13;
            this.stepDataTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.stepDataTxt_KeyDown);
            // 
            // stepIdTxt
            // 
            this.stepIdTxt.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stepIdTxt.Location = new System.Drawing.Point(525, 152);
            this.stepIdTxt.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.stepIdTxt.Name = "stepIdTxt";
            this.stepIdTxt.Size = new System.Drawing.Size(120, 19);
            this.stepIdTxt.TabIndex = 8;
            // 
            // stopBtn
            // 
            this.stopBtn.Enabled = false;
            this.stopBtn.Location = new System.Drawing.Point(24, 346);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(175, 70);
            this.stopBtn.TabIndex = 6;
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
            this.similarityValueBar.Location = new System.Drawing.Point(550, 101);
            this.similarityValueBar.Maximum = 95000;
            this.similarityValueBar.Minimum = 5000;
            this.similarityValueBar.Name = "similarityValueBar";
            this.similarityValueBar.Size = new System.Drawing.Size(147, 45);
            this.similarityValueBar.SmallChange = 1000;
            this.similarityValueBar.TabIndex = 7;
            this.similarityValueBar.TickFrequency = 10000;
            this.similarityValueBar.Value = 30000;
            this.similarityValueBar.Scroll += new System.EventHandler(this.similarityValueBar_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(515, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 28;
            this.label1.Text = "閾値";
            // 
            // similarityValueLbl
            // 
            this.similarityValueLbl.AutoSize = true;
            this.similarityValueLbl.Location = new System.Drawing.Point(515, 118);
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
            // tagsTxt
            // 
            this.tagsTxt.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tagsTxt.Location = new System.Drawing.Point(525, 237);
            this.tagsTxt.Name = "tagsTxt";
            this.tagsTxt.Size = new System.Drawing.Size(200, 19);
            this.tagsTxt.TabIndex = 11;
            // 
            // tagsLbl
            // 
            this.tagsLbl.AutoSize = true;
            this.tagsLbl.Location = new System.Drawing.Point(451, 240);
            this.tagsLbl.Name = "tagsLbl";
            this.tagsLbl.Size = new System.Drawing.Size(46, 12);
            this.tagsLbl.TabIndex = 32;
            this.tagsLbl.Text = "追加タグ";
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMFileMenuItem,
            this.MMConfigurationMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(752, 26);
            this.mainMenu.TabIndex = 33;
            this.mainMenu.Text = "menuStrip1";
            // 
            // MMFileMenuItem
            // 
            this.MMFileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMFileEndMenuItem});
            this.MMFileMenuItem.Name = "MMFileMenuItem";
            this.MMFileMenuItem.Size = new System.Drawing.Size(85, 22);
            this.MMFileMenuItem.Text = "ファイル(&F)";
            // 
            // MMFileEndMenuItem
            // 
            this.MMFileEndMenuItem.Name = "MMFileEndMenuItem";
            this.MMFileEndMenuItem.Size = new System.Drawing.Size(118, 22);
            this.MMFileEndMenuItem.Text = "終了(&X)";
            this.MMFileEndMenuItem.Click += new System.EventHandler(this.MMFileEndMenuItem_Click);
            // 
            // MMConfigurationMenuItem
            // 
            this.MMConfigurationMenuItem.Name = "MMConfigurationMenuItem";
            this.MMConfigurationMenuItem.Size = new System.Drawing.Size(44, 22);
            this.MMConfigurationMenuItem.Text = "設定";
            this.MMConfigurationMenuItem.Click += new System.EventHandler(this.MMConfigurationMenuItem_Click);
            // 
            // fpsLbl
            // 
            this.fpsLbl.AutoSize = true;
            this.fpsLbl.Location = new System.Drawing.Point(22, 458);
            this.fpsLbl.Name = "fpsLbl";
            this.fpsLbl.Size = new System.Drawing.Size(0, 12);
            this.fpsLbl.TabIndex = 34;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(752, 482);
            this.Controls.Add(this.fpsLbl);
            this.Controls.Add(this.tagsLbl);
            this.Controls.Add(this.tagsTxt);
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
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
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
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
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

        private Bitmap captureBmp;
        private Bitmap nextBmp;

        private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CaptureForm f = sender as CaptureForm;
            if (f == null)
            {
                return;
            }

            config.CaptureRect = f.GetCaptureRect();
            config.NextRect = f.GetNextRect();
            config.Save();

            beginCapturing();
        }

        private void beginCapturing()
        {
            IsCapturing = true;
            spoitBtn.Enabled = true;
            startBtn.Enabled = true;
            captureTimer.Start();

            if (captureBmp != null)
            {
                captureBmp.Dispose();
            }
            captureBmp = new Bitmap(config.CaptureRect.Width, config.CaptureRect.Height);

            if (nextBmp != null)
            {
                nextBmp.Dispose();
            }
            captureBmp = new Bitmap(config.CaptureRect.Width, config.CaptureRect.Height);
            nextBmp = new Bitmap(config.NextRect.Width, config.NextRect.Height);
        }

        FpsCalculator fpsCalculator = new FpsCalculator();
        private void captureTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
            fpsCalculator.Refresh();
            fpsLbl.Text = "fps:" + fpsCalculator.GetFpsInt();
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
                    captureBmpG.CopyFromScreen(
                        new Point(config.CaptureRect.Left, config.CaptureRect.Top), new Point(0, 0), captureBmp.Size);

                    // 取り込んだ画像を画面に出力 // TODO: コメント反対？
                    Rectangle dest = new Rectangle(0, 0, 192, 384);
                    Rectangle src = new Rectangle(0, 0, config.CaptureRect.Width, config.CaptureRect.Height);
                    forAnalyzeG.DrawImage(captureBmp, dest, src, GraphicsUnit.Pixel);

                    // 解析用のBMPにも画面と同じ内容を出力
                    fieldImgG.DrawImage(forAnalyzeBmp, dest, dest, GraphicsUnit.Pixel);

                    if (!isPixeling)
                    {
                        // フィールドの状態を解析し、結果を描画
                        curField = a(forAnalyzeBmp);
                        if (config.DebugRectEnabled)
                        {
                            DrawDebugRect(fieldImgG, curField);
                        }
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
                LOGGER.Error("フィールドの描画処理中にエラーが発生", exp);
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

            field.Correct();
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

            config.RecordId = (int)stepIdTxt.Value;
            config.RecordDate = playDate.Text;
            config.PlayerName = playerNameTxt.Text;
            config.Save();
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
                    captureBmpG.CopyFromScreen(
                        new Point(config.CaptureRect.Left, config.CaptureRect.Top), new Point(0, 0), captureBmp.Size);

                    // 取り込んだ画像を画面に出力
                    Rectangle dest = new Rectangle(0, 0, 192, 384);
                    Rectangle src = new Rectangle(0, 0, config.CaptureRect.Width, config.CaptureRect.Height);
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

        PuyofuRecorder recorder = new PuyofuRecorder();
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
                    nextBmpG.CopyFromScreen(new Point(config.NextRect.Left, config.NextRect.Top), new Point(0, 0), nextBmp.Size);
                    Rectangle dest = new Rectangle(0, 0, 32, 64);
                    Rectangle src = new Rectangle(0, 0, config.NextRect.Width, config.NextRect.Height);


                    // 取り込んだ画像を画面に出力
                    forAnalyzeG.DrawImage(nextBmp, dest, src, GraphicsUnit.Pixel);

                    // 解析用のBMPにも画面と同じ内容を出力
                    nextG.DrawImage(forAnalyzeBmp, dest, dest, GraphicsUnit.Pixel);

                    // ツモ情報を解析し、結果を描画
                    CaptureField field = b(forAnalyzeBmp);
                    if (config.DebugRectEnabled)
                    {
                        DrawDebugNextRect(nextG, field);
                    }

                    ColorPairPuyo next = field.GetNext(0);
                    RecordResult result = recorder.DoNext(curField, next);
                    switch (result)
                    {
                        case RecordResult.RECORD_SUCCESS:
                            updateStepData();
                            break;
                        case RecordResult.RECORD_FAILURE:
                            statusLabel.Text = "キャプチャ失敗！！";
                            stopBtn.Enabled = false;
                            break;
                        case RecordResult.RECORD_FORWARD:
                            recordTxt.Text = recorder.GetRecord();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception exp)
            {
                LOGGER.Error("ネクストの描画処理中にエラーが発生", exp);
                throw exp;
            }
        }

        private static readonly string RECORD_TEMPLATE = @"  {
    date: '#date',
    id: '#id',
    record: '#record',
    tags: [#tags],
  },";

        private void updateStepData()
        {
            // タグ文字列を組み立てる
            List<string> tagList = new List<string>();
            if (playerNameTxt.Text.Trim() != "")
            {
                tagList.Add(playerNameTxt.Text);
            }

            // 追加タグの処理
            StringBuilder sb = new StringBuilder();
            if (tagsTxt.Text.Trim() != "")
            {
                tagList.AddRange(tagsTxt.Text.Split(' '));
            }

            // 初手3手の処理
            FCodeDecoder decoder = new FCodeDecoder();
            List<PairPuyo> steps = decoder.Decode(recordTxt.Text);
            FirstStepAnalyzer firstStepAnalyzer = new FirstStepAnalyzer();
            tagList.Add(firstStepAnalyzer.GetPattern(steps, 3));

            foreach (string tag in tagList)
            {
                sb.Append("\r\n      '" + tag + "',");
            }
            sb.Append("\r\n    ");

            // テンプレート変換
            string text = RECORD_TEMPLATE;
            text = text.Replace("#date", playDate.Text);
            text = text.Replace("#id", stepIdTxt.Value.ToString());
            text = text.Replace("#record", recordTxt.Text);
            text = text.Replace("#tags", sb.ToString());

            stepDataTxt.Text = text;
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
            stopBtn.Enabled = true;
            prevField = new CaptureField();
            curField = new CaptureField();

            statusLabel.Text = "";

            recorder = new PuyofuRecorder();
            recorder.BeginRecord(captureTimer.Interval);
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            stopBtn.Enabled = false;
            updateStepData();
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
            config.SimilarityThreshold = similarityValueBar.Value;
            config.Save();
        }

        private void stepDataTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.A & e.Control == true)
            {
                stepDataTxt.SelectAll();
            }
        }

        private void MMFileEndMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MMConfigurationMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurationForm form = new ConfigurationForm(config);
            DialogResult result = form.ShowDialog(this);
        }
    }
}
