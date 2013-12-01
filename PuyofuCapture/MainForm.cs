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
    /// <summary>
    /// メインフォーム
    /// </summary>
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

        /// <summary>ぷよ種別検出機</summary>
        private PuyoTypeDetector detector = new PuyoTypeDetector();
        
        /// <summary>キャプチャ領域</summary>
        private CaptureRects captureRects;

        /// <summary>プレイヤ名テキスト</summary>
        private TextBox[] playerNameTxts;

        /// <summary>タグテキスト</summary>
        private TextBox[] tagsTxts;

        /// <summary>レコードテキスト</summary>
        private TextBox[] recordTxts;

        /// <summary>サンプル画像テーブル</summary>
        private IDictionary<PuyoType, PictureBox> sampleImgs;

        /// <summary>ぷよ画像サンプラ</summary>
        private Sampler sampler = new Sampler();

        /// <summary>キャプチャフォーム</summary>
        CaptureForm captureForm = new CaptureForm();

        /// <summary>FPS計算機</summary>
        FpsCalculator fpsCalculator = new FpsCalculator();

        /// <summary>スクリーンキャプチャ用BMP</summary>
        private Bitmap screenBmp;

        /// <summary>ぷよ譜レコーダ</summary>
        PuyofuRecorder[] recorders = new PuyofuRecorder[2] { new PuyofuRecorder(), new PuyofuRecorder() };

        /// <summary>マウスが上にあるフィールド番号</summary>
        private int fieldNoMouseIsOn = -1;

        /// <summary>フィールド上のマウスの位置</summary>
        private Point pointOnFieldImg;

        /// <summary>
        /// コンストラクタ
        /// </summary>
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

        /// <summary>
        /// 処理対象フィールドラジオボタンを選択する
        /// </summary>
        /// <param name="targetFieldValue">選択対象のラジオボタンの値</param>
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

        /// <summary>
        /// 全サンプル画像の初期設定を行う
        /// </summary>
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

        /// <summary>
        /// サンプル画像の初期設定を行う
        /// </summary>
        /// <param name="puyoType">ぷよ種別</param>
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

        /// <summary>
        /// サンプリングボタンをクリックした
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void samplingBtn_Click(object sender, EventArgs e)
        {
            sampler.Begin();
            statusLabel.Text = sampler.GetText();
        }

        /// <summary>
        /// キャプチャスクリーンボタンをクリックした
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void captureBtn_Click(object sender, EventArgs e)
        {
            if (captureForm.IsCapturing)
            {
                captureForm.Close();
            }

            else
            {
                captureBtn.Text = "選択中止";
                captureTimer.Stop();
                IsCapturing = false;
                captureForm.IsCapturing = true;
                captureForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptureForm_FormClosed);
                captureForm.Show();
            }
        }

        /// <summary>
        /// キャプチャフォームが閉じられた
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CaptureForm f = sender as CaptureForm;
            if (f == null)
            {
                return;
            }

            if (f.IsCaptureEnd)
            {
                // キャプチャ範囲が選択された場合
                config.CaptureRect = f.CaptureRects.CaptureRect;
                config.Save();

                captureRects = f.CaptureRects;
                BeginCapturing();
            }
            else if (config.CaptureRect.Top > 0
                && config.CaptureRect.Left > 0
                && config.CaptureRect.Width > 0
                && config.CaptureRect.Height > 0)
            {
                // キャプチャ範囲が選択されなかった場合でも、以前の選択範囲があるならキャプチャを開始
                BeginCapturing();
            }

            captureBtn.Text = "キャプチャスクリーン";
            captureForm = new CaptureForm();
        }

        /// <summary>
        /// キャプチャを開始する
        /// </summary>
        private void BeginCapturing()
        {
            IsCapturing = true;
            samplingBtn.Enabled = true;
            startBtn.Enabled = true;
            captureTimer.Start();

            if (screenBmp != null)
            {
                screenBmp.Dispose();
            }
            screenBmp = new Bitmap(captureRects.CaptureRect.Width, captureRects.CaptureRect.Height);
        }

        /// <summary>
        /// キャプチャタイマー処理
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
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

        /// <summary>キャプチャ処理中かどうか</summary>
        private bool IsCapturing { get; set; }

        /// <summary>一つ前のフィールド状態</summary>
        private CaptureField[] prevFields = new CaptureField[2]{ new CaptureField(), new CaptureField() };

        /// <summary>現在のフィールド状態</summary>
        private CaptureField[] curFields = new CaptureField[2] { new CaptureField(), new CaptureField() };

        /// <summary>
        /// フィールドの再描画処理
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        /// <param name="fieldNo">フィールド番号</param>
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

                if (!sampler.IsSampling)
                {
                    // フィールドの状態を解析し、結果を描画
                    curFields[fieldNo] = AnalyzeField(forAnalyzeBmp);
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

        /// <summary>
        /// 画面項目からフィールド番号を取得する
        /// </summary>
        /// <param name="control">画面項目</param>
        private int GetFieldNumberFromControl(Control control)
        {
            return int.Parse(control.Name.Substring(control.Name.Length - 1)) - 1;
        }

        /// <summary>
        /// 指定したフィールドが処理対象フィールドかどうかを判定する
        /// </summary>
        /// <param name="fieldNo">フィールド番号</param>
        /// <returns>処理対象フィールドか</returns>
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

        /// <summary>
        /// フィールドの再描画処理
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
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

        /// <summary>
        /// デバッグ枠を表示する
        /// </summary>
        /// <param name="g">描画先グラフィックオブジェクト</param>
        /// <param name="field">フィールド状態</param>
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

        /// <summary>
        /// フィールド状態を解析する
        /// </summary>
        /// <param name="bmp">解析対象画像</param>
        /// <returns>フィールド状態</returns>
        private CaptureField AnalyzeField(Bitmap bmp)
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

        /// <summary>
        /// メインフォームが閉じられた
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 現在の画面の情報を設定ファイルに保存
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

        /// <summary>
        /// 処理対象フィールドラジオボタンの値を取得する
        /// </summary>
        /// <returns>処理対象フィールドラジオボタンの値</returns>
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

        /// <summary>
        /// フィールドがクリックされた
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        /// <param name="fieldNo">フィールド番号</param>
        private void ClickField(object sender, MouseEventArgs e, int fieldNo)
        {
            if (!sampler.IsSampling)
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
                    PuyoType puyoType = sampler.GetSamplingType();

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
                    sampler.SaveSample(cellBmp);
                }
            }

            sampler.Proceed();
            statusLabel.Text = sampler.GetText();
        }

        /// <summary>
        /// フィールドがクリックされた
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
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

        /// <summary>
        /// ネクストの再描画処理
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        /// <param name="fieldNo">フィールド番号</param>
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
                CaptureField field = AnalyzeNext(forAnalyzeBmp);
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

        /// <summary>
        /// ネクストの再描画処理
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
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

        /// <summary>
        /// 譜情報を更新する
        /// </summary>
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

        /// <summary>
        /// 譜情報を取得する
        /// </summary>
        /// <param name="fieldNo">フィールド番号</param>
        /// <param name="idCount">IDカウンタ</param>
        /// <returns>譜情報</returns>
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
            string text = @"  {
    date: '#date',
    id: '#id',
    record: '#record',
    tags: [#tags],
  },
