using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Cubokta.Common;
using Cubokta.Puyo.Common;
using System.IO;
using System.Drawing.Imaging;
using log4net;

namespace Cubokta.Puyo
{
    public partial class MainForm : Form
    {
        private static readonly ILog LOGGER =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // エントリ・ポイント
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }
        
        private Label fpsLbl;
        private PuyofuConfiguration config;
        private PictureBox fieldImg2;
        private PictureBox nextImg2;
        private TextBox playerNameTxt2;
        private TextBox tagsTxt2;
        private TextBox recordTxt2;
        private RadioButton fieldRadioBoth;
        private StreamWriter recordFileWriter;
        private PictureBox sampleAkaImg;
        private PictureBox sampleMidoriImg;
        private PictureBox sampleAoImg;
        private PictureBox sampleKiImg;
        private PictureBox sampleMurasakiImg;
        private TrackBar similarityValueBar;
        private Label label1;
        private Label similarityValueLbl;
        private PictureBox sampleNoneImg;
        private TextBox tagsTxt1;
        private Label tagsLbl;
        private MenuStrip mainMenu;
        private ToolStripMenuItem MMConfigurationMenuItem;
        private ToolStripMenuItem MMFileMenuItem;
        private ToolStripMenuItem MMFileEndMenuItem;

        private PuyoTypeDetector detector = new PuyoTypeDetector();
        private CaptureRects captureRects;
        private TextBox[] playerNameTxts;
        private TextBox[] tagsTxts;
        private TextBox[] recordTxts;

        public MainForm()
        {
            InitializeComponent();

            playerNameTxts = new TextBox[] { playerNameTxt1, playerNameTxt2 };
            tagsTxts = new TextBox[] { tagsTxt1, tagsTxt2 };
            recordTxts = new TextBox[] { recordTxt1, recordTxt2 };

            statusLabel.Text = "";
            fpsLbl.Text = "";

            config = new PuyofuConfiguration();
            config.Init();

            updateSamples();
            detector.SimilarityThreshold = config.SimilarityThreshold;
            similarityValueBar.Value = config.SimilarityThreshold;
            similarityValueLbl.Text = similarityValueBar.Value.ToString();
            stepIdTxt.Text = config.RecordId.ToString();
            playDate.Text = config.RecordDate;
            playerNameTxt1.Text = config.PlayerName1;
            playerNameTxt2.Text = config.PlayerName2;
            CheckFieldRadio(config.TargetField);

            if (config.CaptureRect.Top > 0
                && config.CaptureRect.Left > 0
                && config.CaptureRect.Width > 0
                && config.CaptureRect.Height > 0)
            {
                captureRects = new CaptureRects();
                captureRects.CalculateRects(config.CaptureRect);
                BeginCapturing();
            }
        }

