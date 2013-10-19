using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace Cubokta.Puyo
{
    /// <summary>
    /// キャプチャ範囲指定用フォーム
    /// </summary>
    public partial class CaptureForm : Form
    {
        /// <summary>スクリーン画像表示用ピクチャボックス</summary>
        private PictureBox screenPict;

        /// <summary>
        /// キャプチャ開始時点の画面イメージ
        /// </summary>
        private Bitmap rawScreenImage;

        /// <summary>選択範囲の始点</summary>
        private Point startPoint;

        /// <summary>選択範囲の終点</summary>
        private Point endPoint;

        /// <summary>フィールド番号</summary>
        private int fieldNo;

        /// <summary>選択範囲の1ブロックの幅</summary>
        private float xUnit;

        /// <summary>選択範囲の1ブロックの高さ</summary>
        private float yUnit;

        /// <summary>キャプチャ範囲指定が開始されているかどうか</summary>
        public bool IsSelecting { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fieldNo">フィールド版の具</param>
        public CaptureForm(int fieldNo)
        {
            this.fieldNo = fieldNo;
            InitializeComponent();
        }

        /// <summary>
        /// デザイナが生成したコード
        /// </summary>
        private void InitializeComponent()
        {
            this.screenPict = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.screenPict)).BeginInit();
            this.SuspendLayout();
            // 
            // screenPict
            // 
            this.screenPict.Location = new System.Drawing.Point(0, 0);
            this.screenPict.Name = "screenPict";
            this.screenPict.Size = new System.Drawing.Size(37, 37);
            this.screenPict.TabIndex = 0;
            this.screenPict.TabStop = false;
            this.screenPict.Paint += new System.Windows.Forms.PaintEventHandler(this.screenPict_Paint);
            this.screenPict.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pict_MouseDown);
            this.screenPict.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pict_MouseMove);
            // 
            // CaptureForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 356);
            this.Controls.Add(this.screenPict);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CaptureForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptureForm_FormClosed);
            this.Load += new System.EventHandler(this.CaptureForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.screenPict)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// フォームのロードイベント
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void CaptureForm_Load(object sender, EventArgs e)
        {
            // プライマリスクリーンと同じ幅、高さにフォームと、ピクチャボックスを拡張
            Size screenSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.Size = screenSize;
            screenPict.Size = screenSize;

            // 画面全体をキャプチャ
            rawScreenImage = new Bitmap(screenSize.Width, screenSize.Height);
            using (Graphics g = Graphics.FromImage(rawScreenImage))
            {
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), rawScreenImage.Size);
            }
        }

        /// <summary>
        /// マウスダウンイベント
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void pict_MouseDown(object sender, MouseEventArgs e)
        {
            if (!IsSelecting && e.Button != MouseButtons.Left)
            {
                return;
            }

            if (!IsSelecting)
            {
                // キャプチャ範囲指定開始
                startPoint = new Point(e.X, e.Y);
                endPoint = new Point(e.X, e.Y);
                xUnit = 0f;
                yUnit = 0f;

                IsSelecting = true;

                Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                // キャンセル
                IsSelecting = false;
                Refresh();
            }
            else if (e.Button == MouseButtons.Left)
            {
                // キャプチャ範囲指定終了
                endPoint = new Point(e.X, e.Y);

                int captureWidth = Math.Abs(endPoint.X - startPoint.X);
                int captureHeight = Math.Abs(endPoint.Y - startPoint.Y);
                xUnit = captureWidth / (X_BLOCK_NUM);
                yUnit = captureHeight / Y_BLOCK_NUM;
                this.Close();
            }
        }

        /// <summary>
        /// マウス移動イベント
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void pict_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsSelecting)
            {
                return;
            }

            endPoint = new Point(e.X, e.Y);
            int captureWidth = Math.Abs(endPoint.X - startPoint.X);
            int captureHeight = Math.Abs(endPoint.Y - startPoint.Y);
            xUnit = captureWidth / (X_BLOCK_NUM);
            yUnit = captureHeight / Y_BLOCK_NUM;

            Refresh();
        }


        /// <summary>
        /// キャプチャ範囲を取得する
        /// </summary>
        /// <returns>
        /// キャプチャ範囲
        /// </returns>
        public Rectangle GetCaptureRect()
        {
            if (!IsSelecting)
            {
                return new Rectangle(0, 0, 0, 0);
            }

            int captureWidth = Math.Abs(endPoint.X - startPoint.X);
            int captureHeight = Math.Abs(endPoint.Y - startPoint.Y);
            int left = Math.Min(startPoint.X, endPoint.X);
            int top = Math.Min(startPoint.Y, endPoint.Y);

            return new Rectangle(left, top, (int)(xUnit * 6), captureHeight);
        }

        /// <summary>
        /// ネクスト範囲を取得する
        /// </summary>
        /// <returns>
        /// ネクスト範囲
        /// </returns>
        public Rectangle GetNextRect()
        {
            if (!IsSelecting)
            {
                return new Rectangle(0, 0, 0, 0);
            }

            int captureWidth = Math.Abs(endPoint.X - startPoint.X);
            int captureHeight = Math.Abs(endPoint.Y - startPoint.Y);
            int left = Math.Min(startPoint.X, endPoint.X);
            int top = Math.Min(startPoint.Y, endPoint.Y);

            int nextLeft;
            int nextTop = top + (int)(yUnit * 2);

            if (fieldNo == 0)
            {
                nextLeft = left + captureWidth + (int)xUnit;
            }
            else
            {
                nextLeft = left - (int)(xUnit * 2);
            }

            return new Rectangle(nextLeft, nextTop, (int)xUnit, (int)(yUnit * 2));
        }

        private const int X_BLOCK_NUM = 6;
        private const int Y_BLOCK_NUM = 12;

        /// <summary>
        /// 画面を再描画
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void screenPict_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            using (Bitmap bm = new Bitmap(rawScreenImage))
            {
                // オリジナルのスクリーン画像を描画
                g.DrawImage(rawScreenImage, new Point(0, 0));

                if (!IsSelecting)
                {
                    return;
                }

                // 選択範囲を描画
                Rectangle rect = GetCaptureRect();
                Rectangle nextRect = GetNextRect();

                using (Pen pen = new Pen(System.Drawing.Color.Red, 2))
                {
                    for (int y = 0; y <= Y_BLOCK_NUM; y++)
                    {
                        float yy = yUnit * y + rect.Top;
                        g.DrawLine(pen, rect.Left, yy, rect.Left + xUnit * X_BLOCK_NUM, yy);
                    }

                    for (int x = 0; x <= X_BLOCK_NUM; x++)
                    {
                        float xx = xUnit * x + rect.Left;
                        g.DrawLine(pen, xx, rect.Top, xx, rect.Top + yUnit * Y_BLOCK_NUM);
                    }

                    for (int y = 0; y <= 2; y++)
                    {
                        float yy = yUnit * y + nextRect.Top;
                        g.DrawLine(pen, nextRect.Left, yy, nextRect.Left + nextRect.Width, yy);
                    }

                    for (int x = 0; x <= 1; x++)
                    {
                        float xx = xUnit * x + nextRect.Left;
                        g.DrawLine(pen, xx, nextRect.Top, xx, nextRect.Top + nextRect.Height);
                    }
                }
            }
        }

        /// <summary>
        /// フォームが閉じた
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            rawScreenImage.Dispose();
        }
    }
}
