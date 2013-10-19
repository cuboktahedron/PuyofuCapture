using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using CaptureTest;
using Cubokta;
using System.Collections.Generic;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        // エントリ・ポイント（1）
        [STAThread]
        static void Main()
        {
            Application.Run(new Form1());
        }

        public Form1()
        {
            InitializeComponent();
            status.Text = "";

            Func<Color, String> f = (c) =>
            {
                return "" + string.Format("{0:x2}{1:x2}{2:x2}", c.R, c.G, c.B);
            };

            colorInfo.Text =
                "赤:：" + f(baseColors[PuyoType.AKA]) + "\n" + 
                "緑:：" + f(baseColors[PuyoType.MIDORI]) + "\n" + 
                "青:：" + f(baseColors[PuyoType.AO]) + "\n" + 
                "黄:：" + f(baseColors[PuyoType.KI]) + "\n" + 
                "紫:：" + f(baseColors[PuyoType.MURASAKI]);
        }

        private Button spoitBtn;
        private Button captureBtn;
        private PictureBox fieldImage;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.IContainer components;
        private Label status;
        private Label colorInfo;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private PictureBox nextImage;
        private Label label1;
        private Label label2;
        private Label label3;
        private DateTimePicker playDate;
        private TextBox playerTxt;
        private Button startBtn;
        private Label label4;
        private TextBox record;
        private TextBox stepDataTxt;
        private NumericUpDown stepIdTxt;
        private Button stopBtn;
        private Splitter splitter1;

        private void button1_Click_1(object sender, EventArgs e)
        {
            // 画像のサイズを指定し、Bitmapオブジェクトのインスタンスを作成
            Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            // Bitmap bm = new Bitmap(500, 300);   // 幅500ピクセル × 高さ300ピクセルの場合

            // Graphicsオブジェクトのインスタンスを作成
            Graphics gr = Graphics.FromImage(bm);

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            // 画面全体をコピー
            gr.CopyFromScreen(new Point(0, 0), new Point(0, 0), bm.Size);

            sw.Stop();
            //結果を表示する
            // (240×120で、約25ms)
            // (480×240で、約30ms)
            // (1280×1024で、約50ms)
            Console.WriteLine(sw.Elapsed);

            //// PNGで保存
            //bm.Save("C:\\samplePNG.png", System.Drawing.Imaging.ImageFormat.Png);
            // BMPで保存
            bm.Save("D:\\sampleBMP.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            //// JPGで保存
            //bm.Save("C:\\sampleJPG.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //// TIFFで保存
            //bm.Save("C:\\sampleTIFF.tiff", System.Drawing.Imaging.ImageFormat.Tiff);

            gr.Dispose();

            // Bitmap#GetPixel版
            const int TIMES = 300000;
            sw.Restart();
            for (int i = 0; i < TIMES; i++)
            {
                Color c = bm.GetPixel(100, 100);
            }
            sw.Stop();

            //結果を表示する(30000callで約60ms)
            Console.WriteLine("methodA:" + sw.Elapsed);

            // System.Runtime.InteropServices.Marshalを使用する方法
            BitmapPlusA ba = new BitmapPlusA(bm);
            ba.BeginAccess();
            sw.Restart();
            for (int i = 0; i < TIMES; i++)
            {
                Color c = ba.GetPixel(100, 100);
            }
            ba.EndAccess();
            sw.Stop();

            //結果を表示する(30000callで約60ms)
            Console.WriteLine("methodB:" + sw.Elapsed);

            // アンセーフコード(unsafe)を使用する方法
            // System.Runtime.InteropServices.Marshalを使用する方法
            RapidBitmapAccessor bb = new RapidBitmapAccessor(bm);
            bb.BeginAccess();
            sw.Restart();
            for (int i = 0; i < TIMES; i++)
            {
                Color c = bb.GetPixel(100, 100);
            }
            bb.EndAccess();
            sw.Stop();

            //結果を表示する(30000callで約60ms)
            Console.WriteLine("methodC:" + sw.Elapsed);

            //            MessageBox.Show("Dドライブ直下に出力しました");
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.spoitBtn = new System.Windows.Forms.Button();
            this.captureBtn = new System.Windows.Forms.Button();
            this.fieldImage = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.status = new System.Windows.Forms.Label();
            this.colorInfo = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.nextImage = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.playDate = new System.Windows.Forms.DateTimePicker();
            this.playerTxt = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.startBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.record = new System.Windows.Forms.TextBox();
            this.stepDataTxt = new System.Windows.Forms.TextBox();
            this.stepIdTxt = new System.Windows.Forms.NumericUpDown();
            this.stopBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fieldImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImage)).BeginInit();
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
            this.spoitBtn.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // captureBtn
            // 
            this.captureBtn.Location = new System.Drawing.Point(63, 127);
            this.captureBtn.Name = "captureBtn";
            this.captureBtn.Size = new System.Drawing.Size(175, 70);
            this.captureBtn.TabIndex = 2;
            this.captureBtn.Text = "キャプチャスクリーン";
            this.captureBtn.UseVisualStyleBackColor = true;
            this.captureBtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // fieldImage
            // 
            this.fieldImage.Location = new System.Drawing.Point(263, 33);
            this.fieldImage.Name = "fieldImage";
            this.fieldImage.Size = new System.Drawing.Size(192, 384);
            this.fieldImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fieldImage.TabIndex = 3;
            this.fieldImage.TabStop = false;
            this.fieldImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.fieldImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Location = new System.Drawing.Point(61, 446);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(41, 12);
            this.status.TabIndex = 4;
            this.status.Text = "説明文";
            // 
            // colorInfo
            // 
            this.colorInfo.AutoSize = true;
            this.colorInfo.Location = new System.Drawing.Point(562, 33);
            this.colorInfo.Name = "colorInfo";
            this.colorInfo.Size = new System.Drawing.Size(41, 12);
            this.colorInfo.TabIndex = 5;
            this.colorInfo.Text = "色情報";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(70, 209);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(36, 16);
            this.radioButton1.TabIndex = 6;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "1P";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AccessibleName = "";
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(112, 209);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(36, 16);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.Text = "2P";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // nextImage
            // 
            this.nextImage.Location = new System.Drawing.Point(477, 46);
            this.nextImage.Name = "nextImage";
            this.nextImage.Size = new System.Drawing.Size(32, 64);
            this.nextImage.TabIndex = 7;
            this.nextImage.TabStop = false;
            this.nextImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(490, 209);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "日付";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(490, 236);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "プレイヤ名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(490, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "id";
            // 
            // playDate
            // 
            this.playDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.playDate.Location = new System.Drawing.Point(564, 204);
            this.playDate.Name = "playDate";
            this.playDate.Size = new System.Drawing.Size(200, 19);
            this.playDate.TabIndex = 11;
            // 
            // playerTxt
            // 
            this.playerTxt.Location = new System.Drawing.Point(564, 236);
            this.playerTxt.Name = "playerTxt";
            this.playerTxt.Size = new System.Drawing.Size(200, 19);
            this.playerTxt.TabIndex = 13;
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(490, 269);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "レコード";
            // 
            // record
            // 
            this.record.Location = new System.Drawing.Point(564, 269);
            this.record.Name = "record";
            this.record.ReadOnly = true;
            this.record.Size = new System.Drawing.Size(200, 19);
            this.record.TabIndex = 17;
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
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(793, 482);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.stepIdTxt);
            this.Controls.Add(this.stepDataTxt);
            this.Controls.Add(this.record);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.playerTxt);
            this.Controls.Add(this.playDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nextImage);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.colorInfo);
            this.Controls.Add(this.status);
            this.Controls.Add(this.fieldImage);
            this.Controls.Add(this.captureBtn);
            this.Controls.Add(this.spoitBtn);
            this.Name = "Form1";
            this.Text = "movi";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.fieldImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepIdTxt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        int pixelingTargetIndex;
        bool isPixeling = false;
        private void button2_Click_1(object sender, EventArgs e)
        {
            isPixeling = true;
            pixelingTargetIndex = (int)PuyoType.AKA;
            status.Text = (PuyoType)pixelingTargetIndex + "のサンプルピクセルをクリックしてください。右クリックでスキップします。";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            IsCapturing = false;

            int fieldNo = radioButton1.Checked ? 0 : 1;
            Form captureForm = new CaptureForm(fieldNo);
            captureForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptureForm_FormClosed);
            captureForm.Show();
        }

        private Rectangle captureRect;
        private Bitmap captureBm;
        private Rectangle nextRect;
        private Bitmap nextBm;

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
            timer1.Start();

            if (captureBm != null)
            {
                captureBm.Dispose();
            }
            captureBm = new Bitmap(captureRect.Width, captureRect.Height);

            if (nextBm != null)
            {
                nextBm.Dispose();
            }
            captureBm = new Bitmap(captureRect.Width, captureRect.Height);
            nextBm = new Bitmap(nextRect.Width, nextRect.Height);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!isPixeling)
            {
                Refresh();
            }
        }

        private bool IsCapturing { get; set; }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!IsCapturing)
            {
                return;
            }
            Graphics g = e.Graphics;
            using (Graphics gg = Graphics.FromImage(captureBm))
            using (Bitmap bmp = new Bitmap(fieldImage.Width, fieldImage.Height))
            using (Graphics ggg = Graphics.FromImage(bmp))
            {
                gg.CopyFromScreen(new Point(captureRect.Left, captureRect.Top), new Point(0, 0), captureBm.Size);
                Rectangle dest = new Rectangle(0, 0, 192, 384);
                Rectangle src = new Rectangle(0, 0, captureRect.Width, captureRect.Height);
                ggg.DrawImage(captureBm, dest, src, GraphicsUnit.Pixel);
                g.DrawImage(bmp, dest, dest, GraphicsUnit.Pixel);

                CaptureField field = a(bmp);
                DrawDebugRect(g, field);
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
            ba.BeginAccess();
            for (int y = 0; y < CaptureField.Y_MAX; y++)
            {
                for (int x = 0; x < CaptureField.X_MAX; x++)
                {
                    field.SetPuyoType(x, y, DetectPuyoType(field.GetRect(x, y), ba));
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

        private PuyoType DetectPuyoType(Rectangle rect, RapidBitmapAccessor ba)
        {
            int r = 0;
            int g = 0;
            int b = 0;
            IDictionary<PuyoType, int> typeCounts = new Dictionary<PuyoType, int> {
                { PuyoType.AKA, 0 },
                { PuyoType.MIDORI, 0 },
                { PuyoType.AO, 0 },
                { PuyoType.KI, 0 },
                { PuyoType.MURASAKI, 0 },
            };

            rect = new Rectangle()
            {
                X = rect.X + CaptureField.UNIT / 4,
                Y = rect.Y + CaptureField.UNIT / 4,
                Width = rect.Width / 2,
                Height = rect.Height / 2,
            };

            for (int y = rect.Top; y < rect.Top + rect.Height; y++)
            {
                for (int x = rect.Left; x < rect.Left + rect.Width; x++)
                {
                    Color c = ba.GetPixel(x, y);
                    r = (int)c.R;
                    g = (int)c.G;
                    b = (int)c.B;
                    Color bc = Color.FromArgb(r, g, b);
                    int diffValueOfRed = DetectColorDiff(bc, baseColors[PuyoType.AKA]);
                    int diffValueOfGreen = DetectColorDiff(bc, baseColors[PuyoType.MIDORI]);
                    int diffValueOfBlue = DetectColorDiff(bc, baseColors[PuyoType.AO]);
                    int diffValueOfYellow = DetectColorDiff(bc, baseColors[PuyoType.KI]);
                    int diffValueOfPurple = DetectColorDiff(bc, baseColors[PuyoType.MURASAKI]);

                    int[] diffs = new int[]
                    {
                        diffValueOfRed,
                        diffValueOfGreen,
                        diffValueOfBlue,
                        diffValueOfYellow,
                        diffValueOfPurple,
                    };

                    PuyoType type = GetPuyoType(diffs);
                    if (type != PuyoType.NONE)
                    {
                        typeCounts[type]++;
                    }
                }
            }

            if (typeCounts.Max(pair => pair.Value) < 32)
            {
                return PuyoType.NONE;
            }
            PuyoType p = (from n in typeCounts
                          where n.Value == (typeCounts.Max(pair => pair.Value))
                          select n.Key).First();
            return p;
        }

        private const int COLOR_THRESHOLD = 32 * 32 * 3;
        private PuyoType GetPuyoType(int[] diffs)
        {
            PuyoType[] TYPES = new PuyoType[]
            {
                PuyoType.AKA,
                PuyoType.MIDORI,
                PuyoType.AO,
                PuyoType.KI,
                PuyoType.MURASAKI,
            };

            int minIndex = 0;
            int minValue = int.MaxValue;
            for (int i = 0; i < diffs.Length; i++)
            {
                if (diffs[i] < minValue)
                {
                    minIndex = i;
                    minValue = diffs[i];
                }
            }

            if (minValue > COLOR_THRESHOLD)
            {
                return PuyoType.NONE;
            }
            else
            {
                return TYPES[minIndex];
            }
        }

        private int DetectColorDiff(Color c1, Color c2)
        {
            int diffRed = (c1.R - c2.R);
            int diffGreen = (c1.G - c2.G);
            int diffBlue = (c1.B - c2.B);

            return diffRed * diffRed + diffGreen * diffGreen + diffBlue * diffBlue;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (captureBm != null)
            {
                captureBm.Dispose();
            }

            if (nextBm != null)
            {
                nextBm.Dispose();
            }
        }

        private IDictionary<PuyoType, Color> baseColors = new Dictionary<PuyoType, Color>()
        {
            { PuyoType.AKA, Color.FromArgb(0xaf, 0x74, 0x85) },
            { PuyoType.MIDORI, Color.FromArgb(0x53, 0x9f, 0x37) },
            { PuyoType.AO, Color.FromArgb(0x11, 0x36, 0x7a) },
            { PuyoType.KI, Color.FromArgb(0x95, 0x5c, 0x00) },
            { PuyoType.MURASAKI, Color.FromArgb(0x65, 0x05, 0x6c) },
        };

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (!isPixeling)
                {
                    return;
                }

                if (e.Button == MouseButtons.Left)
                {
                    using (Bitmap bmp = new Bitmap(fieldImage.Width, fieldImage.Height))
                    {
                        fieldImage.DrawToBitmap(bmp, new Rectangle(0, 0, fieldImage.Width, fieldImage.Height));
                        RapidBitmapAccessor ba = new RapidBitmapAccessor(bmp);

                        ba.BeginAccess();
                        Color c = ba.GetPixel(e.X, e.Y);
                        baseColors[(PuyoType)pixelingTargetIndex] = c;
                        ba.EndAccess();
                    }
                }

                pixelingTargetIndex++;
                if (pixelingTargetIndex > (int)PuyoType.MURASAKI)
                {
                    isPixeling = false;
                }
                else
                {
                    status.Text = (PuyoType)pixelingTargetIndex + "のサンプルピクセルをクリックしてください。右クリックでスキップします。";
                }

                Func<Color, String> f = (c) =>
                {
                    return "" + string.Format("{0:x2}{1:x2}{2:x2}", c.R, c.G, c.B);
                };

                colorInfo.Text =
                    "赤:：" + f(baseColors[PuyoType.AKA]) + "\n" +
                    "緑:：" + f(baseColors[PuyoType.MIDORI]) + "\n" +
                    "青:：" + f(baseColors[PuyoType.AO]) + "\n" +
                    "黄:：" + f(baseColors[PuyoType.KI]) + "\n" +
                    "紫:：" + f(baseColors[PuyoType.MURASAKI]);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }
        }

        private ColorPairPuyo prevNext;
        bool readyForNextStepRecord = false;
        bool isFirstTsumo = true;
        bool isRecording = false;
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (!IsCapturing)
                {
                    return;
                }
                Graphics g = e.Graphics;
                using (Graphics gg = Graphics.FromImage(nextBm))
                using (Bitmap bmp = new Bitmap(nextImage.Width, nextImage.Height))
                using (Graphics ggg = Graphics.FromImage(bmp))
                {
                    gg.CopyFromScreen(new Point(nextRect.Left, nextRect.Top), new Point(0, 0), nextBm.Size);
                    Rectangle dest = new Rectangle(0, 0, 32, 64);
                    Rectangle src = new Rectangle(0, 0, nextRect.Width, nextRect.Height);
                    ggg.DrawImage(nextBm, dest, src, GraphicsUnit.Pixel);
                    g.DrawImage(bmp, dest, dest, GraphicsUnit.Pixel);

                    CaptureField field = b(bmp);
                    DrawDebugNextRect(g, field);

                    ColorPairPuyo next = field.GetNext(0);
//                    Console.WriteLine(next.Pivot + " " + next.Satellite);

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
                Console.WriteLine(exp.ToString());
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
                pp[y] = DetectPuyoType(field.GetNextRect(0, y), ba);
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