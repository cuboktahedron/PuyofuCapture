/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/blob/master/license/LICENSE-MIT.txt
 */
using Cubokta.Puyo.Common;
using log4net;
using System.Collections.Generic;
using System.Linq;

namespace Cubokta.Puyo
{
    /// <summary>
    /// レコード処理の結果
    /// </summary>
    public enum RecordResult
    {
        NOT_RECORDING,
        RECORDING,
        RECORD_SUCCESS,
        RECORD_FAILURE,
        RECORD_FORWARD,
        RECORD_ENDED,
    }

    /// <summary>
    /// ぷよ譜レコーダ
    /// </summary>
    public class PuyofuRecorder
    {
        /// <summary>ロガー</summary>
        private static readonly ILog LOGGER =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>譜情報</summary>
        private Steps steps = new Steps();

        /// <summary>現在のツモ</summary>
        private ColorPairPuyo curNext;

        /// <summary>前回のツモ</summary>
        private ColorPairPuyo prevNext;

        /// <summary>次の譜のレコード処理フラグ１</summary>
        private bool isReadyForNextStepRecord;

        /// <summary>次の譜のレコード処理フラグ２</summary>
        private bool isReadyForNextStepRecord2;

        /// <summary>レコード中かどうか</summary>
        private bool isRecording;

        /// <summary>レコードが終了したかどうか</summary>
        public bool IsRecordEnded { get; set; }

        /// <summary>レコードが成功したかどうか</summary>
        public bool IsRecordSucceeded { get; set; }

        /// <summary>レコードが失敗したかどうか</summary>
        public bool IsRecordFailed { get; set; }

        /** キャプチャする手数 */
        private int captureStepNum;

        /// <summary>キャプチャ失敗カウンタ</summary>
        private int captureFailCount;

        /// <summary>ネクストツモ決定に使用するワーク用ディクショナリ</summary>
        private IDictionary<ColorPairPuyo, int> currents = new Dictionary<ColorPairPuyo, int>();

        /// <summary>前回までのフィールド状態</summary>
        private CaptureField prevField = new CaptureField();

        /// <summary>キャプチャ間隔(ms)</summary>
        private int captureInterval;

        /// <summary>ツモのみキャプチャするかどうか</summary>
        private bool captureOnlyTsumo;

        /// <summary>
        /// レコードを開始する
        /// </summary>
        /// <param name="captureInterval">キャプチャ間隔(ms)</param>
        /// <param name="captureStepNum">キャプチャする手数</param>
        /// <param name="captureOnlyTsumo">ツモのみキャプチャするかどうか</param>
        /// TODO: 現状はかなりエラー検出までに時間がかかっている。
        public void BeginRecord(int captureInterval, int captureStepNum, bool captureOnlyTsumo)
        {
            isRecording = true;
            this.captureInterval = captureInterval;
            this.captureStepNum = captureStepNum;
            this.captureOnlyTsumo = captureOnlyTsumo;
        }

