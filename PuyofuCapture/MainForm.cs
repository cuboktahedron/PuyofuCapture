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

namespace Cubokta.Puyo
{
    public partial class MainForm : Form
    {
        // エントリ・ポイント（1）
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }

        private PuyoTypeDetector detector = new PuyoTypeDetector();
        public MainForm()
        {
            InitializeComponent();
            statusLabel.Text = "";

            Func<Color, String> f = (c) =>
            {
                return "" + string.Format("{0:x2}{1:x2}{2:x2}", c.R, c.G, c.B);
            };

            colorInfoLbl.Text =
                "赤:：" + f(detector.BaseColors[PuyoType.AKA]) + "\n" + 
                "緑:：" + f(detector.BaseColors[PuyoType.MIDORI]) + "\n" + 
                "青:：" + f(detector.BaseColors[PuyoType.AO]) + "\n" + 
                "黄:：" + f(detector.BaseColors[PuyoType.KI]) + "\n" + 
                "紫:：" + f(detector.BaseColors[PuyoType.MURASAKI]);
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
            ((System.ComponentModel.ISupportInitialize)(this.fieldImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepIdTxt)).BeginInit();
            this.SuspendLayout();
            // 
            // spoitBtn
            // 
            this.spoitBtn.Enabled = false;
            this.spoitBtn.Location = new System.Drawing.Point(63, 33);
            this.spoitBtn.Name = "spoitBtn";
            this.spoitBtn.Size = new System.Drawing.Size(175, 63);
            this.spoitBtn.TabIndex = 1;
            this.spoitBtn.Text = "スポイト";
            this.spoitBtn.UseVisualStyleBackColor = true;
            this.spoitBtn.Click += new System.EventHandler(this.spoitBtn_Click);
            // 
            // captureBtn
            // 
            this.captureBtn.Location = new System.Drawing.Point(63, 127);
            this.captureBtn.Name = "captureBtn";
            this.captureBtn.Size = new System.Drawing.Size(175, 70);
            this.captureBtn.TabIndex = 2;
            this.captureBtn.Text = "キャプチャスクリーン";
            this.captureBtn.UseVisualStyleBackColor = true;
            this.captureBtn.Click += new System.EventHandler(this.captureBtn_Click);
            // 
            // fieldImg
            // 
            this.fieldImg.Location = new System.Drawing.Point(263, 33);
            this.fieldImg.Name = "fieldImg";
            this.fieldImg.Size = new System.Drawing.Size(192, 384);
            this.fieldImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fieldImg.TabIndex = 3;
            this.fieldImg.TabStop = false;
            this.fieldImg.Paint += new System.Windows.Forms.PaintEventHandler(this.fieldImg_Paint);
            this.fieldImg.MouseClick += new System.Windows.Forms.MouseEventHandler(this.fieldImg_MouseClick);
            // 
            // captureTimer
            // 
            this.captureTimer.Interval = 50;
            this.captureTimer.Tick += new System.EventHandler(this.captureTimer_Tick);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(61, 446);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(41, 12);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "説明文";
            // 
            // colorInfoLbl
            // 
            this.colorInfoLbl.AutoSize = true;
            this.colorInfoLbl.Location = new System.Drawing.Point(562, 33);
            this.colorInfoLbl.Name = "colorInfoLbl";
            this.colorInfoLbl.Size = new System.Drawing.Size(41, 12);
            this.colorInfoLbl.TabIndex = 5;
            this.colorInfoLbl.Text = "色情報";
            // 
            // FieldRadio1P
            // 
            this.FieldRadio1P.AutoSize = true;
            this.FieldRadio1P.Checked = true;
            this.FieldRadio1P.Location = new System.Drawing.Point(70, 209);
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
            this.FieldRadio2P.Location = new System.Drawing.Point(112, 209);
            this.FieldRadio2P.Name = "FieldRadio2P";
            this.FieldRadio2P.Size = new System.Drawing.Size(36, 16);
            this.FieldRadio2P.TabIndex = 6;
            this.FieldRadio2P.Text = "2P";
            this.FieldRadio2P.UseVisualStyleBackColor = true;
            // 
            // nextImg
            // 
            this.nextImg.Location = new System.Drawing.Point(477, 46);
            this.nextImg.Name = "nextImg";
            this.nextImg.Size = new System.Drawing.Size(32, 64);
            this.nextImg.TabIndex = 7;
            this.nextImg.TabStop = false;
            this.nextImg.Paint += new System.Windows.Forms.PaintEventHandler(this.nextImg_Paint);
            // 
            // playDateLbl
            // 
            this.playDateLbl.AutoSize = true;
            this.playDateLbl.Location = new System.Drawing.Point(490, 209);
            this.playDateLbl.Name = "playDateLbl";
            this.playDateLbl.Size = new System.Drawing.Size(29, 12);
            this.playDateLbl.TabIndex = 8;
            this.playDateLbl.Text = "日付";
            // 
            // playerNameLbl
            // 
            this.playerNameLbl.AutoSize = true;
            this.playerNameLbl.Location = new System.Drawing.Point(490, 236);
            this.playerNameLbl.Name = "playerNameLbl";
            this.playerNameLbl.Size = new System.Drawing.Size(54, 12);
            this.playerNameLbl.TabIndex = 9;
            this.playerNameLbl.Text = "プレイヤ名";
            // 
            // stepIdLbl
            // 
            this.stepIdLbl.AutoSize = true;
            this.stepIdLbl.Location = new System.Drawing.Point(490, 175);
            this.stepIdLbl.Name = "stepIdLbl";
            this.stepIdLbl.Size = new System.Drawing.Size(14, 12);
            this.stepIdLbl.TabIndex = 10;
            this.stepIdLbl.Text = "id";
            // 
            // playDate
            // 
            this.playDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.playDate.Location = new System.Drawing.Point(564, 204);
            this.playDate.Name = "playDate";
            this.playDate.Size = new System.Drawing.Size(200, 19);
            this.playDate.TabIndex = 11;
            // 
            // playerNameTxt
            // 
            this.playerNameTxt.Location = new System.Drawing.Point(564, 236);
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
            this.startBtn.Location = new System.Drawing.Point(63, 269);
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
            this.recordLbl.Location = new System.Drawing.Point(490, 269);
            this.recordLbl.Name = "recordLbl";
            this.recordLbl.Size = new System.Drawing.Size(41, 12);
            this.recordLbl.TabIndex = 16;
            this.recordLbl.Text = "レコード";
            // 
            // recordTxt
            // 
            this.recordTxt.Location = new System.Drawing.Point(564, 269);
            this.recordTxt.Name = "recordTxt";
            this.recordTxt.ReadOnly = true;
            this.recordTxt.Size = new System.Drawing.Size(200, 19);
            this.recordTxt.TabIndex = 17;
            // 
            // stepDataTxt
            // 
            this.stepDataTxt.Location = new System.Drawing.Point(498, 325);
            this.stepDataTxt.Multiline = true;
            this.stepDataTxt.Name = "stepDataTxt";
            this.stepDataTxt.Size = new System.Drawing.Size(266, 92);
            this.stepDataTxt.TabIndex = 18;
            // 
            // stepIdTxt
            // 
            this.stepIdTxt.Location = new System.Drawing.Point(564, 173);
            this.stepIdTxt.Name = "stepIdTxt";
            this.stepIdTxt.Size = new System.Drawing.Size(120, 19);
            this.stepIdTxt.TabIndex = 19;
            // 
            // stopBtn
            // 
            this.stopBtn.Enabled = false;
            this.stopBtn.Location = new System.Drawing.Point(63, 347);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(175, 70);
            this.stopBtn.TabIndex = 20;
            this.stopBtn.Text = "停止";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(793, 482);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        int pixelingTargetIndex;
        bool isPixeling = false;
        private void spoitBtn_Click(object sender, EventArgs e)
        {
            isPixeling = true;
            pixelingTargetIndex = (int)PuyoType.AKA;
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

            captureRect = f.GetCaptureRect();
            nextRect = f.GetNextRect();

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
        }

        private void captureTimer_Tick(object sender, EventArgs e)
        {
            if (!isPixeling)
            {
                Refresh();
            }
        }

        private bool IsCapturing { get; set; }

        private void fieldImg_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (!IsCapturing)
                {
                    return;
                }

                Graphics fieldImageG = e.Graphics;
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
                    fieldImageG.DrawImage(forAnalyzeBmp, dest, dest, GraphicsUnit.Pixel);

                    // フィールドの状態を解析し、結果を描画
                    CaptureField field = a(forAnalyzeBmp);
                    DrawDebugRect(fieldImageG, field);
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                throw exp;
            }
        }

        private void DrawDebugRect(Graphics g, CaptureField field)
        {
            using (Pen redPen = new Pen(Color.Red, 2))
            using (Pen greenPen = new Pen(Color.Green, 2))
            using (Pen bluePen = new Pen(Color.Blue, 2))
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

                        g.DrawRectangle(pen, rect);
                    }
                }
            }
        }

        private CaptureField a(Bitmap bmp)
        {
            CaptureField field = new CaptureField();
            RapidBitmapAccessor ba = new RapidBitmapAccessor(bmp);
            PuyoTypeDetector detector = new PuyoTypeDetector();
            ba.BeginAccess();
            for (int y = 0; y < CaptureField.Y_MAX; y++)
            {
                for (int x = 0; x < CaptureField.X_MAX; x++)
                {
                    field.SetPuyoType(x, y, detector.Detect(ba, field.GetRect(x, y)));
                }
            }

            //for (int y = 0; y < 1; y++)
            //{
            //    for (int x = 0; x < 1; x++)
            //    {
            //        field.SetPuyoType(x, y, DetectPuyoType(field.GetRect(x, y), ba));
            //    }
            //}
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
                using (Bitmap fieldBmp = new Bitmap(fieldImg.Width, fieldImg.Height))
                {
                    fieldImg.DrawToBitmap(fieldBmp, new Rectangle(0, 0, fieldImg.Width, fieldImg.Height));
                    RapidBitmapAccessor ba = new RapidBitmapAccessor(fieldBmp);

                    ba.BeginAccess();
                    Color c = ba.GetPixel(e.X, e.Y);
                    detector.BaseColors[(PuyoType)pixelingTargetIndex] = c;
                    ba.EndAccess();
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

            Func<Color, String> f = (c) =>
            {
                return "" + string.Format("{0:x2}{1:x2}{2:x2}", c.R, c.G, c.B);
            };

            colorInfoLbl.Text =
                "赤:：" + f(detector.BaseColors[PuyoType.AKA]) + "\n" +
                "緑:：" + f(detector.BaseColors[PuyoType.MIDORI]) + "\n" +
                "青:：" + f(detector.BaseColors[PuyoType.AO]) + "\n" +
                "黄:：" + f(detector.BaseColors[PuyoType.KI]) + "\n" +
                "紫:：" + f(detector.BaseColors[PuyoType.MURASAKI]);
        }

        private ColorPairPuyo prevNext;
        bool readyForNextStepRecord = false;
        bool isFirstTsumo = true;
        bool isRecording = false;
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
                            Console.WriteLine(next.Pivot + " " + next.Satellite);
                            prevNext = next;
                        }
                        readyForNextStepRecord = true;
                    }
                    else if (next.Pivot == PuyoType.NONE && next.Satellite == PuyoType.NONE)
                    {
                        if (!isFirstTsumo && readyForNextStepRecord)
                        {
                            // TODO: ここで、どこにおいたかを判定する。

                            steps.Add(prevNext);
                            readyForNextStepRecord = false;
                            FCodeEncoder encoder = new FCodeEncoder();
                            stepDataTxt.Text = encoder.Encode(steps);
                        }

                        isFirstTsumo = false;
                    }
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                throw exp;
            }
        }

        private CaptureField b(Bitmap bmp)
        {
            CaptureField field = new CaptureField();
            RapidBitmapAccessor ba = new RapidBitmapAccessor(bmp);
            PuyoTypeDetector detector = new PuyoTypeDetector();
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
            using (Pen bluePen = new Pen(Color.Blue, 2))
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
            isRecording = true;
            stopBtn.Enabled = true;

            readyForNextStepRecord = false;
            isFirstTsumo = true;
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            isRecording = false;
            stopBtn.Enabled = false;
        }
    }
}