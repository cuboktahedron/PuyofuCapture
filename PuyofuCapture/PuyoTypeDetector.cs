/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/license/LICENSE-MIT.txt
 */
using Cubokta.Common;
using Cubokta.Puyo.Common;
using log4net;
using System.Collections.Generic;
using System.Drawing;

namespace Cubokta.Puyo
{
    /// <summary>
    /// ぷよ種別検出機
    /// </summary>
    class PuyoTypeDetector
    {
        /// <summary>ロガー</summary>
        private static readonly ILog LOGGER =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>色識別用にRGBの各要素を分類する際に必要となるビット数</summary>
        private const int COLOR_ELEMENT_BIT = 3;

        /// <summary>色の分類数(0～255を何分割するか)</summary>
        private const int COLOR_DIVISION_NUM = 2 << (COLOR_ELEMENT_BIT - 1);

        /// <summary>類似判定に使用するピクセルのマスクパターン</summary>
        private static readonly int[,] MASK = new int[CaptureField.UNIT, CaptureField.UNIT] {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        };

        /// <summary>色識別用サンプルデータ</summary>
        private IDictionary<PuyoType, int[,,]> colorSamples = new Dictionary<PuyoType, int[,,]>();

        /// <summary>同じぷよタイプであると見なす類似値の閾値</summary>
        public int SimilarityThreshold { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PuyoTypeDetector()
        {
            colorSamples[PuyoType.AKA] = new int[COLOR_DIVISION_NUM, COLOR_DIVISION_NUM, COLOR_DIVISION_NUM];
            colorSamples[PuyoType.MIDORI] = new int[COLOR_DIVISION_NUM, COLOR_DIVISION_NUM, COLOR_DIVISION_NUM];
            colorSamples[PuyoType.AO] = new int[COLOR_DIVISION_NUM, COLOR_DIVISION_NUM, COLOR_DIVISION_NUM];
            colorSamples[PuyoType.KI] = new int[COLOR_DIVISION_NUM, COLOR_DIVISION_NUM, COLOR_DIVISION_NUM];
            colorSamples[PuyoType.MURASAKI] = new int[COLOR_DIVISION_NUM, COLOR_DIVISION_NUM, COLOR_DIVISION_NUM];
        }

        /// <summary>
        /// ぷよ種別を判定する
        /// </summary>
        /// <param name="ba">判定元画像のアクセサ</param>
        /// <param name="rect">判定に使用する範囲</param>
        /// <returns>判定されたぷよ種別</returns>
        public PuyoType Detect(RapidBitmapAccessor ba, Rectangle rect)
        {
            int[,,] pattern = GetPattern(ba, rect);
            return GetPuyoType(pattern);
        }

        /// <summary>同じぷよタイプであると見なす類似値の閾値</summary>
        private PuyoType GetPuyoType(int[,,] pattern)
        {
            // サンプル画像との類似値が最小となるぷよ種別を探す
            PuyoType similarType = PuyoType.NONE;
            int minSimilarityValue = int.MaxValue;
            for (int typeIndex = (int)PuyoType.AKA; typeIndex <= (int)PuyoType.MURASAKI; typeIndex++)
            {
                PuyoType type = (PuyoType)typeIndex;
                int similarityValue = GetSimilarityValue(type, pattern);
//                LOGGER.Debug(type + ":" + similarityValue + " ");
                if (minSimilarityValue > similarityValue) {
                    minSimilarityValue = similarityValue;
                    similarType = type;
                }
            }

            if (minSimilarityValue >= SimilarityThreshold)
            {
                // 最小値が閾値よりも大きい場合は、判定不可できなかった扱いとする
//                LOGGER.Debug("【" + PuyoType.NONE + ":" + minSimilarityValue + "】"); 
                return PuyoType.NONE;
            }
            else
            {
//                LOGGER.Debug("【" + similarType + ":" + minSimilarityValue + "】"); 
                return similarType;
            }
        }

        /// <summary>
        /// 類似値を取得
        /// </summary>
        /// <param name="puyoType">ぷよ種別</param>
        /// <param name="pattern">出現数パターン配列</param>
        /// <returns>類似値</returns>
        private int GetSimilarityValue(PuyoType puyoType, int[,,] pattern)
        {
            // サンプルと実測値との距離を計算
            // 各色分布の差を2乗した総和値を距離を類似値とする
            int similarityValue = 0;
            int[,,] sample = colorSamples[puyoType];
            for (int ri = 0; ri < COLOR_DIVISION_NUM; ri++)
            {
                for (int gi = 0; gi < COLOR_DIVISION_NUM; gi++)
                {
                    for (int bi = 0; bi < COLOR_DIVISION_NUM; bi++)
                    {
                        int diff = sample[ri, gi, bi] - pattern[ri, gi, bi];
                        similarityValue += (diff * diff);
                    }
                }
            }

            return similarityValue;
        }

        /// <summary>
        /// 出現数パターン配列を取得
        /// </summary>
        /// <param name="ba">パターン判定元画像のアクセサ</param>
        /// <param name="rect">判定に使用する範囲</param>
        /// <returns>出現数パターン配列</returns>
        private int[,,] GetPattern(RapidBitmapAccessor ba, Rectangle rect)
        {
            int[,,] patterns = new int[COLOR_DIVISION_NUM, COLOR_DIVISION_NUM, COLOR_DIVISION_NUM];
            int colorRange = 256 / COLOR_DIVISION_NUM;
            for (int x = rect.X; x < rect.X + rect.Width; x = x + 2)
            {
                for (int y = rect.Y; y < rect.Y + rect.Height; y = y + 2)
                {
                    if (MASK[x - rect.X, y - rect.Y] != 1)
                    {
                        // マスクされている個所は処理しない
                        continue;
                    }

                    // RGBから色番号を決定
                    Color c = ba.GetPixel(x, y);
                    int ri = c.R / colorRange;
                    int gi = c.G / colorRange;
                    int bi = c.B / colorRange;

                    patterns[ri, gi, bi]++;
                }
            }

            return patterns;
        }

        /// <summary>
        /// ぷよタイプ識別用のサンプルデータを更新する。
        /// </summary>
        /// <param name="puyoType">ぷよ種別</param>
        /// <param name="ba">サンプルデータ画像のアクセサ</param>
        /// <remarks>サンプルデータ画像のサイズは1セルのサイズとする。</remarks>
        public void UpdateSample(PuyoType puyoType, RapidBitmapAccessor ba)
        {
            colorSamples[puyoType] = GetPattern(ba, new Rectangle()
            {
                X = 0,
                Y = 0,
                Width = CaptureField.UNIT,
                Height = CaptureField.UNIT
            });
        }
    }
}