        /// <summary>
        /// ぷよ譜のレコード状況を進める
        /// </summary>
        /// <param name="curField">現在のフィールドの状態</param>
        /// <param name="next">ネクスト組ぷよ</param>
        /// <returns>処理結果</returns>
        public RecordResult DoNext(CaptureField curField, ColorPairPuyo next)
        {
            if (IsRecordEnded)
            {
                return RecordResult.RECORD_ENDED;
            }

            if (!isRecording)
            {
                return RecordResult.NOT_RECORDING;
            }

            if (steps.Count >= captureStepNum)
            {
                isRecording = false;
                IsRecordSucceeded = true;
                IsRecordEnded = true;
                return RecordResult.RECORD_SUCCESS;
            }

            if (next.Pivot != PuyoType.NONE && next.Satellite != PuyoType.NONE)
            {
                // ネクストぷよがネクスト領域にセットされ待ち状態になっている時の処理

                if (!currents.ContainsKey(next))
                {
                    currents[next] = 0;
                }
                currents[next]++;
                isReadyForNextStepRecord = true;

                return RecordResult.RECORDING;
            }
            else if (next.Pivot == PuyoType.NONE && next.Satellite == PuyoType.NONE && !isReadyForNextStepRecord2)
            {
                // ネクストぷよが設定されていない時の処理

                if (!isReadyForNextStepRecord)
                {
                    // ネクストぷよはセット⇒ツモの順に行われるため、それに該当しないようなものは無視する
                    return RecordResult.RECORDING;
                }

                // ここに来るのはネクストがツモられた瞬間のみ
                curNext = DecideCurNext();
                if (captureOnlyTsumo)
                {
                    // ツモのみキャプチャする場合は、確定したツモを譜情報に追加し、次のツモ処理に戻る

                    if (prevNext == null)
                    {
                        // 初手の場合

                        prevNext = curNext;
                        isReadyForNextStepRecord = false;
                        currents = new Dictionary<ColorPairPuyo, int>();
                        curNext = null;

                        return RecordResult.RECORD_FORWARD;
                    }
                    else
                    {
                        // 2手目以降

                        // 譜の情報を追記し、次のツモの処理待ち状態に戻す
                        steps.Add(prevNext);
                        prevNext = curNext;
                        isReadyForNextStepRecord = false;
                        currents = new Dictionary<ColorPairPuyo, int>();
                        curNext = null;
                        captureFailCount = 0;

                        return RecordResult.RECORD_FORWARD;
                    }
                }
                else
                {
                    isReadyForNextStepRecord2 = true;
                }
            }

            if (isReadyForNextStepRecord2)
            {
                // ネクストがツモられた後、前のツモがどこに設置されたか判定できるまでここが呼ばれる

                if (prevNext == null)
                {
                    // 初手の場合

                    prevNext = curNext;
                    isReadyForNextStepRecord = false;
                    isReadyForNextStepRecord2 = false;
                    currents = new Dictionary<ColorPairPuyo, int>();
                    curNext = null;

                    return RecordResult.RECORD_FORWARD;
                }
                else
                {
                    // 2手目以降

                    ColorPairPuyo prevStep = prevField.GetStepFromDiff(curField, prevNext);
                    if (prevStep != null)
                    {
                        // 前ツモをどこにおいたか確定できた場合

                        LOGGER.Debug(prevStep.Pivot + " " + prevStep.Satellite + " " + prevStep.Dir + " " + prevStep.Pos);

                        // 譜の情報を追記し、次のツモの処理待ち状態に戻す
                        steps.Add(prevStep);
                        prevField.Drop(prevStep);
                        prevNext = curNext;
                        isReadyForNextStepRecord = false;
                        isReadyForNextStepRecord2 = false;
                        currents = new Dictionary<ColorPairPuyo, int>();
                        curNext = null;
                        captureFailCount = 0;

                        return RecordResult.RECORD_FORWARD;
                    }
                    else
                    {
                        LOGGER.Info("前回：\n" + prevField);
                        LOGGER.Info("今回：\n" + curField);

                        captureFailCount++;

                        // 計1秒以上譜が特定できなければ失敗とする
                        if (captureFailCount > (1000 / captureInterval))
                        {
                            isRecording = false;
                            IsRecordFailed = true;
                            IsRecordEnded = true;
                            return RecordResult.RECORD_FAILURE;
                        }
                    }
                }
            }

            return RecordResult.RECORDING;
        }

        /// <summary>
        /// カレントツモを決定する
        /// </summary>
        /// <returns>カレントの組ぷよ</returns>
        private ColorPairPuyo DecideCurNext()
        {
            // ツモの待機中に一番長い間検出されていた色の組み合わせを現在のツモとする
            // これによりツモ移動中による誤判定の確率を減らすことができる
            ColorPairPuyo next = (from n in currents
                                  where n.Value == (from nn in currents select nn.Value).Max()
                                  select n.Key).ElementAt(0);

            if (currents.Count > 1)
            {
                LOGGER.Debug("ツモ候補が2つありました。");
                foreach (KeyValuePair<ColorPairPuyo, int> pair in currents)
                {
                    LOGGER.Debug(pair.Key.Pivot + " " + pair.Key.Satellite + " :" + pair.Value);
                }
            }

            LOGGER.Debug("確定ツモ:" + next.Pivot + " " + next.Satellite);
            return next;
        }

        /// <summary>
        /// ぷよ譜コードを取得する
        /// </summary>
        /// <returns>ぷよ譜コード</returns>
        public string GetStepRecord()
        {
            return steps.Encode();
        }

        /// <summary>
        /// 初手の配色パターンを取得する
        /// </summary>
        /// <returns>初手の配色パターン</returns>
        public string GetFirstStepsPattern() {
            return steps.GetFirstStepsPattern(3);
        }
    }
}
