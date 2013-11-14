using Cubokta.Puyo.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo
{
    public enum RecordResult
    {
        NOT_RECORDING,
        RECORDING,
        RECORD_SUCCESS,
        RECORD_FAILURE,
        RECORD_FORWARD,
        RECORD_ENDED,
    }

    public class PuyofuRecorder
    {
        private static readonly ILog LOGGER =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private FCodeEncoder encoder = new FCodeEncoder();
        private List<PairPuyo> steps = new List<PairPuyo>();
        private ColorPairPuyo curNext;
        private ColorPairPuyo prevNext;
        private bool isReadyForNextStepRecord;
        private bool isReadyForNextStepRecord2;
        private bool isRecording;
        public bool IsRecordEnded { get; set; }
        private int captureFailCount;
        private IDictionary<ColorPairPuyo, int> currents = new Dictionary<ColorPairPuyo, int>();
        private CaptureField prevField = new CaptureField();
        private int captureInterval;

        public void BeginRecord(int captureInterval)
        {
            isRecording = true;
            this.captureInterval = captureInterval;
        }

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

            if (steps.Count() >= 16)
            {
                isRecording = false;
                IsRecordEnded = true;
                return RecordResult.RECORD_SUCCESS;
            }

            if (next.Pivot != PuyoType.NONE && next.Satellite != PuyoType.NONE)
            {
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
                if (!isReadyForNextStepRecord)
                {
                    return RecordResult.RECORDING;
                }

                curNext = DecideCurNext();
                isReadyForNextStepRecord2 = true;
            }

            if (isReadyForNextStepRecord2)
            {
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
                        LOGGER.Debug(prevStep.Pivot + " " + prevStep.Satellite + " " + prevStep.Dir + " " + prevStep.Pos);
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
                        LOGGER.Error("前回：\n" + prevField);
                        LOGGER.Error("今回：\n" + curField);

                        captureFailCount++;

                        // 計1秒以上譜が特定できなければ失敗とする
                        if (captureFailCount > (1000 / captureInterval))
                        {
                            isRecording = false;
                            IsRecordEnded = true;
                            return RecordResult.RECORD_FAILURE;
                        }
                    }
                }
            }

            return RecordResult.RECORDING;
        }

        private ColorPairPuyo DecideCurNext()
        {
            // ツモの待機中に一番長い間検出されていた色の組み合わせを現在のツモとする
            // これによりツモ移動中によるご判定の確率を減らすことができる
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

        public string GetRecord()
        {
            return encoder.Encode(steps);
        }

        internal List<PairPuyo> GetSteps()
        {
            return new List<PairPuyo>(steps);
        }
    }
}