        private void CheckFieldRadio(string targetFieldValue)
        {
            if (targetFieldValue == "1")
            {
                fieldRadio1P.Checked = true;
            }
            else if (targetFieldValue == "2")
            {
                fieldRadio2P.Checked = true;
            }
            else
            {
                fieldRadioBoth.Checked = true;
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
        private PictureBox fieldImg1;
        private System.Windows.Forms.Timer captureTimer;
        private System.ComponentModel.IContainer components;
        private Label statusLabel;
        private Label colorInfoLbl;
        private RadioButton fieldRadio1P;
        private RadioButton fieldRadio2P;
        private PictureBox nextImg1;
        private Label playDateLbl;
        private Label playerNameLbl;
        private Label stepIdLbl;
        private DateTimePicker playDate;
        private TextBox playerNameTxt1;
        private Button startBtn;
        private Label recordLbl;
        private TextBox recordTxt1;
        private TextBox stepDataTxt;
        private NumericUpDown stepIdTxt;
        private Button cancelBtn;
        private Splitter splitter;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.spoitBtn = new System.Windows.Forms.Button();
            this.captureBtn = new System.Windows.Forms.Button();
            this.fieldImg1 = new System.Windows.Forms.PictureBox();
            this.captureTimer = new System.Windows.Forms.Timer(this.components);
            this.statusLabel = new System.Windows.Forms.Label();
            this.colorInfoLbl = new System.Windows.Forms.Label();
            this.fieldRadio1P = new System.Windows.Forms.RadioButton();
            this.fieldRadio2P = new System.Windows.Forms.RadioButton();
            this.nextImg1 = new System.Windows.Forms.PictureBox();
            this.playDateLbl = new System.Windows.Forms.Label();
            this.playerNameLbl = new System.Windows.Forms.Label();
            this.stepIdLbl = new System.Windows.Forms.Label();
            this.playDate = new System.Windows.Forms.DateTimePicker();
            this.playerNameTxt1 = new System.Windows.Forms.TextBox();
            this.splitter = new System.Windows.Forms.Splitter();
            this.startBtn = new System.Windows.Forms.Button();
            this.recordLbl = new System.Windows.Forms.Label();
            this.recordTxt1 = new System.Windows.Forms.TextBox();
            this.stepDataTxt = new System.Windows.Forms.TextBox();
            this.stepIdTxt = new System.Windows.Forms.NumericUpDown();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.sampleAkaImg = new System.Windows.Forms.PictureBox();
            this.sampleMidoriImg = new System.Windows.Forms.PictureBox();
            this.sampleAoImg = new System.Windows.Forms.PictureBox();
            this.sampleKiImg = new System.Windows.Forms.PictureBox();
            this.sampleMurasakiImg = new System.Windows.Forms.PictureBox();
            this.similarityValueBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.similarityValueLbl = new System.Windows.Forms.Label();
            this.sampleNoneImg = new System.Windows.Forms.PictureBox();
            this.tagsTxt1 = new System.Windows.Forms.TextBox();
            this.tagsLbl = new System.Windows.Forms.Label();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.MMFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileEndMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MMConfigurationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fpsLbl = new System.Windows.Forms.Label();
            this.fieldImg2 = new System.Windows.Forms.PictureBox();
            this.nextImg2 = new System.Windows.Forms.PictureBox();
            this.playerNameTxt2 = new System.Windows.Forms.TextBox();
            this.tagsTxt2 = new System.Windows.Forms.TextBox();
            this.recordTxt2 = new System.Windows.Forms.TextBox();
            this.fieldRadioBoth = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.fieldImg1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImg1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepIdTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleAkaImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleMidoriImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleAoImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleKiImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleMurasakiImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.similarityValueBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleNoneImg)).BeginInit();
            this.mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldImg2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImg2)).BeginInit();
            this.SuspendLayout();
            // 
            // spoitBtn
            // 
            this.spoitBtn.Enabled = false;
            this.spoitBtn.Location = new System.Drawing.Point(24, 32);
            this.spoitBtn.Name = "spoitBtn";
            this.spoitBtn.Size = new System.Drawing.Size(110, 63);
            this.spoitBtn.TabIndex = 1;
            this.spoitBtn.Text = "スポイト";
            this.spoitBtn.UseVisualStyleBackColor = true;
            this.spoitBtn.Click += new System.EventHandler(this.spoitBtn_Click);
            // 
            // captureBtn
            // 
            this.captureBtn.Location = new System.Drawing.Point(24, 126);
            this.captureBtn.Name = "captureBtn";
            this.captureBtn.Size = new System.Drawing.Size(110, 70);
            this.captureBtn.TabIndex = 2;
            this.captureBtn.Text = "キャプチャスクリーン";
            this.captureBtn.UseVisualStyleBackColor = true;
            this.captureBtn.Click += new System.EventHandler(this.captureBtn_Click);
            // 
            // fieldImg1
            // 
            this.fieldImg1.Location = new System.Drawing.Point(140, 32);
            this.fieldImg1.Name = "fieldImg1";
            this.fieldImg1.Size = new System.Drawing.Size(192, 384);
            this.fieldImg1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fieldImg1.TabIndex = 3;
            this.fieldImg1.TabStop = false;
            this.fieldImg1.Paint += new System.Windows.Forms.PaintEventHandler(this.fieldImg_Paint);
            this.fieldImg1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.fieldImg_MouseClick);
            this.fieldImg1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fieldImg_MouseMove);
            // 
            // captureTimer
            // 
            this.captureTimer.Interval = 50;
            this.captureTimer.Tick += new System.EventHandler(this.captureTimer_Tick);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(22, 510);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(41, 12);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "説明文";
            // 
            // colorInfoLbl
            // 
            this.colorInfoLbl.AutoSize = true;
            this.colorInfoLbl.Location = new System.Drawing.Point(606, 32);
            this.colorInfoLbl.Name = "colorInfoLbl";
            this.colorInfoLbl.Size = new System.Drawing.Size(67, 12);
            this.colorInfoLbl.TabIndex = 5;
            this.colorInfoLbl.Text = "サンプル画像";
            // 
            // fieldRadio1P
            // 
            this.fieldRadio1P.AutoSize = true;
            this.fieldRadio1P.Checked = true;
            this.fieldRadio1P.Location = new System.Drawing.Point(31, 208);
            this.fieldRadio1P.Name = "fieldRadio1P";
            this.fieldRadio1P.Size = new System.Drawing.Size(36, 16);
            this.fieldRadio1P.TabIndex = 3;
            this.fieldRadio1P.TabStop = true;
            this.fieldRadio1P.Text = "1P";
            this.fieldRadio1P.UseVisualStyleBackColor = true;
            // 
            // fieldRadio2P
            // 
            this.fieldRadio2P.AccessibleName = "";
            this.fieldRadio2P.AutoSize = true;
            this.fieldRadio2P.Location = new System.Drawing.Point(73, 208);
            this.fieldRadio2P.Name = "fieldRadio2P";
            this.fieldRadio2P.Size = new System.Drawing.Size(36, 16);
            this.fieldRadio2P.TabIndex = 4;
            this.fieldRadio2P.Text = "2P";
            this.fieldRadio2P.UseVisualStyleBackColor = true;
            // 
            // nextImg1
            // 
            this.nextImg1.Location = new System.Drawing.Point(336, 47);
            this.nextImg1.Name = "nextImg1";
            this.nextImg1.Size = new System.Drawing.Size(32, 64);
            this.nextImg1.TabIndex = 7;
            this.nextImg1.TabStop = false;
            this.nextImg1.Paint += new System.Windows.Forms.PaintEventHandler(this.nextImg_Paint);
            // 
            // playDateLbl
            // 
            this.playDateLbl.AutoSize = true;
            this.playDateLbl.Location = new System.Drawing.Point(606, 184);
            this.playDateLbl.Name = "playDateLbl";
            this.playDateLbl.Size = new System.Drawing.Size(29, 12);
            this.playDateLbl.TabIndex = 8;
            this.playDateLbl.Text = "日付";
            // 
            // playerNameLbl
            // 
            this.playerNameLbl.AutoSize = true;
            this.playerNameLbl.Location = new System.Drawing.Point(80, 435);
            this.playerNameLbl.Name = "playerNameLbl";
            this.playerNameLbl.Size = new System.Drawing.Size(54, 12);
            this.playerNameLbl.TabIndex = 9;
            this.playerNameLbl.Text = "プレイヤ名";
            // 
            // stepIdLbl
            // 
            this.stepIdLbl.AutoSize = true;
            this.stepIdLbl.Location = new System.Drawing.Point(619, 155);
            this.stepIdLbl.Name = "stepIdLbl";
            this.stepIdLbl.Size = new System.Drawing.Size(14, 12);
            this.stepIdLbl.TabIndex = 10;
            this.stepIdLbl.Text = "id";
            // 
            // playDate
            // 
            this.playDate.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.playDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.playDate.Location = new System.Drawing.Point(641, 179);
            this.playDate.Name = "playDate";
            this.playDate.Size = new System.Drawing.Size(200, 19);
            this.playDate.TabIndex = 10;
            // 
            // playerNameTxt1
            // 
            this.playerNameTxt1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.playerNameTxt1.Location = new System.Drawing.Point(140, 432);
            this.playerNameTxt1.Name = "playerNameTxt1";
            this.playerNameTxt1.Size = new System.Drawing.Size(192, 19);
            this.playerNameTxt1.TabIndex = 11;
            // 
            // splitter
            // 
            this.splitter.Location = new System.Drawing.Point(0, 26);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(3, 505);
            this.splitter.TabIndex = 14;
            this.splitter.TabStop = false;
            // 
            // startBtn
            // 
            this.startBtn.Enabled = false;
            this.startBtn.Location = new System.Drawing.Point(24, 268);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(110, 70);
            this.startBtn.TabIndex = 6;
            this.startBtn.Text = "開始";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // recordLbl
            // 
            this.recordLbl.AutoSize = true;
            this.recordLbl.Location = new System.Drawing.Point(93, 483);
            this.recordLbl.Name = "recordLbl";
            this.recordLbl.Size = new System.Drawing.Size(41, 12);
            this.recordLbl.TabIndex = 16;
            this.recordLbl.Text = "レコード";
            // 
            // recordTxt1
            // 
            this.recordTxt1.Location = new System.Drawing.Point(140, 480);
            this.recordTxt1.Name = "recordTxt1";
            this.recordTxt1.Size = new System.Drawing.Size(192, 19);
            this.recordTxt1.TabIndex = 13;
            // 
            // stepDataTxt
            // 
            this.stepDataTxt.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stepDataTxt.Location = new System.Drawing.Point(601, 219);
            this.stepDataTxt.Multiline = true;
            this.stepDataTxt.Name = "stepDataTxt";
            this.stepDataTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.stepDataTxt.Size = new System.Drawing.Size(266, 280);
            this.stepDataTxt.TabIndex = 17;
            this.stepDataTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.stepDataTxt_KeyDown);
            // 
            // stepIdTxt
            // 
            this.stepIdTxt.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stepIdTxt.Location = new System.Drawing.Point(641, 153);
            this.stepIdTxt.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.stepIdTxt.Name = "stepIdTxt";
            this.stepIdTxt.Size = new System.Drawing.Size(120, 19);
            this.stepIdTxt.TabIndex = 9;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Enabled = false;
            this.cancelBtn.Location = new System.Drawing.Point(24, 346);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(110, 70);
            this.cancelBtn.TabIndex = 7;
            this.cancelBtn.Text = "やりおなす";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // sampleAkaImg
            // 
            this.sampleAkaImg.Location = new System.Drawing.Point(632, 47);
            this.sampleAkaImg.Name = "sampleAkaImg";
            this.sampleAkaImg.Size = new System.Drawing.Size(32, 32);
            this.sampleAkaImg.TabIndex = 21;
            this.sampleAkaImg.TabStop = false;
            // 
            // sampleMidoriImg
            // 
            this.sampleMidoriImg.Location = new System.Drawing.Point(663, 47);
            this.sampleMidoriImg.Name = "sampleMidoriImg";
            this.sampleMidoriImg.Size = new System.Drawing.Size(32, 32);
            this.sampleMidoriImg.TabIndex = 22;
            this.sampleMidoriImg.TabStop = false;
            // 
            // sampleAoImg
            // 
            this.sampleAoImg.Location = new System.Drawing.Point(694, 47);
            this.sampleAoImg.Name = "sampleAoImg";
            this.sampleAoImg.Size = new System.Drawing.Size(32, 32);
            this.sampleAoImg.TabIndex = 23;
            this.sampleAoImg.TabStop = false;
            // 
            // sampleKiImg
            // 
            this.sampleKiImg.Location = new System.Drawing.Point(725, 47);
            this.sampleKiImg.Name = "sampleKiImg";
            this.sampleKiImg.Size = new System.Drawing.Size(32, 32);
            this.sampleKiImg.TabIndex = 24;
            this.sampleKiImg.TabStop = false;
            // 
            // sampleMurasakiImg
            // 
            this.sampleMurasakiImg.Location = new System.Drawing.Point(756, 47);
            this.sampleMurasakiImg.Name = "sampleMurasakiImg";
            this.sampleMurasakiImg.Size = new System.Drawing.Size(32, 32);
            this.sampleMurasakiImg.TabIndex = 25;
            this.sampleMurasakiImg.TabStop = false;
            // 
            // similarityValueBar
            // 
            this.similarityValueBar.LargeChange = 100;
            this.similarityValueBar.Location = new System.Drawing.Point(641, 100);
            this.similarityValueBar.Maximum = 10000;
            this.similarityValueBar.Minimum = 100;
            this.similarityValueBar.Name = "similarityValueBar";
            this.similarityValueBar.Size = new System.Drawing.Size(147, 45);
            this.similarityValueBar.SmallChange = 10;
            this.similarityValueBar.TabIndex = 8;
            this.similarityValueBar.TickFrequency = 1000;
            this.similarityValueBar.Value = 2500;
            this.similarityValueBar.Scroll += new System.EventHandler(this.similarityValueBar_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(606, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 28;
            this.label1.Text = "閾値";
            // 
            // similarityValueLbl
            // 
            this.similarityValueLbl.AutoSize = true;
            this.similarityValueLbl.Location = new System.Drawing.Point(606, 117);
            this.similarityValueLbl.Name = "similarityValueLbl";
            this.similarityValueLbl.Size = new System.Drawing.Size(35, 12);
            this.similarityValueLbl.TabIndex = 29;
            this.similarityValueLbl.Text = "30000";
            // 
            // sampleNoneImg
            // 
            this.sampleNoneImg.Location = new System.Drawing.Point(601, 47);
            this.sampleNoneImg.Name = "sampleNoneImg";
            this.sampleNoneImg.Size = new System.Drawing.Size(32, 32);
            this.sampleNoneImg.TabIndex = 30;
            this.sampleNoneImg.TabStop = false;
            // 
            // tagsTxt1
            // 
            this.tagsTxt1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tagsTxt1.Location = new System.Drawing.Point(140, 455);
            this.tagsTxt1.Name = "tagsTxt1";
            this.tagsTxt1.Size = new System.Drawing.Size(192, 19);
            this.tagsTxt1.TabIndex = 12;
            // 
            // tagsLbl
            // 
            this.tagsLbl.AutoSize = true;
            this.tagsLbl.Location = new System.Drawing.Point(88, 458);
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
            this.mainMenu.Size = new System.Drawing.Size(900, 26);
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
            this.fpsLbl.Location = new System.Drawing.Point(841, 510);
            this.fpsLbl.Name = "fpsLbl";
            this.fpsLbl.Size = new System.Drawing.Size(26, 12);
            this.fpsLbl.TabIndex = 34;
            this.fpsLbl.Text = "FPS";
            // 
            // fieldImg2
            // 
            this.fieldImg2.Location = new System.Drawing.Point(390, 32);
            this.fieldImg2.Name = "fieldImg2";
            this.fieldImg2.Size = new System.Drawing.Size(192, 384);
            this.fieldImg2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fieldImg2.TabIndex = 35;
            this.fieldImg2.TabStop = false;
            this.fieldImg2.Paint += new System.Windows.Forms.PaintEventHandler(this.fieldImg_Paint);
            this.fieldImg2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.fieldImg_MouseClick);
            this.fieldImg2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fieldImg_MouseMove);
            // 
            // nextImg2
            // 
            this.nextImg2.Location = new System.Drawing.Point(354, 208);
            this.nextImg2.Name = "nextImg2";
            this.nextImg2.Size = new System.Drawing.Size(32, 64);
            this.nextImg2.TabIndex = 36;
            this.nextImg2.TabStop = false;
            this.nextImg2.Paint += new System.Windows.Forms.PaintEventHandler(this.nextImg_Paint);
            // 
            // playerNameTxt2
            // 
            this.playerNameTxt2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.playerNameTxt2.Location = new System.Drawing.Point(390, 432);
            this.playerNameTxt2.Name = "playerNameTxt2";
            this.playerNameTxt2.Size = new System.Drawing.Size(192, 19);
            this.playerNameTxt2.TabIndex = 14;
            // 
            // tagsTxt2
            // 
            this.tagsTxt2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tagsTxt2.Location = new System.Drawing.Point(390, 455);
            this.tagsTxt2.Name = "tagsTxt2";
            this.tagsTxt2.Size = new System.Drawing.Size(192, 19);
            this.tagsTxt2.TabIndex = 15;
            // 
            // recordTxt2
            // 
            this.recordTxt2.Location = new System.Drawing.Point(390, 480);
            this.recordTxt2.Name = "recordTxt2";
            this.recordTxt2.Size = new System.Drawing.Size(192, 19);
            this.recordTxt2.TabIndex = 16;
            // 
            // fieldRadioBoth
            // 
            this.fieldRadioBoth.AccessibleName = "";
            this.fieldRadioBoth.AutoSize = true;
            this.fieldRadioBoth.Location = new System.Drawing.Point(31, 230);
            this.fieldRadioBoth.Name = "fieldRadioBoth";
            this.fieldRadioBoth.Size = new System.Drawing.Size(55, 16);
            this.fieldRadioBoth.TabIndex = 5;
            this.fieldRadioBoth.Text = "1P・2P";
            this.fieldRadioBoth.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(900, 531);
            this.Controls.Add(this.fieldRadioBoth);
            this.Controls.Add(this.recordTxt2);
            this.Controls.Add(this.tagsTxt2);
            this.Controls.Add(this.playerNameTxt2);
            this.Controls.Add(this.nextImg2);
            this.Controls.Add(this.fieldImg2);
            this.Controls.Add(this.fpsLbl);
            this.Controls.Add(this.tagsLbl);
            this.Controls.Add(this.tagsTxt1);
            this.Controls.Add(this.sampleNoneImg);
            this.Controls.Add(this.similarityValueLbl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.similarityValueBar);
            this.Controls.Add(this.sampleMurasakiImg);
            this.Controls.Add(this.sampleKiImg);
            this.Controls.Add(this.sampleAoImg);
            this.Controls.Add(this.sampleMidoriImg);
            this.Controls.Add(this.sampleAkaImg);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.stepIdTxt);
            this.Controls.Add(this.stepDataTxt);
            this.Controls.Add(this.recordTxt1);
            this.Controls.Add(this.recordLbl);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.playerNameTxt1);
            this.Controls.Add(this.playDate);
            this.Controls.Add(this.stepIdLbl);
            this.Controls.Add(this.playerNameLbl);
            this.Controls.Add(this.playDateLbl);
            this.Controls.Add(this.nextImg1);
            this.Controls.Add(this.fieldRadio2P);
            this.Controls.Add(this.fieldRadio1P);
            this.Controls.Add(this.colorInfoLbl);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.fieldImg1);
            this.Controls.Add(this.captureBtn);
            this.Controls.Add(this.spoitBtn);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "PuyofuCapture";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.fieldImg1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImg1)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.fieldImg2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImg2)).EndInit();
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

            int fieldNo = fieldRadio1P.Checked ? 0 : 1;
            Form captureForm = new CaptureForm(fieldNo);
            captureForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptureForm_FormClosed);
            captureForm.Show();
        }

        private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CaptureForm f = sender as CaptureForm;
            if (f == null)
            {
                return;
            }

            config.CaptureRect = f.CaptureRects.CaptureRect;
            config.Save();

            captureRects = f.CaptureRects;
            BeginCapturing();
        }

        private void BeginCapturing()
        {
            IsCapturing = true;
            spoitBtn.Enabled = true;
            startBtn.Enabled = true;
            captureTimer.Start();

            if (screenBmp != null)
            {
                screenBmp.Dispose();
            }
            screenBmp = new Bitmap(captureRects.CaptureRect.Width, captureRects.CaptureRect.Height);
        }

        FpsCalculator fpsCalculator = new FpsCalculator();
        private Bitmap screenBmp;
        private void captureTimer_Tick(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(screenBmp))
            {
                Rectangle captureRect = captureRects.CaptureRect;
                g.CopyFromScreen(new Point(captureRect.X, captureRect.Y), new Point(0, 0), screenBmp.Size);
            }

            Refresh();
            fpsCalculator.Refresh();
            fpsLbl.Text = "fps:" + fpsCalculator.GetFpsInt();
        }

        private bool IsCapturing { get; set; }
        private CaptureField[] prevFields = new CaptureField[2]{ new CaptureField(), new CaptureField() };
        private CaptureField[] curFields = new CaptureField[2]{ new CaptureField(), new CaptureField() };

        private void PaintField(object sender, PaintEventArgs e, int fieldNo)
        {
            if (!IsCapturing)
            {
                return;
            }

            PictureBox fieldImg = (PictureBox)sender;
            Graphics fieldImgG = e.Graphics;
            using (Bitmap forAnalyzeBmp = new Bitmap(fieldImg.Width, fieldImg.Height))
            using (Graphics forAnalyzeG = Graphics.FromImage(forAnalyzeBmp))
            {
                // フィールドのキャプチャ範囲を取り込む
                Rectangle fieldRect = captureRects.GetFieldRect(fieldNo);

                //取り込んだ画像を解析用のBMPに出力
                Rectangle dest = new Rectangle(0, 0, 192, 384);
                Rectangle src = new Rectangle() {
                    X = fieldRect.X - captureRects.CaptureRect.X,
                    Y = fieldRect.Y - captureRects.CaptureRect.Y,
                    Width = fieldRect.Width,
                    Height = fieldRect.Height
                };
                forAnalyzeG.DrawImage(screenBmp, dest, src, GraphicsUnit.Pixel);

                // 取り込んだ画像を画面に出力
                fieldImgG.DrawImage(forAnalyzeBmp, dest, dest, GraphicsUnit.Pixel);

                if (!isPixeling)
                {
                    // フィールドの状態を解析し、結果を描画
                    curFields[fieldNo] = a(forAnalyzeBmp);
                    if (config.DebugRectEnabled)
                    {
                        DrawDebugRect(fieldImgG, curFields[fieldNo]);
                    }
                }
                else if (fieldNo == fieldNoMouseIsOn)
                {
                    // サンプル選択枠を描画
                    int x = pointOnFieldImg.X - (pointOnFieldImg.X % CaptureField.UNIT);
                    int y = pointOnFieldImg.Y - (pointOnFieldImg.Y % CaptureField.UNIT);

                    Rectangle pixelingCellRect = new Rectangle(x, y, CaptureField.UNIT, CaptureField.UNIT);
                    fieldImgG.DrawRectangle(new Pen(Color.Red, 2), pixelingCellRect);
                }
            }
        }

        private int GetFieldNumberFromControl(Control control)
        {
            return int.Parse(control.Name.Substring(control.Name.Length - 1)) - 1;
        }

        private bool IsProcessingField(int fieldNo)
        {
            if ((fieldRadio1P.Checked || fieldRadioBoth.Checked) && fieldNo == 0)
            {
                return true;
            }

            if ((fieldRadio2P.Checked || fieldRadioBoth.Checked) && fieldNo == 1)
            {
                return true;
            }

            return false;
        }

        private void fieldImg_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Control img = (Control)sender;
                int fieldNo = GetFieldNumberFromControl(img);
                if (!IsProcessingField(fieldNo))
                {
                    return;
                }
                
                PaintField(sender, e, fieldNo);
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
            config.RecordId = (int)stepIdTxt.Value;
            config.RecordDate = playDate.Text;
            config.PlayerName1 = playerNameTxt1.Text;
            config.PlayerName2 = playerNameTxt2.Text;
            config.TargetField = GetTargetFieldValue();
            config.Save();

            if (screenBmp != null)
            {
                screenBmp.Dispose();
            }

            if (recordFileWriter != null)
            {
                recordFileWriter.Close();
            }
        }

        private string GetTargetFieldValue()
        {
            if (fieldRadio1P.Checked)
            {
                return "1";
            }
            else if (fieldRadio2P.Checked)
            {
                return "2";
            }
            else
            {
                return "3";
            }
        }

        private void ClickField(object sender, MouseEventArgs e, int fieldNo)
        {
            if (!isPixeling)
            {
                return;
            }

            PictureBox fieldImg = (PictureBox)sender;

            if (e.Button == MouseButtons.Left)
            {
                // サンプル選択
                int x = pointOnFieldImg.X - (pointOnFieldImg.X % CaptureField.UNIT);
                int y = pointOnFieldImg.Y - (pointOnFieldImg.Y % CaptureField.UNIT);
                Rectangle pixelingCellRect = new Rectangle(x, y, CaptureField.UNIT, CaptureField.UNIT);

                using (Bitmap forAnalyzeBmp = new Bitmap(fieldImg.Width, fieldImg.Height))
                using (Graphics forAnalyzeG = Graphics.FromImage(forAnalyzeBmp))
                {
                    // フィールドのキャプチャ範囲を取り込む
                    Rectangle fieldRect = captureRects.GetFieldRect(fieldNo);

                    //取り込んだ画像を解析用のBMPに出力
                    Rectangle dest = new Rectangle(0, 0, 192, 384);
                    Rectangle src = new Rectangle()
                    {
                        X = fieldRect.X - captureRects.CaptureRect.X,
                        Y = fieldRect.Y - captureRects.CaptureRect.Y,
                        Width = fieldRect.Width,
                        Height = fieldRect.Height
                    };
                    forAnalyzeG.DrawImage(screenBmp, dest, src, GraphicsUnit.Pixel);

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

        private void fieldImg_MouseClick(object sender, MouseEventArgs e)
        {
            Control img = (Control)sender;
            int fieldNo = GetFieldNumberFromControl(img);
            if (!IsProcessingField(fieldNo))
            {
                return;
            }

            ClickField(sender, e, fieldNo);
        }

        PuyofuRecorder[] recorders = new PuyofuRecorder[2] { new PuyofuRecorder(), new PuyofuRecorder() };

        private void PaintNext(object sender, PaintEventArgs e, int fieldNo)
        {
            if (!IsCapturing)
            {
                return;
            }

            PictureBox nextImg = (PictureBox)sender;

            Graphics nextG = e.Graphics;
            using (Bitmap forAnalyzeBmp = new Bitmap(nextImg.Width, nextImg.Height))
            using (Graphics forAnalyzeG = Graphics.FromImage(forAnalyzeBmp))
            {
                // ネクストのキャプチャ範囲を取り込む
                Rectangle nextRect = captureRects.GetNextRect(fieldNo);

                //取り込んだ画像を解析用のBMPに出力
                Rectangle dest = new Rectangle(0, 0, 32, 64);
                Rectangle src = new Rectangle()
                {
                    X = nextRect.X - captureRects.CaptureRect.X,
                    Y = nextRect.Y - captureRects.CaptureRect.Y,
                    Width = nextRect.Width,
                    Height = nextRect.Height
                };
                forAnalyzeG.DrawImage(screenBmp, dest, src, GraphicsUnit.Pixel);

                // 取り込んだ画像を画面に出力
                nextG.DrawImage(forAnalyzeBmp, dest, dest, GraphicsUnit.Pixel);

                // ツモ情報を解析し、結果を描画
                CaptureField field = b(forAnalyzeBmp);
                if (config.DebugRectEnabled)
                {
                    DrawDebugNextRect(nextG, field);
                }

                ColorPairPuyo next = field.Next;
                RecordResult result = recorders[fieldNo].DoNext(curFields[fieldNo], next);
                switch (result)
                {
                    case RecordResult.RECORD_SUCCESS:
                        updateStepData();
                        break;
                    case RecordResult.RECORD_FAILURE:
                        statusLabel.Text = "キャプチャ失敗！！";
                        updateStepData();
                        break;
                    case RecordResult.RECORD_FORWARD:
                        recordTxts[fieldNo].Text = recorders[fieldNo].GetRecord();
                        break;
                    case RecordResult.RECORD_ENDED:
                        updateStepData();
                        break;
                    default:
                        break;
                }
            }
        }

        private void nextImg_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Control img = (Control)sender;
                int fieldNo = GetFieldNumberFromControl(img);
                if (!IsProcessingField(fieldNo))
                {
                    return;
                } 
                
                PaintNext(sender, e, fieldNo);
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
  },
";

        private void updateStepData()
        {
            int idCount = 0;
            string step1 = GetStepData(0, ref idCount);
            string step2 = GetStepData(1, ref idCount);
            string step = "";
            if (step1 != "")
            {
                step += step1 + "\r\n";
            }

            if (step2 != "")
            {
                step += step2 + "\r\n";
            }

            stepDataTxt.Text = step;
        }

        private string GetStepData(int fieldNo, ref int idCount)
        {
            if (!recorders[fieldNo].IsRecordSucceeded)
            {
                return "";
            }

            // タグ文字列を組み立てる
            List<string> tagList = new List<string>();
            if (playerNameTxts[fieldNo].Text.Trim() != "")
            {
                tagList.Add(playerNameTxts[fieldNo].Text);
            }

            // 追加タグの処理
            StringBuilder sb = new StringBuilder();
            if (tagsTxts[fieldNo].Text.Trim() != "")
            {
                tagList.AddRange(tagsTxts[fieldNo].Text.Split(
                    new char[] { ' ', '　' }, StringSplitOptions.RemoveEmptyEntries));
            }

            // 初手3手の処理
            List<PairPuyo> steps = recorders[fieldNo].GetSteps();
            FirstStepAnalyzer firstStepAnalyzer = new FirstStepAnalyzer();
            if (steps.Count < 3)
            {
                tagList.Add(firstStepAnalyzer.GetPattern(steps));
            }
            else
            {
                tagList.Add(firstStepAnalyzer.GetPattern(steps, 3));
            }

            foreach (string tag in tagList)
            {
                sb.Append("\r\n      '" + tag + "',");
            }
            sb.Append("\r\n    ");

            // テンプレート変換
            string text = RECORD_TEMPLATE;
            text = text.Replace("#date", playDate.Text);
            text = text.Replace("#id", (stepIdTxt.Value + idCount).ToString());
            text = text.Replace("#record", recordTxts[fieldNo].Text);
            text = text.Replace("#tags", sb.ToString());

            idCount++;
            return text;
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
            field.Next = pp;

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
                ColorPairPuyo pp = field.Next;
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

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (recordFileWriter == null)
            {
                recordFileWriter = new StreamWriter("record.js", false, Encoding.UTF8);
            }
            else
            {
                if ((!IsProcessingField(0) || (IsProcessingField(0) && recorders[0].IsRecordEnded))
                    && (!IsProcessingField(1) || (IsProcessingField(1) && recorders[1].IsRecordEnded)))
                {
                    recordFileWriter.Write(stepDataTxt.Text);
                    recordFileWriter.Flush();
                }
            }

            stepDataTxt.Text = "";
            cancelBtn.Enabled = true;
            statusLabel.Text = "";

            if (IsProcessingField(0))
            {
                if (recorders[0].IsRecordSucceeded)
                {
                    stepIdTxt.UpButton();
                }
                prevFields[0] = new CaptureField();
                curFields[0] = new CaptureField();
                recorders[0] = new PuyofuRecorder();
                recorders[0].BeginRecord(captureTimer.Interval);
            }

            if (IsProcessingField(1))
            {
                if (recorders[1].IsRecordSucceeded)
                {
                    stepIdTxt.UpButton();
                }
                prevFields[1] = new CaptureField();
                curFields[1] = new CaptureField();
                recorders[1] = new PuyofuRecorder();
                recorders[1].BeginRecord(captureTimer.Interval);
            }

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            cancelBtn.Enabled = false;
            statusLabel.Text = "";

            recorders[0] = new PuyofuRecorder();
            recorders[1] = new PuyofuRecorder();
        }

        private int fieldNoMouseIsOn = -1;
        private Point pointOnFieldImg;
        private void fieldImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isPixeling)
            {
                return;
            }

            Control img = (Control)sender;
            int fieldNo = GetFieldNumberFromControl(img);
            if (!IsProcessingField(fieldNo))
            {
                return;
            }

            fieldNoMouseIsOn = fieldNo;
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
