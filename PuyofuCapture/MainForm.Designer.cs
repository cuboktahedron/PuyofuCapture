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
        private Label fpsLbl;
        private PuyofuConfiguration config;
        private PictureBox fieldImg2;
        private PictureBox nextImg2;
        private TextBox playerNameTxt2;
        private TextBox tagsTxt2;
        private TextBox stepRecordTxt2;
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

        private Button samplingBtn;
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
        private DateTimePicker recordDate;
        private TextBox playerNameTxt1;
        private Button startBtn;
        private Label recordLbl;
        private TextBox stepRecordTxt1;
        private TextBox recordDataTxt;
        private NumericUpDown recordIdTxt;
        private Button cancelBtn;
        private Splitter splitter;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.samplingBtn = new System.Windows.Forms.Button();
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
            this.recordDate = new System.Windows.Forms.DateTimePicker();
            this.playerNameTxt1 = new System.Windows.Forms.TextBox();
            this.splitter = new System.Windows.Forms.Splitter();
            this.startBtn = new System.Windows.Forms.Button();
            this.recordLbl = new System.Windows.Forms.Label();
            this.stepRecordTxt1 = new System.Windows.Forms.TextBox();
            this.recordDataTxt = new System.Windows.Forms.TextBox();
            this.recordIdTxt = new System.Windows.Forms.NumericUpDown();
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
            this.stepRecordTxt2 = new System.Windows.Forms.TextBox();
            this.fieldRadioBoth = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.fieldImg1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImg1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recordIdTxt)).BeginInit();
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
            // samplingBtn
            // 
            this.samplingBtn.Enabled = false;
            this.samplingBtn.Location = new System.Drawing.Point(24, 57);
            this.samplingBtn.Name = "samplingBtn";
            this.samplingBtn.Size = new System.Drawing.Size(110, 63);
            this.samplingBtn.TabIndex = 1;
            this.samplingBtn.Text = "サンプリング";
            this.samplingBtn.UseVisualStyleBackColor = true;
            this.samplingBtn.Click += new System.EventHandler(this.samplingBtn_Click);
            // 
            // captureBtn
            // 
            this.captureBtn.Location = new System.Drawing.Point(24, 126);
            this.captureBtn.Name = "captureBtn";
            this.captureBtn.Size = new System.Drawing.Size(110, 70);
            this.captureBtn.TabIndex = 2;
            this.captureBtn.Text = "キャプチャ範囲選択";
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
            this.colorInfoLbl.Size = new System.Drawing.Size(82, 12);
            this.colorInfoLbl.TabIndex = 5;
            this.colorInfoLbl.Text = "サンプリング画像";
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
            // recordDate
            // 
            this.recordDate.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.recordDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.recordDate.Location = new System.Drawing.Point(641, 179);
            this.recordDate.Name = "recordDate";
            this.recordDate.Size = new System.Drawing.Size(200, 19);
            this.recordDate.TabIndex = 10;
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
            // stepRecordTxt1
            // 
            this.stepRecordTxt1.Location = new System.Drawing.Point(140, 480);
            this.stepRecordTxt1.Name = "stepRecordTxt1";
            this.stepRecordTxt1.Size = new System.Drawing.Size(192, 19);
            this.stepRecordTxt1.TabIndex = 13;
            // 
            // recordDataTxt
            // 
            this.recordDataTxt.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.recordDataTxt.Location = new System.Drawing.Point(601, 219);
            this.recordDataTxt.Multiline = true;
            this.recordDataTxt.Name = "recordDataTxt";
            this.recordDataTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.recordDataTxt.Size = new System.Drawing.Size(266, 280);
            this.recordDataTxt.TabIndex = 17;
            this.recordDataTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.recordDataTxt_KeyDown);
            // 
            // recordIdTxt
            // 
            this.recordIdTxt.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.recordIdTxt.Location = new System.Drawing.Point(641, 153);
            this.recordIdTxt.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.recordIdTxt.Name = "recordIdTxt";
            this.recordIdTxt.Size = new System.Drawing.Size(120, 19);
            this.recordIdTxt.TabIndex = 9;
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
            this.similarityValueBar.Value = 2700;
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
            this.similarityValueLbl.Size = new System.Drawing.Size(29, 12);
            this.similarityValueLbl.TabIndex = 29;
            this.similarityValueLbl.Text = "2700";
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
            // stepRecordTxt2
            // 
            this.stepRecordTxt2.Location = new System.Drawing.Point(390, 480);
            this.stepRecordTxt2.Name = "stepRecordTxt2";
            this.stepRecordTxt2.Size = new System.Drawing.Size(192, 19);
            this.stepRecordTxt2.TabIndex = 16;
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
            this.Controls.Add(this.stepRecordTxt2);
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
            this.Controls.Add(this.recordIdTxt);
            this.Controls.Add(this.recordDataTxt);
            this.Controls.Add(this.stepRecordTxt1);
            this.Controls.Add(this.recordLbl);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.playerNameTxt1);
            this.Controls.Add(this.recordDate);
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
            this.Controls.Add(this.samplingBtn);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "PuyofuCapture";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.fieldImg1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImg1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recordIdTxt)).EndInit();
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
    }
}