";
            text = text.Replace("#date", playDate.Text);
            text = text.Replace("#id", (stepIdTxt.Value + idCount).ToString());
            text = text.Replace("#record", recordTxts[fieldNo].Text);
            text = text.Replace("#tags", sb.ToString());

            idCount++;
            return text;
        }

        /// <summary>
        /// ネクスト画像を解析する
        /// </summary>
        /// <param name="bmp">解析する画像</param>
        /// <returns>ネクスト状態</returns>
        private CaptureField AnalyzeNext(Bitmap bmp)
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

        /// <summary>
        /// ネクストのデバッグ枠を表示する
        /// </summary>
        /// <param name="g">グラフィックオブジェクト</param>
        /// <param name="field">ネクスト状態</param>
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

        /// <summary>
        /// 開始ボタンをクリックした
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
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

        /// <summary>
        /// やりなおすボタンをクリックした
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            cancelBtn.Enabled = false;
            statusLabel.Text = "";

            recorders[0] = new PuyofuRecorder();
            recorders[1] = new PuyofuRecorder();
        }

        /// <summary>
        /// フィールド上でマウスが移動した
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void fieldImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (!sampler.IsSampling)
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

        /// <summary>
        /// 閾値バーがスクロールされた
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void similarityValueBar_Scroll(object sender, EventArgs e)
        {
            detector.SimilarityThreshold = similarityValueBar.Value;
            similarityValueLbl.Text = similarityValueBar.Value.ToString();
            config.SimilarityThreshold = similarityValueBar.Value;
            config.Save();
        }

        /// <summary>
        /// 譜情報テキストでキー入力された
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void stepDataTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.A & e.Control == true)
            {
                // Ctrl + Aでテキストを全選択
                stepDataTxt.SelectAll();
            }
        }

        /// <summary>
        /// 閉じるメニューがクリックされた
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void MMFileEndMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 設定メニューがクリックされた
        /// </summary>
        /// <param name="sender">イベント発生源</param>
        /// <param name="e">イベント情報</param>
        private void MMConfigurationMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurationForm form = new ConfigurationForm(config);
            DialogResult result = form.ShowDialog(this);
        }
    }
}
